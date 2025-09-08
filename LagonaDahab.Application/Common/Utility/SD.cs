using LagonaDahab.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagonaDahab.Application.Common.Utility
{

    public static class SD
    {
        // / Constants for roles

        public const string Role_Admin = "Admin";
        public const string Role_Customer = "Customer";

        public const string StatusPending = "Pending";
        public const string StatusApproved = "Approved";
        public const string StatusCheckedIn = "ChechedIn";
        public const string StatusCompleted = "Completed";
        public const string StatusCancelled = "Cancelled";
        public const string StatusRefunded = "Refunded";


        public static int VillaRoomsAvailable_Count(
            int villaId,
            List<VillaNumber> villaNumberList,
            DateOnly checkInDate,
            int nights,
            List<Booking> bookings)
        {
            // Get the total number of rooms in the villa
            var roomsInVilla = villaNumberList.Count(x => x.VillaId == villaId);

            // For each night, calculate the number of booked rooms and determine availability
            return Enumerable.Range(0, nights)
                .Select(i =>
                {
                    var bookingsForDate = bookings
                        .Where(b => b.villaId == villaId &&
                                   b.CheckInDate <= checkInDate.AddDays(i) &&
                                   b.CheckOutDate > checkInDate.AddDays(i))
                        .Select(b => b.Id)
                        .Distinct()
                        .Count();
                    // Available rooms for this specific date
                    return roomsInVilla - bookingsForDate;
                })
                .Min();
        }

    }
}
