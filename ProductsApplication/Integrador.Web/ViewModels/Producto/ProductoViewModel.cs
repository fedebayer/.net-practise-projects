using System.ComponentModel.DataAnnotations;
using System.Security.AccessControl;
using System.Web.Mvc;

namespace Integrador.Web.ViewModels.Producto
{
    public class ProductoViewModel : GridViewModel
    {
        [Display(Name = "Descripcion", ResourceType = typeof(Resources.Global))]
        public string Descripcion { get; set; }

        [Display(Name = "Categoria", ResourceType = typeof(Resources.Global))]
        public string Categoria { get; set; }

        public int Id { get; set; }

    }
}
