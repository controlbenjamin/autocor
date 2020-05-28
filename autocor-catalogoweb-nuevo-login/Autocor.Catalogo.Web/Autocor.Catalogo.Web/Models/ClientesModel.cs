using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Autocor.Catalogo.Web.Models
{
    public class ClientesModel
    {
        public int Codigo { get; set; }
        public string CUIT { get; set; }
        public string RazonSocial { get; set; }
    }
}