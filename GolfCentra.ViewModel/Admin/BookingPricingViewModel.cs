using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.ViewModel.Admin
{
    /// <summary>
    ///  /// <summary>
    /// Properties For Booking Pricing's Operations Viz. Add, Edit, Delete, List And Etc.
    /// </summary>
    /// </summary>
    public class BookingPricingViewModel
    {
        public long SlotId { get; set; }
        public string SlotTime { get; set; }
        public long HoleTypeId { get; set; }
        public string HoleTypeName { get; set; }
        public decimal GreenFee { get; set; }
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
        public decimal RangeFee{ get; set; }
        public decimal BallPrice { get; set; }
        public int NoOfBalls { get; set; }
        public DateTime SearchDate { get; set; }
        public long BucketTypeId { get; set; }
        public long BookingTypeId { get; set; }

        public decimal Caddie27HolePrice { get; set; }
        public decimal Cart27HolePrice { get; set; }


        public ApiClientViewModel ApiClientViewModel { get; set; }
        public List<MemberTypeViewModel> MemberTypeViewModels { get; set; }
        public List<CourseTaxMappingViewModel> CourseTaxMappingViewModels { get; set; }
        public long MemberTypeId { get; set; }
        public List<BookingEquipmentMappingViewModel> BookingEquipmentMappingViewModels { get; set; }
    }
}
