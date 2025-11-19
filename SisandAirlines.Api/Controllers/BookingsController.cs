using Microsoft.AspNetCore.Mvc;
using SisandAirlines.Api.Models.Bookings;
using SisandAirlines.Domain.Entities;
using SisandAirlines.Domain.Enums;
using SisandAirlines.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;


namespace SisandAirlines.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class BookingsController : ControllerBase

    {
        private readonly IUserRepository _userRepository;
        private readonly IFlightRepository _flightRepository;
        private readonly IBookingRepository _bookingRepository;

        public BookingsController(
            IUserRepository userRepository,
            IFlightRepository flightRepository,
            IBookingRepository bookingRepository)
        {
            _userRepository = userRepository;
            _flightRepository = flightRepository;
            _bookingRepository = bookingRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBookingRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (request.Passengers == null || request.Passengers.Count == 0)
                return BadRequest("At least one passenger is required.");

            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user is null)
                return BadRequest("Usuário não encontrado");

            var flight = await _flightRepository.GetByIdAsync(request.FlightId);
            if (flight is null)
                return BadRequest("Voo não encontrado");

            int economyMax = 5;
            int firstMax = 2;

            var economyCount = request.Passengers.Count(p => p.SeatClass == SeatClass.Economy);
            var firstCount = request.Passengers.Count(p => p.SeatClass == SeatClass.FirstClass);

            if (economyCount > economyMax || firstCount > firstMax)
                return BadRequest("Passenger count exceeds available seats for one of the classes.");

            var booking = new Booking(
                userId: request.UserId,
                flightId: request.FlightId,
                totalAmount: CalculateTotalAmount(request.Passengers),
                paymentMethod: request.PaymentMethod
            );

            var passengers = request.Passengers.Select(p =>
                new BookingPassenger(
                    bookingId: booking.Id,
                    fullName: p.FullName,
                    documentCpf: p.DocumentCpf
                )).ToList();

            var reservations = new List<SeatReservation>();

            var takenEconomySeats = new HashSet<int>();
            var takenFirstSeats = new HashSet<int>();

            int nextEconomySeat = 1;
            int nextFirstSeat = 6;

            foreach (var p in passengers.Zip(request.Passengers, (entity, dto) => new { entity, dto }))
            {
                int seatNumber;

                if (p.dto.SeatClass == SeatClass.Economy)
                {
                    if (nextEconomySeat > 5)
                        return BadRequest("No available economy seats.");

                    seatNumber = nextEconomySeat++;
                }
                else
                {
                    if (nextFirstSeat > 7)
                        return BadRequest("No available first class seats.");

                    seatNumber = nextFirstSeat++;
                }

                reservations.Add(new SeatReservation(
                    flightId: request.FlightId,
                    bookingId: booking.Id,
                    seatNumber: seatNumber,
                    seatClass: p.dto.SeatClass
                ));
            }

            await _bookingRepository.AddAsync(booking, passengers, reservations);

            var response = new BookingResponse
            {
                Id = booking.Id,
                UserId = booking.UserId,
                FlightId = booking.FlightId,
                TotalAmount = booking.TotalAmount,
                PaymentMethod = booking.PaymentMethod,
                ConfirmationCode = booking.ConfirmationCode,
                CreatedAt = booking.CreatedAt,
                Passengers = reservations
                    .Join(passengers, r => r.BookingId, p => p.BookingId,
                        (r, p) => new BookingPassengerResponse
                        {
                            FullName = p.FullName,
                            DocumentCpf = p.DocumentCpf,
                            SeatNumber = r.SeatNumber,
                            SeatClass = r.SeatClass
                        }).ToList()
            };

            return CreatedAtAction(nameof(Create), new { id = booking.Id }, response);
        }

        private decimal CalculateTotalAmount(IEnumerable<CreateBookingPassengerRequest> passengers)
        {
            decimal economyPrice = 100m;
            decimal firstClassPrice = 180m;

            decimal total = 0m;

            foreach (var p in passengers)
            {
                total += p.SeatClass == SeatClass.Economy ? economyPrice : firstClassPrice;
            }

            return total;
        }
    }
}
