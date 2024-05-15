using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ResponsePackage;
using System.Text.Json;

namespace Extensions.MediatorExtension.Middlewares
{
    public class ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger) : IMiddleware
    {
        private readonly ILogger<ExceptionHandlingMiddleware> logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception e)
            {
                logger.LogError("[Exception] {exceptionmessage}", e.Message);
                await HandleException(context, e);
            }
        }

        private static async Task HandleException(HttpContext context, Exception e)
        {
            ResponseCodes code = ResponseCodes.UndefinedServerException;

            List<string>? errors = null;
            if (e is CustomExceptionResponse custom)
            {
                errors = custom.Errors?.Distinct().ToList();
                code = custom.Code;
            }

            if (e is ValidationException validation)
            {
                code = ResponseCodes.ValidationError;
                errors = validation.Errors.Select(i => i.ErrorMessage).Distinct().ToList();
            }
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = code == ResponseCodes.UndefinedServerException ? StatusCodes.Status500InternalServerError : 
                                          code == ResponseCodes.Unauthorized ? StatusCodes.Status401Unauthorized : 
                                          code == ResponseCodes.ObjectNotUpdated ? StatusCodes.Status304NotModified :
                                          StatusCodes.Status400BadRequest;

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DictionaryKeyPolicy = JsonNamingPolicy.CamelCase
            };
            if(errors != null) 
            { 
                var response = new BaseResponse<List<string>?>
                {
                    Code = code,
                    Message = e.Message,
                    Data = errors
                };
                await context.Response.WriteAsync(JsonSerializer.Serialize(response, options));
            }
            else
            {
                var response = new BaseResponse
                {
                    Code = code,
                    Message = e.Message
                };
                await context.Response.WriteAsync(JsonSerializer.Serialize(response, options));
            }
        }
    }
}