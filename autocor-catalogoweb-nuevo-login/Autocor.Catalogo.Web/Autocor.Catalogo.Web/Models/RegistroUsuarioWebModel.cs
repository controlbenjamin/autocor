using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Autocor.Catalogo.Web.Models
{
    public class RegistroUsuarioWebModel
    {
        public int? CodigoCliente { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "El E-mail es requerido")]
        [EmailAddress(ErrorMessage = "Ingrese un E-mail valido")]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "La contraseña es requerida")]
        public string Contraseña { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "La contraseña es requerida")]
        [Compare("Contraseña" , ErrorMessage = "Las dos contraseñas no son iguales")]
        public string RepetirContraseña { get; set; }
    }
}