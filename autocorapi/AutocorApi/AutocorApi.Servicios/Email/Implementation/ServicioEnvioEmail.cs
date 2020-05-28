using System;
using System.Configuration;
using System.Linq;
using System.Text;
using AutocorApi.Servicios.Dto;
using AutocorApi.Servicios.Dto.Clientes;

namespace AutocorApi.Servicios.Email.Implementation
{
    public class ServicioEnvioEmail : IServicioEmail
    {
        private IEnviadorEmail enviadorEmail;
        private string emailInterno;    // es el email dónde van a recibir los correos los de Autocor
        private string emailInternoWeb; // es el email dónde van a recibir los correos del sitio web

        public ServicioEnvioEmail(IEnviadorEmail enviadorEmail)
        {
            this.enviadorEmail = enviadorEmail;

            this.EmailEnabled = ConfigurationManager.AppSettings["email_enabled"] == "true";
            this.emailInterno = ConfigurationManager.AppSettings["email_interno"];
            this.emailInternoWeb = ConfigurationManager.AppSettings["email_interno_web"];

            if (string.IsNullOrEmpty(emailInterno))
                throw new Exception("Email interno no inicializado");
        }

        public bool EmailEnabled { get; private set; }

        public bool EnviarEmailAltaClienteWeb(AltaClienteWebDto altaCliente)
        {
            string asunto = $"Solicitud de cuenta";

            string cuerpo = $@"
                Nueva solicitud de Cuenta Corriente -> <strong>Raz&oacute;n Social: {altaCliente.RazonSocial}</strong><br><br>
                Nombre de fantas&iacute;a: {altaCliente.NombreFantasia}<br><br>
                Tel&eacute;fono: {altaCliente.Telefono}<br><br>
                Celular: {altaCliente.Cecular}<br><br>
                Calle: {altaCliente.Calle} - N&#176;: {altaCliente.NumeroDomicilio} - Barrio: {altaCliente.Barrio} - CP: {altaCliente.CodigoPostal}<br><br>
                Localidad: {altaCliente.Localidad}<br><br>
                Provincia: {altaCliente.Provincia}<br><br>
                Email: {altaCliente.Email}<br><br>
                Fax: {altaCliente.Fax}<br><br>
                <hr />
                <br>
                Condici&oacute;n IVA: {altaCliente.CondicionIva}<br><br>
                CUIT: {altaCliente.CUIT}<br><br>
                Ingresos Brutos: {altaCliente.IngresosBrutos}<br><br>
                Transporte Solicitado: {altaCliente.TransporteSolicitado}<br><br>
                Transporte Alternativo: {altaCliente.TransporteSolicitado ?? "-"}<br><br>
                <hr />
                <br>
                Marcas que comercializa: {altaCliente.MarcasComercializa}<br><br>
                Rubros que comercializa: {altaCliente.RubrosComercializa}<br><br>
                <hr />
                <br>
                <strong>ENCARGADOS DE LA EMPRESA</strong><br><br>
                <strong>Encargado 1:</strong><br>
                Apellido y Nombre Encargado 1: {altaCliente.Encargado1NombreCompleto}<br><br>
                <strong>Encargado 2:</strong><br>
                Apellido y Nombre Encargado 2: {altaCliente.Encargado2NombreCompleto ?? "-"}<br><br>
                <hr /><br>";

            if (altaCliente.ReferenciasComerciales.Count() > 0)
            {
                cuerpo += "<strong>REFERENCIAS COMERCIALES</strong><br><br>";

                int numero = 1;

                foreach (var referencia in altaCliente.ReferenciasComerciales)
                {
                    cuerpo += $@"<strong>Empresa {numero}:</strong><br>
                                Empresa Referencia {numero}: {referencia}<br><br>";
                }
            }

            cuerpo += "<hr /><br>";

            var mensaje = new MensajeEmailBase(cuerpo);

            return EnviarEmail(emailInterno, asunto, mensaje.GenerarMensaje());
        }

        public bool EnviarEmailRestaurarClave(string emailReset)
        {
            string asunto = $"Solicitud de restaruacion de clave";

            string cuerpo = $@"el usuario del mail " + emailReset + " ah solicitado una restauracion de su clave";
               
            var mensaje = new MensajeEmailBase(cuerpo);

            return EnviarEmail(emailInterno, asunto, mensaje.GenerarMensaje());
        }

        public bool EnviarEmailConsulta(ConsultaDto consulta)
        {
            string asunto = $"Catálogo - Consulta: {consulta.Asunto}";

            string cuerpo = $@"
                <table>
                    <tbody>
                        <tr>
                            <th style=""text-align:left"">Nombre</th>
                            <td>{consulta.Nombre}</td>
                        </tr>
                        <tr>
                            <th style=""text-align:left"">Teléfono</th>
                            <td>({consulta.TelefonoCaracteristica}) {consulta.TelefonoNumero}</td>
                        </tr>
                        <tr>
                            <th style=""text-align:left"">Mensaje</th>
                            <td>{consulta.Mensaje}</td>
                        </tr>
                    </tbody>
                </table>";

            var mensaje = new MensajeEmail(cuerpo);
            mensaje.AgregarTitulo("Consulta");

            return EnviarEmail(emailInterno, asunto, mensaje.GenerarMensaje());
        }

