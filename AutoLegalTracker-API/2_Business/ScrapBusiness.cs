using AutoLegalTracker_API.WebServices;
using AutoLegalTracker_API.Models;
using System.Globalization;
using System.Text.RegularExpressions;
using AutoLegalTracker_API.DataAccess;

namespace AutoLegalTracker_API.Business
{
    public class ScrapBusiness
    {
        #region Constructor
        private readonly PuppeteerService _puppeteerService;
        private readonly IConfiguration _configuration;
        private readonly LegalCaseDataAccessAsync _legalCaseDataAccessAsync;
        
        public ScrapBusiness(PuppeteerService puppeteerService, IConfiguration configuration, LegalCaseDataAccessAsync legalCaseDataAccessAsync)
        {
            _puppeteerService = puppeteerService;
            _configuration = configuration;
            _legalCaseDataAccessAsync = legalCaseDataAccessAsync;
        }
        #endregion Constructor

        #region Public Methods
        // public async Task CheckNewCases()
        // {
        //     // TODO: reduce the text in the methods
        //     #region Variables
        //     var timeout = int.Parse(_configuration["timeout"]);

        //     #endregion Variables
        //     try{
        //         await _puppeteerService.InicializeService();
                
        //         var controlador = await _puppeteerService.LogIn(_configuration["urlDeLaPaginaLocalhost"], _configuration["selectorInputUsuario"], _configuration["selectorInputContraseña"], _configuration["contenidoInputUsuario"], _configuration["contenidoInputContraseña"], _configuration["selectorBotonIngresar"]);
        //         await _puppeteerService.Wait(timeout);
        //         controlador = await _puppeteerService.GoToUrl(await _puppeteerService.GetPropertyWithSelector(_configuration["selectorMisCausas"], _configuration["propiedadHipervinculo"], int.Parse(_configuration["posicionMisCausas"])));
        //         await _puppeteerService.Wait(int.Parse(_configuration["timeout"]));
        //         await _puppeteerService.ClickSelector(_configuration["selectorBotonMostrarTodasLasCausas"], _configuration["selectorObjetoConfirmacion"]);
        //         await _puppeteerService.Wait(int.Parse(_configuration["timeout"]));
        //         var result = await _puppeteerService.GetStringArray(_configuration["funcionParaStringArrayObtenerHrefCasos"]);
        //         await _puppeteerService.Wait(int.Parse(_configuration["timeout"]));
        //         foreach(var causa in LlenarListaDeCausas(result))
        //             {
        //                 await _puppeteerService.GoToUrl(causa.ScrapUrl);
        //                 await CargarCausa(causa);
        //                 await _puppeteerService.Wait(int.Parse(_configuration["timeout"]));
        //                 var r = await _puppeteerService.GetStringArray(_configuration["funcionParaStringArrayObtenerHrefItramites"]);
        //                 foreach (var a in r)
        //                 {
        //                     AgregarTramiteALista(causa, a);
        //                 }

        //                 foreach(var legalNotification in causa.LegalNotifications)
        //                 {
        //                     await _puppeteerService.GoToUrl(legalNotification.ScrapUrl);
        //                     await _puppeteerService.Wait(int.Parse(_configuration["timeout"]));
        //                     await CargarTramite(legalNotification);
        //                     Console.ReadLine();
        //                 }

        //             }
        //     }
        //     catch(Exception e){
        //         Console.WriteLine(e.Message);
        //     }
        // }

        public async Task CargarCausa(LegalCase causa)
        {
            causa.CaseNumber = await _puppeteerService.GetPropertyWithSelector(_configuration["selectorNumeroCausa"], _configuration["propiedadTexto"]);
            causa.Caption = await _puppeteerService.GetPropertyWithSelector(_configuration["selectorCaratula"], _configuration["propiedadTexto"]);
            causa.Jurisdiction = await _puppeteerService.GetPropertyWithSelector(_configuration["selectorOrganismo"], _configuration["propiedadTexto"]);
        }

