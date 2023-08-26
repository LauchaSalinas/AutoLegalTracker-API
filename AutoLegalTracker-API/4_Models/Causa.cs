using System;
namespace AutoLegalTracker_API.Models
{
    public class Causa
    {
        
        public List<ITramite> _tramiteList = new();

        public string caratula { get; set; }
        public uint numDeCausa { get; set; }
        public string juzgado { get; set; }
        public string url { get; set; }

    }

    public abstract class ITramite
    {

        public string hipervinculo { get; set; }
        public string parrafo { get; set; }

    }

    public class Notificacion : ITramite
    {
        public string tipo { get; set; }
    }

    public class Presentacion : ITramite
    {
        public string titulo { get; set; }
        public string tipo { get; set; }
    }

    public class Tramite : ITramite
    {
        public string observacion { get; set; }
    }

}

