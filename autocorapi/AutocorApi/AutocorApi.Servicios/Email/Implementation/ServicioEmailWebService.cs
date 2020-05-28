using System;
using System.Linq;
using System.Text;
using AutocorApi.Servicios.Core;
using AutocorApi.Servicios.Dto;

namespace AutocorApi.Servicios.Email.Implementation
{
    [Obsolete]
    public class ServicioEmailWebService 
    {
        private class MensajeEmailAutocor
        {
            private const string SECCION_TITULO = "{{TITULO}}";
            private const string SECCION_CUERPO = "{{CUERPO}}";
            private const string SECCION_FOOTER = "{{FOOTER}}";

            private static readonly string _template;
            private static readonly string _divisor;

            private string titulo;
            private string cuerpo;
            private string footer;

            static MensajeEmailAutocor()
            {
                _divisor = @"<div class=""contenedor__divisor"" style="" margin: 10px 0 9px 0; border-bottom: 1px solid #066938;""></div>";
                _template = @"
<!DOCTYPE html>
<html>
<head>
    <title>Email</title>
    <link href=""https://fonts.googleapis.com/css?family=Roboto+Condensed:400,700"" rel=""stylesheet"">
    <style>
        @media(max-width: 395px)
        {
            .hidden-xs {
                display: none;
            }
        }
    </style>
</head>

<body style=""background-color: #066938; color: #ffffff; font-family: 'Roboto Condensed', sans-serif;"">

<table style=""margin: auto; max-width: 600px;"">
    <tr>
    <td width=""100%"" height=""100%"" bgcolor=""#066938"">
    <div id=""main"" >

        <header style=""min-width: 395px;"">
            <img src=""http://www.autocor.com.ar/serviciosWeb/images/autocor-email-header-logo.png"" alt=""Autocor"" style=""float:left"">
            <img src=""http://www.autocor.com.ar/serviciosWeb/images/autocor-email-header-icon.png"" alt=""E-mail"" style=""float:right"" class=""hidden-xs"">
            <div class=""clearfix"" style=""clear: both;""></div>
        </header>

        <div class=""contenedor"" style="" background-color: #FFFFFF; color: #212121; padding: 15px 25px; border-radius: 10px;"">

            {{TITULO}}

            {{CUERPO}}

            {{FOOTER}}

        </div>

        <footer style=""margin-top: 10px; text-align: center; font-size: 1.1em; font-weight: 600;"">
            <a href=""http://www.autocor.com.ar"" class=""unstyled"" target=""_blank"" style=""text-decoration: none; color: inherit;"">www.autocor.com.ar</a>
        </footer>
    </div>

    </td>
    </tr>

</table>

</body>
</html>";
            }

            public MensajeEmailAutocor(string cuerpoMensaje)
            {
                AgregarCuerpo(cuerpoMensaje);
            }

            public MensajeEmailAutocor()
            {
            }

