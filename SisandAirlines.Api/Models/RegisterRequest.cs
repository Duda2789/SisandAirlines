namespace SisandAirlines.Api.Models.Auth
{
    public class RegisterRequest
    {
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Cpf { get; set; } = null!;
        public DateTime BirthDate { get; set; }
        public string Password { get; set; } = null!;
    }
}
