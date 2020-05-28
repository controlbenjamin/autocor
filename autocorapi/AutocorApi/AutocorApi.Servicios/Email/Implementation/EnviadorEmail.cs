using System;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace AutocorApi.Servicios.Email.Implementation
{
    public class EnviadorEmail : IEnviadorEmail
    {
        private IConfiguracionEmail configuracion;

        public EnviadorEmail(IConfiguracionEmail configuracion)
        {
            this.configuracion = configuracion;
        }

        public bool EnviarEmail(string para, string asunto, string mensaje, string from = null, string cc = null, string bcc = null, string replyTo = null, bool esHtml = true)
        {
            if(string.IsNullOrEmpty(from))
            {
                from = configuracion.From;
            }

            using (MailMessage message = new MailMessage())
            {
                message.To.Add(para); // a que dirección se envía
                message.From = new MailAddress(configuracion.Email, from, Encoding.UTF8);  // desde dónde se envía
                message.Subject = asunto;
                message.Body = mensaje;

                if (!string.IsNullOrEmpty(cc)) { message.CC.Add(cc); }
                if (!string.IsNullOrEmpty(bcc)) { message.Bcc.Add(bcc); }
                if (!string.IsNullOrEmpty(replyTo)) { message.ReplyToList.Add(replyTo); }

                message.SubjectEncoding = Encoding.UTF8;
                message.BodyEncoding = Encoding.UTF8;
                message.IsBodyHtml = esHtml;

                SmtpClient client = new SmtpClient(configuracion.Host, configuracion.Port)
                {
                    Credentials = new NetworkCredential(configuracion.Email, configuracion.Password),
                    EnableSsl = configuracion.SSL
                };

                try
                {
                    client.Send(message);
                    return true;
                }
                catch (Exception ex)
                {
                    // TODO: agregar log
                }
            }

            return false;
        }
    }
}