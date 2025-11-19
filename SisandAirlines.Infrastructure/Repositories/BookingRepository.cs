using System.Data;
using Dapper;
using SisandAirlines.Domain.Entities;
using SisandAirlines.Domain.Interfaces;

namespace SisandAirlines.Infrastructure.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly IDbConnection _connection;

        public BookingRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task AddAsync(
            Booking booking,
            IEnumerable<BookingPassenger> passengers,
            IEnumerable<SeatReservation> reservations)
        {
            using var transaction = _connection.BeginTransaction();

            try
            {
                const string insertBookingSql = @"
                    INSERT INTO bookings
                        (id, user_id, flight_id, total_amount, payment_method, confirmation_code, created_at)
                    VALUES
                        (@Id, @UserId, @FlightId, @TotalAmount, @PaymentMethod, @ConfirmationCode, @CreatedAt);";

                await _connection.ExecuteAsync(insertBookingSql, booking, transaction);

                const string insertPassengerSql = @"
                    INSERT INTO booking_passengers
                        (id, booking_id, full_name, document_cpf)
                    VALUES
                        (@Id, @BookingId, @FullName, @DocumentCpf);";

                foreach (var p in passengers)
                {
                    await _connection.ExecuteAsync(insertPassengerSql, p, transaction);
                }

                const string insertReservationSql = @"
                    INSERT INTO seat_reservations
                        (id, flight_id, booking_id, seat_number, seat_class)
                    VALUES
                        (@Id, @FlightId, @BookingId, @SeatNumber, @SeatClass);";

                foreach (var r in reservations)
                {
                    await _connection.ExecuteAsync(insertReservationSql, r, transaction);
                }

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}
