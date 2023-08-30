using AutoLegalTracker_API._5_WebServices;
using AutoLegalTracker_API.DataAccess;
using AutoLegalTracker_API.Models;
using AutoLegalTracker_API.Services;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query;
using System.Drawing.Text;

namespace AutoLegalTracker_API._2_Business
{
    public class CasoBLL
    {
        private readonly PuppeteerService _puppeteerService;
        
        public CasoBLL(PuppeteerService puppeteerService)
        {
            _puppeteerService = puppeteerService;
        }
        public async Task checkNewCases()
        {
            var miPagina = await _puppeteerService.InicializeService();
            var controlador = await _puppeteerService.LogIn(miPagina, "http://localhost:7000", "#txtDomicilioElectronico",
                "#txtpass", "20289537679@cme.notificaciones", "YourStrong(!)Password", "#cmdentrar");

            if (controlador != null)
            {
                controlador = await _puppeteerService.ClickSelector(miPagina,
                    "#misCausas", ".btn.btn-info.center-block.boton100.letrablanca");

                if (controlador != null)
                {
                    controlador = await _puppeteerService.ClickSelector(miPagina,
                      ".btn.btn-info.center-block.boton100.letrablanca", ".btn.btn-sm.btn-success.boton100");

                    if (controlador != null)
                    {
                        var result = await _puppeteerService.GetStringArray(miPagina, "()=>{"
                            + "const a = document.querySelectorAll('.btn.btn-sm.btn-success.boton100');"
                            + "const res=[];"
                            + "for(let i=0; i<a.length; i++)"
                            + "    res.push(a[i].href);"
                            + "return res;"
                            + "}");

                        List<Causa> causas = new();
                        foreach (var item in result)
                        {
                            Causa causa = new()
                            {
                                Url = item
                            };
                            causas.Add(causa);
                        }

                        foreach(var causa in causas)
                        {
                            await _puppeteerService.GoToUrl(miPagina, causa.Url);
                            causa.NumDeCausa = await _puppeteerService.GetNumberWithSelector(miPagina, "#numeroCausa");
                            causa.Caratula = await _puppeteerService.GetTextWithSelector(miPagina, "#caratula");
                            causa.Juzgado = await _puppeteerService.GetTextWithSelector(miPagina, "#organismo");

                            var r = await _puppeteerService.GetStringArray(miPagina, "()=>{" +
                                "const a = document.querySelectorAll('.btn.btn-xs.btn-success');" +
                                "const res=[];" +
                                "for(let i=0; i<a.length; i++)" +
                                "    res.push(a[i].href);" +
                                "return res;" +
                                "}");

                            foreach (var a in r)
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
                                }
                            }

                            foreach(ITramite i in causa.TramiteList)
                            {
                                await _puppeteerService.GoToUrl(miPagina, i.Hipervinculo);

                                switch (i)
                                {
                                    case Notificacion notificacion:
                                        
                                        notificacion.Tipo = await _puppeteerService.GetTextWithSelector(miPagina,
                                            "#ctl00_cph_ucTextoNotificacion_lblTipoNotificacion");
                                        
                                        notificacion.Parrafo = await _puppeteerService.GetTextWithSelector(miPagina,
                                            "#textoNotificacion table", 1);
                                        
                                        Console.WriteLine("Este tramite es notificacion de tipo {0}." +
                                            "\nSu texto es: {1}", notificacion.Tipo, notificacion.Parrafo);
                                        break;

                                    case Presentacion presentacion:

                                        presentacion.Titulo = await _puppeteerService.GetTextWithSelector(miPagina, 
                                            "#ctl00_cph_lblDetalle");

                                        presentacion.Tipo = await _puppeteerService.GetTextWithSelector(miPagina, 
                                            "#ctl00_cph_lblTipo");

                                        presentacion.Parrafo = await _puppeteerService.GetTextWithSelector(miPagina, 
                                            "#textoPresentacion table", 1);

                                        Console.WriteLine("Este tramite es presentacion de tipo {0}, con titulo {1}" +
                                            "\nTexto: {2}",
                                            presentacion.Tipo, presentacion.Titulo, presentacion.Parrafo);
                                        break;

                                    case Tramite tramite:

                                        tramite.Parrafo = await _puppeteerService.GetTextWithSelector(miPagina, 
                                            "#listaNovedades table", 2);

                                        Console.WriteLine("Este es un tramite." +
                                            "\nTexto: {0}", tramite.Parrafo);
                                        break;
                                }
                                Console.ReadLine();

                            }

                        }
                    }
                }
            }

            Console.WriteLine("Error de logeo.");
        }

    }
}
