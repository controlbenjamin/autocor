using System.Configuration;

namespace AutocorApi.Servicios.Email.Implementation
{
    public class ConfiguracionEmailDefault : IConfiguracionEmail
    {
        private string from;
        private string email;
        private string password;
        private string host;
        private int port;
        private bool ssl;

        public ConfiguracionEmailDefault()
        {
            // inicializar datos de cuenta que envía emails desde Web.config
            this.from = ConfigurationManager.AppSettings["email_from"];
            this.email = ConfigurationManager.AppSettings["email_email"];
            this.password = ConfigurationManager.AppSettings["email_password"];
            this.host = ConfigurationManager.AppSettings["email_host"];
            this.ssl = ConfigurationManager.AppSettings["email_ssl"] == "true";

            if (!int.TryParse(ConfigurationManager.AppSettings["email_host_port"], out this.port))
            {
                this.port = 587;
            }
        }

        public string From => from;

        public string Email => email;

        public string Password => password;

        public string Host => host;

        public int Port => port;

        public bool SSL => ssl;
    }

    public class ConfiguracionEmailTest : IConfiguracionEmail
    {
        private string from;
        private string email;
        private string password;
        private string host;
        private int port;

        public ConfiguracionEmailTest()
        {
            this.from = "Autocor Test";
            this.email = "test@autocor.com.ar";
            this.password = "123";
            this.host = "localhost";
            this.port = 25;
        }

        public string From => from;

        public string Email => email;

        public string Password => password;

        public string Host => host;

        public int Port => port;

        public bool SSL => false;
    }
}