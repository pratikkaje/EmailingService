using SendGrid;
using SendGrid.Helpers.Mail;

namespace EmailingService
{
    public class EmailSender : IEmailSender
    {
        private readonly string apiKey;
        private readonly string fromEmail;

        public EmailSender()
        {
            apiKey = Environment.GetEnvironmentVariable("SendGridApiKey");
            fromEmail = Environment.GetEnvironmentVariable("FromEmail");
        }

        public async Task<bool> SendEmailAsync(EmailRequest emailRequest)
        {
            try
            {
                var client = new SendGridClient(apiKey);
                var from = new EmailAddress(fromEmail, "App Support");
                var to = new EmailAddress(emailRequest.To);
                var msg = MailHelper.CreateSingleEmail(from, to, emailRequest.Subject, emailRequest.Body, emailRequest.IsHtml ? emailRequest.Body : null);
                var response = await client.SendEmailAsync(msg);

                if (!response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Body.ReadAsStringAsync();
                    throw new ApplicationException($"Failed to send email. SendGrid returned {response.StatusCode} - {responseBody}");
                }

                return response.StatusCode is System.Net.HttpStatusCode.OK or System.Net.HttpStatusCode.Accepted;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An unexpected error occurred while sending the email.", ex);
            }
        }
    }
}
