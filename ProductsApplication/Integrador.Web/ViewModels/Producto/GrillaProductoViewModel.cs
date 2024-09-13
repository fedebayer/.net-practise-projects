using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Integrador.Web.ViewModels.Producto
{
    public class GrillaProductoViewModel
    {
        public List<ItemViewModel> Items { get; set; }

        public class ItemViewModel
        {
            public string ID { get; set; }
            public string Descripcion { get; set; }
            public int Precio { get; set; }

            public int IdCategoria { get; set; }

            public string DescripcionCategoria { get; set; }
            public string FechaAlta { get; set; }
            public bool Activo { get; set; }
        }

    }
}