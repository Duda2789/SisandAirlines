using System;

namespace SisandAirlines.Domain.Entities
{
    public class BookingPassenger
    {
        public Guid Id { get; private set; }
        public Guid BookingId { get; private set; }
        public string FullName { get; private set; } = null!;
        public string DocumentCpf { get; private set; } = null!;

        private BookingPassenger() { }

        public BookingPassenger(Guid bookingId, string fullName, string documentCpf)
        {
            Id = Guid.NewGuid();
            BookingId = bookingId;
            FullName = fullName;
            DocumentCpf = documentCpf;
        }
    }
}
