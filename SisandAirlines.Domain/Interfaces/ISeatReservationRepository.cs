using System;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SisandAirlines.Domain.Entities;

namespace SisandAirlines.Domain.Interfaces
{
    public interface ISeatReservationRepository
    {
        Task<IEnumerable<SeatReservation>> GetByFlightAsync(Guid flightId);
        Task AddAsync(SeatReservation reservation);
    }
}
