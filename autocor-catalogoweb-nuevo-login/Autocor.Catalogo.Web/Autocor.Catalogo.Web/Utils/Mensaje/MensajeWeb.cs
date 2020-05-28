namespace Autocor.Catalogo.Web.Utils.Mensaje
{
    public class MensajeWeb
    {
        /* constantes de contextos para css */
        public const string CONTEXTO_INFO = "info";
        public const string CONTEXTO_SUCCESS = "success";
        public const string CONTEXTO_WARNING = "warning";
        public const string CONTEXTO_DANGER = "danger";

        public string Texto { get; set; }
        public string CssContext { get; set; }
        public string Titulo { get; set; }

        public MensajeWeb()
        {
            CssContext = CONTEXTO_INFO;
            Texto = string.Empty;
            AutoTitulo();
        }

        public MensajeWeb(string texto, string contexto, string titulo)
        {
            Texto = texto;
            CssContext = contexto;
            Titulo = titulo;
        }

        public MensajeWeb(string texto, string contexto, bool autoTitulo = true)
        {
            Texto = texto;
            CssContext = contexto;
            if (autoTitulo)
                AutoTitulo();
        }

        private void AutoTitulo()
        {
            switch (CssContext)
            {
                case CONTEXTO_DANGER:
                    Titulo = "Error";
                    break;
                case CONTEXTO_SUCCESS:
                    Titulo = "Éxito";
                    break;
                case CONTEXTO_WARNING:
                    Titulo = "Aviso";
                    break;
                case CONTEXTO_INFO:
                default:
                    Titulo = "Información";
                    break;
            }
        }
        
    }
}