using System;
using System.Collections.Generic;
using SisandAirlines.Domain.Enums;

namespace SisandAirlines.Api.Models.Bookings
{
    public class CreateBookingPassengerRequest
    {
        public string FullName { get; set; } = null!;
        public string DocumentCpf { get; set; } = null!;
        public SeatClass SeatClass { get; set; } // Economy ou FirstClass
    }

    public class CreateBookingRequest
    {
        public Guid UserId { get; set; }
        public Guid FlightId { get; set; }
        public string PaymentMethod { get; set; } = null!;
        public List<CreateBookingPassengerRequest> Passengers { get; set; } = new();
    }
}
