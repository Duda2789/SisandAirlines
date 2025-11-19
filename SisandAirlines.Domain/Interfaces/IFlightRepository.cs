using SisandAirlines.Domain.Entities;

namespace SisandAirlines.Domain.Interfaces
{
    public interface IFlightRepository
    {
        Task<IEnumerable<Flight>> GetByDateAsync(DateTime? date);
        Task<int> CountReservedSeatsAsync(Guid flightId);
        Task<Flight?> GetByIdAsync(Guid id);
    }
}
