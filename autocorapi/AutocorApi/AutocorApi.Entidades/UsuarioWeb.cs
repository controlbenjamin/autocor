using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutocorApi.Entidades
{
    public class UsuarioWeb
    {
        public int NroCliente { get; set; }
        public string Email { get; set; }
        public string NombreCliente { get; set; }
        public string Password { get; set; }
        public bool? Estado { get; set; }
        public DateTime FechaHora { get; set; }
    }
}
