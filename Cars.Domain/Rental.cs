using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cars.Domain
{
    public enum RentalStatus
    {
        Pending,
        Confirmed,
        Active,
        Completed,
        Cancelled
    }

    public class Rental
    {
        public Guid Id { get; set; }

        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        public Guid CarId { get; set; }
        public Car Car { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public decimal TotalPrice { get; set; }
        public RentalStatus Status { get; set; } = RentalStatus.Pending;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
