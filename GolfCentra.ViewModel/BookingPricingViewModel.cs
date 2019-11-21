using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.ViewModel
{
    /// <summary>
    /// Properties For Booking Pricing's Operations Viz. Add, Edit, Delete, List And Etc.
    /// </summary>
    public class BookingPricingViewModel
    {
        public BookingPricingViewModel()
        {
            SlotId = 0;
            SlotTime = string.Empty;
            HoleTypeId = 0;
            HoleTypeName = string.Empty;
            GreenFeeMember = 0;
            GreenFeeNonMember = 0;
            NoOfPlayer = 0;
            NoOfMemberPlayer = 0;
            NoOfNonMemberPlayer = 0;
            TaxAndFee = 0;
            Caddie18HolePrice = 0;
            Caddie9HolePrice = 0;
            Cart18HolePrice = 0;
            Cart9HolePrice = 0;
            Date = "";
            TotalPrice = 0;
            CurrencyName = string.Empty;
            RangeFeeMember = 0;
            RangeFeeNonMember = 0;
            BallPrice = 0;
            NoOfBalls = 0;
            Cart27HolePrice = 0;
            Caddie27HolePrice = 0;
            MemberTypeViewModels = new List<MemberTypeViewModel>();
            CourseTaxMappingViewModels = new List<CourseTaxMappingViewModel>();
            IsMember = false;
            BookingEquipmentMappingViewModels = new List<BookingEquipmentMappingViewModel>();
            CoursePairingId = 0;
            CoursePairingName = string.Empty;
            BookingPlayerDetailViewModels = new List<BookingPlayerDetailViewModel>();
            PromotionPrice = 0;
            PromotionsId = 0;
            PaymentGatewayControlViewModel = new PaymentGatewayControlViewModel();
        }
        public long SlotId { get; set; }
        public string SlotTime { get; set; }
        public long HoleTypeId { get; set; }
        public string HoleTypeName { get; set; }
        public decimal GreenFeeMember { get; set; }
        public decimal GreenFeeNonMember { get; set; }
        public int NoOfPlayer { get; set; }
        public int NoOfMemberPlayer { get; set; }
        public int NoOfNonMemberPlayer { get; set; }
        public decimal TaxAndFee { get; set; }
        public decimal Caddie9HolePrice { get; set; }
        public decimal Caddie18HolePrice { get; set; }
        public decimal Cart9HolePrice { get; set; }
        public decimal Cart18HolePrice { get; set; }
        public string Date { get; set; }
        public decimal TotalPrice { get; set; }
        public string CurrencyName { get; set; }
        public decimal RangeFeeMember { get; set; }
        public decimal RangeFeeNonMember { get; set; }
        public decimal BallPrice { get; set; }
        public int NoOfBalls { get; set; }
        public decimal Caddie27HolePrice { get; set; }
        public decimal Cart27HolePrice { get; set; }
        public List<MemberTypeViewModel> MemberTypeViewModels { get; set; }
        public List<CourseTaxMappingViewModel> CourseTaxMappingViewModels { get; set; }
        public bool IsMember { get; set; }
        public List<BookingEquipmentMappingViewModel> BookingEquipmentMappingViewModels { get; set; }
        public long CoursePairingId { get; set; }
        public string CoursePairingName { get; set; }
        public List<BookingPlayerDetailViewModel> BookingPlayerDetailViewModels { get; set; }
        public decimal PromotionPrice { get; set; }
        public long PromotionsId { get; set; }
        public PaymentGatewayControlViewModel PaymentGatewayControlViewModel { get; set; }
    }
}
