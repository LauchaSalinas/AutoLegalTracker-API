using System.Globalization;
using System.Text.RegularExpressions;
using LegalTracker.Application.Services;
using LegalTracker.Domain.DTOs;
using LegalTracker.Domain.Entities;

namespace LegalTracker.Scrapper.ExternalServices
{
    public partial class ScrapBusiness
    {
        #region Constructor
        private readonly IConfiguration _configuration;
        private readonly PuppeteerService _puppeteerService;
        private readonly ScrapperService _scrapperService;

        public ScrapBusiness(IConfiguration configuration, PuppeteerService puppeteerService, ScrapperService scrapperService)
        {
            _configuration = configuration;
            _puppeteerService = puppeteerService;
            _scrapperService = scrapperService;
            _puppeteerService.InicializeService().GetAwaiter().GetResult();
        }
        #endregion Constructor

        /// <summary>
        /// Get last scrap page and last scrapped case. if null, set to 1
        /// uses builtin web js function buscar [scrapage] and iterates through all pages until 
        /// scrapper detects that the first case of the page is the same as the first case of the previous page
        /// </summary>
        /// <param name="user"></param>
        /// <returns>the user with the filled cases</returns>
        /// <exception cref="Exception"></exception>
        public async Task<UserToScrapNewLegalCasesDTO> ScrapUserGetAllCases(UserToScrapNewLegalCasesDTO user)
        {
            if (user == null || user.LastScrappedLegalCase == null)
                throw new Exception("User is null");

            await _puppeteerService.GoToUrl("https://notificaciones.scba.gov.ar/InterfazBootstrap/vercausas.aspx");
            await _puppeteerService.Wait(int.Parse(_configuration["timeout"]));

            var legalCases = new List<LegalCase>();
            bool stopScraping = false;
            var firstCaseOfPage = new LegalCase { CaseNumber = "0" };
            while (!stopScraping)
            {
                await _puppeteerService.ExecuteJs("buscar", user.LastScrappedLegalCase.ToString());
                await _puppeteerService.Wait(int.Parse(_configuration["timeout"]));
                var hrefDeCasos = await _puppeteerService.GetStringArray(_configuration["funcionParaStringArrayObtenerHrefCasos"]);
                var numerosDeCasos = await _puppeteerService.GetStringArray(_configuration["funcionParaStringArrayObtenerNumeroDeCausas"]);
                var caratulasDeCasos = await _puppeteerService.GetStringArray(_configuration["funcionParaStringArrayObtenerCaratulaDeCausas"]);
                var juzgadosDeCasos = await _puppeteerService.GetStringArray(_configuration["funcionParaStringArrayObtenerJuzgadoDeCausas"]);


                for (int i = 0; i < hrefDeCasos.Count; i++)
                {
                    var legalCase = new LegalCase()
                    {
                        UserId = user.Id,
                        ScrapUrl = hrefDeCasos[i],
                        CaseNumber = numerosDeCasos[i],
                        Caption = caratulasDeCasos[i],
                        Jurisdiction = juzgadosDeCasos[i],
                        CreatedAt = DateTime.Now,
                        LastScrappedAt = DateTime.Now
                    };
                    // check first case of list to check if we can stop scrapping
                    if (i == 0 && firstCaseOfPage.CaseNumber == legalCase.CaseNumber)
                    {
                        stopScraping = true;
                        user.LastScrappedPage -= 2;
                        break;
                    }
                    else
                    {
                        legalCases.Add(legalCase);
                    }

                    // if we reached the last scrapped case from previous scrap, 
                    // we reset the list to start scrapping again from the new cases.
                    if (user.LastScrappedLegalCase.CaseNumber == legalCase.CaseNumber)
                    {
                        legalCases.Clear();
                    }
                }

                user.LastScrappedPage++;
                firstCaseOfPage = new LegalCase()
                {
                    UserId = user.Id,
                    ScrapUrl = hrefDeCasos[0],
                    CaseNumber = numerosDeCasos[0],
                    Caption = caratulasDeCasos[0],
                    Jurisdiction = juzgadosDeCasos[0],
                    CreatedAt = DateTime.Now,
                    LastScrappedAt = DateTime.Now
                };
            }
            user.ScrappedLegalCases.AddRange(legalCases);
            return user;
        }

