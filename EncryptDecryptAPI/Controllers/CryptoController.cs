using Common.Extension;
using Core.Models.KeyIvModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = "Bearer")]
public class CryptoController : ControllerBase
{
    private readonly ILogger<CryptoController> _logger;
    public CryptoController(ILogger<CryptoController> logger)
    {
        _logger = logger;
    }

    [HttpPost("generate")]
    public IActionResult GenerateKeyAndIV()
    {

        try
        {
            // AES-256 key (32 bytes) and IV (16 bytes)
            KeyIvModel model = new()
            {
                KEY = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32)),
                IV = Convert.ToBase64String(RandomNumberGenerator.GetBytes(16))
            };

            return Ok(model.Save(_logger));
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to save. Exception: {ExceptionMessage}", ex.Message);
             KeyIVResponse response = new()
             {
                Response = string.Format("Failed to save. Exception: {0}", ex.Message),
                StatusCode = 500
             };
             return BadRequest(response);
        }
    }

}