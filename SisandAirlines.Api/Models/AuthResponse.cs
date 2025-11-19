namespace SisandAirlines.Api.Models.Auth
{
    public class AuthResponse
    {
        public string Token { get; set; } = null!;
        public DateTime ExpiresAt { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public Guid UserId { get; set; }  
    }
}
