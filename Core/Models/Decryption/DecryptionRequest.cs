namespace Core.Models.Decryption
{
    public class DecryptionRequest
    {
        /// <summary>
        /// Base64-encoded encrypted string to be decrypted.
        /// </summary>
        public string? EncryptedValue { get; set; }

    }
}
