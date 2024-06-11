using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCApplication.Models
{
    public class DatosPopup
    {
        public bool InSitu { get; set; }
        public bool Tercerizado { get; set; }

        public IList<TipoTrabajo> TiposTrabajo { get; set; }
        public int IdTipoTrabajo { get; set; }
    }

    public class TipoTrabajo
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
    }
}