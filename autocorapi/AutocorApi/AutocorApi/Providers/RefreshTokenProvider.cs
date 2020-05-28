using AutocorApi.Servicios.Core;
using AutocorApi.Servicios.Dto;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Infrastructure;
using Ninject;
using Ninject.Web.Common;
using System;
using System.Threading.Tasks;

namespace AutocorApi.Providers
{
    public class RefreshTokenProvider : IAuthenticationTokenProvider
    {
        //private static ConcurrentDictionary<string, AuthenticationTicket> _refreshTokens = new ConcurrentDictionary<string, AuthenticationTicket>();
        private readonly IServicioToken _srvToken;
        private static readonly IKernel kernel = (new Bootstrapper()).Kernel;

        public RefreshTokenProvider()
        {
            //IKernel kernel = (new Bootstrapper()).Kernel;
            _srvToken = kernel.Get<IServicioToken>();
        }

        public void Create(AuthenticationTokenCreateContext context)
        {
            throw new NotImplementedException();
        }

        public void Receive(AuthenticationTokenReceiveContext context)
        {
            throw new NotImplementedException();
        }

        public Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            var clientId = context.Ticket.Properties.Dictionary["as:client_id"];
            var userId = context.Ticket.Properties.Dictionary["user_id"];
            var guid = Guid.NewGuid().ToString("n");

            var refreshTokenLifeTime = context.OwinContext.Get<string>("as:clientRefreshTokenLifeTime");

            var token = new RefreshTokenDto
            {
                Id = _srvToken.HashGuid(guid),
                ClientId = clientId,
                IssuedUTC = DateTime.UtcNow,
                ExpiresUTC = DateTime.UtcNow.AddDays(int.Parse(refreshTokenLifeTime)),
                IdUsuario = int.Parse(userId)
            };

            // copiar propiedades y setear la duración del refresh token
            var refreshTokenProperties = new AuthenticationProperties(context.Ticket.Properties.Dictionary)
            {
                IssuedUtc = token.IssuedUTC,
                ExpiresUtc = token.ExpiresUTC
            };

            var refreshTokenTicket = new AuthenticationTicket(context.Ticket.Identity, refreshTokenProperties);
            //_refreshTokens.TryAdd(guid, refreshTokenTicket);

            token.ProtectedTicket = context.SerializeTicket();

            _srvToken.RegistrarToken(token);

            context.SetToken(guid);

            return Task.FromResult(0);
        }

        public Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            string hashedTokenId = _srvToken.HashGuid(context.Token);
            var refreshToken = _srvToken.ObtenerTokenPorId(hashedTokenId);

            if (refreshToken != null)
            {
                context.DeserializeTicket(refreshToken.ProtectedTicket);
                _srvToken.EliminarToken(hashedTokenId);
            }

            return Task.FromResult(0);
        }
    }
}