using Common.Extension;

namespace BAL.Interface
{
    public interface IEncryptService
    {
        Task<APIResponse> EncryptHelperAsync(string painText);
    }
}