        public bool EnviarEmailConsultaSitioWeb(ConsultaWebDto consulta)
        {
            string asunto = "Nueva consulta SITIO WEB";

            string cuerpo = $@"
                <table>
                    <tbody>
                        <tr>
                            <th style=""text-align:left"">Nombre</th>
                            <td>{consulta.Nombre}</td>
                        </tr>
                         <tr>
                            <th style=""text-align:left"">Email</th>
                            <td>{consulta.Email}</td>
                        </tr>
                        <tr>
                            <th style=""text-align:left"">Teléfono</th>
                            <td>{consulta.Telefono}</td>
                        </tr>
                        <tr>
                            <th style=""text-align:left"">Localidad</th>
                            <td>{consulta.Localidad}</td>
                        </tr>
                        <tr>
                            <th style=""text-align:left"">Mensaje</th>
                            <td>{consulta.Mensaje}</td>
                        </tr>
                    </tbody>
                </table>";

            var mensaje = new MensajeEmail(cuerpo);
            mensaje.AgregarTitulo("Consulta Sitio Web");

            return EnviarEmail(emailInternoWeb, asunto, mensaje.GenerarMensaje());
        }

        public bool EnviarEmailInicioSesionCatalogoEscritorio(ClienteDto cliente, InicioSesionDto inicioSesion, bool notificiarAutocor)
        {
            bool res = false;

            string asunto = "Autocor - Inicio de sesión";

            // enviar email al cliente
            string cuerpo = string.Format("Hemos registrado un nuevo inicio de sesión en nuestro catálogo con el número de cuenta <strong>{0}</strong>", inicioSesion.CodigoCliente);

            cuerpo += $@"
<table style=""margin-top:20px"">
    <tr style=""font-size:1.3rem;"">
        <th style=""text-align:left;background-color:#066938;color:#FFFFFF;font-weight:normal;padding:0 5px;height:30px;border-radius:5px"">
            <div style=""background-color:white;display:inline-block;height:30px;border-radius:5px"">
                <img src=""http://www.autocor.com.ar/serviciosWeb/images/calendar.png"" style=""height:30px;margin:0 0 -5px -5px;"">
            </div>
            FECHA
         </th>
         <td style=""padding-left:10px;color:#000000"">{inicioSesion.Fecha:dd/MM/yyyy 'a las' HH:mm}</td>
    </tr>

    <tr style=""font-size:1.3rem;"">
        <th style=""text-align:left;background-color:#066938;color:#FFFFFF;font-weight:normal;padding:0 5px;height:30px;border-radius:5px"">
            <div style=""background-color:white;display:inline-block;height:30px;border-radius:5px"">
                <img src=""http://www.autocor.com.ar/serviciosWeb/images/email.png"" style=""height:30px;margin:0 0 -5px -5px"">
            </div>
            EMAIL
        </th>
       <td style=""padding-left:10px;color:#000000""> {inicioSesion.Email} </td>
    </tr>

    <tr style=""font-size:1.3rem;"">
        <th style=""text-align:left;background-color:#066938;color:#FFFFFF;font-weight:normal;padding:0 5px;height:30px;border-radius:5px"">
            <div style=""background-color:white;display:inline-block;height:30px;border-radius:5px"">
                <img src=""http://www.autocor.com.ar/serviciosWeb/images/display.png"" style=""height:30px;margin:0 0 -5px -5px"">
            </div>
            NOMBRE PC
        </th>
        <td style=""padding-left:10px;color:#000000""> {inicioSesion.NombrePC} </td>
    </tr>
</table>";

            string tituloCliente = $"Hola <strong>{cliente.RazonSocial}</strong>";

            string footerCliente = @"
                  <p>
                    Si ha sido usted desestime este mensaje. Caso contrario póngase en contacto con nosotros al [0351] 4731941 o vía e-mail a
                    <a href=""mailto: info @autocor.com.ar"">info@autocor.com.ar</a>
                  </p>";

            var mensajeCliente = new MensajeEmail(cuerpo);
            mensajeCliente.AgregarTitulo(tituloCliente);
            mensajeCliente.AgregarFooter(footerCliente);

            res = enviadorEmail.EnviarEmail(inicioSesion.Email, asunto, mensajeCliente.GenerarMensaje());

            // enviar email a Autocor
            if (notificiarAutocor)
            {
                asunto = $"Catálogo - Login de cliente: {inicioSesion.CodigoCliente}";

                cuerpo = $@"
                 Cliente: {cliente.ToString()}  <br/>
                 E-mail: {inicioSesion.Email}   <br/>
                 Fecha: {inicioSesion.Fecha:dd/MM/yyyy 'a las' HH:mm}
                               <br/><br/>
                 PC: {inicioSesion.NombrePC}       <br/>
                 Usuario: {inicioSesion.UsuarioPC}";

                // si cambió de usuario...
                if (inicioSesion.CodigoClienteAnterior.HasValue)
                {
                    asunto = $"Catálogo - Cambio de usuario: {inicioSesion.CodigoCliente}";
                    cuerpo += $"<br/><br/>El cliente hizo un cambio de usuario desde el cliente <strong>{inicioSesion.CodigoCliente}</strong> al <strong>{inicioSesion.CodigoClienteAnterior}</strong>.";
                }

                var mensajeAutocor = new MensajeEmail(cuerpo);
                mensajeAutocor.AgregarTitulo($"Nuevo inicio de sesión del cliente {inicioSesion.CodigoCliente}");

                res = res && enviadorEmail.EnviarEmail(emailInterno, asunto, mensajeAutocor.GenerarMensaje());
            }

            return res;
        }

