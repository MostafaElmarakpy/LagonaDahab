using LagonaDahab.Application.Common.Interfaces;
using LagonaDahab.Application.Common.Utility;
using LagonaDahab.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
using System.Data;
using System.Security.Claims;

namespace LagonaDahab.Web.Controllers
{
    public class BookingController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public BookingController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetAll(string status)
        {
            IEnumerable<Booking> objBooking;
            string userId = "";
            if (User.IsInRole(SD.Role_Admin))
            {
                 objBooking = _unitOfWork.Booking.GetAll(includeProperty:"Villa,User");
            }
            else
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;

                 userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

                 objBooking = _unitOfWork.Booking.GetAll(u=>u.UserId== userId, includeProperty:"Villa,User");
            }
            return Json(new { data = objBooking });
        }


        [Authorize]
        public IActionResult FinalieBooking(int villaId, int nights, DateOnly checkInDate)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;

            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ApplicationUser user = _unitOfWork.User.Get(u => u.Id == userId);

            Booking booking = new Booking()
            {
                //UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                villaId = villaId,

                Villa = _unitOfWork.Villa.Get(u => u.Id == villaId, includeProperty: "VillaAmenity"),
                CheckInDate = checkInDate,
                NumberOfNights = nights,
                CheckOutDate = checkInDate.AddDays(nights),
                BookingDate = DateTime.Now,
                Phone = user.PhoneNumber,
                Email = user.Email,
                Name = user.Name,


            };
            var villa = _unitOfWork.Villa.Get(u => u.Id == booking.villaId);
            booking.TotalPrice = villa.Price * nights;

            return View(booking);
        }

        [Authorize]
        [HttpPost]
        public IActionResult FinalieBooking(Booking booking)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;

            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            var villa = _unitOfWork.Villa.Get(u => u.Id == booking.villaId);

            booking.TotalPrice = booking.NumberOfNights * villa.Price;
            booking.TotalPrice = booking.VillaNumber;
            booking.UserId = userId;
            booking.Status = SD.StatusPending;
            booking.BookingDate = DateTime.Now;

            _unitOfWork.Booking.Add(booking);
            _unitOfWork.Save();


            var domain = Request.Scheme + "://" + Request.Host.Value + "/";
            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>(){},
                Mode = "payment",
                SuccessUrl = domain + $"Booking/BookingConfiermation?bookingId={booking.Id}",
                CancelUrl = domain + $"Booking/FinalieBooking?bookingId={booking.villaId}&nights={booking.NumberOfNights}&checkInDate={booking.CheckInDate}",
            };
            // 
            options.LineItems.Add(new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmount = (long)(booking.TotalPrice * 100),
                    Currency = "egp",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = villa.Name,
                        //Images = new List<string> {domain +villa.Image }
                    }
                },
                Quantity = 1,
            });


            var service = new SessionService();
            Session session = service.Create(options);

            _unitOfWork.Booking.updateStripePaymentId(booking.Id, session.Id, session.PaymentIntentId);
            _unitOfWork.Booking.UpdateStatus(booking.Id,session.Status,0);
            _unitOfWork.Save();

            Response.Headers.Append("Location", session.Url);

            return new StatusCodeResult(303);


        }

        [Authorize] 
        public IActionResult BookingConfiermation(int bookingId)
        {
            return View(bookingId);
        }

        [Authorize]

        public IActionResult BookingDetails(int bookingId)
        {
            var booking = _unitOfWork.Booking.Get(u => u.Id == bookingId, includeProperty: "Villa,User");
            return View(booking);
        }


    }


}
