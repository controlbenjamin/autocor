using System;

namespace AutocorApi.Entidades
{
    public enum Rol
    {
        ADMIN,      // administrador
        CLIENTE,    // cliente
        CATALOGO,   // usuario default del catálogo
        VIAJANTE
    }

    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; }
        public string Nombre { get; set; }
        public string Password { get; set; }
        public Rol Rol { get; set; }
        public DateTime? FechaCreacionUtc { get; set; }
        public bool Activo { get; set; }
        public int? CodigoCliente { get; set; }
    }
}