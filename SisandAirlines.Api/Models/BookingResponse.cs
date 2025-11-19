using System;
using System.Collections.Generic;
using SisandAirlines.Domain.Enums;

namespace SisandAirlines.Api.Models.Bookings
{
    public class BookingPassengerResponse
    {
        public string FullName { get; set; } = null!;
        public string DocumentCpf { get; set; } = null!;
        public int SeatNumber { get; set; }
        public SeatClass SeatClass { get; set; }
    }

    public class BookingResponse
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid FlightId { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentMethod { get; set; } = null!;
        public string ConfirmationCode { get; set; } = null!;
        public DateTime CreatedAt { get; set; }

        public List<BookingPassengerResponse> Passengers { get; set; } = new();
    }
}
