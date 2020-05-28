using AutocorApi.Extensions;
using AutocorApi.Servicios.Core;
using System.Web.Http;

namespace AutocorApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/auth")]
    public class AuthController : ApiController
    {
        private readonly IServicioToken servicioToken;

        public AuthController(IServicioToken servicioToken)
        {
            this.servicioToken = servicioToken;
        }

        [HttpDelete]
        [Route("refToken/{refreshToken}")]
        public IHttpActionResult EliminarToken(string refreshToken)
        {
            var claims = this.GetClaimsUsuario();
            int idUsuario = claims.IdUsuario;

            var idToken = servicioToken.HashGuid(refreshToken);
            servicioToken.EliminarToken(idToken, idUsuario);

            return Ok();
        }
    }
}