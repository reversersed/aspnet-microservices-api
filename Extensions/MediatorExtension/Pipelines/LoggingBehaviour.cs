using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Text.Json;

namespace Extensions.MediatorExtension.Pipelines
{
    public class LoggingBehaviour<TRequest, TResponse>(ILogger<LoggingBehaviour<TRequest, TResponse>> _logger) : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
    {
        private readonly ILogger<LoggingBehaviour<TRequest, TResponse>> logger = _logger ?? throw new ArgumentNullException(nameof(_logger));

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            logger.LogInformation("[Logging] Handling {requestname}...", typeof(TRequest).Name);
            var stopwatch = Stopwatch.StartNew();

            var response = await next();

            stopwatch.Stop();
            logger.LogInformation("[Logging] {requestname} handled within {time}ms", typeof(TRequest).Name, stopwatch.ElapsedMilliseconds);
            return response;
        }
    }
}
