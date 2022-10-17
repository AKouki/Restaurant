using System.Text.Json;

namespace Restaurant.Services.ReCaptcha
{
    public class ReCaptchaV3Service : IReCaptchaService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ReCaptchaV3Service> _logger;

        public ReCaptchaV3Service(HttpClient httpClient,
            IConfiguration configuration,
            ILogger<ReCaptchaV3Service> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<bool> IsValid(string userResponse, string remoteIP, string action)
        {
            try
            {
                var secretKey = _configuration["ReCaptcha:SecretKey"];
                var url = $"?secret={secretKey}&response={userResponse}&remoteip={remoteIP}";

                var json = await _httpClient.GetStringAsync(url);
                var response = JsonSerializer.Deserialize<ReCaptchaV3Response>(json);

                if (response != null &&
                    response.IsSuccess &&
                    response.Action == action)
                {
                    return true;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Error validating CAPTCHA", ex.Message);
            }

            return false;
        }
    }
}
