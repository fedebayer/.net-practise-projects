using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCIntegrador1.Models
{
    public class TipoDocumentoModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo TipoDoc es obligatorio.")]
        [StringLength(15, ErrorMessage = "El campo Nombre no puede tener mas de 15 caracteres.")]
        public string TipoDoc { get; set; }
    }
}