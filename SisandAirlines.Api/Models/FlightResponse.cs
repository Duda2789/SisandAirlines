namespace SisandAirlines.Api.Models
{
    public class FlightResponse
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan DepartureTime { get; set; }
        public TimeSpan ArrivalTime { get; set; }

        public int AircraftNumber { get; set; }
        public int AvailableSeats { get; set; }
    }
}
