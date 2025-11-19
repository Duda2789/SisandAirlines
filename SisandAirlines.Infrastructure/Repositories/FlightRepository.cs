using Dapper;
using SisandAirlines.Domain.Entities;
using SisandAirlines.Domain.Interfaces;

namespace SisandAirlines.Infrastructure.Repositories
{
    public class FlightRepository : IFlightRepository
    {
        private readonly IUnitOfWork _uow;

        public FlightRepository(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IEnumerable<Flight>> GetByDateAsync(DateTime? date)
        {
            var baseSql = @"
                SELECT 
                    id,
                    date            AS Date,
                    departure_time  AS DepartureTime,
                    arrival_time    AS ArrivalTime,
                    aircraft_number AS AircraftNumber
                FROM flights
            ";

            string sql;
            object parameters;

            if (date.HasValue)
            {
                sql = baseSql + " WHERE date = @Date ORDER BY departure_time;";
                parameters = new { Date = date.Value.Date };
            }
            else
            {
                sql = baseSql + " ORDER BY date, departure_time;";
                parameters = new { };
            }

            return await _uow.Connection
                .QueryAsync<Flight>(sql, parameters, _uow.Transaction);
        }


        public async Task<int> CountReservedSeatsAsync(Guid flightId)
        {
            var sql = @"SELECT COUNT(*) FROM seat_reservations WHERE flight_id = @Id";
            return await _uow.Connection.ExecuteScalarAsync<int>(sql, new { Id = flightId }, _uow.Transaction);
        }

        public async Task<Flight?> GetByIdAsync(Guid id)
        {
            const string sql = @"
                SELECT 
                    id,
                    date            AS Date,
                    departure_time  AS DepartureTime,
                    arrival_time    AS ArrivalTime,
                    aircraft_number AS AircraftNumber
                FROM flights
                WHERE id = @Id;
            ";

            return await _uow.Connection
                .QueryFirstOrDefaultAsync<Flight>(sql, new { Id = id }, _uow.Transaction);
        }
    }
}
