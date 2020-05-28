using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AutocorApi.Servicios.Validation;

namespace AutocorApi.Servicios.Exceptions
{
    public class ServiceValidationException : Exception
    {
        public ServiceValidationException(IEnumerable<ValidationResult> validationResults)
        {
            this.ValidationResults = validationResults;
        }

        public ServiceValidationException(ValidationResult validationResult)
        {
            this.ValidationResults = new List<ValidationResult> { validationResult };
        }

        public ServiceValidationException(ServiceValidation serviceValidation)
        {
            this.ValidationResults = serviceValidation.ValidationResults;
        }

        public IEnumerable<ValidationResult> ValidationResults { get; private set; }
    }
}