        public async Task<ICollection<LegalNotification>> ScrapCaseGetNewNotifications(LegalCaseToScrapNotificationsDTO legalCase)
        {
            // get all users
            // foreach user in users
            // login
            // get all cases with the last scrap date < today - 1 day , last scrapped notification 
            // foreach case in cases
            await _puppeteerService.Wait(int.Parse(_configuration["timeout"]) / 10);
            await _puppeteerService.GoToUrl(legalCase.ScrapUrl);
            await _puppeteerService.Wait(int.Parse(_configuration["timeout"]) / 10);

            // var pageDidntThrowError = bool.Parse(await _puppeteerService.GetString("() => {const elements = document.querySelectorAll('h3'); const filteredElements = Array.from(elements).filter((element) => element.innerText.trim() === 'Error'); return filteredElements.length == 0;}"));

            // while(!pageDidntThrowError){
            //     await _puppeteerService.refreshNoTimeout();
            //     await _puppeteerService.Wait(int.Parse(_configuration["timeout"]));

            //     pageDidntThrowError = bool.Parse(await _puppeteerService.GetString("() => {const elements = document.querySelectorAll('h3'); const filteredElements = Array.from(elements).filter((element) => element.innerText.trim() === 'Error'); return filteredElements.length == 0;}"));
            //     throw new Exception("Error en la pagina");
            // }




            await _puppeteerService.GetStringArray(@"() => {document.querySelector('#tabla_length > label > select > option:nth-child(2)').value=10000; return true;}");
            await _puppeteerService.Wait(int.Parse(_configuration["timeout"]) / 10);
            await _puppeteerService.GetStringArray(@"() => {document.querySelector('#tabla_length > label > select').selectedIndex = 1; return true;}");
            await _puppeteerService.Wait(int.Parse(_configuration["timeout"]) / 10);
            await _puppeteerService.Select("select[name=tabla_length]", "1000");
            await _puppeteerService.Wait(int.Parse(_configuration["timeout"]) / 10);

            var nombreLegalNotification = await _puppeteerService.GetStringArray(_configuration["funcionParaStringArrayObtenerNombreDeLegalNotification"]);
            var fechaLegalNotification = await _puppeteerService.GetStringArray(_configuration["funcionParaStringArrayObtenerFechaDeLegalNotification"]);
            var boolFirmaLegalNotification = await _puppeteerService.GetStringArray(_configuration["funcionParaStringArrayObtenerBoolFirmaLegalNotification"]);
            var hrefLegalNotification = await _puppeteerService.GetStringArray(_configuration["funcionParaStringArrayObtenerHrefLegalNotification"]);

            var legalNotifications = new List<LegalNotification>();
            for (int i = hrefLegalNotification.Count - 1; i > -1; i--)
            {

                var legalNotification = new LegalNotification()
                {
                    LegalCaseId = legalCase.Id,
                    Title = nombreLegalNotification[i],
                    NotificationDate = DateTime.Parse(fechaLegalNotification[i], new CultureInfo("es-AR")),
                    Signed = boolFirmaLegalNotification[i] == "True" || boolFirmaLegalNotification[i] == "true",
                    ScrapUrl = hrefLegalNotification[i],
                    CreatedAt = DateTime.Now,
                    NotificationType = hrefLegalNotification[i].Contains("idTramite") ? "Tramite" : hrefLegalNotification[i].Contains("idPresentacion") ? "Presentacion" : "Notificacion"
                };

                legalNotifications.Add(legalNotification);
                if (legalCase.LastScrappedNotification != null && hrefLegalNotification[i] == legalCase.LastScrappedNotification.ScrapUrl)
                    legalNotifications.Clear();
            }

            return legalNotifications;
            // goto case url
            // get last scrap notification. if null, scrap all notifications
            // document.querySelector('#tabla_next > a').click() until no results
            // save new notifications to the database
        }

