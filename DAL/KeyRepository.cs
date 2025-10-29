using Common.Service;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace DAL
{
    public class KeyRepository : IKeyRepository
    {

        private readonly IConfiguration _config;
        private readonly SessionService _sessionService;
        private readonly byte[] _key;
        private readonly byte[] _iv;
        public KeyRepository(IConfiguration config, SessionService sessionService)
        {
            _config = config;
            _sessionService = sessionService;
            (_key,_iv)= _sessionService.GetOrCreateKeyAndIV();
        }
        public byte[]? GetKey()
        {
            var keyString = _config["EncryptionKeyandIV:KEY"];
            if (keyString != null)
            {
                if (!string.IsNullOrEmpty(keyString))
                    return Encoding.UTF8.GetBytes(keyString);
                else
                    return _key;
            }
            else
            {
                return null;
            }
        }

        public byte[]? GetIV()
        {
            var ivString = _config["EncryptionKeyandIV:IV"];
            if (!string.IsNullOrEmpty(ivString))
                return Encoding.UTF8.GetBytes(ivString);
            else
                return _iv;
        }
    }

}
