namespace AutocorApi.Servicios.Email
{
    public interface IConfiguracionEmail
    {
        string From { get; }
        string Email { get; }
        string Password { get; }
        string Host { get; }
        int Port { get; }
        bool SSL { get; }
    }
}