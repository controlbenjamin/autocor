using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using System.Web.Http.ModelBinding;
using AutocorApi.Servicios.Exceptions;

namespace AutocorApi.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ServiceExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            if (context.Exception is ServiceValidationException)
            {
                var ex = context.Exception as ServiceValidationException;

                ModelStateDictionary modelState = new ModelStateDictionary();

                foreach (ValidationResult valItem in ex.ValidationResults)
                {
                    foreach (string member in valItem.MemberNames)
                    {
                        modelState.AddModelError(member, valItem.ErrorMessage);
                    }
                }

                context.Response = context.Request.CreateErrorResponse(HttpStatusCode.BadRequest, modelState);

                return;
            }

            base.OnException(context);
        }
    }
}