using Microsoft.AspNetCore.Mvc.Filters;

namespace RybalkaWebAPI.Attributes.Action
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

        public override void OnResultExecuted(ResultExecutedContext resultExecutedContext)
        {
            var responseTo = resultExecutedContext.ActionDescriptor.DisplayName;
            var responseCode = resultExecutedContext.HttpContext.Response.StatusCode;

            _logger.LogInformation(
                "RESPONSE CODE: {responseCode} RESPONSE TO: {responseTo}",
                responseCode,
                responseTo);
        }
    }
}
