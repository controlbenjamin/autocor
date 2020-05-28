namespace Autocor.Catalogo.Web.Utils.Mensaje
{
    public class MensajesErrorHttp
    {
        public const string ErrorGenerico = "Ha ocurrido un error al procesar su solicitud";
        public const string Error400 = "Petición no válida";
        public const string Error403 = "Acceso denegado";
        public const string Error404 = "Recurso no encontrado";
        public const string Error500 = "Error interno";

        public static string GetError(int numeroError)
        {
            switch (numeroError)
            {
                case 400: return Error404;
                case 403: return Error403;
                case 404: return Error404;
                case 500: return Error500;
                default: return ErrorGenerico;
            }
        }
    }
}