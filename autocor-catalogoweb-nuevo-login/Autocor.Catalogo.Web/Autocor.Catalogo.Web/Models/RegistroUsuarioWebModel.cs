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

        [Required(AllowEmptyStrings = false, ErrorMessage = "La clave es requerida")]
        public string Clave { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "La clave es requerida")]
        [Compare("Clave" , ErrorMessage = "Las dos claves no son iguales")]
        public string RepetirClave { get; set; }
    }
}