        public bool EnviarEmailRegistro(RegistroDto registro)
        {
            string asunto = "Catálogo - Nuevo Cliente";

            var sb = new StringBuilder();

            sb.Append($@"
                <table>
                    <tbody>
                        <tr>
                            <th style=""text-align:left"">Nombre de Fantasía</th>
                            <td>{registro.Nombre}</td>
                        </tr>
                        <tr>
                            <th style=""text-align:left"">Titular</th>
                            <td>{registro.Titular}</td>
                        </tr>
                        <tr>
                            <th style=""text-align:left"">CUIT</th>
                            <td>{registro.CUIT}</td>
                        </tr>
                        <tr>
                            <th style=""text-align:left"">Cat. AFIP</th>
                            <td>{registro.TipoIVA}</td>
                        </tr>
                        <tr>
                            <th style=""text-align:left"">Ing. Brutos</th>
                            <td>{registro.IngresosBrutos}</td>
                        </tr>
                        <tr>
                            <th style=""text-align:left"">Domicilio</th>
                            <td>{registro.DomicilioComercial}</td>
                        </tr>
                        <tr>
                            <th style=""text-align:left"">Cod. Postal</th>
                            <td>{registro.CodigoPostal}</td>
                        </tr>
                        <tr>
                            <th style=""text-align:left"">Localidad</th>
                            <td>{registro.Localidad}</td>
                        </tr>
                        <tr>
                            <th style=""text-align:left"">Provincia</th>
                            <td>{registro.Provincia}</td>
                        </tr>
                        <tr>
                            <th style=""text-align:left"">Teléfono</th>
                            <td>{registro.Telefono}</td>
                        </tr>
                        <tr>
                            <th style=""text-align:left"">Ceular</th>
                            <td>{registro.Celular}</td>
                        </tr>
                        <tr>
                            <th style=""text-align:left"">E-mail</th>
                            <td>{registro.Email}</td>
                        </tr>
                        <tr>
                            <th style=""text-align:left"">Marcas que comercializa</th>
                            <td>{registro.MarcasComercializa}</td>
                        </tr>
                        <tr>
                            <th style=""text-align:left"">Rubros que comercializa</th>
                            <td>{registro.RubrosComercializa}</td>
                        </tr>
                        <tr>
                            <th style=""text-align:left"">Flete habitual</th>
                            <td>{registro.FleteHabitual}</td>
                        </tr>
                        <tr>
                            <th style=""text-align:left"">Flete alternativo</th>
                            <td>{registro.FleteAlternativo}</td>
                        </tr>
                    </tbody>
                </table>
            ");

            if (registro.Contactos.Count() > 0)
            {
                sb.Append("<br/><h2>Contactos</h2>");

                sb.Append(
                    @"<table>
                        <thead>
                            <tr>
                                <th style=""text-align:left"">Nombre</th>
                                <th style=""text-align:left"">Función</th>
                                <th style=""text-align:left"">Cumple</th>
                            </tr>
                        </thead>
                        <tbody>");

                // listado de contactos
                foreach (var contacto in registro.Contactos)
                {
                    string fechaNacimiento = contacto.FechaNacimiento.HasValue ?
                            contacto.FechaNacimiento.Value.ToShortDateString() : string.Empty;

                    sb.Append(
                        $@"<tr>
                            <td>{contacto.Nombre}</td>
                            <td>{contacto.Funcion}</td>
                            <td>{fechaNacimiento}</td>
                          </tr>");
                }

                sb.Append("</tbody></table>");
            }

            var mensaje = new MensajeEmail(sb.ToString());
            mensaje.AgregarTitulo("Nuevo Cliente");

            return enviadorEmail.EnviarEmail(emailInterno, asunto, mensaje.GenerarMensaje());
        }

        private bool EnviarEmail(string para, string asunto, string mensaje, string cc = null, string bcc = null, string replyTo = null)
        {
            // verificar si está habilitado el envío de emails
            if (!EmailEnabled) return true;

            return enviadorEmail.EnviarEmail(
                para,
                asunto,
                mensaje,
                cc,
                bcc,
                replyTo,
                esHtml: true);
        }
    }
}