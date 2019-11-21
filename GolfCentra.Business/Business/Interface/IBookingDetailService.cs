using GolfCentra.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.Business.Business.Interface
{
    public interface IBookingDetailService
    {
        List<BookingViewModel> GetBookingsByBookingStatus(long golferId, long bookingSearchTypeId);
        BookingViewModel GetBookingDetailsByBookingId(string bookingId, long golferId);
        bool CancelBookingByGolfer(string bookingId, long golferId);
        List<BookingViewModel> GetBookingsByBookingTypeId(long golferId, long bookingTypeId);
    }
}
