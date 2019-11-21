using GolfCentra.ViewModel.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.Business.BusinessAdmin.Interface
{
    public interface IBookingService
    {
        List<BookingViewModel> GetBookingDetailsByTake(int noOfRecord);
        BookingViewModel GetBookingDetailsByBookingId(long bookingId);
        DashBoardTopBarViewModel GetDataForTopBar();
        List<BookingViewModel> GetBookingDetails();
        List<BookingViewModel> GetBookingDetailsBySearch(BookingViewModel bookingModel);
        BookingPricingViewModel GetPricingCalculation(BookingPricingViewModel bookingPricingViewModel);
        List<BucketViewModel> GetBucketDetailList(DateTime date, long memberTypeId);
        bool SaveBooking(SaveBookingViewModel bookingViewModel);
        List<BookingTypeViewModel> GetAllBookingType();
        List<DayTypeViewModel> GetAllDayType();
        List<BookingViewModel> GetBookingDetailsByGolferId(long golferId);
        List<MemberTypeViewModel> GetAllMemberType();
        bool CancelBooking(long bookingId, long uniqueSessionId);
        List<BookingStatusViewModel> GetAllBookingStatus();
        CouponViewModel GetCouponAmountByCouponCode(string code);
    }
}
