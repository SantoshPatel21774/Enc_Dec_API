using Core.Models.KeyIvModel;
using Microsoft.Extensions.Logging;

namespace Common.Extension
{
    public static class SaveKeyAndIV
    {
        public static KeyIVResponse Save(this KeyIvModel model, ILogger _logger)
        {
            var envPath = Path.Combine(Directory.GetCurrentDirectory(), ".env");
            var content = $"EncryptionKeyandIV_KEY={model.KEY}\nEncryptionKeyandIV_IV={model.IV}";
            File.WriteAllText(envPath, content);
            _logger.LogInformation("Save the KEY and IV successfully. Key: [{Key}], IV: [{IV}] ", model.KEY, model.IV);
             return new KeyIVResponse()
            {
                Response = "Generated and saved the KEY and IV successfully.",
                StatusCode = 200
            };
        }
    }
}
