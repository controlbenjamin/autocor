namespace Autocor.Catalogo.Web.Utils.Mensaje
{
    public class MensajeDataError : MensajeData
    {
        public MensajeDataError(string errorMessage, string titulo = null)
            : base(errorMessage, MensajeData.CONTEXTO_DANGER, success: false)
        {
            base.SetTipo(TipoMensajeData.ERROR);
        }
    }
}