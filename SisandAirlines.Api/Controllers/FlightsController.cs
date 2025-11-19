using Microsoft.AspNetCore.Mvc;
using SisandAirlines.Domain.Interfaces;
using SisandAirlines.Api.Models;

namespace SisandAirlines.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FlightsController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly IFlightRepository _flightRepository;

        public FlightsController(IUnitOfWork uow, IFlightRepository flightRepository)
        {
            _uow = uow;
            _flightRepository = flightRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetFlights( [FromQuery] DateTime? date,[FromQuery] int passengers)
        {
            if (passengers < 1)
                return BadRequest("Deve adicionar no mínimo 1 passageiro!");

            await _uow.BeginAsync();

            try
            {
                var flights = await _flightRepository.GetByDateAsync(date);
                var list = new List<FlightResponse>();

                foreach (var flight in flights)
                {
                    var reserved = await _flightRepository.CountReservedSeatsAsync(flight.Id);
                    int totalSeats = 7;
                    int available = totalSeats - reserved;

                    if (available >= passengers)
                    {
                        list.Add(new FlightResponse
                        {
                            Id = flight.Id,
                            Date = flight.Date,
                            DepartureTime = flight.DepartureTime,
                            ArrivalTime = flight.ArrivalTime,
                            AircraftNumber = flight.AircraftNumber,
                            AvailableSeats = available
                        });
                    }
                }

                await _uow.CommitAsync();
                return Ok(list);
            }
            catch
            {
                await _uow.RollbackAsync();
                throw;
            }
        }
    }
}
