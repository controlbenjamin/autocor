using Autocor.Catalogo.Web.Models.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static AutocorApi.Servicios.Dto.RegistroDto;

namespace Autocor.Catalogo.Web.Models
{
    public class RegistroModel 
    {
        public RegistroModel()
        {
            ListadoProvincias = new List<string>()
            {
                "Buenos Aires",
                "Catamarca",
                "Ciudad Autónoma de Buenos Aires",
                "Chaco",
                "Chubut",
                "Córdoba",
                "Corrientes",
                "Entre Ríos",
                "Formosa",
                "Jujuy",
                "La Pampa",
                "La Rioja",
                "Mendoza",
                "Misiones",
                "Neuquén",
                "Rio Negro",
                "Salta",
                "San Juan",
                "San Luis",
                "Santa Cruz",
                "Santa Fe",
                "Santiago del Estero",
                "Tierra del Fuego",
                "Tucuman"
            };

            ListadoTipoIva = new List<string>()
            {
              "Monotributo",
              "Responsable Inscripto"
            };
        }

        public string Nombre { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe ingresar un Titular")]
        public string Titular { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe ingresar  CUIT")]
        public string CUIT { get; set; }

        public string TipoIVA { get; set; }

        public string IngresosBrutos { get; set; }

        public string DomicilioComercial { get; set; }

        public string CodigoPostal { get; set; }

        public string Localidad { get; set; }

        public string Provincia { get; set; }
                                                            
        [Required(AllowEmptyStrings = false, ErrorMessage = "La caracteristica y el Teléfono son obligatorios")]
        public string TelCaracteristica { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe ingresar  Teléfono")]
        public string Telefono { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe ingresar Email")]
        [EmailAddress(ErrorMessage = "Debe ingresar un Email válido")]
        public string Email { get; set; }

        public string CelCaracteristica { get; set; }

        public string Celular { get; set; }

        public string MarcasComercializa { get; set; }

        public string RubrosComercializa { get; set; }

        public string FleteHabitual { get; set; }

        public string FleteAlternativo { get; set; }

        //[AsegurarUno(ErrorMessage = "Ingrese al menos un contacto")]
        public IEnumerable<ContactoRegistroDto> Contactos { get; set; }
        
        public IEnumerable<string> ListadoProvincias { get; set; }

        public IEnumerable<string> ListadoTipoIva { get; set; }

    }
}
