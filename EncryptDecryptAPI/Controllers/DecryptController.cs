using BAL.Interface;
using Common.Extension;
using Core.Models.Decryption;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Encrpt_Decrpt_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class DecryptController : ControllerBase
    {
        private readonly IDecryptService _decryptService;
        private readonly ILogger<DecryptController> _logger;

        public DecryptController(IDecryptService decryptService, ILogger<DecryptController> logger)
        {
            _decryptService = decryptService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Decrypt([FromBody] DecryptionRequest decryptionRequest)
        {
            try
            {
                if (string.IsNullOrEmpty(decryptionRequest?.EncryptedValue))
                    return BadRequest("EncryptedValue cannot be null or empty.".ToFailureResponse());

                var result = await _decryptService.DecryptHelperAsync(decryptionRequest.EncryptedValue);
                
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[DecryptController.Decrypt] : " + "Unhandled exception occurred");
                string messgage = "An unexpected error occurred.(" + ex.Message + ")";
                return BadRequest(messgage.ToFailureResponse());
            }
        }
    }
}