        public async Task<LegalNotificationToFillDTO> ScrapNotificationUpdateContent(LegalNotificationToFillDTO legalNotificationToFill)
        {
            // get all users
            // foreach user in users
            // login
            // get all notifications with null body


            // foreach notification in notifications
            // goto notification url
            // scrap notification text
            // update notification to the database

            await _puppeteerService.GoToUrl(legalNotificationToFill.ScrapUrl);
            await _puppeteerService.Wait(int.Parse(_configuration["timeout"]));

            var isValidNotification = bool.Parse(await _puppeteerService.GetString("() => {const elements = document.querySelectorAll('*'); const filteredElements = Array.from(elements).filter((element) => element.innerText.trim() === 'Texto no disponible'); return filteredElements.length == 0;}"));

            if (isValidNotification)
            {
                string table = string.Empty;
                switch (legalNotificationToFill.NotificationType)
                {
                    case "Tramite":
                        table = "#listaTramites > table";
                        legalNotificationToFill.Body = await _puppeteerService.GetPropertyWithSelector("#listaNovedades > table:nth-child(4) > tbody > tr:nth-child(2)", "innerHTML");
                        // legalNotification.From = await _puppeteerService.GetPropertyWithSelector("#listaTramites > table > tbody > tr:nth-child(6) > td:nth-child(2) > p", "innerText");
                        // legalNotification.NotificationDate = DateTime.Parse(await _puppeteerService.GetPropertyWithSelector("#listaTramites > table > tbody > tr:nth-child(8) > td:nth-child(2) > p", "innerText"), new CultureInfo("es-AR"));;                      
                        // legalNotification.Observation = await _puppeteerService.GetPropertyWithSelector("#listaTramites > table > tbody > tr:nth-child(2) > td:nth-child(2)", "innerText");
                        break;
                    case "Presentacion":
                        table = "#textoPresentacion > table:nth-child(3)";
                        legalNotificationToFill.Body = await _puppeteerService.GetPropertyWithSelector("#hdTexto", "value");
                        // legalNotification.From = await _puppeteerService.GetPropertyWithSelector("#ctl00_cph_lblUsuarioGenerador", "innerText");
                        // legalNotification.NotificationDate = DateTime.Parse(await _puppeteerService.GetPropertyWithSelector("#ctl00_cph_lblFechaPresentacion", "innerText"), new CultureInfo("es-AR"));
                        break;
                    case "Notificacion":
                        table = "#textoNotificacion > table:nth-child(1)";
                        // remove hidden text
                        await _puppeteerService.GetStringArray("() => {document.querySelector('#ctl00_cph_ucTextoNotificacion_hdTextoOculto').remove(); return true;}");
                        legalNotificationToFill.Body = await _puppeteerService.GetPropertyWithSelector("#textoNotificacion > table:nth-child(2) > tbody > tr:nth-child(2)", "innerHTML");
                        // legalNotification.NotificationDate = DateTime.Parse(await _puppeteerService.GetPropertyWithSelector("#ctl00_cph_ucTextoNotificacion_lblFechaAlta", "innerText"), new CultureInfo("es-AR"));
                        // legalNotification.From = await _puppeteerService.GetString("() => {var e = document.querySelector('#textoNotificacion > table:nth-child(1) > tbody > tr:nth-child(11) > td:nth-child(2) > p'); var returnString = e.innerText; if (e.innerText.indexOf('Certificado')){ returnString = e.innerText.substring(0, e.innerText.indexOf('Certificado')).trim(); }; if (e.innerText.indexOf('---')){ returnString = e.innerText.substring(0, e.innerText.indexOf('---')).trim(); }; return returnString;}");
                        break;
                }

                var functionToGetReferences = await _puppeteerService.GetStringArray("() => { const keyValues = []; const table = document.querySelector('" + table + "'); if (!table) { return keyValues; } const rows = table.querySelectorAll('tr'); for (let i = 1; i < rows.length; i++) { const row = rows[i]; const cells = row.querySelectorAll('td'); if (cells.length >= 2) {const key = cells[0].textContent.trim(); const value = cells[1].textContent.trim(); if(key != ''){ keyValues.push(key); keyValues.push(value);}}} return keyValues; }");
                var references = new List<NotificationReference>();
                for (int i = 0; i < functionToGetReferences.Count; i += 2)
                {
                    references.Add(new NotificationReference()
                    {
                        Key = functionToGetReferences[i],
                        Value = functionToGetReferences[i + 1],
                        LegalNotificationId = legalNotificationToFill.Id
                    });
                }
                legalNotificationToFill.NotificationReferences = references;
            }
            else
            {
                legalNotificationToFill.Body = "Texto no disponible";
                //legalNotification.LastScrappedAt = DateTime.Now;
                // log error
            }

            return legalNotificationToFill;
        }

