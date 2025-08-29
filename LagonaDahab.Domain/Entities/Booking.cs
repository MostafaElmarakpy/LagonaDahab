using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagonaDahab.Domain.Entities
{
    public class Booking
    {
        [Key]
        public int Id { get; set; }

        [Required]

        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        public int villaId { get; set; }
        [ForeignKey("villaId")]
        public Villa Villa { get; set; }


        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        public string? Phone { get; set; }

        [Required]
        public double TotalPrice { get; set; }

        public int NumberOfNights { get; set; } // e.g., 2 nights
        public string? Status { get; set; } // e.g., "Pending", "Confirmed", "Cancelled"


        [Required]
        public DateTime BookingDate { get; set; }
        [Required]
        public DateOnly CheckInDate { get; set; }
        [Required]
        public DateOnly CheckOutDate { get; set; }

        
        public bool IsPaymentSuccessed { get; set; }
        public DateTime PaymentDate { get; set; }
        public string? StripeSessionId { get; set; }
        public string? StripePaymentIntentId { get; set; } // For Stripe integration

        public DateTime AcutualCheckInDate { get; set; }
        public DateTime AcutualCheckOutDate { get; set; }

        public int VillaNumber { get; set; } // e.g., 101, 102, etc.


    }
}
