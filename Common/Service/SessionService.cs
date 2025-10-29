using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;

namespace Common.Service
{
    public class SessionService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SessionService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public (byte[] Key, byte[] IV) GetOrCreateKeyAndIV()
        {
            var session = _httpContextAccessor.HttpContext.Session;

            if (session.TryGetValue("SecureKey", out var keyBytes) &&
                session.TryGetValue("SecureIV", out var ivBytes))
            {
                return (keyBytes, ivBytes);
            }

            using var aes = Aes.Create();
            aes.GenerateKey();
            aes.GenerateIV();

            session.Set("SecureKey", aes.Key);
            session.Set("SecureIV", aes.IV);

            return (aes.Key, aes.IV);
        }
    }
}
