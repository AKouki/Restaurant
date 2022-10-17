namespace Restaurant.Services.Email
{
    public interface IEmailService
    {
        Task SendEmailAsync(string email, string subject, string htmlContent);
    }
}
