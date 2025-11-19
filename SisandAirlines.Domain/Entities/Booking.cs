using System;

namespace SisandAirlines.Domain.Entities
{
    public class Booking
    {
        public Guid Id { get; private set; }
        public Guid UserId { get; private set; }
        public Guid FlightId { get; private set; }
        public decimal TotalAmount { get; private set; }
        public string PaymentMethod { get; private set; } = null!;
        public string ConfirmationCode { get; private set; } = null!;
        public DateTime CreatedAt { get; private set; }

        // construtor sem parâmetros para o Dapper/EF
        private Booking() { }

        // construtor usado na aplicação (no controller)
        public Booking(Guid userId, Guid flightId, decimal totalAmount, string paymentMethod)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            FlightId = flightId;
            TotalAmount = totalAmount;
            PaymentMethod = paymentMethod;
            CreatedAt = DateTime.UtcNow;
            ConfirmationCode = GenerateConfirmationCode();
        }

        private static string GenerateConfirmationCode()
        {
            // 8 caracteres, maiúsculos – simples e legível
            return Guid.NewGuid().ToString("N")[..8].ToUpper();
        }
    }
}
