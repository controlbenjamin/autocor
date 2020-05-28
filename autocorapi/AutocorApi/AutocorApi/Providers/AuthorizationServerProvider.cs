using AutocorApi.Servicios.Core;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Ninject;
using Ninject.Web.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AutocorApi.Providers
{
    public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        private readonly IServicioAutenticacion servicioAutenticacion;
        private readonly IServicioUsuarios servicioUsuarios;

        private static readonly IKernel kernel = (new Bootstrapper()).Kernel;

        public AuthorizationServerProvider()
        {
            //IKernel kernel = (new Bootstrapper()).Kernel;
            this.servicioAutenticacion = kernel.Get<IServicioAutenticacion>();
            this.servicioUsuarios = kernel.Get<IServicioUsuarios>();
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            if (context.TryGetFormCredentials(out string clientId, out string clientSecret))
            {
                int refreshTokenLifetime = int.Parse(ConfigurationManager.AppSettings["api_token_refresh_expiration_days"]);
                context.OwinContext.Set<string>("as:clientRefreshTokenLifeTime", refreshTokenLifetime.ToString());
                context.Validated();
            }
            else
            {
                context.SetError("invalid_client", "Client credentials could not be retrieved from the Authorization header");
            }

            return Task.FromResult(0);
        }

        public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var clienteApi = servicioAutenticacion.ObtenerClienteAPI(new Guid(context.ClientId));
            var usuario = servicioAutenticacion.ValidarUsuario(context.UserName, context.Password);

            // validación cliente
            if (clienteApi == null)
            {
                context.SetError("invalid_client", "El cliente no existe.");
                context.Rejected();
                return Task.FromResult(0);
            }

            if (!clienteApi.Activo)
            {
                context.SetError("invalid_client", "El cliente no está activo.");
                context.Rejected();
                return Task.FromResult(0);
            }

            // validación usuario
            if (usuario == null)
            {
                context.SetError("invalid_grant", "El usuario y la password provistos son incorrectos.");
                return Task.FromResult(0);
            }

            if (!usuario.Activo)
            {
                context.SetError("invalid_grant", "Usuario desactivado.");
                return Task.FromResult(0);
            }

            // validación rol de usuario con cliente
            bool rolValido = false;
            foreach (var rol in clienteApi.Roles)
            {
                if (rol == usuario.Rol)
                {
                    rolValido = true;
                    break;
                }
            }

            if (!rolValido)
            {
                context.SetError("invalid_grant", "Usuario no tiene permiso para el cliente.");
                return Task.FromResult(0);
            }

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()));
            identity.AddClaim(new Claim(ClaimTypes.Name, usuario.NombreUsuario));
            identity.AddClaim(new Claim(ClaimTypes.Surname, usuario.Nombre));
            identity.AddClaim(new Claim(ClaimTypes.Role, usuario.Rol.ToString()));

            var propsDic = new Dictionary<string, string>
            {
                { "user_id", usuario.IdUsuario.ToString() },
                { "user_name", context.UserName },
                { "user_full_name", usuario.Nombre },
                { "role", usuario.Rol.ToString() },
                { "as:client_id", context.ClientId ?? string.Empty  }
            };

            switch (usuario.Rol)
            {
                case Servicios.Dto.Rol.CLIENTE:

                    if (usuario.CodigoCliente.HasValue)
                    {
                        identity.AddClaim(new Claim(AppClaims.Cliente, usuario.CodigoCliente.Value.ToString()));
                        propsDic.Add("codigo_cliente", usuario.CodigoCliente.Value.ToString());
                    }

                    break;

                case Servicios.Dto.Rol.VIAJANTE:

                    int zona = servicioUsuarios.ObtenerZonaUsuarioViajante(usuario.IdUsuario);
                    identity.AddClaim(new Claim(AppClaims.ZonaViajante, zona.ToString()));
                    propsDic.Add("zona_viajante", zona.ToString());

                    break;

                default:
                    break;
            }

            var props = new AuthenticationProperties(propsDic); // lo que recibe el cliente de la api en el token

            var ticket = new AuthenticationTicket(identity, props);
            context.Validated(ticket);

            return Task.FromResult(0);
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            // se agregan propiedades extras al token
            foreach (var property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult(0);
        }

        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            var newIdentity = new ClaimsIdentity(context.Ticket.Identity);
            var newTicket = new AuthenticationTicket(newIdentity, context.Ticket.Properties);
            context.Validated(newTicket);
            return Task.FromResult(0);
        }
    }
}