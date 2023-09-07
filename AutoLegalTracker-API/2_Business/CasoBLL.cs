using AutoLegalTracker_API.WebServices;
using AutoLegalTracker_API.DataAccess;
using AutoLegalTracker_API.Models;
using AutoLegalTracker_API.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Configuration;
using PuppeteerSharp;
using System.Drawing.Text;
using System.Text;

namespace AutoLegalTracker_API.Business
{
    public class CasoBLL
    {
        #region Constructor
        private readonly PuppeteerService _puppeteerService;
        private readonly IConfiguration _configuration;
        
        public CasoBLL(PuppeteerService puppeteerService, IConfiguration configuration)
        {
            _puppeteerService = puppeteerService;
            _configuration = configuration;
        }
        #endregion Constructor

        #region Public Methods
        public async Task CheckNewCases()
        {
            try{
                await _puppeteerService.InicializeService();
                
                var controlador = await _puppeteerService.LogIn(_configuration["urlDeLaPaginaLocalhost"], _configuration["selectorInputUsuario"], _configuration["selectorInputContraseña"], _configuration["contenidoInputUsuario"], _configuration["contenidoInputContraseña"], _configuration["selectorBotonIngresar"]);
                await _puppeteerService.Wait(int.Parse(_configuration["timeout"]));
                controlador = await _puppeteerService.GoToUrl(await _puppeteerService.GetPropertyWithSelector(_configuration["selectorMisCausas"], _configuration["propiedadHipervinculo"], int.Parse(_configuration["posicionMisCausas"])));
                await _puppeteerService.Wait(int.Parse(_configuration["timeout"]));
                await _puppeteerService.ClickSelector(_configuration["selectorBotonMostrarTodasLasCausas"], _configuration["selectorObjetoConfirmacion"]);
                await _puppeteerService.Wait(int.Parse(_configuration["timeout"]));
                var result = await _puppeteerService.GetStringArray(_configuration["funcionParaStringArrayObtenerHrefCasos"]);
                await _puppeteerService.Wait(int.Parse(_configuration["timeout"]));
                foreach(var causa in LlenarListaDeCausas(result))
                    {
                        await _puppeteerService.GoToUrl(causa.Url);
                        await CargarCausa(causa);
                        await _puppeteerService.Wait(int.Parse(_configuration["timeout"]));
                        var r = await _puppeteerService.GetStringArray(_configuration["funcionParaStringArrayObtenerHrefItramites"]);
                        foreach (var a in r)
                        {
                            AgregarTramiteALista(causa, a);
                        }

                        foreach(ITramite i in causa.TramiteList)
                        {
                            await _puppeteerService.GoToUrl(i.Hipervinculo);
                            await _puppeteerService.Wait(int.Parse(_configuration["timeout"]));
                            await CargarTramite(i);
                            Console.ReadLine();
                        }

                    }
            }
            catch(Exception e){
                Console.WriteLine(e.Message);
            }
        }

        public async Task CargarCausa(Causa causa)
        {
            causa.NumDeCausa = await _puppeteerService.GetNumberWithSelector(_configuration["selectorNumeroCausa"]);
            causa.Caratula = await _puppeteerService.GetPropertyWithSelector(_configuration["selectorCaratula"], _configuration["propiedadTexto"]);
            causa.Juzgado = await _puppeteerService.GetPropertyWithSelector(_configuration["selectorOrganismo"], _configuration["propiedadTexto"]);
        }

        public void AgregarTramiteALista(Causa causa, string a)
        {
            switch (a.ToString())
            {
                case string s when s.Contains("Presentacion"):
                    Presentacion pres = new()
                    {
                        Hipervinculo = s
                    };
                    causa.TramiteList.Add(pres);
                    break;

                case string s when s.Contains("Notificacion"):
                    Notificacion noti = new()
                    {
                        Hipervinculo = s
                    };
                    causa.TramiteList.Add(noti);
                    break;

                case string s when s.Contains("Tramite"):
                    Tramite tra = new()
                    {
                        Hipervinculo = s
                    };
                    causa.TramiteList.Add(tra);
                    break;

                default: break;
            }
            return;
        }

        public async Task CargarTramite(ITramite i)
        {
            switch (i)
            {
                case Notificacion notificacion:

                    notificacion.Tipo = await _puppeteerService.GetPropertyWithSelector(
                        _configuration["selectorTipoNotificacion"], _configuration["propiedadTexto"]);

                    notificacion.Parrafo = await _puppeteerService.GetPropertyWithSelector(
                        _configuration["selectorTextoNotificacion"], _configuration["propiedadTexto"], int.Parse(_configuration["posicionTextoNotificacionYPresentacion"]));

                    Console.WriteLine("Este tramite es notificacion de tipo {0}." +
                        "\nSu texto es: {1}", notificacion.Tipo, notificacion.Parrafo);
                    break;

                case Presentacion presentacion:

                    presentacion.Titulo = await _puppeteerService.GetPropertyWithSelector(_configuration["selectorTituloPresentacion"], _configuration["propiedadTexto"]);

                    presentacion.Tipo = await _puppeteerService.GetPropertyWithSelector(_configuration["selectorTipoPresentacion"], _configuration["propiedadTexto"]);

                    presentacion.Parrafo = await _puppeteerService.GetPropertyWithSelector(
                        _configuration["selectorTextoPresentacion"], _configuration["propiedadTexto"], int.Parse(_configuration["posicionTextoNotificacionYPresentacion"]));

                    Console.WriteLine("Este tramite es presentacion de tipo {0}, con titulo {1}" +
                        "\nTexto: {2}",
                        presentacion.Tipo, presentacion.Titulo, presentacion.Parrafo);
                    break;

                case Tramite tramite:

                    tramite.Parrafo = await _puppeteerService.GetPropertyWithSelector(
                        _configuration["selectorTextoTramite"], _configuration["propiedadTexto"], int.Parse(_configuration["posicionTextoTramite"]));

                    Console.WriteLine("Este es un tramite." +
                        "\nTexto: {0}", tramite.Parrafo);
                    break;
            }
        }

        public List<Causa> LlenarListaDeCausas(string[] result)
        {
            List<Causa> causas = new();
            foreach (var item in result)
            {
                Causa causa = new()
                {
                    Url = item
                };
                causas.Add(causa);
            }
            return causas;
        }

        
        #endregion Public Methods
    }
}
