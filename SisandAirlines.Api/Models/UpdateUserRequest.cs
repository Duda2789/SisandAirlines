using System;

namespace SisandAirlines.Api.Models
{
    public class UpdateUserRequest
    {
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Cpf { get; set; } = null!;
        public DateTime BirthDate { get; set; }
        public string? Password { get; set; } 
    }
}