        public void AgregarTramiteALista(LegalCase causa, string a)
        {
            switch (a.ToString())
            {
                case string s when s.Contains("Presentacion"):
                    LegalNotification pres = new()
                    {
                        NotificationType = "Presentacion",
                        ScrapUrl = s
                    };
                    causa.LegalNotifications.Add(pres);
                    break;

                case string s when s.Contains("Notificacion"):
                    LegalNotification noti = new()
                    {
                        NotificationType = "Notificacion",
                        ScrapUrl = s
                    };
                    causa.LegalNotifications.Add(noti);
                    break;

                case string s when s.Contains("Tramite"):
                    LegalNotification tra = new()
                    {
                        NotificationType = "Tramite",
                        ScrapUrl = s
                    };
                    causa.LegalNotifications.Add(tra);
                    break;

                default: break;
            }
            return;
        }

        public async Task CargarTramite(LegalNotification i)
        {
            switch (i.NotificationType)
            {
                case "Notificacion":

                    i.Observation = await _puppeteerService.GetPropertyWithSelector(
                        _configuration["selectorTipoNotificacion"], _configuration["propiedadTexto"]);
                    i.Title = i.Observation;

                    i.Body = await _puppeteerService.GetPropertyWithSelector(
                        _configuration["selectorTextoNotificacion"], _configuration["propiedadTexto"], int.Parse(_configuration["posicionTextoNotificacionYPresentacion"]));

                    Console.WriteLine("Este tramite es notificacion de tipo {0}." +
                        "\nSu texto es: {1}", i.Observation, i.Body);
                    break;

                case "Presentacion":

                    i.Title = await _puppeteerService.GetPropertyWithSelector(_configuration["selectorTituloPresentacion"], _configuration["propiedadTexto"]);
                    // TODO store the subtype of the presentation but not in the Observation property
                    i.Observation = await _puppeteerService.GetPropertyWithSelector(_configuration["selectorTipoPresentacion"], _configuration["propiedadTexto"]);

                    i.Body = await _puppeteerService.GetPropertyWithSelector(
                        _configuration["selectorTextoPresentacion"], _configuration["propiedadTexto"], int.Parse(_configuration["posicionTextoNotificacionYPresentacion"]));

                    Console.WriteLine("Este tramite es presentacion de tipo {0}, con titulo {1}" +
                        "\nTexto: {2}",
                        i.Observation, i.Title, i.Body);
                    break;

                case "Tramite":

                    i.Body = await _puppeteerService.GetPropertyWithSelector(_configuration["selectorTextoTramite"], _configuration["propiedadTexto"], int.Parse(_configuration["posicionTextoTramite"]));

                    Console.WriteLine("Este es un tramite." +
                        "\nTexto: {0}", i.Body);
                    break;
            }
        }

        public List<LegalCase> LlenarListaDeCausas(string[] result)
        {
            List<LegalCase> causas = new();
            foreach (var item in result)
            {
                LegalCase causa = new()
                {
                    ScrapUrl = item
                };
                causas.Add(causa);
            }
            return causas;
        }

        
        #endregion Public Methods

        #region Private Methods

        public void LoadConfiguration()
        {
            if (_configuration == null)
                throw new Exception("Configuration is null");
            

        }

        public async Task<(List<LegalCase>, int)> ScrapUserGetNewCases(User user, string caseNumber, int lastScrappedPage)
        {
            // get all users with the role "Admin", scrap url and login password and last scrap date from the database
            // foreach user in users 
            // login
            // short wait


            // get last scrap page and last scrap case. if null, set to 1
            // buscar [scrapage] and long wait until no results
            // add new cases to the database, save last scrap page and last scrap case

            _puppeteerService.GoToUrl("https://notificaciones.scba.gov.ar/InterfazBootstrap/vercausas.aspx");
            await _puppeteerService.Wait(int.Parse(_configuration["timeout"]));

            var legalCases = new List<LegalCase>();
            bool stopScraping = false;
            var firstCaseOfPage = new LegalCase { CaseNumber = "0" };
            while (!stopScraping)
            {
                await _puppeteerService.ExecuteJs("buscar", lastScrappedPage.ToString());
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
                    if( i == 0 && firstCaseOfPage.CaseNumber == legalCase.CaseNumber)
                    {
                        stopScraping = true;
                        lastScrappedPage -= 2;
                        break;
                    }
                    else
                    {
                        legalCases.Add(legalCase);
                    }
                    
                    // if we reached the last scrapped case from previous scrap, 
                    // we reset the list to start scrapping again from the new cases.
                    if(caseNumber == legalCase.CaseNumber)
                    {
                        legalCases.Clear();
                    }
                }

                lastScrappedPage++;
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
            return (legalCases, lastScrappedPage);
        }

