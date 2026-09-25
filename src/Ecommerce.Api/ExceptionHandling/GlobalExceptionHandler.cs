using Ecommerce.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.ExceptionHandling
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var (statusCode, title) = MapException(exception);

            _logger.LogError(exception, "Erro não tratado ao processar {Method} {Path}", httpContext.Request.Method, httpContext.Request.Path);

            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = statusCode == StatusCodes.Status500InternalServerError
                    ? "Ocorreu um erro inesperado ao processar a requisição."
                    : exception.Message,
                Instance = httpContext.Request.Path
            };

            httpContext.Response.StatusCode = statusCode;

            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }

        private static (int StatusCode, string Title) MapException(Exception exception) => exception switch
        {
            ConcurrencyException => (StatusCodes.Status409Conflict, "Conflito de concorrência"),
            DomainException => (StatusCodes.Status400BadRequest, "Regra de negócio violada"),
            ArgumentException => (StatusCodes.Status400BadRequest, "Requisição inválida"),
            InvalidOperationException => (StatusCodes.Status409Conflict, "Operação inválida para o estado atual"),
            _ => (StatusCodes.Status500InternalServerError, "Erro interno do servidor")
        };
    }
}
