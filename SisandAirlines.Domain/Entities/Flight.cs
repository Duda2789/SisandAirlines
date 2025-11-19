using System;
using System.Collections.Generic;
using System.Linq;
using SisandAirlines.Domain.Enums;

namespace SisandAirlines.Domain.Entities
{
    public class Flight
    {
        public Guid Id { get; private set; }
        public DateTime Date { get; private set; }
        public TimeSpan DepartureTime { get; private set; }
        public TimeSpan ArrivalTime { get; private set; }
        public int AircraftNumber { get; private set; } // 1, 2 ou 3

        private readonly List<SeatReservation> _reservations = new();
        public IReadOnlyCollection<SeatReservation> Reservations => _reservations;

        private const int EconomySeats = 5;
        private const int FirstClassSeats = 2;

        private Flight() { }

        public Flight(DateTime date, TimeSpan departureTime, int aircraftNumber)
        {
            Id = Guid.NewGuid();
            Date = date.Date; 
            DepartureTime = departureTime;
            ArrivalTime = departureTime + TimeSpan.FromHours(1); 
            AircraftNumber = aircraftNumber;
        }

        public bool HasAvailableSeat(SeatClass seatClass)
        {
            var totalSeats = seatClass == SeatClass.Economy ? EconomySeats : FirstClassSeats;
            var used = _reservations.Count(r => r.SeatClass == seatClass);
            return used < totalSeats;
        }

        public void AddReservation(SeatReservation reservation)
        {
            // regra básica de negócio: não deixar estourar a capacidade
            if (!HasAvailableSeat(reservation.SeatClass))
                throw new InvalidOperationException("Não há mais assentos disponíveis nesse voo.");

            _reservations.Add(reservation);
        }
    }
}
