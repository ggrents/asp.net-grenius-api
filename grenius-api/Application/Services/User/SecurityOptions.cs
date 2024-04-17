namespace grenius_api.Application.Services
{
    public class SecurityOptions
    {
        public const string Security = "Security";
        public string Secret { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public int ExpirationHours { get; set; }

    }
}
