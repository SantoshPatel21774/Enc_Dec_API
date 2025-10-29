using Core.Models.Token;

namespace BAL.Interface
{
    public interface ITokenService
    {
        Task<TokenResponse> GenerateTokenAsync(TokenResquest tokenResquest);
    }
}
