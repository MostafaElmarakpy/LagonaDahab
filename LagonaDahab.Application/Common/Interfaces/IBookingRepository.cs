using LagonaDahab.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagonaDahab.Application.Common.Interfaces
{
    public interface IBookingRepository : IGenaricRepository<Booking>
    {
        void Update(Booking entity);
        void UpdateStatus(int bookingId, string sessionStatus, int villaNumber);
        void updateStripePaymentId(int bookingId, string sessionId, string paymentIntentId);
    }
}