        public async Task<IEnumerable<LegalNotification>> ScrapCaseGetNewNotifications(LegalCase legalCase, LegalNotification? lastScrappedNotification)
        {
            // get all users
            // foreach user in users
            // login
            // get all cases with the last scrap date < today - 1 day , last scrapped notification 
            // foreach case in cases
            await _puppeteerService.Wait(int.Parse(_configuration["timeout"])/10);
            await _puppeteerService.GoToUrl(legalCase.ScrapUrl);
            await _puppeteerService.Wait(int.Parse(_configuration["timeout"])/10);
            
            // var pageDidntThrowError = bool.Parse(await _puppeteerService.GetString("() => {const elements = document.querySelectorAll('h3'); const filteredElements = Array.from(elements).filter((element) => element.innerText.trim() === 'Error'); return filteredElements.length == 0;}"));

            // while(!pageDidntThrowError){
            //     await _puppeteerService.refreshNoTimeout();
            //     await _puppeteerService.Wait(int.Parse(_configuration["timeout"]));

            //     pageDidntThrowError = bool.Parse(await _puppeteerService.GetString("() => {const elements = document.querySelectorAll('h3'); const filteredElements = Array.from(elements).filter((element) => element.innerText.trim() === 'Error'); return filteredElements.length == 0;}"));
            //     throw new Exception("Error en la pagina");
            // }
                



            await _puppeteerService.GetStringArray(@"() => {document.querySelector('#tabla_length > label > select > option:nth-child(2)').value=1000; return true;}");
            await _puppeteerService.Wait(int.Parse(_configuration["timeout"])/10);
            await _puppeteerService.GetStringArray(@"() => {document.querySelector('#tabla_length > label > select').selectedIndex = 1; return true;}");
            await _puppeteerService.Wait(int.Parse(_configuration["timeout"])/10);
            await _puppeteerService.Select("select[name=tabla_length]","1000");
            await _puppeteerService.Wait(int.Parse(_configuration["timeout"])/10);

            var nombreLegalNotification = await _puppeteerService.GetStringArray(_configuration["funcionParaStringArrayObtenerNombreDeLegalNotification"]);
            var fechaLegalNotification = await _puppeteerService.GetStringArray(_configuration["funcionParaStringArrayObtenerFechaDeLegalNotification"]);
            var boolFirmaLegalNotification = await _puppeteerService.GetStringArray(_configuration["funcionParaStringArrayObtenerBoolFirmaLegalNotification"]);
            var hrefLegalNotification = await _puppeteerService.GetStringArray(_configuration["funcionParaStringArrayObtenerHrefLegalNotification"]);

            var legalNotifications = new List<LegalNotification>();
            for (int i = hrefLegalNotification.Count-1; i > -1; i--)
            {

                var legalNotification = new LegalNotification()
                {
                    LegalCaseId = legalCase.Id,
                    Title  = nombreLegalNotification[i],
                    NotificationDate = DateTime.Parse(fechaLegalNotification[i], new CultureInfo("es-AR")),
                    Signed = boolFirmaLegalNotification[i] == "True" || boolFirmaLegalNotification[i] == "true" ,
                    ScrapUrl = hrefLegalNotification[i],
                    CreatedAt = DateTime.Now,
                    NotificationType = hrefLegalNotification[i].Contains("idTramite") ? "Tramite" : hrefLegalNotification[i].Contains("idPresentacion") ? "Presentacion" : "Notificacion"
                };
                 
                legalNotifications.Add(legalNotification);
                if (lastScrappedNotification != null && hrefLegalNotification[i] == lastScrappedNotification.ScrapUrl)
                    legalNotifications.Clear();
            }
            
            return legalNotifications;
            // goto case url
            // get last scrap notification. if null, scrap all notifications
            // document.querySelector('#tabla_next > a').click() until no results
            // save new notifications to the database
        }

