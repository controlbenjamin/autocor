namespace Autocor.Catalogo.Web.Utils.Mensaje
{
    public class MensajeDataRedirect : MensajeData
    {
        public MensajeDataRedirect(string url)
            : base(string.Empty, string.Empty, string.Empty)
        {
            base.Data = new { url = url };
            base.SetTipo(TipoMensajeData.REDIRECT);
        }
    }
}