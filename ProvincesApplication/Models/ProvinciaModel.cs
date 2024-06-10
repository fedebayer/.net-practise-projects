using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;

namespace MVCApplication.Models
{
    public class ProvinciaModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [StringLength(20)]
        public string Descripcion { get; set; }
    }

    public static class ProvinciaValidations
    {
        public static ValidationResult ValidateId(int id)
        {
            public static ValidationResult IsValidIdProvincia(int value)
            {
                if (value > 0)
                {
                    return ValidationResult.Success;
                }
                return new ValidationResult("El Id de la Provincia debe ser mayor a cero");
            }
        }
    }
}
}