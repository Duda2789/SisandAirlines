using System;
using SisandAirlines.Domain.Enums;

namespace SisandAirlines.Domain.Entities
{
    public class SeatReservation
    {
        public Guid Id { get; private set; }
        public Guid FlightId { get; private set; }
        public Guid BookingId { get; private set; }
        public int SeatNumber { get; private set; }
        public SeatClass SeatClass { get; private set; }

        private SeatReservation() { }

        public SeatReservation(Guid flightId, Guid bookingId, int seatNumber, SeatClass seatClass)
        {
            Id = Guid.NewGuid();
            FlightId = flightId;
            BookingId = bookingId;
            SeatNumber = seatNumber;
            SeatClass = seatClass;
        }
    }
}
