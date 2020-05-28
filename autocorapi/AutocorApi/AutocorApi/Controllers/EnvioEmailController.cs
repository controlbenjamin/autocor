using System.Web.Http;
using AutocorApi.Servicios.Core;
using AutocorApi.Servicios.Dto;
using AutocorApi.Servicios.Email;

namespace AutocorApi.Controllers
{
    [RoutePrefix("api/envioemail")]
    public class EnvioEmailController : ApiController
    {
        private IServicioEmail servicioEmail;
        private IServicioClientes servicioClientes;

        public EnvioEmailController(IServicioEmail servicioEmail, IServicioClientes servicioClientes)
        {
            this.servicioEmail = servicioEmail;
            this.servicioClientes = servicioClientes;
        }

        [HttpPost, Route("consulta")]
        public IHttpActionResult Consulta(ConsultaDto consulta)
        {
            if (consulta == null)
                return BadRequest("No data");

            var res = servicioEmail.EnviarEmailConsulta(consulta);

            if (!res) return InternalServerError();

            return Ok();
        }

        [HttpPost, Route("registro")]
        public IHttpActionResult Registro(RegistroDto registro)
        {
            if (registro == null)
                return BadRequest("No data");

            var res = servicioEmail.EnviarEmailRegistro(registro);

            if (!res) return InternalServerError();

            return Ok();
        }

        [HttpPost, Route("iniciosesionescritorio")]
        public IHttpActionResult InicioSesionEscritorio(InicioSesionDto inicioSesion, bool notificar = false)
        {
            if (inicioSesion == null)
                return BadRequest("No data");

            if (inicioSesion.TipoCatalogo != InicioSesionDto.TipoCatalogo_Escritorio)
                ModelState.AddModelError("TipoCatalogo", "Tipo de catálogo no válido");

            var cliente = servicioClientes.BuscarPorNumero(inicioSesion.CodigoCliente);

            if (cliente == null)
                ModelState.AddModelError("", "Cliente no válido");

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var res = servicioEmail.EnviarEmailInicioSesionCatalogoEscritorio(cliente, inicioSesion, notificar);

            if (!res) return InternalServerError();

            return Ok();
        }
    }
}