using System.Collections.Generic;
using System.Linq;

namespace AutocorApi.Servicios.Dto
{
    public class AltaClienteWebDto
    {
        public string RazonSocial { get; set; }
        public string NombreFantasia { get; set; }
        public string Telefono { get; set; }
        public string Cecular { get; set; }
        public string Calle { get; set; }
        public string NumeroDomicilio { get; set; }
        public string Barrio { get; set; }
        public string CodigoPostal { get; set; }
        public string Localidad { get; set; }
        public string Provincia { get; set; }
        public string Email { get; set; }
        public string Fax { get; set; }
        public string CondicionIva { get; set; }
        public string CUIT { get; set; }
        public string IngresosBrutos { get; set; }
        public string TransporteSolicitado { get; set; }
        public string TransporteAlternativo { get; set; }
        public string MarcasComercializa { get; set; }
        public string RubrosComercializa { get; set; }
        public string Encargado1NombreCompleto { get; set; }
        public string Encargado2NombreCompleto { get; set; }

        public IEnumerable<string> ReferenciasComerciales { get; set; }

        public AltaClienteWebDto()
        {
            ReferenciasComerciales = Enumerable.Empty<string>();
        }
    }
}