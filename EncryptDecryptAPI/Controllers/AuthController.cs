using BAL.Interface;
using Core.Models.Token;
using Microsoft.AspNetCore.Mvc;

namespace EncryptDecryptAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(ITokenService tokenService, ILogger<AuthController> logger)
        {
            _tokenService = tokenService;
            _logger = logger;
        }

        [HttpPost("token")]
        public async Task<IActionResult> GetToken(TokenResquest tokenResquest)
        {
            try
            {
                TokenResponse response = new();
                if (ModelState.IsValid)
                {
                    if (tokenResquest==null || string.IsNullOrWhiteSpace(tokenResquest.userName))
                        return BadRequest(ModelState);

                     response = await _tokenService.GenerateTokenAsync(tokenResquest);
                }
                return Ok(response);
            }
            catch(Exception ex) 
            {
                _logger.LogError("An exception occurred while generating token: {Message}", ex.Message);
                return BadRequest(new { Status ="Token Generation Failed", ex.Message });

            }
           
        }

    }
}
