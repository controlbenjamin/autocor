namespace Autocor.Catalogo.Web.Utils.Mensaje
{
    public class MensajeDataSimple : MensajeData
    {
        public MensajeDataSimple(object data)
            : base("OK", MensajeDataSimple.CONTEXTO_SUCCESS, data, success: true) { }   

        public MensajeDataSimple()
            : base("OK", MensajeDataSimple.CONTEXTO_SUCCESS, success: true)
        {

        }
    }
}