using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Autocor.Catalogo.Web.Utils.Mensaje
{
    public class MensajeSimple : MensajeDataSimple
    {
        public MensajeSimple(string texto)
            :base(null)
        {
            base.Texto = texto;
        }

        public MensajeSimple()
            : base()
        {

        }
    }
}