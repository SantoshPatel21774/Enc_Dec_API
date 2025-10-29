using DAL;
using System.Security.Cryptography;
using Microsoft.Extensions.Logging;
using BAL.Interface;
using Core.Models.Encryption;
using Common.Extension;

namespace BAL.Service
{
    public class EncryptService : IEncryptService
    {
        private readonly ILogger<EncryptService> _logger;
        private readonly IKeyRepository _keyRepo;

        public EncryptService(ILogger<EncryptService> logger, IKeyRepository keyRepo)
        {
            _logger = logger;
            _keyRepo = keyRepo;
        }

        public async Task<APIResponse> EncryptHelperAsync(string plainText)
        {
            try
            {
                var key = _keyRepo.GetKey();
                var iv = _keyRepo.GetIV();

                if (key == null)
                    throw new InvalidOperationException("Encryption key cannot be null.");
                if (iv == null)
                    throw new InvalidOperationException("Encryption IV cannot be null.");

                using var aes = Aes.Create();
                aes.Key = key;
                aes.IV = iv;
                aes.Padding = PaddingMode.PKCS7;

                using var encryptor = aes.CreateEncryptor();
                using var ms = new MemoryStream();
                using var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
                using var sw = new StreamWriter(cs);
                sw.Write(plainText);
                sw.Flush();
                cs.FlushFinalBlock();

                var encryptedBytes = ms.ToArray();
                var encryptedBase64 = Convert.ToBase64String(encryptedBytes);

                _logger.LogInformation("Encryption successful for input: {input}", plainText);

                return await Task.FromResult(new EncryptionResponse { EncryptedValue= encryptedBase64 }.ToSuccessResponse("Encryption completed"));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Encryption failed.");
                string message = "An unexpected error occurred.(" + ex.Message + ")";
                return message.ToFailureResponse();
            }
        }
    }
}
