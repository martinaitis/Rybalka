using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace RybalkaWebAPI.Controllers
{
    public class ErrorController : ControllerBase
    {
        private readonly ILogger<ErrorController> _logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            _logger = logger;
        }

        [Route("/error")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult HandleError()
        {
            var exceptionHandlerFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>()!;
            _logger.LogError(
                $"Endpoint: {exceptionHandlerFeature.Endpoint?.DisplayName}{System.Environment.NewLine}" +
                $"Message: {exceptionHandlerFeature.Error.Message}{System.Environment.NewLine}" +
                $"StackTrace: {exceptionHandlerFeature.Error.StackTrace}");
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
