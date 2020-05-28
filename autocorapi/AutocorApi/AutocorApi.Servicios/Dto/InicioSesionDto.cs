using System;

namespace AutocorApi.Servicios.Dto
{
    public class InicioSesionDto
    {
        public const string TipoCatalogo_Web = "W";
        public const string TipoCatalogo_Escritorio = "E";
        public const string TipoCatalogo_Mobile = "M";

        public int CodigoCliente { get; set; }
        public DateTime Fecha { get; set; }
        public string TipoCatalogo { get; set; }
        public string Email { get; set; }

        public string NombrePC { get; set; }
        public string UsuarioPC { get; set; }

        public int? CodigoClienteAnterior { get; set; }
    }
}