        public async Task LogOut()
        {
            await _puppeteerService.GoToUrl("https://notificaciones.scba.gov.ar/desconectar.aspx");
        }

        internal async Task<IEnumerable<LegalNotificationByNotificationsPageDTO>> GetNotificationsByNotificationsPage(UserToScrapLastNotificationsDTO user, int daysFromTodayToScrap)
        {

            var customCulture = new CultureInfo("es-AR");
            // goto notifications page
            // set date
            // set Estado = "Todas"
            // click buscar

            // get all cases

            await _puppeteerService.Wait(int.Parse(_configuration["timeout"]) / 2);
            await _puppeteerService.GoToUrl("https://notificaciones.scba.gov.ar/InterfazBootstrap/notificaciones.aspx#");
            await _puppeteerService.Wait(int.Parse(_configuration["timeout"]) / 2);
            await _puppeteerService.ClearInputAsync("#Desde");
            await _puppeteerService.ClearInputAsync("#Hasta");
            // NOT WORKING await _puppeteerService.ExecuteJs("() => {document.querySelector('#Desde').value = ''; return true;}");
            await _puppeteerService.Wait(int.Parse(_configuration["timeout"]) / 2);
            await _puppeteerService.TypeAsync("#Desde", DateTime.Now.AddDays(-daysFromTodayToScrap).ToString("dd/MM/yyyy"));
            await _puppeteerService.TypeAsync("#Hasta", DateTime.Now.AddDays(-daysFromTodayToScrap + 1).ToString("dd/MM/yyyy"));
            await _puppeteerService.Wait(int.Parse(_configuration["timeout"]) / 2);
            await _puppeteerService.Select("#selectProcesada", "-1");
            await _puppeteerService.Wait(int.Parse(_configuration["timeout"]) / 5);
            // TODO make this in the other pages
            await _puppeteerService.ExecuteJs("buscar", "1");
            await _puppeteerService.Wait(int.Parse(_configuration["timeout"]) / 6);


            var legalNotifications = new List<LegalNotificationByNotificationsPageDTO>();
            LegalNotificationByNotificationsPageDTO legalNotification;
            var hrefNotificationStringArray = new List<string>();
            var totalPages = int.Parse(await _puppeteerService.GetString("() => {return document.querySelector('#cantPag').value;}"));
            if(totalPages == 0)
                return legalNotifications;

            // repeat for all pages
            for (int i = 0; i < totalPages; i++)
            {
                await _puppeteerService.ExecuteJs("buscar", (i + 1).ToString());
                await _puppeteerService.Wait(int.Parse(_configuration["timeout"]) / 6);
                // todo make this
                var hrefNotifications = await _puppeteerService.GetStringArray("() => {const elements = []; document.querySelectorAll('.btn.btn-sm.btn-success.boton100.Detalle').forEach(function(element) {elements.push(element.href)}); return elements ; }");
                hrefNotificationStringArray.AddRange(hrefNotifications);

                var tableTexts = await _puppeteerService.GetStringArray("() => {const tablesTexts = []; const tables = document.querySelectorAll('#notificaciones > div > div > div').forEach(table => { tablesTexts.push(table.innerText)}); return tablesTexts;}");


                foreach (var tableText in tableTexts)
                {
                    var jurisdiction = Regex.Match(tableText, @"(?<=Organismo:)(.*?)(?=\r\n)").Value;
                    var caseNumber = Regex.Match(tableText, @"(?<=Número:)(.*?)(?=\r\n|-)").Value.Trim();
                    var caption = Regex.Match(tableText, @"(?<=Carátula:)(.*?)(?=\r\n|- Número)").Value.Trim();
                    var Destinatario = Regex.Match(tableText, @"(?<=Destinatario:)(.*?)(?=\r\n)").Value;
                    var Domicilio = Regex.Match(tableText, @"(?<=Domicilio:)(.*?)(?=\r\n)").Value;
                    var FechaAlta = Regex.Match(tableText, @"(?<=Alta o Disponibilidad:)(.*?)(?=\r\n|- Notifica)").Value.Trim();
                    var Notificacion = Regex.Match(tableText, @"(?<=Notificación:)(.*?)(?=\r\n)").Value;
                    var Tramite = Regex.Match(tableText, @"(?<=Trámite:)(.*?)(?=\r\n)").Value;
                    var Codigo = Regex.Match(tableText, @"(?<=Código:)(.*?)(?=\r\n)").Value;
                    legalNotification = new LegalNotificationByNotificationsPageDTO()
                    {
                        UserId = user.Id,
                        CaseNumber = caseNumber,
                        CaseCaption = caption,
                        CaseJurisdiction = jurisdiction,
                        NotificationCreatedAt = DateTime.ParseExact(FechaAlta, "d/MM/yyyy HH:mm:ss", customCulture), // "d/MM/yyyy HH:mm:ss"
                        NotificationNotifiedDate = DateTime.ParseExact(Notificacion, "d/MM/yyyy HH:mm:ss", customCulture),
                        LastScrappedAt = DateTime.MinValue,
                        NotificationTitle = Tramite
                    };
                    legalNotifications.Add(legalNotification);
                }
            }

            // at the end of this process we have all the scrapUrl of the notifications of that day, with some info about the case.
            return legalNotifications;
            
        }

