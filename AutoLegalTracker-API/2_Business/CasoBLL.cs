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
        
        public CasoBLL(PuppeteerService puppeteerService, IDataAccesssAsync<Causa> dataAccesss, IDataAccesssAsync<ITramite> dataAccesssITramite)
        {
            _puppeteerService = puppeteerService;
        }
        public async Task checkNewCases()
        {
            var miPagina = await _puppeteerService.inicializeService();
            var controlador = await _puppeteerService.logeoService(miPagina, "http://localhost:7000", "#txtDomicilioElectronico",
                "#txtpass", "20289537679@cme.notificaciones", "YourStrong(!)Password", "#cmdentrar");

            if (controlador != null)
            {
                controlador = await _puppeteerService.clickeoService(miPagina,
                    "#misCausas", ".btn.btn-info.center-block.boton100.letrablanca");

                if (controlador != null)
                {
                    controlador = await _puppeteerService.clickeoService(miPagina,
                      ".btn.btn-info.center-block.boton100.letrablanca", ".btn.btn-sm.btn-success.boton100");

                    if (controlador != null)
                    {
                        var result = await _puppeteerService.stringsService(miPagina, "()=>{"
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
                                url = item
                            };
                            causas.Add(causa);
                        }

                        foreach(var causa in causas)
                        {
                            await _puppeteerService.irService(miPagina, causa.url);
                            causa.numDeCausa = await _puppeteerService.innerUintService(miPagina, "#numeroCausa");
                            causa.caratula = await _puppeteerService.innerStringService(miPagina, "#caratula");
                            causa.juzgado = await _puppeteerService.innerStringService(miPagina, "#organismo");

                            var r = await _puppeteerService.stringsService(miPagina, "()=>{" +
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
                                            hipervinculo = s
                                        };
                                        causa._tramiteList.Add(pres);
                                        break;

                                    case string s when s.Contains("Notificacion"):
                                        Notificacion noti = new()
                                        {
                                            hipervinculo = s
                                        };
                                        causa._tramiteList.Add(noti);
                                        break;

                                    case string s when s.Contains("Tramite"):
                                        Tramite tra = new()
                                        {
                                            hipervinculo = s
                                        };
                                        causa._tramiteList.Add(tra);
                                        break;
                                }
                            }

                            foreach(ITramite i in causa._tramiteList)
                            {
                                await _puppeteerService.irService(miPagina, i.hipervinculo);

                                switch (i)
                                {
                                    case Notificacion notificacion:
                                        
                                        notificacion.tipo = await _puppeteerService.innerStringService(miPagina,
                                            "#ctl00_cph_ucTextoNotificacion_lblTipoNotificacion");
                                        
                                        notificacion.parrafo = await _puppeteerService.innerStringService(miPagina,
                                            "#textoNotificacion table", 1);
                                        
                                        Console.WriteLine("Este tramite es notificacion de tipo {0}." +
                                            "\nSu texto es: {1}", notificacion.tipo, notificacion.parrafo);
                                        break;

                                    case Presentacion presentacion:

                                        presentacion.titulo = await _puppeteerService.innerStringService(miPagina, 
                                            "#ctl00_cph_lblDetalle");

                                        presentacion.tipo = await _puppeteerService.innerStringService(miPagina, 
                                            "#ctl00_cph_lblTipo");

                                        presentacion.parrafo = await _puppeteerService.innerStringService(miPagina, 
                                            "#textoPresentacion table", 1);

                                        Console.WriteLine("Este tramite es presentacion de tipo {0}, con titulo {1}" +
                                            "\nTexto: {2}",
                                            presentacion.tipo, presentacion.titulo, presentacion.parrafo);
                                        break;

                                    case Tramite tramite:

                                        tramite.parrafo = await _puppeteerService.innerStringService(miPagina, 
                                            "#listaNovedades table", 2);

                                        Console.WriteLine("Este es un tramite." +
                                            "\nTexto: {0}", tramite.parrafo);
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
