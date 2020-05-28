namespace AutocorApi.Servicios.Email
{
    public interface IEnviadorEmail
    {
        bool EnviarEmail(string para, string asunto, string mensaje, string from = null, string cc = null, string bcc = null, string replyTo = null, bool esHtml = true);
    }
}