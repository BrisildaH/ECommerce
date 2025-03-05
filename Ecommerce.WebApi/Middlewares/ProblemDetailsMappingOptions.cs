using Ecommerce.Common.Exceptions;
using FluentValidation;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;


namespace Ecommerce.WebApi.Middlewares
{
    public static class ProblemDetailsMappingOptions
    {
        public static IServiceCollection AddProblemDetailsMappingOptions(this IServiceCollection services)
        {
            services.AddProblemDetails(options =>
            {
                options.OnBeforeWriteDetails = (ctx, problem) =>
                {
                    problem.Extensions["traceId"] = ctx.TraceIdentifier;
                    problem.Instance = ctx.Request.Path;
                };
                // This will map validation errors.
                options.Map<ValidationException>((_, exception) =>
                {
                    var errors = exception.Errors.GroupBy(failure => failure.PropertyName)
                        .Select(failures => failures)
                        .ToDictionary(failures => failures.Key,
                            failures => failures.Select(failure => failure.ErrorMessage).ToArray());

                    return new ValidationProblemDetails(errors)
                    {
                        Type = HttpCodeTypes.Error400Type,
                        Status = StatusCodes.Status400BadRequest
                    };
                });

                // This will map service errors.
                #region Service exception
                options.Map<ForbiddenException>((exception) =>
                {
                    return new ProblemDetails
                    {
                        Type = exception.Type,
                        Title = exception.Title,
                        Status = exception.Status,
                        Detail = exception.Detail
                    };
                });

                options.Map<NotFoundException>((exception) =>
                {
                    return new ProblemDetails
                    {
                        Type = exception.Type,
                        Title = exception.Title,
                        Status = exception.Status,
                        Detail = exception.Detail
                    };
                });

                options.Map<ConflictException>((exception) =>
                {
                    return new ProblemDetails
                    {
                        Type = exception.Type,
                        Title = exception.Title,
                        Status = exception.Status,
                        Detail = exception.Detail
                    };
                });
                options.Map<BadRequestException>((exception) =>
                {
                    return new ProblemDetails
                    {
                        Type = exception.Type,
                        Title = exception.Title,
                        Status = exception.Status,
                        Detail = exception.Detail
                    };
                });
                #endregion


                // You can configure the middleware to re-throw certain types of exceptions, all exceptions or based on a predicate.
                // This is useful if you have upstream middleware that needs to do additional handling of exceptions.
                options.Rethrow<NotSupportedException>();

                // This will map NotImplementedException to the 501 Not Implemented status code.
                options.MapToStatusCode<NotImplementedException>(StatusCodes.Status501NotImplemented);

                // This will map HttpRequestException to the 503 Service Unavailable status code.
                options.MapToStatusCode<HttpRequestException>(StatusCodes.Status503ServiceUnavailable);

                // This will map every other not mapped error.
                options.Map<Exception>((ctx, exception) =>
                {
                    return new StatusCodeProblemDetails(StatusCodes.Status500InternalServerError)
                    {
                        Type = HttpCodeTypes.Error500Type
                    };
                });
            });
            return services;
        }

    }
}

