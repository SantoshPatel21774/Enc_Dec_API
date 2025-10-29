namespace Core.Models.Token
{
    public class TokenResponse
    {
        public string? Token { get; set; }
        public string? Username { get; set; }
        public DateTime IssuedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public string TokenType { get; set; } = "Bearer";
    }

}
