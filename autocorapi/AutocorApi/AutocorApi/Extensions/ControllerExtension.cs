using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;
using AutocorApi.Providers;
using AutocorApi.Servicios.Dto;

namespace AutocorApi.Extensions
{
    public static class ControllerExtension
    {
        public static ClaimsUsuario GetClaimsUsuario(this ApiController controller)
        {
            return new ClaimsUsuario(GetClaimsIdentity(controller));
        }

        public static bool IsInRol(this ApiController controller, params Rol[] roles)
        {
            var claims = GetClaimsUsuario(controller);
            return claims.IsInRol(roles);
        }

        private static ClaimsIdentity GetClaimsIdentity(ApiController controller)
        {
            return controller.User.Identity as ClaimsIdentity;
        }
    }

    public class ClaimsUsuario
    {
        private IEnumerable<Claim> claims;

        public ClaimsUsuario(ClaimsIdentity claimsIdentity)
        {
            this.claims = claimsIdentity.Claims;

            var claimCodigoCliente = claimsIdentity.Claims.FirstOrDefault(x => x.Type == AppClaims.Cliente);
            this.CodigoCliente = claimCodigoCliente != null ? int.Parse(claimCodigoCliente.Value) : -1;

            var claimZonaViajante = claimsIdentity.Claims.FirstOrDefault(x => x.Type == AppClaims.ZonaViajante);
            this.ZonaViajante = claimZonaViajante != null ? int.Parse(claimZonaViajante.Value) : -1;
        }

        public int IdUsuario => int.Parse(claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);
        public string NombreUsuario => claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value;
        public string NombreCompleto => claims.FirstOrDefault(x => x.Type == ClaimTypes.Surname).Value;
        public Rol Rol => (Rol)Enum.Parse(typeof(Rol), claims.FirstOrDefault(x => x.Type == ClaimTypes.Role).Value);

        public bool IsInRol(params Rol[] roles)
        {
            if (roles == null)
                return false;

            return roles.Contains(Rol);
        }

        public int CodigoCliente { get; private set; }

        public int ZonaViajante { get; private set; }
    }
}

