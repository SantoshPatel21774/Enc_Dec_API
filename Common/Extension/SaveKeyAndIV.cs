using Core.Models.KeyIvModel;
using Microsoft.Extensions.Logging;

namespace Common.Extension
{
    public static class SaveKeyAndIV
    {
        public static KeyIVResponse Save(this KeyIvModel model, ILogger logger)
        {
            try
            {
                var envPath = Path.Combine(Directory.GetCurrentDirectory(), ".env");
                var lines = File.Exists(envPath) ? File.ReadAllLines(envPath).ToList() : new List<string>();

                // Update or add KEY
                var keyLine = $"EncryptionKeyandIV__KEY={model.KEY}";
                var keyIndex = lines.FindIndex(line => line.StartsWith("EncryptionKeyandIV__KEY="));
                if (keyIndex >= 0)
                    lines[keyIndex] = keyLine;
                else
                    lines.Add(keyLine);

                // Update or add IV
                var ivLine = $"EncryptionKeyandIV__IV={model.IV}";
                var ivIndex = lines.FindIndex(line => line.StartsWith("EncryptionKeyandIV__IV="));
                if (ivIndex >= 0)
                    lines[ivIndex] = ivLine;
                else
                    lines.Add(ivLine);

                File.WriteAllLines(envPath, lines);

                logger.LogInformation("Saved KEY and IV successfully. Key: [{Key}], IV: [{IV}]", model.KEY, model.IV);

                return new KeyIVResponse
                {
                    Response = "Generated and saved the KEY and IV successfully.",
                    StatusCode = 200
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error saving KEY and IV.");
                return new KeyIVResponse
                {
                    Response = $"Failed to save KEY and IV. Exception: {ex.Message}",
                    StatusCode = 500
                };
            }
        }
    }
}