        public async Task<IEnumerable<LegalNotification>> ScrapNotificationsUpdateContent(IEnumerable<LegalNotification> legalNotificationsToFill)
        {
            // get all users
            // foreach user in users
            // login
            // get all notifications with null body


            // foreach notification in notifications
            // goto notification url
            // scrap notification text
            // update notification to the database

            foreach (var legalNotification in legalNotificationsToFill)
            {
                await _puppeteerService.GoToUrl(legalNotification.ScrapUrl);
                await _puppeteerService.Wait(int.Parse(_configuration["timeout"]));                

                var isValidNotification = bool.Parse(await _puppeteerService.GetString("() => {const elements = document.querySelectorAll('*'); const filteredElements = Array.from(elements).filter((element) => element.innerText.trim() === 'Texto no disponible'); return filteredElements.length == 0;}"));
                
                if(isValidNotification)
                {
                    string table = string.Empty;
                    switch (legalNotification.NotificationType)
                    {
                        case "Tramite":
                            table = "#listaTramites > table";
                            legalNotification.Body = await _puppeteerService.GetPropertyWithSelector("#listaNovedades > table:nth-child(4) > tbody > tr:nth-child(2)", "innerHTML");
                            // legalNotification.From = await _puppeteerService.GetPropertyWithSelector("#listaTramites > table > tbody > tr:nth-child(6) > td:nth-child(2) > p", "innerText");
                            // legalNotification.NotificationDate = DateTime.Parse(await _puppeteerService.GetPropertyWithSelector("#listaTramites > table > tbody > tr:nth-child(8) > td:nth-child(2) > p", "innerText"), new CultureInfo("es-AR"));;                      
                            // legalNotification.Observation = await _puppeteerService.GetPropertyWithSelector("#listaTramites > table > tbody > tr:nth-child(2) > td:nth-child(2)", "innerText");
                            break;
                        case "Presentacion":
                            table = "#textoPresentacion > table:nth-child(3)";
                            legalNotification.Body = await _puppeteerService.GetPropertyWithSelector("#hdTexto", "value");
                            // legalNotification.From = await _puppeteerService.GetPropertyWithSelector("#ctl00_cph_lblUsuarioGenerador", "innerText");
                            // legalNotification.NotificationDate = DateTime.Parse(await _puppeteerService.GetPropertyWithSelector("#ctl00_cph_lblFechaPresentacion", "innerText"), new CultureInfo("es-AR"));
                            break;
                        case "Notificacion":
                            table = "#textoNotificacion > table:nth-child(1)";
                            // remove hidden text
                            await _puppeteerService.GetStringArray("() => {document.querySelector('#ctl00_cph_ucTextoNotificacion_hdTextoOculto').remove(); return true;}");
                            legalNotification.Body = await _puppeteerService.GetPropertyWithSelector("#textoNotificacion > table:nth-child(2) > tbody > tr:nth-child(2)", "innerHTML");
                            // legalNotification.NotificationDate = DateTime.Parse(await _puppeteerService.GetPropertyWithSelector("#ctl00_cph_ucTextoNotificacion_lblFechaAlta", "innerText"), new CultureInfo("es-AR"));
                            // legalNotification.From = await _puppeteerService.GetString("() => {var e = document.querySelector('#textoNotificacion > table:nth-child(1) > tbody > tr:nth-child(11) > td:nth-child(2) > p'); var returnString = e.innerText; if (e.innerText.indexOf('Certificado')){ returnString = e.innerText.substring(0, e.innerText.indexOf('Certificado')).trim(); }; if (e.innerText.indexOf('---')){ returnString = e.innerText.substring(0, e.innerText.indexOf('---')).trim(); }; return returnString;}");
                            break;
                    }

                    var functionToGetReferences = await _puppeteerService.GetStringArray("() => { const keyValues = []; const table = document.querySelector('"+ table +"'); if (!table) { return keyValues; } const rows = table.querySelectorAll('tr'); for (let i = 1; i < rows.length; i++) { const row = rows[i]; const cells = row.querySelectorAll('td'); if (cells.length >= 2) {const key = cells[0].textContent.trim(); const value = cells[1].textContent.trim(); if(key != ''){ keyValues.push(key); keyValues.push(value);}}} return keyValues; }");
                    var references = new List<NotificationReference>();
                    for (int i = 0; i < functionToGetReferences.Count; i+=2)
                    {
                        references.Add(new NotificationReference()
                        {
                            Key = functionToGetReferences[i],
                            Value = functionToGetReferences[i+1],
                            LegalNotificationId = legalNotification.Id
                        });
                    }
                    legalNotification.NotificationReferences = references;
                }
                else
                {
                    legalNotification.Body = "Texto no disponible";
                    //legalNotification.LastScrappedAt = DateTime.Now;
                    // log error
                }
                
            }

            return legalNotificationsToFill;
        }