        public async Task GetLegalCaseFromOrphanNotification(OrphanNotificationDTO orphanNotification)
        {
            await _puppeteerService.Wait(int.Parse(_configuration["timeout"]) / 2);
            await _puppeteerService.GoToUrl(orphanNotification.ScrapUrl);
            await _puppeteerService.ClickSelector("#lnkVerCausa", "#numeroCausa");
            await _puppeteerService.Wait(int.Parse(_configuration["timeout"]) * 2);

            var numeroCausa = await _puppeteerService.GetPropertyWithSelector("#numeroCausa", "innerText");
            var scarpURL = await _puppeteerService.GetString("() => {return window.location.href;}");
            if (numeroCausa != null)
            {
                orphanNotification.ScrappedLegalCase.CaseNumber = numeroCausa;
                orphanNotification.ScrappedLegalCase.ScrapUrl = scarpURL;
            }
        }
        
        #region Entry Point Methods
        /// <summary>
        /// get all users with the role "Admin", scrap url and login password and last scrap date from the database
        /// foreach user in users 
        /// login
        /// Scrap all cases from "Mis Causas" page
        /// </summary>
        /// <returns></returns>
        public async Task ScrapUserGetAllCases()
        {
            UserToScrapNewLegalCasesDTO userToScrap = await _scrapperService.GetUserToScrapAllCases();
            if (userToScrap == null)
            {
                Console.WriteLine("No users to scrap");
                return;
            }
            try
            {
                await _puppeteerService.LogIn(userToScrap.WebCredentialUser, userToScrap.WebCredentialPassword);
                // Scrap all cases from "Mis Causas" page
                var scrappedUser = await ScrapUserGetAllCases(userToScrap);
                await _scrapperService.UpdateScrappedUserAddLegalCases(scrappedUser);
                await LogOut();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error at scrapping user: {userToScrap.WebCredentialUser} ex: {ex}");
                throw;
            }
        }

