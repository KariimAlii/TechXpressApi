using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace PresentationLayer.Api.Filters
{
    public class LogExecutionTimeFilter : ActionFilterAttribute   //  []   Synchronous
    {
        private readonly ILogger<LogExecutionTimeFilter> _logger;
        private Stopwatch _stopwatch;

        public LogExecutionTimeFilter(ILogger<LogExecutionTimeFilter> logger)
        {
            _logger = logger;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation("Action Execution started............");
            _stopwatch = Stopwatch.StartNew();
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation("Action Execution ended............");
            _stopwatch.Stop();
            _logger.LogInformation($"Action executed in {_stopwatch.ElapsedMilliseconds} ms");
            if(context.Result is ObjectResult objectResult) // (1) check on data type  (2) safe casting
            {
                // var objectResult = context.Result as ObjectResult; // safe casting
                _logger.LogInformation($"Status Code is {objectResult.StatusCode}");
            }
        }
    }
}
