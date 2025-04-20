using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace EmailingService
{
    public class Functions
    {
        private readonly IEmailSender _emailSender;
        private readonly ILogger<Functions> _logger;

        public Functions(IEmailSender emailSender, ILogger<Functions> logger)
        {
            _emailSender = emailSender;
            _logger = logger;
        }

        [Function("sendsgemail")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonSerializer.Deserialize<EmailRequest>(requestBody);

            if (data == null || string.IsNullOrWhiteSpace(data.To))
            {
                return new BadRequestObjectResult("Invalid request payload.");
            }

            try
            {
                var result =
                    await _emailSender.SendEmailAsync(data);
                if (result)
                {
                    return new OkObjectResult("Email sent successfully.");
                }

                return new BadRequestObjectResult("Failed to send email.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new BadRequestObjectResult("Failed to send email.");
            }
        }
    }
}
