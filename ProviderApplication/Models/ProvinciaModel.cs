using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCIntegrador2.Models
{
    public class ProvinciaModel
    {
        public int Id { get; set; }
        [Display(Name = "Provincia", ResourceType=typeof(Recursos.Global))]
        [Required(ErrorMessage = "El campo Provincia es obligatorio.")]
        [StringLength(100, ErrorMessage = "El campo Provincia no puede tener mas de 100 caracteres.")]
        public string Provincia { get; set; }
    }
}