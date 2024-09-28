using OtpNet;
using System.Drawing;
using QRCoder.Core;

namespace TwoFactorAuthAPI.Services
{
    public class TwoFactorAuthenticationService
    {
        public string GenerateSecretKey()
        {
            // Generate a secret key for the user
            var key = KeyGeneration.GenerateRandomKey(20);
            return Base32Encoding.ToString(key);
        }

        public string GenerateQrCodeUri(string secretKey, string userEmail, string issuer = "TwoFactorAuthAPI")
        {
            // Generate the QR code URI for Google Authenticator
            var totp = new Totp(Base32Encoding.ToBytes(secretKey));
            return $"otpauth://totp/{issuer}:{userEmail}?secret={secretKey}&issuer={issuer}&digits={totp.TotpSize}";
        }

        public Bitmap GenerateQrCodeImage(string qrCodeUri)
        {
            // Generate the QR code image
            var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(qrCodeUri, QRCodeGenerator.ECCLevel.Q);
            var qrCode =new QRCode(qrCodeData);

            return qrCode.GetGraphic(20); // Returns a Bitmap
        }

        public bool ValidateOtpCode(string secretKey, string code)
        {
            // Validate the provided OTP code
            var totp = new Totp(Base32Encoding.ToBytes(secretKey));
            return totp.VerifyTotp(code, out _); // The out parameter will capture the matched timestamp.
        }
    }
}