        public async Task ScrapCasesGetAllNotifications()
        {
            UsersLegalCasesToScrapNotificationsDTO userToScrap = await _scrapperService.GetUserWithEmptyCasesToScrap();
            if (userToScrap == null)
            {
                Console.WriteLine("No users to scrap");
                return;
            }

            try
            {
                await _puppeteerService.LogIn(userToScrap.WebCredentialUser, userToScrap.WebCredentialPassword);
                foreach (var legalCase in userToScrap.LegalCasesToGetNotifications)
                {
                    try
                    {
                        legalCase.ScrappedLegalNotifications = await ScrapCaseGetNewNotifications(legalCase);
                        await _scrapperService.UpdateLegalCasesAddNotifications(legalCase);
                        Console.WriteLine($"Legal Case updated {legalCase.Id}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error at scrapping user: {userToScrap.WebCredentialUser} legal case: {legalCase.Id} " + ex.ToString());
                    }
                }
                await LogOut();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task ScrapNotificationsUpdateContent()
        {
            UsersLegalNotificationsToUpdateContentDTO userToScrap = await _scrapperService.GetUserToScrapNotificationContent();
            if (userToScrap == null)
            {
                Console.WriteLine("No users to scrap");
                return;
            }
            
            try
            {
                await _puppeteerService.LogIn(userToScrap.WebCredentialUser, userToScrap.WebCredentialPassword);
                foreach (var legalNotification in userToScrap.LegalNotificationsToFill)
                {
                    try
                    {
                        // TODO Ensure that only fills the notifications of the last 5 days, if less than 200 notifications, fill 200 notifications
                        var filledLegalNotification = await ScrapNotificationUpdateContent(legalNotification);
                        await _scrapperService.UpdateFilledLegalNotification(filledLegalNotification);// todo define if its user or legalcase
                        Console.WriteLine($"Legal notification's {legalNotification.Id} updated");

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error at scrapping user: {userToScrap.WebCredentialUser} legal notification: {legalNotification.Id} " + ex.ToString());
                    }

                }
                await LogOut();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error at scrapping user: {userToScrap.WebCredentialUser}" + ex.ToString());
                throw;
            }
            
        }

        public async Task ScrapNotificationsByDay()
        {
            UserToScrapLastNotificationsDTO userToScrap = await _scrapperService.GetUserToScrapLastNotifications();
            if (userToScrap == null)
            {
                Console.WriteLine("No users to scrap");
                return;
            }
            try
            {
                await _puppeteerService.LogIn(userToScrap.WebCredentialUser, userToScrap.WebCredentialPassword);
                for (int pastDaysToScrap = 1; pastDaysToScrap <= 5; pastDaysToScrap++)
                {
                    try
                    {
                            var legalNotificationsFroNewNotificationPage = await GetNotificationsByNotificationsPage(userToScrap, pastDaysToScrap);
                            _scrapperService.SaveLegalNotificationsByNotificationsPage(legalNotificationsFroNewNotificationPage);
                            // entonces la concha puta de mi hermana
                            // intentar encontrar el caso padre de esta notificacion, si no lo encuentra, crearlo dejando esta notificacion en una tabla de notificaciones sin caso padre
                            Console.WriteLine($"Notifications from {pastDaysToScrap} days ago saved");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error at scrapping user: {userToScrap.WebCredentialUser} at day {DateTime.Now.AddDays(-pastDaysToScrap)}" + ex.ToString());
                    }
                }
                await LogOut();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task ScrapLegalCasesFromOrphanNotifications()
        {
            // get orphan notifications
            // foreach notification
            // get case url
            // check if exists in db, if not, create it LO TIENE QUE DEJAREN EL MISMO ESTADO QUE EL METODO DE ARRIBA PARA QUE LO AGARRE EL METODO DE ARRIBA
            // it DOESNT scraps the notification content

            UserToScrapOrphanNotificationsDTO userToScrap = await _scrapperService.GetUserToScrapOrphanNotifications();
            if (userToScrap == null)
            {
                Console.WriteLine("No users to scrap");
                return;
            }
            try
            {
                await _puppeteerService.LogIn(userToScrap.WebCredentialUser, userToScrap.WebCredentialPassword);
                foreach (var orphanNotification in userToScrap.OrphanNotifications)
                {
                    try
                    {
                        await GetLegalCaseFromOrphanNotification(orphanNotification);
                        _scrapperService.AddOrUpdateLegalCase(orphanNotification);
                        Console.WriteLine($"Legal case from orphan notification {orphanNotification.ScrapUrl} saved");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error at scrapping user: {userToScrap.WebCredentialUser} orphan notification: {orphanNotification.ScrapUrl}" + ex.ToString());
                    }
                }
                await LogOut();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }

        }

        #endregion Entry Point Methods
    }
}