            public void AgregarCuerpo(string cuerpo)
            {
                this.cuerpo = string.Format(@"
                <div class=""contenedor__body"" style=""font-size: 1.4em;"">
                    <p>{0}</p>
                </div>", cuerpo);
            }

            public void AgregarTitulo(string titulo)
            {
                this.titulo = string.Format(@"
                <div class=""contenedor__titulo"" style=""font-size: 1.6rem;"">{0}</div>", titulo);
            }

            public void AgregarFooter(string footer)
            {
                this.footer = string.Format(@"
                <div class=""contenedor__footer"" style=""font-size: 1em;"">{0}</div>", footer);
            }

            public string GenerarMensaje()
            {
                StringBuilder sb = new StringBuilder(_template);

                // título
                if (!string.IsNullOrEmpty(titulo))
                {
                    sb.Replace(SECCION_TITULO, titulo + _divisor);
                }
                else
                {
                    sb.Replace(SECCION_TITULO, string.Empty);
                }

                // cuerpo
                sb.Replace(SECCION_CUERPO, cuerpo);

                // footer
                if (!string.IsNullOrEmpty(footer))
                {
                    sb.Replace(SECCION_FOOTER, _divisor + footer);
                }
                else
                {
                    sb.Replace(SECCION_FOOTER, string.Empty);
                }

                return sb.ToString();
            }
        }

        //private const string EMAIL_AUTOCOR = "autocor@autocor.com.ar";
        private const string EMAIL_AUTOCOR = "rodrigo.carrion.e2@gmail.com";

        private IServicioClientes servicioClientes;

        public ServicioEmailWebService(IServicioClientes servicioClientes)
        {
            this.servicioClientes = servicioClientes;
        }

        public bool EnviarEmailConsulta(ConsultaDto consulta)
        {
            string asunto = string.Format("Catálogo - Consulta: {0}", consulta.Asunto);

            string cuerpo = string.Format(@"
                <table>
                    <tbody>
                        <tr>
                            <th style=""text-align:left"">Nombre</th>
                            <td>{0}</td>
                        </tr>
                        <tr>
                            <th style=""text-align:left"">Teléfono</th>
                            <td>({1}) {2}</td>
                        </tr>
                        <tr>
                            <th style=""text-align:left"">Mensaje</th>
                            <td>{3}</td>
                        </tr>
                    </tbody>
                </table>", consulta.Nombre, consulta.TelefonoCaracteristica, consulta.TelefonoNumero, consulta.Mensaje);

            var mensaje = new MensajeEmailAutocor(cuerpo);
            mensaje.AgregarTitulo("Consulta");

            return EnviarEmailWS(mensaje.GenerarMensaje(), consulta.Email, asunto, EMAIL_AUTOCOR);
        }

        public bool EnviarEmailInicioSesionCatalogoEscritorio(InicioSesionDto inicioSesion, bool notificiarAutocor)
        {
            bool res = false;

            // enviar email al cliente
            var cliente = servicioClientes.BuscarPorNumero(inicioSesion.CodigoCliente);
            string cuerpoCliente = string.Format("Hemos registrado un nuevo inicio de sesión en nuestro catálogo con el número de cuenta <strong>{0}</strong>", inicioSesion.CodigoCliente);

            cuerpoCliente += string.Format(@"
<table style=""margin-top:20px"">
    <tr style=""font-size:1.3rem;"">
        <th style=""text-align:left;background-color:#066938;color:#FFFFFF;font-weight:normal;padding:0 5px;height:30px;border-radius:5px"">
            <div style=""background-color:white;display:inline-block;height:30px;border-radius:5px"">
                <img src=""http://www.autocor.com.ar/serviciosWeb/images/calendar.png"" style=""height:30px;margin:0 0 -5px -5px;"">
            </div>
            FECHA
         </th>
         <td style=""padding-left:10px;color:#000000"">{0:dd/MM/yyyy 'a las' HH:mm}</td>
    </tr>

    <tr style=""font-size:1.3rem;"">
        <th style=""text-align:left;background-color:#066938;color:#FFFFFF;font-weight:normal;padding:0 5px;height:30px;border-radius:5px"">
            <div style=""background-color:white;display:inline-block;height:30px;border-radius:5px"">
                <img src=""http://www.autocor.com.ar/serviciosWeb/images/email.png"" style=""height:30px;margin:0 0 -5px -5px"">
            </div>
            EMAIL
        </th>
       <td style=""padding-left:10px;color:#000000""> {1} </td>
    </tr>

    <tr style=""font-size:1.3rem;"">
        <th style=""text-align:left;background-color:#066938;color:#FFFFFF;font-weight:normal;padding:0 5px;height:30px;border-radius:5px"">
            <div style=""background-color:white;display:inline-block;height:30px;border-radius:5px"">
                <img src=""http://www.autocor.com.ar/serviciosWeb/images/display.png"" style=""height:30px;margin:0 0 -5px -5px"">
            </div>
            NOMBRE PC
        </th>
        <td style=""padding-left:10px;color:#000000""> {2} </td>
    </tr>
</table>", inicioSesion.Fecha, inicioSesion.Email, inicioSesion.NombrePC);

            string tituloCliente = string.Format("Hola <strong>{0}</strong>", cliente.RazonSocial);

            string footerCliente = @"
                  <p>
                    Si ha sido usted desestime este mensaje. Caso contrario póngase en contacto con nosotros al [0351] 4731941 o vía e-mail a
                    <a href=""mailto: info @autocor.com.ar"">info@autocor.com.ar</a>
                  </p>";

            var mensajeCliente = new MensajeEmailAutocor(cuerpoCliente);
            mensajeCliente.AgregarTitulo(tituloCliente);
            mensajeCliente.AgregarFooter(footerCliente);

            res = EnviarEmailWS(mensajeCliente.GenerarMensaje(), inicioSesion.Email, "Autocor - Inicio de sesión", inicioSesion.Email);

            // enviar email a Autocor
            if (notificiarAutocor)
            {
                string asunto = string.Format("Catálogo - Login de usuario: {0}", inicioSesion.CodigoCliente);
                string cuerpoAutocor = string.Format(@"
                 Cliente: {0}  <br/>
                 E-mail: {1}   <br/>
                 Fecha: {2:dd/MM/yyyy 'a las' HH:mm}
                               <br/><br/>
                 PC: {3}       <br/>
                 Usuario: {4}
                 ", cliente.ToString(), inicioSesion.Email, inicioSesion.Fecha, inicioSesion.NombrePC, inicioSesion.UsuarioPC);

                // si cambió de usuario
                if (inicioSesion.CodigoClienteAnterior.HasValue)
                {
                    asunto = "Catálogo - Cambio de usuario: " + inicioSesion.CodigoCliente;
                    cuerpoAutocor += string.Format("<br/><br/>El cliente hizo un cambio de usuario desde el cliente <strong>{0}</strong> al <strong>{1}</strong>.", inicioSesion.CodigoCliente, inicioSesion.CodigoClienteAnterior);
                }

                var mensajeAutocor = new MensajeEmailAutocor(cuerpoAutocor);
                mensajeAutocor.AgregarTitulo(string.Format("Nuevo inicio de sesión del cliente {0}", inicioSesion.CodigoCliente));

                var a = mensajeAutocor.GenerarMensaje();

                res = res && EnviarEmailWS(mensajeAutocor.GenerarMensaje(), "", asunto, EMAIL_AUTOCOR);
            }

            return res;
        }

        public bool EnviarEmailRegistro(RegistroDto registro)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat(@"
                <table>
                    <tbody>
                        <tr>
                            <th style=""text-align:left"">Nombre de Fantasía</th>
                            <td>{0}</td>
                        </tr>
                        <tr>
                            <th style=""text-align:left"">Titular</th>
                            <td>{1}</td>
                        </tr>
                        <tr>
                            <th style=""text-align:left"">CUIT</th>
                            <td>{2}</td>
                        </tr>
                        <tr>
                            <th style=""text-align:left"">Cat. AFIP</th>
                            <td>{3}</td>
                        </tr>
                        <tr>
                            <th style=""text-align:left"">Ing. Brutos</th>
                            <td>{4}</td>
                        </tr>
                        <tr>
                            <th style=""text-align:left"">Domicilio</th>
                            <td>{5}</td>
                        </tr>
                        <tr>
                            <th style=""text-align:left"">Cod. Postal</th>
                            <td>{6}</td>
                        </tr>
                        <tr>
                            <th style=""text-align:left"">Localidad</th>
                            <td>{7}</td>
                        </tr>
                        <tr>
                            <th style=""text-align:left"">Provincia</th>
                            <td>{8}</td>
                        </tr>
                        <tr>
                            <th style=""text-align:left"">Teléfono</th>
                            <td>{9}</td>
                        </tr>
                        <tr>
                            <th style=""text-align:left"">Ceular</th>
                            <td>{10}</td>
                        </tr>
                        <tr>
                            <th style=""text-align:left"">E-mail</th>
                            <td>{11}</td>
                        </tr>
                        <tr>
                            <th style=""text-align:left"">Marcas que comercializa</th>
                            <td>{12}</td>
                        </tr>
                        <tr>
                            <th style=""text-align:left"">Rubros que comercializa</th>
                            <td>{13}</td>
                        </tr>
                        <tr>
                            <th style=""text-align:left"">Flete habitual</th>
                            <td>{14}</td>
                        </tr>
                        <tr>
                            <th style=""text-align:left"">Flete alternativo</th>
                            <td>{15}</td>
                        </tr>
                    </tbody>
                </table>
            ", registro.Nombre, registro.Titular, registro.CUIT, registro.TipoIVA, registro.IngresosBrutos, registro.DomicilioComercial, registro.CodigoPostal,
                registro.Localidad, registro.Provincia, registro.Telefono, registro.Celular, registro.Email, registro.MarcasComercializa, registro.RubrosComercializa,
                registro.FleteHabitual, registro.FleteAlternativo);

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

                foreach (var contacto in registro.Contactos)
                {
                    sb.AppendFormat(
                        @"<tr>
                            <td>{0}</td>
                            <td>{1}</td>
                            <td>{2}</td>
                          </tr>",
                        contacto.Nombre,
                        contacto.Funcion,
                        contacto.FechaNacimiento.HasValue ?
                            contacto.FechaNacimiento.Value.ToShortDateString() : string.Empty);
                }

                sb.Append("</tbody></table>");
            }

            // listado de contactos

            var mensaje = new MensajeEmailAutocor(sb.ToString());
            mensaje.AgregarTitulo("Nuevo Cliente");

            return EnviarEmailWS(mensaje.GenerarMensaje(), "", "Catálogo - Nuevo Cliente", EMAIL_AUTOCOR);
        }

        private bool EnviarEmailWS(string mensajeHtml, string reply, string asunto, string destino)
        {
            try
            {
                AutocorEmailTestWS.AutocorWebServices ws = new AutocorEmailTestWS.AutocorWebServices();
                return ws.mailHtml(mensajeHtml, reply, asunto, destino);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}