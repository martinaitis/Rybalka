using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace RybalkaWebAPI.Attributes
{
    public class LogAttribute : ActionFilterAttribute
    {
        private readonly ILogger<LogAttribute> _logger;

        public LogAttribute(ILogger<LogAttribute> logger)
        {
            _logger = logger;
        }

        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            var request = actionContext.ActionDescriptor.DisplayName;
            _logger.LogInformation("REQUEST {request}", request);
        }

        public override void OnActionExecuted(ActionExecutedContext actionExecutedContext)
        {
            var request = actionExecutedContext.ActionDescriptor.DisplayName;
            ObjectResult result = (ObjectResult)actionExecutedContext.Result!;
            var responseCode = result.StatusCode;
            
            if (responseCode >= 200 && responseCode <= 299)
            {
                _logger.LogInformation(
                    "RESPONSE: {request}\n RESPONSE CODE: {responseCode}",
                    request,
                    responseCode);

                return;
            }

            var responseValue = result.Value;
            _logger.LogInformation(
                "RESPONSE: {request}\n RESPONSE CODE: {responseCode}\n RESPONSE VALUE: {responseValue} ",
                request,
                responseCode,
                responseValue);
        }
    }
}
