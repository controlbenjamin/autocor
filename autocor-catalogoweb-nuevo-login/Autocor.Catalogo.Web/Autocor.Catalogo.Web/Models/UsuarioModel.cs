using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Autocor.Catalogo.Web.Models
{
    public class UsuarioModel
    {
        public int Id { get; set; }
        [Required(AllowEmptyStrings =false,ErrorMessage ="El Nro es requerido")]
        public string Nombre { get; set; }
        [Required(AllowEmptyStrings =false,ErrorMessage ="La contraseña es requerida")]
        public string Password { get; set; }
        public bool MantenerSesion { get; set; } = false;
    }
}

