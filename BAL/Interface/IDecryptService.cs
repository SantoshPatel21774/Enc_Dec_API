using Common.Extension;

namespace BAL.Interface
{
    public interface IDecryptService
    {
        Task<APIResponse> DecryptHelperAsync(string encryptedValue);
    }
}
