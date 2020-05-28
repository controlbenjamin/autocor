using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Autocor.Catalogo.Web.Utils.Mensaje
{
    public class MensajeDataErrorProperty : MensajeData
    {
        public MensajeDataErrorProperty(ModelStateDictionary modelState, string titulo = "")
            : base(string.Empty, MensajeData.CONTEXTO_DANGER, titulo, success: false)
        {
            var errorDic = modelState.Where(x => x.Value.Errors.Count > 0).ToDictionary(
                   kvp => kvp.Key,
                   kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray());

            base.Data = new { errors = errorDic };
            base.SetTipo(TipoMensajeData.ERROR_PROPERTY);
        }
    }
}