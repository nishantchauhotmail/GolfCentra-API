using GolfCentra.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.Business.Business.Interface
{
    public interface IBookingService
    {
        List<BookingViewModel> GetBookingsByBookingStatus(long golferId, long bookingSearchTypeId);
        BookingViewModel GetBookingDetailsByBookingIdAndGolferId(long bookingId, long golferId);
        bool CancelBookingByGolfer(long bookingId, long golferId);
        decimal GetCouponAmountByCouponCode(string code);
        List<String> GetRateCardByDateAndSlot(DateTime date, long slotId, long bookingTypeId, long golferId);
        List<SessionSlotViewModel> GetSlotDetailsByDateAndBookingType(DateTime date, long bookingTypeId, long? coursePairingId);
        BookingPricingViewModel GetPricingCalculation(long holeTypeId, DateTime date, long slotId, int TotalNoOfPlayer, int noOfMemberPlayer, int noOfNonMemberPlayer, List<MemberTypeViewModel> memberTypeViewModels,long golferId, long? coursePairingId, List<BookingPlayerDetailViewModel> bookingPlayerDetailViewModels,long promotionsId);
        BookingViewModel SaveTeeTimeBooking(CommonViewModel commonViewModel);
        List<BucketViewModel> GetBucketDetailList(DateTime date, long golferId);
        BookingPricingViewModel GetPricingCalculationBDT(long bucketTypeId, DateTime date, long slotId, int TotalNoOfPlayer, int noOfMemberPlayer, int noOfNonMemberPlayer, List<MemberTypeViewModel> memberTypeViewModels,long golferId);
        BookingViewModel SaveDrivingRangeBooking(CommonViewModel commonViewModel);
        Tuple<bool, string> UpdateBookingStatus(CommonViewModel commonViewModel);
        bool CheckNationalHolidayOrWeekend(DateTime date);
        List<MemberTypeViewModel> GetAllMemberType(long golferId);
        SlotViewModel GetSlotDetailsByDateAndBookingType(DateTime date, long bookingTypeId, long? coursePairingId, long golferId, long? promotionId);
        BookingConditionViewModel BookingSetting(long bookingTypeId);
        bool CheckMemberShipId(CommonViewModel golferViewModel);
    }
}
