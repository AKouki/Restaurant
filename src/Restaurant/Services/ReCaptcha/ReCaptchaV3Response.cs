using System.Text.Json.Serialization;

namespace Restaurant.Services.ReCaptcha
{
    public class ReCaptchaV3Response
    {
        //"success": true|false,      // whether this request was a valid reCAPTCHA token for your site
        //"score": number             // the score for this request (0.0 - 1.0)
        //"action": string            // the action name for this request (important to verify)
        //"challenge_ts": timestamp,  // timestamp of the challenge load (ISO format yyyy-MM-dd'T'HH:mm:ssZZ)
        //"hostname": string,         // the hostname of the site where the reCAPTCHA was solved
        //"error-codes": [...]        // optional

        [JsonPropertyName("success")]
        public bool IsSuccess { get; set; }

        [JsonPropertyName("score")]
        public float Score { get; set; }

        [JsonPropertyName("action")]
        public string? Action { get; set; }

        [JsonPropertyName("challenge_ts")]
        public DateTime? ChallengeDateTime { get; set; }

        [JsonPropertyName("hostname")]
        public string? Hostname { get; set; }

        [JsonPropertyName("error-codes")]
        public List<string>? Errors { get; set; } = new();
    }
}
