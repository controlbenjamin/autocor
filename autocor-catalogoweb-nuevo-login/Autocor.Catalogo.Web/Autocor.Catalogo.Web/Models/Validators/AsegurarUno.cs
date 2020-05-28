using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace Autocor.Catalogo.Web.Models.Validators
{
    public class AsegurarUno : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                bool res = false;
                var list = value as IList;

                if (list != null)
                {
                    res = list.Count > 0;
                }

                return new ValidationResult("Debe ingresar al menos uno");
            }

            return ValidationResult.Success;
        }


    }
}