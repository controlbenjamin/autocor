using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutocorApi.Models.Utils
{
    public class AgregarUsuarioWeb
    {
        public int NroCliente { get; set; }
        public string Cuil { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}