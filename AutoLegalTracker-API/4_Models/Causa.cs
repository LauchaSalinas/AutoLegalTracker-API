using Microsoft.AspNetCore.Mvc;
using System;
namespace AutoLegalTracker_API.Models
{
    public class Causa
    {
        
        public List<ITramite> TramiteList = new();

        public string Caratula { get; set; }
        public uint NumDeCausa { get; set; }
        public string Juzgado { get; set; }
        public string Url { get; set; }

        public Causa()
        {
            this.Caratula = string.Empty;
            this.NumDeCausa = 0;
            this.Juzgado = string.Empty;
            this.Url = string.Empty;
        }
    }

    public abstract class ITramite
    {

        public string Hipervinculo { get; set; }
        public string Parrafo { get; set; }

    }

    public class Notificacion : ITramite
    {
        public Notificacion()
        {
            Hipervinculo = string.Empty;
            Parrafo = string.Empty;
            Tipo = string.Empty;
        }
        public string Tipo { get; set; }
    }

    public class Presentacion : ITramite
    {
        public Presentacion()
        {
            Hipervinculo = string.Empty;
            Parrafo = string.Empty;
            Tipo = string.Empty;
            Titulo = string.Empty;
        }
        public string Titulo { get; set; }
        public string Tipo { get; set; }
    }

    public class Tramite : ITramite
    {
        public Tramite()
        {
            Hipervinculo = string.Empty;
            Parrafo = string.Empty;
            Observacion = string.Empty;
        }
        public string Observacion { get; set; }
    }

}

