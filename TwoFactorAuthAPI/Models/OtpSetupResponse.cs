namespace TwoFactorAuthAPI.Models
{
    public class OtpSetupResponse
    {
        public string SecretKey { get; set; }
        public string QrCodeUri { get; set; }
        public string QrCodeBase64 { get; set; }
    }
}
