using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.ViewModel
{
    /// <summary>
    /// Properties For API Request's Operations Viz. Add, Edit, Delete, List And Etc.
    /// </summary>
    public class CommonViewModel
    {
        public CommonViewModel()
        {
            UniqueSessionId = string.Empty;
            GrossScore = 0;
            NewPassword = string.Empty;
            OldPassword = string.Empty;
            ScoreId = 0;
            BookingId = 0;
            BookingSearchTypeId = 0;
            BookingTypeId = 0;
            SlotId = 0;
            Date = System.DateTime.UtcNow;
            CouponCode = string.Empty;
            HoleTypeId = 0;
            NoOfPlayer = 0;
            NoOfMemberPlayer = 0;
            NoOfNonMemberPlayer = 0;
            TotalScore = 0;
            ApiPassword = string.Empty;
            ApiUserName = string.Empty;
            ScoreViewModels = new List<ScoreViewModel>();
            CourseHoleTypeId = 0;
            CoursePairingId = 0;

        }
        public string UniqueSessionId { get; set; }
        public long ScoreId { get; set; }
        public long GrossScore { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public long BookingId { get; set; }
        public long BookingSearchTypeId { get; set; }
        public long BookingTypeId { get; set; }
        public long SlotId { get; set; }
        public DateTime Date { get; set; }
        public string CouponCode { get; set; }
        public long HoleTypeId { get; set; }
        public int NoOfPlayer { get; set; }
        public int NoOfMemberPlayer { get; set; }
        public int NoOfNonMemberPlayer { get; set; }
        public long TotalScore { get; set; }
        public string ApiUserName { get; set; }
        public string ApiPassword { get; set; }
        public List<ScoreViewModel> ScoreViewModels { get; set; }
        public string Time { get; set; }
        public int NoOfBalls { get; set; }
        public int NoOfHole { get; set; }
        public decimal TotalAmount { get; set; }
        public string CurrencyName { get; set; }




        public long GolferId { get; set; }
        public System.DateTime BookingDate { get; set; }

        public System.DateTime TeeOffDate { get; set; }
        public string TeeOffSlot { get; set; }
        public long BookingStatusId { get; set; }
        public int NoOfMember { get; set; }
        public int NoOfNonMember { get; set; }
        public Nullable<decimal> CartFee { get; set; }
        public int CartCount { get; set; }
        public Nullable<decimal> CaddieFee { get; set; }
        public int CaddieCount { get; set; }
        public Nullable<decimal> GreenFee { get; set; }
        public Nullable<decimal> RangeFee { get; set; }
        public Nullable<decimal> BallFee { get; set; }
        public Nullable<decimal> Discount { get; set; }
        public Nullable<decimal> FeeAndTaxes { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<bool> OnSpot { get; set; }
        public Nullable<long> CouponId { get; set; }
        public string PaymentGatewayBookingId { get; set; }
        public string PaymentMode { get; set; }
        public Nullable<long> PaymentStatusId { get; set; }

        public int BucketId { get; set; }

        public decimal AppVersion { get; set; }
        public string currencyISO { get; set; }
        public string baseCurrency { get; set; }

        //Golfer
        public string PhoneCode { get; set; }
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public string MobileNumber { get; set; }
        public string Password { get; set; }
        public string MemberShipId { get; set; }
        public string OTP { get; set; }
        public int PlatformTypeId { get; set; }

        public string MacAddress { get; set; }
        public string IpAddress { get; set; }

        public long CourseHoleTypeId { get; set; }
        public List<MemberTypeViewModel> MemberTypeViewModels { get; set; }
        public List<CourseTaxMappingViewModel> CourseTaxMappingViewModels { get; set; }
        public List<BookingEquipmentMappingViewModel> BookingEquipmentMappingViewModels { get; set; }

        public int CourseNameTypeId { get; set; }
        public long CoursePairingId { get; set; }

        public List<BookingPlayerDetailViewModel> BookingPlayerDetailViewModels { get; set; }

        public long PromotionsId { get; set; }
        public string DeviceTokenId { get; set; }
        public decimal PaidAmount { get; set; }
        public string SS { get; set; }
        public string ENB { get; set; }
        public string ENS { get; set; }
    }
}
