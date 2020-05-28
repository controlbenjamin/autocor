using System;

namespace AutocorApi.Servicios.Dto
{
    public enum Rol
    {
        ADMIN,
        CLIENTE,
        CATALOGO,
        VIAJANTE,
    }

    public enum EstadoWeb
    {
        USUARIO_SIN_USUARIO_WEB,
        USUARIO_WEB
    }

    public class UsuarioDto
    {
        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; }
        public string Nombre { get; set; }
        public string Password { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaCreacionUtc { get; set; }
        public Rol Rol{ get; set; }
        public EstadoWeb? EstadoWeb { get; set; }
        public int? CodigoCliente { get; set; }
        public bool? Estado { get; set; }
    }
}