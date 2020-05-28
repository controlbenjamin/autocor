using System.Text;

namespace AutocorApi.Servicios.Email.Implementation
{
    public class MensajeEmailBase
    {
        protected const string SECCION_TITULO = "{{TITULO}}";
        protected const string SECCION_CUERPO = "{{CUERPO}}";
        protected const string SECCION_FOOTER = "{{FOOTER}}";

        protected string _divisor;
        protected string _template;

        protected string titulo;
        protected string cuerpo;
        protected string footer;

        public MensajeEmailBase(string cuerpoMensaje)
        {
            SetTemplate();
            AgregarCuerpo(cuerpoMensaje);
        }

        public MensajeEmailBase()
        {
            SetTemplate();
        }

        public string GenerarMensaje()
        {
            StringBuilder sb = new StringBuilder(_template);

            // título
            if (!string.IsNullOrEmpty(titulo))
            {
                sb.Replace(SECCION_TITULO, titulo + _divisor);
            }
            else
            {
                sb.Replace(SECCION_TITULO, string.Empty);
            }

            // cuerpo
            sb.Replace(SECCION_CUERPO, cuerpo);

            // footer
            if (!string.IsNullOrEmpty(footer))
            {
                sb.Replace(SECCION_FOOTER, _divisor + footer);
            }
            else
            {
                sb.Replace(SECCION_FOOTER, string.Empty);
            }

            return sb.ToString();
        }

        protected virtual void SetTemplate()
        {
            _divisor = "<hr>";
            _template =
                @"{{TITULO}} <br/>
                  {{CUERPO}} <br/>
                  {{FOOTER}}";
        }

        public virtual void AgregarCuerpo(string cuerpo)
        {
            this.cuerpo = cuerpo;
        }

        public virtual void AgregarTitulo(string titulo)
        {
            this.titulo = titulo;
        }

        public virtual void AgregarFooter(string footer)
        {
            this.footer = footer;
        }
    }

    public class MensajeEmail : MensajeEmailBase
    {
        public MensajeEmail(string cuerpoMensaje) : base(cuerpoMensaje)
        {
            //SetTemplate();
            //AgregarCuerpo(cuerpoMensaje);
        }

        public MensajeEmail() : base()
        {
            // SetTemplate();
        }

        protected override void SetTemplate()
        {
            _divisor = @"<div class=""contenedor__divisor"" style="" margin: 10px 0 9px 0; border-bottom: 1px solid #066938;""></div>";
            _template = @"
<!DOCTYPE html>
<html>
<head>
    <title>Email</title>
    <link href=""https://fonts.googleapis.com/css?family=Roboto+Condensed:400,700"" rel=""stylesheet"">
    <style>
        @media(max-width: 395px)
        {
            .hidden-xs {
                display: none;
            }
        }
    </style>
</head>

<body style=""background-color: #066938; color: #ffffff; font-family: 'Roboto Condensed', sans-serif;"">

<table style=""margin: auto; max-width: 600px;"">
    <tr>
    <td width=""100%"" height=""100%"" bgcolor=""#066938"">
    <div id=""main"" >

        <header style=""min-width: 395px;"">
            <img src=""http://www.autocor.com.ar/autocor/serviciosWeb/images/autocor-email-header-logo.png"" alt=""Autocor"" style=""float:left"">
            <img src=""http://www.autocor.com.ar/autocor/serviciosWeb/images/autocor-email-header-icon.png"" alt=""E-mail"" style=""float:right"" class=""hidden-xs"">
            <div class=""clearfix"" style=""clear: both;""></div>
        </header>

        <div class=""contenedor"" style="" background-color: #FFFFFF; color: #212121; padding: 15px 25px; border-radius: 10px;"">

            {{TITULO}}

            {{CUERPO}}

            {{FOOTER}}

        </div>

        <footer style=""margin-top: 10px; text-align: center; font-size: 1.1em; font-weight: 600;"">
            <a href=""http://www.autocor.com.ar"" class=""unstyled"" target=""_blank"" style=""text-decoration: none; color: inherit;"">www.autocor.com.ar</a>
        </footer>
    </div>

    </td>
    </tr>

</table>

</body>
</html>";
        }

        public override void AgregarCuerpo(string cuerpo)
        {
            this.cuerpo = string.Format(@"
                <div class=""contenedor__body"" style=""font-size: 1.4em;"">
                    <p>{0}</p>
                </div>", cuerpo);
        }

        public override void AgregarTitulo(string titulo)
        {
            this.titulo = string.Format(@"
                <div class=""contenedor__titulo"" style=""font-size: 1.6rem;"">{0}</div>", titulo);
        }

        public override void AgregarFooter(string footer)
        {
            this.footer = string.Format(@"
                <div class=""contenedor__footer"" style=""font-size: 1em;"">{0}</div>", footer);
        }
    }
}