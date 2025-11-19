using System;
using System.Threading.Tasks;
using SisandAirlines.Domain.Entities;

namespace SisandAirlines.Domain.Interfaces
{
    public interface IBookingRepository
    {
      //  Task<Booking?> GetByIdAsync(Guid id);
        Task AddAsync(Booking booking, IEnumerable<BookingPassenger> passengers, IEnumerable<SeatReservation> reservations);
    }
}
