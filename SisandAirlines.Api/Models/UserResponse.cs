using System;

namespace SisandAirlines.Api.Models
{
    public class UserResponse
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Cpf { get; set; } = null!;
        public DateTime BirthDate { get; set; }
    }
}
