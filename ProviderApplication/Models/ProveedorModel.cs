using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCIntegrador2.Models
{
    public class ProveedorModel
    {
        public int Id { get; set; }
        [Display(Name = "Nombre", ResourceType = typeof(Recursos.Global))]
        [Required(ErrorMessage = "El campo Nombre es obligatorio.")]
        [StringLength(100, ErrorMessage = "El campo Nombre no puede tener mas de 100 caracteres.")]
        public string Nombre { get; set; }
        [Display(Name = "Domicilio", ResourceType = typeof(Recursos.Global))]
        [Required(ErrorMessage = "El campo Domicilio es obligatorio.")]
        [StringLength(100, ErrorMessage = "El campo Domicilio no puede tener mas de 100 caracteres.")]
        public string Domicilio { get; set; }
        [Display(Name = "Localidad", ResourceType = typeof(Recursos.Global))]
        [Required(ErrorMessage = "El campo Localidad es obligatorio.")]
        public int IdLocalidad { get; set; }
        [Display(Name = "Provincia", ResourceType = typeof(Recursos.Global))]
        [Required(ErrorMessage = "El campo Provincia es obligatorio.")]
        public int IdProvincia { get; set; }

        public LocalidadModel Localidad { get; set; }

        public ProvinciaModel Provincia { get; set; }
    }
}