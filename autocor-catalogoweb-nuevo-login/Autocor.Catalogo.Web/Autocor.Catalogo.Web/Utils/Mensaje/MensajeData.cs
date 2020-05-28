namespace Autocor.Catalogo.Web.Utils.Mensaje
{
    public class MensajeData : MensajeWeb
    {
        protected enum TipoMensajeData { DEFAULT, REDIRECT, ERROR, ERROR_PROPERTY }

        private string _tipoMensajeStr;

        public object Data { get; set; }
        public bool Success { get; set; }
        public string Tipo { get { return _tipoMensajeStr; } }

        public MensajeData(string texto, string contexto, string titulo, object data, bool success = true)
            : base(texto, contexto, titulo)
        {
            Data = data;
            Success = success;
            SetTipo(TipoMensajeData.DEFAULT);
        }

        public MensajeData(string texto, string contexto, object data, bool success = true)
            : base(texto, contexto)
        {
            Data = data;
            Success = success;
            SetTipo(TipoMensajeData.DEFAULT);
        }

        public MensajeData(string texto, string contexto, bool success = true)
            : base(texto, contexto)
        {
            Data = null;
            Success = success;
            SetTipo(TipoMensajeData.DEFAULT);
        }

        protected void SetTipo(TipoMensajeData tipo)
        {
            switch(tipo)
            {
                case TipoMensajeData.DEFAULT:
                    _tipoMensajeStr = "default";
                    break;
                case TipoMensajeData.REDIRECT:
                    _tipoMensajeStr = "redirect";
                    break;
                case TipoMensajeData.ERROR:
                    _tipoMensajeStr = "error";
                    break;
                case TipoMensajeData.ERROR_PROPERTY:
                    _tipoMensajeStr = "error_property";
                    break;
                default:
                    _tipoMensajeStr = "";
                    break;
            }
        }

    }
}