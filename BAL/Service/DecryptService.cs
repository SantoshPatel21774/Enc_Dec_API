using DAL;
using System.Security.Cryptography;
using Microsoft.Extensions.Logging;
using BAL.Interface;
using Core.Models.Decryption;
using Common.Extension;

namespace BAL.Service
{
    public class DecryptService : IDecryptService
    {
        private readonly ILogger<DecryptService> _logger;
        private readonly IKeyRepository _keyRepo;

        public DecryptService(ILogger<DecryptService> logger, IKeyRepository keyRepo)
        {
            _logger = logger;
            _keyRepo = keyRepo;
        }

        public async Task<APIResponse> DecryptHelperAsync(string encryptedValue)
        {
            try
            {
                var key = _keyRepo.GetKey();
                var iv = _keyRepo.GetIV();

                if (key == null)
                    throw new InvalidOperationException("Encryption key cannot be null.");
                if (iv == null)
                    throw new InvalidOperationException("Encryption IV cannot be null.");

                var cipherText = Convert.FromBase64String(encryptedValue);
                using var aes = Aes.Create();
                aes.Key = key;
                aes.IV = iv;
                aes.Padding = PaddingMode.PKCS7;


                using var decryptor = aes.CreateDecryptor();
                using var ms = new MemoryStream(cipherText);
                using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
                using var sr = new StreamReader(cs);
                var result = sr.ReadToEnd();


                _logger.LogInformation("Decryption successful.");

                return await Task.FromResult(new DecryptionResponse { DecryptedValue = result }.ToSuccessResponse("Decryption completed"));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Decryption failed.");
                string message= "An unexpected error occurred.(" + ex.Message + ")";
                return message.ToFailureResponse();
            }
        }
    }
}
