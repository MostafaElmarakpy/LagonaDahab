using LagonaDahab.Application.Common.Interfaces;
using LagonaDahab.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LagonaDahab.Web.Controllers
{
    public class BookingController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public BookingController( IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult FinalieBooking(int villaId , int nights , DateOnly checkInDate)
        {
            Booking booking = new Booking()
            {
                //UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                villaId = villaId,

                Villa = _unitOfWork.Villa.Get(u=>u.Id== villaId, includeProperty: "VillaAmenity"),
                CheckInDate = checkInDate.ToDateTime(TimeOnly.MinValue),
                NumberOfNights = nights,
                CheckOutDate = checkInDate.AddDays(nights).ToDateTime(TimeOnly.MinValue),
                BookingDate = DateTime.Now,
            }; 
            return View(booking); 
        }
    }
}
