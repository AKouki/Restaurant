namespace Restaurant.Services.ReCaptcha
{
    public interface IReCaptchaService
    {
        Task<bool> IsValid(string userResponse, string remoteIp, string action);
    }
}
