using LagonaDahab.Application.Common.Interfaces;
using LagonaDahab.Application.Common.Utility;
using LagonaDahab.Domain.Entities;
using LagonaDahab.Infrastructure.Data;
using LagonaDahab.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagonaDahab.Infrastructure.Repository
{
    public class BookingRepository : GenaricRepository<Booking>, IBookingRepository
    {

        private readonly ApplicationDbContext _dbContext;

        public BookingRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public void Update(Booking entity)
        {
            _dbContext.Update(entity);
        }
        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }

        public void UpdateStatus(int bookingId, string sessionStatus, int villaNumber = 0)
        {
            var bookFromDb = _dbContext.Bookings.FirstOrDefault(m => m.Id== bookingId);
            if (bookFromDb != null)
            {
                bookFromDb.Status = sessionStatus;
                if (sessionStatus == SD.StatusPending)
                {

                    bookFromDb.VillaNumber = villaNumber;
                    bookFromDb.AcutualCheckInDate = DateTime.Now;
                }
                if (sessionStatus == SD.StatusCompleted)
                {
                    bookFromDb.AcutualCheckOutDate = DateTime.Now;
                }
            }


        }

        public void updateStripePaymentId(int bookingId, string sessionId, string paymentIntentId)
        {
            var bookFromDb = _dbContext.Bookings.FirstOrDefault(m => m.Id == bookingId);
            if (bookFromDb == null)
            {
                throw new ArgumentException($"Booking with ID {bookingId} not found");
            }
            if (!string.IsNullOrEmpty(sessionId))
            {
                bookFromDb.StripeSessionId = sessionId;

            }
            if (!string.IsNullOrEmpty(paymentIntentId))
            {
                bookFromDb.StripePaymentIntentId = paymentIntentId;
                bookFromDb.PaymentDate = DateTime.Now;
                bookFromDb.IsPaymentSuccessed = true;


            }
        }
    }
}
