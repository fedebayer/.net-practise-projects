using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCIntegrador2.Models
{
    public class LocalidadModel
    {
        public int Id { get; set; }
        [Display(Name = "Localidad", ResourceType = typeof(Recursos.Global))]
        [Required(ErrorMessage = "El campo Localidad es obligatorio.")]
        [StringLength(100, ErrorMessage = "El campo Localidad no puede tener mas de 100 caracteres.")]
        public string Localidad { get; set; }
        [Display(Name = "Provincia", ResourceType = typeof(Recursos.Global))]
        [Required(ErrorMessage = "El campo Provincia es obligatorio.")]
        public int IdProvincia { get; set; }

        public LocalidadModel Provincia { get; set; }
    }
}