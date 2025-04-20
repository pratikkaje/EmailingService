namespace EmailingService
{
    public interface IEmailSender
    {
        Task<bool> SendEmailAsync(EmailRequest emailRequest);
    }
}
