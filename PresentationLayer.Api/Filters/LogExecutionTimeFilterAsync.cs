using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace PresentationLayer.Api.Filters
{
    public class LogExecutionTimeFilterAsync : IAsyncActionFilter
    {
        private readonly ILogger<LogExecutionTimeFilterAsync> _logger;
        private Stopwatch _stopwatch;

        public LogExecutionTimeFilterAsync(ILogger<LogExecutionTimeFilterAsync> logger)
        {
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // logic before
            _logger.LogInformation("Action Execution started............");
            _stopwatch = Stopwatch.StartNew();

            await next();
            // logic after

            _logger.LogInformation("Action Execution ended............");
            _stopwatch.Stop();
            _logger.LogInformation($"Action executed in {_stopwatch.ElapsedMilliseconds} ms");
            if (context.Result is ObjectResult objectResult) // (1) check on data type  (2) safe casting
            {
                // var objectResult = context.Result as ObjectResult; // safe casting
                _logger.LogInformation($"Status Code is {objectResult.StatusCode}");
            }
        }
    }
}
