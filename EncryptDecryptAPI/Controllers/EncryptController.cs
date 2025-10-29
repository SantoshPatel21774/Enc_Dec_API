using BAL.Interface;
using Core.Models.Encryption;
using Common.Extension;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Encrpt_Decrpt_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class EncryptController : ControllerBase
    {
        private readonly IEncryptService _encryptService;
        private readonly ILogger<EncryptController> _logger;

        public EncryptController(IEncryptService encryptService, ILogger<EncryptController> logger)
        {
            _encryptService = encryptService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Encrypt([FromBody] EncryptionRequest encryptionRequest)
        {
            try
            {
                if (string.IsNullOrEmpty(encryptionRequest?.RequestValue))
                {
                    return BadRequest("RequestValue cannot be null or empty.".ToFailureResponse());
                }

                var result = await _encryptService.EncryptHelperAsync(encryptionRequest.RequestValue);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[EncryptController.Encrypt] : " + "Unhandled exception occurred");
                string messgage = "An unexpected error occurred.(" + ex.Message + ")";
                return BadRequest(messgage.ToFailureResponse());
            }
        }
    }
}
