using System;
using System.Collections.Generic;

namespace AutocorApi.Servicios.Dto
{
    public class RegistroDto
    {
        public class ContactoRegistroDto
        {
                public string Nombre { get; set; }
                public string Funcion { get; set; }
                public DateTime? FechaNacimiento { get; set; }
        }

        public string Nombre { get; set; }
        public string Titular { get; set; }
        public string CUIT { get; set; }
        public string TipoIVA { get; set; }
        public string IngresosBrutos { get; set; }
        public string DomicilioComercial { get; set; }
        public string CodigoPostal { get; set; }
        public string Localidad { get; set; }
        public string Provincia { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string Celular { get; set; }
        public string MarcasComercializa { get; set; }
        public string RubrosComercializa { get; set; }
        public string FleteHabitual { get; set; }
        public string FleteAlternativo { get; set; }

        public IEnumerable<ContactoRegistroDto> Contactos { get; set; }

        public RegistroDto()
        {
            Contactos = new List<ContactoRegistroDto>();
        }
    }
}