        public async Task LogIn(User user)
        {
            await _puppeteerService.InicializeService();
                
            var controlador = await _puppeteerService.LogIn(_configuration["urlDeLaPaginaLocalhost"], _configuration["selectorInputUsuario"], _configuration["selectorInputContraseña"], user.WebCredentialUser, user.WebCredentialPassword, _configuration["selectorBotonIngresar"]);
            await _puppeteerService.Wait(int.Parse(_configuration["timeout"]));
        }
        
        public async Task LogOut()
        {
            await _puppeteerService.GoToUrl("https://notificaciones.scba.gov.ar/desconectar.aspx");
        }

        internal async Task<IEnumerable<LegalCase>> SaveCasesByNotificationsPage(int userId, int daysFromTodayToScrap)
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
            await _puppeteerService.TypeAsync("#Hasta", DateTime.Now.AddDays(-daysFromTodayToScrap+1).ToString("dd/MM/yyyy"));
            await _puppeteerService.Wait(int.Parse(_configuration["timeout"]) / 2);
            await _puppeteerService.Select("#selectProcesada", "-1");
            await _puppeteerService.Wait(int.Parse(_configuration["timeout"]) / 5);
            // TODO make this in the other pages
            await _puppeteerService.ExecuteJs("buscar", "1");
            await _puppeteerService.Wait(int.Parse(_configuration["timeout"]) / 6);
            var totalPages = int.Parse(await _puppeteerService.GetString("() => {return document.querySelector('#cantPag').value;}"));
            
            
            var legalCases = new List<LegalCase>();
            LegalCase legalCase;
            var hrefNotificationStringArray = new List<string>();
            // todo make this
            var hrefNotifications = await _puppeteerService.GetStringArray("() => {const elements = []; document.querySelectorAll('.btn.btn-sm.btn-success.boton100.Detalle').forEach(function(element) {elements.push(element.href)}); return elements ; }");
            var tableTexts = await _puppeteerService.GetStringArray("() => {const tablesTexts = []; const tables = document.querySelectorAll('#notificaciones > div > div > div').forEach(table => { tablesTexts.push(table.innerText)}); return tablesTexts;}");

            
            foreach (var tableText in tableTexts)
            {
                var jurisdiction = Regex.Match(tableText, @"(?<=Organismo:)(.*?)(?=\n)").Value;
                var caption = Regex.Match(tableText, @"(?<=Carátula:)(.*?)(?=\n|- Número)").Value.Trim();
                var caseNumber = Regex.Match(tableText, @"(?<=Número:)(.*?)(?=\n|-)").Value.Trim();
                var Destinatario = Regex.Match(tableText, @"(?<=Destinatario:)(.*?)(?=\n)").Value;
                var Domicilio = Regex.Match(tableText, @"(?<=Domicilio:)(.*?)(?=\n)").Value;
                var Alta = Regex.Match(tableText, @"(?<=Alta o Disponibilidad:)(.*?)(?=\n|- Notifica)").Value.Trim();
                var Notificacion = Regex.Match(tableText, @"(?<=Notificación:)(.*?)(?=\n)").Value;
                var Tramite = Regex.Match(tableText, @"(?<=Trámite:)(.*?)(?=\n)").Value;
                var Codigo = Regex.Match(tableText, @"(?<=Código:)(.*?)(?=\n)").Value;
                legalCase = new LegalCase()
                {
                    UserId = userId,
                    CaseNumber = caseNumber,
                    Caption = caption,
                    Jurisdiction = jurisdiction,
                    CreatedAt = DateTime.ParseExact(Alta, "d/MM/yyyy HH:mm:ss", customCulture), // "d/MM/yyyy HH:mm:ss"
                    LastScrappedAt = DateTime.Now
                };
                legalCases.Add(legalCase);
            }

