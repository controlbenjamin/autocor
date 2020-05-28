using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace AutocorApi.Servicios.Validation
{
    public class ServiceValidation
    {
        private List<ValidationResult> validationList;

        public ServiceValidation()
        {
            this.validationList = new List<ValidationResult>();
        }

        public IEnumerable<ValidationResult> ValidationResults => validationList;
        public bool IsValid => validationList.Count() == 0;
        public bool IsNotValid => !IsValid;

        public void AddError(ValidationResult validationResult)
        {
            this.validationList.Add(validationResult);
        }

        public void AddError(string message, string property)
        { 
            this.validationList.Add(new ValidationResult(message, new[] { property }));
        }
    }
}