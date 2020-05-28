using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Autocor.Catalogo.Web.Models
{
    public class ContactoModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "El Nombre es requerido")]
        public string NombreCliente { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "El E-mail es requerido")]
        public string EmailCliente { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "El Mensaje es requerido")]
        public string Mensaje { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "El Asunto es requerido")]
        public string Asunto { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "La Característica es requerido")]
        public string TelefonoCaracteristica { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "El Teléfono es requerido")]
        public string TelefonoNumero { get; set; }


    }
}
