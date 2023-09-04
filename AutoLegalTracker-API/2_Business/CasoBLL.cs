using AutoLegalTracker_API._5_WebServices;
using AutoLegalTracker_API.DataAccess;
using AutoLegalTracker_API.Models;
using AutoLegalTracker_API.Services;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query;
using PuppeteerSharp;
using System.Drawing.Text;

namespace AutoLegalTracker_API._2_Business
{
    public class CasoBLL
    {
        #region Constructor
        private readonly PuppeteerService _puppeteerService;
        private readonly int timeout = 5000;
        private readonly string funcionParaStringArrayObtenerHrefCasos = "() => {" +
            "const a = document.querySelectorAll('.btn.btn-sm.btn-success.boton100');" +
            "const res = [];" +
            "for(let i=0; i<a.length; i++)" +
            "    res.push(a[i].href);" +
            "return res;" +
            "}";
        private readonly string funcionParaStringArrayObtenerHrefItramites = "()=>{" +
            "const a = document.querySelectorAll('.btn.btn-xs.btn-success');" +
            "const res=[];" +
            "for(let i=0; i<a.length; i++)" +
            "    res.push(a[i].href);" +
            "return res;" +
            "}";

        public CasoBLL(PuppeteerService puppeteerService)
        {
            _puppeteerService = puppeteerService;
        }
        #endregion Constructor

        #region Public Methods
        public async Task CheckNewCases()
        {
            try{
                await _puppeteerService.InicializeService();
                
                var controlador = await _puppeteerService.LogIn("http://localhost:7000", "#txtDomicilioElectronico", "#txtpass", "20289537679@cme.notificaciones", "YourStrong(!)Password", "#cmdentrar");
                await _puppeteerService.Wait(timeout);
                controlador = await _puppeteerService.ClickSelector("#misCausas", ".btn.btn-info.center-block.boton100.letrablanca");
                await _puppeteerService.Wait(timeout);
                await _puppeteerService.ClickSelector(".btn.btn-info.center-block.boton100.letrablanca", ".btn.btn-sm.btn-success.boton100");
                await _puppeteerService.Wait(timeout);
                var result = await _puppeteerService.GetStringArray(funcionParaStringArrayObtenerHrefCasos);
                await _puppeteerService.Wait(timeout);
                foreach(var causa in LlenarListaDeCausas(result))
                    {
                        await _puppeteerService.GoToUrl(causa.Url);
                        await CargarCausa(causa);
                        await _puppeteerService.Wait(1000);
                        var r = await _puppeteerService.GetStringArray(funcionParaStringArrayObtenerHrefItramites);
                        foreach (var a in r)
                        {
                            AgregarTramiteALista(causa, a);
                        }

                        foreach(ITramite i in causa.TramiteList)
                        {
                            await _puppeteerService.GoToUrl(i.Hipervinculo);
                            await _puppeteerService.Wait(timeout);
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
            causa.NumDeCausa = await _puppeteerService.GetNumberWithSelector("#numeroCausa");
            causa.Caratula = await _puppeteerService.GetTextWithSelector("#caratula");
            causa.Juzgado = await _puppeteerService.GetTextWithSelector("#organismo");
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

                    notificacion.Tipo = await _puppeteerService.GetTextWithSelector(
                        "#ctl00_cph_ucTextoNotificacion_lblTipoNotificacion");

                    notificacion.Parrafo = await _puppeteerService.GetTextWithSelector(
                        "#textoNotificacion table", 1);

                    Console.WriteLine("Este tramite es notificacion de tipo {0}." +
                        "\nSu texto es: {1}", notificacion.Tipo, notificacion.Parrafo);
                    break;

                case Presentacion presentacion:

                    presentacion.Titulo = await _puppeteerService.GetTextWithSelector("#ctl00_cph_lblDetalle");

                    presentacion.Tipo = await _puppeteerService.GetTextWithSelector("#ctl00_cph_lblTipo");

                    presentacion.Parrafo = await _puppeteerService.GetTextWithSelector(
                        "#textoPresentacion table", 1);

                    Console.WriteLine("Este tramite es presentacion de tipo {0}, con titulo {1}" +
                        "\nTexto: {2}",
                        presentacion.Tipo, presentacion.Titulo, presentacion.Parrafo);
                    break;

                case Tramite tramite:

                    tramite.Parrafo = await _puppeteerService.GetTextWithSelector(
                        "#listaNovedades table", 2);

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
