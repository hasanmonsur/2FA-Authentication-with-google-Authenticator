using Microsoft.AspNetCore.Mvc;
using System.Drawing.Imaging;
using System.IO;
using TwoFactorAuthAPI.Models;
using TwoFactorAuthAPI.Services;

namespace TwoFactorAuthAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TwoFactorAuthController : ControllerBase
    {
        private readonly TwoFactorAuthenticationService _twoFactorAuthService;

        public TwoFactorAuthController(TwoFactorAuthenticationService twoFactorAuthService)
        {
            _twoFactorAuthService = twoFactorAuthService;
        }

        [HttpGet("setup")]
        public ActionResult<OtpSetupResponse> Setup(string email)
        {
            // Generate a new secret key
            var secretKey = _twoFactorAuthService.GenerateSecretKey();

            // Generate QR code URI and image
            var qrCodeUri = _twoFactorAuthService.GenerateQrCodeUri(secretKey, email);
            var qrCodeImage = _twoFactorAuthService.GenerateQrCodeImage(qrCodeUri);

            // Convert QR code image to Base64
            using (var ms = new MemoryStream())
            {
                qrCodeImage.Save(ms, ImageFormat.Png);
                var qrCodeBase64 = Convert.ToBase64String(ms.ToArray());

                // Return the setup details to the client
                return Ok(new OtpSetupResponse
                {
                    SecretKey = secretKey,
                    QrCodeUri = qrCodeUri,
                    QrCodeBase64 = qrCodeBase64
                });
            }
        }

        [HttpPost("validate")]
        public ActionResult ValidateOtp([FromBody] OtpValidationRequest request)
        {
            // Validate the provided OTP code
            var isValid = _twoFactorAuthService.ValidateOtpCode(request.SecretKey, request.Code);
            return Ok(new { isValid });
        }
    }
}
