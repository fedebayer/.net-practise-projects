using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCIntegrador1.Models
{
    public class PersonaModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo Nombre es obligatorio.")]
        [StringLength(100, ErrorMessage = "El campo Nombre no puede tener mas de 100 caracteres.")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El campo Apellido es obligatorio.")]
        [StringLength(100, ErrorMessage = "El campo Apellido no puede tener mas de 100 caracteres.")]
        public string Apellido { get; set; }
        [Required(ErrorMessage = "El campo Edad es obligatorio.")]
        [Range(19, int.MaxValue, ErrorMessage = "El campo Edad debe ser mayor a 18")]
        public int Edad { get; set; }
        [Display(Name = "Tipo de documento")]
        [Required(ErrorMessage = "El campo Id_tipo_doc es obligatorio.")]
        public int Id_tipo_doc { get; set; }
        [Display(Name = "Numero de documento")]
        [Required(ErrorMessage = "El campo Nro_doc es obligatorio.")]
        [MaxLength(15, ErrorMessage = "El campo Nro_doc no puede tener mas de 15 caracteres")]
        public string Nro_doc { get; set; }
        [Required(ErrorMessage = "El campo Email es obligatorio.")]
        [EmailAddress(ErrorMessage = "El campo Email debe ser un correo valido")]
        public string Email { get; set; }

        public TipoDocumentoModel TipoDocumento { get; set; }
    }
}