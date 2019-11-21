using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.ViewModel.Admin
{
    /// <summary>
    /// Properties For Booking's Operations Viz. Add, Edit, Delete, List And Etc.
    /// </summary>
    public class BookingViewModel
    {
        public long GolferId { get; set; }
        public long BookingId { get; set; }
        public string Time { get; set; }
        public string TeeOffDate { get; set; }
        public int NoOfPlayer { get; set; }
        public long BookingTypeId { get; set; }
        public int NoOfBalls { get; set; }
        public int NoOfHole { get; set; }
        public decimal TotalAmount { get; set; }
        public string CurrencyName { get; set; }

        public string UserName { get; set; }
        public DateTime BookingDate { get; set; }

        public string BookingStatus { get; set; }
        public int NoOfRecord { get; set; }
        public int NoOfMemberPlayer { get; set; }
        public int NoOfNonMemberPlayer { get; set; }

        public decimal CartFee { get; set; }
        public decimal CaddieFee { get; set; }
        public decimal GreenFee { get; set; }
        public decimal RangeFee { get; set; }
        public decimal BallFee { get; set; }
        public decimal Discount { get; set; }
        public bool OnSpot { get; set; }
        public string PaymentMode { get; set; }
        public int NoOfCart { get; set; }
        public int NoOfCaddie { get; set; }

        public ApiClientViewModel ApiClientViewModel { get; set; }

        //Search
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int SearchTypeId { get; set; }

        public string PaymentGatewayBookingId { get; set; }
        public decimal ConvertedAmount { get; set; }
        public List<MemberTypeViewModel> MemberTypeViewModels { get; set; }
        public List<BookingEquipmentMappingViewModel> BookingEquipmentMappingViewModels { get; set; }

        public long CoursePairingId { get; set; }
        public String CoursePairingName { get; set; }
        public long BookingStatusId { get; set; }

        public BookingPlayerDetailViewModel BookingPlayerDetailViewModel { get; set; }
        public DateTime PlayingDate { get; set; }
        public PromotionViewModel PromotionViewModel { get; set; }

        public decimal PaidAmount { get; set; }
    }
}
