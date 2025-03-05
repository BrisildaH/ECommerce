using Ecommerce.WebApi.Common;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Ecommerce.WebApi.Middlewares
{
    public static class ValidationExtensions
    {
        public static void CheckModelState(ModelStateDictionary modelState)
        {
            //Nel caso di errore invalid request negli array, raggruppiamo per la prima parte della key e prendiamo solo il primo errore
            var entries = modelState.Where(mod => mod.Value != null && mod.Value.Errors != null && mod.Value.Errors.Count > 0)
                .GroupBy(mod => mod.Key.Split(".")[0])
                .Select(mod => mod.First());


            //Se abbiamo due errori sullo stesso field, prendiamo solo quello nostro Custom
            var errors = entries.SelectMany(pair => pair.Value.Errors?.OrderByDescending(e => e.ErrorMessage.Contains(ApiCommonStringResources.Separator)).Take(1), (pair, error) =>
            {
                //Distinguiamo gli errori custom dagli invalid request tramite il separatore custom
                if (error.ErrorMessage.Contains(ApiCommonStringResources.Separator))
                {
                    var chunks = error.ErrorMessage.Split(ApiCommonStringResources.Separator);
                    return new ValidationFailure
                    {
                        ErrorMessage = chunks[1],
                        PropertyName = pair.Key
                    };
                }
                else
                {
                    return new ValidationFailure
                    {
                        ErrorMessage = "Invalid request",
                        PropertyName = pair.Key
                    };
                }

            }).ToList();
            throw new ValidationException(errors);
        }
    }
}