            hrefNotificationStringArray.AddRange(hrefNotifications);
            for (int i = 1; i < totalPages; i++)
            {
                await _puppeteerService.ExecuteJs("buscar", (i+1).ToString());
                await _puppeteerService.Wait(int.Parse(_configuration["timeout"]) / 6);
                // todo make this
                hrefNotifications = await _puppeteerService.GetStringArray("() => {const elements = []; document.querySelectorAll('.btn.btn-sm.btn-success.boton100.Detalle').forEach(function(element) {elements.push(element.href)}); return elements ; }");
                hrefNotificationStringArray.AddRange(hrefNotifications);


                tableTexts = await _puppeteerService.GetStringArray("() => {const tablesTexts = []; const tables = document.querySelectorAll('#notificaciones > div > div > div').forEach(table => { tablesTexts.push(table.innerText)}); return tablesTexts;}");


                foreach (var tableText in tableTexts)
                {
                    var jurisdiction = Regex.Match(tableText, @"(?<=Organismo:)(.*?)(?=\r\n)").Value;
                    var caption = Regex.Match(tableText, @"(?<=Carátula:)(.*?)(?=\r\n|- Número)").Value.Trim();
                    var caseNumber = Regex.Match(tableText, @"(?<=Número:)(.*?)(?=\r\n|-)").Value.Trim();
                    var Destinatario = Regex.Match(tableText, @"(?<=Destinatario:)(.*?)(?=\r\n)").Value;
                    var Domicilio = Regex.Match(tableText, @"(?<=Domicilio:)(.*?)(?=\r\n)").Value;
                    var Alta = Regex.Match(tableText, @"(?<=Alta o Disponibilidad:)(.*?)(?=\r\n|- Notifica)").Value.Trim();
                    var Notificacion = Regex.Match(tableText, @"(?<=Notificación:)(.*?)(?=\r\n)").Value;
                    var Tramite = Regex.Match(tableText, @"(?<=Trámite:)(.*?)(?=\r\n)").Value;
                    var Codigo = Regex.Match(tableText, @"(?<=Código:)(.*?)(?=\r\n)").Value;
                    legalCase = new LegalCase()
                    {
                        UserId = 1,
                        CaseNumber = caseNumber,
                        Caption = caption,
                        Jurisdiction = jurisdiction,
                        CreatedAt = DateTime.ParseExact(Alta, "d/MM/yyyy HH:mm:ss", customCulture), // "d/MM/yyyy HH:mm:ss"
                        LastScrappedAt = DateTime.Now
                    };
                    legalCases.Add(legalCase);
                }


            }

            foreach (var item in hrefNotificationStringArray)
            {
                await _puppeteerService.GoToUrl(item, 0);
                await _puppeteerService.Wait(int.Parse(_configuration["timeout"]) / 2);
                await _puppeteerService.ClickSelector("#lnkVerCausa", "#numeroCausa");
                await _puppeteerService.Wait(int.Parse(_configuration["timeout"]) *2);

                var numeroCausa = await _puppeteerService.GetPropertyWithSelector("#numeroCausa", "innerText");
                var scarpURL = await _puppeteerService.GetString("() => {return window.location.href;}");
                if(numeroCausa != null)
                {
                    var legalcaseToUpdate = legalCases.Find(x => numeroCausa.Contains(x.CaseNumber));
                    if (legalcaseToUpdate != null)
                    {
                        legalCases.Remove(legalcaseToUpdate);
                        legalcaseToUpdate.ScrapUrl = scarpURL;
                        legalcaseToUpdate.CaseNumber = numeroCausa;
                        legalcaseToUpdate = _legalCaseDataAccessAsync.AddOrUpdate(legalcaseToUpdate);
                        // update the legal case with the one from the db
                        legalCases.Add(legalcaseToUpdate);
                    }   
                }
            }
            return legalCases;
        }



        #endregion Private Methods
    }
}
