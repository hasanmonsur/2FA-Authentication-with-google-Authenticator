namespace TwoFactorAuthAPI.Models
{
    public class OtpValidationRequest
    {
        public string SecretKey { get; set; }
        public string Code { get; set; }
    }
}
