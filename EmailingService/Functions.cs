using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace EmailingService
{
    public class Functions
    {
        private readonly ILogger<Functions> _logger;

        public Functions(ILogger<Functions> logger)
        {
            _logger = logger;
        }

        [Function("sendsgemail")]
        public IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function,"post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult("Welcome to Azure Functions!");
        }
    }
}
