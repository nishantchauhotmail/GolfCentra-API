using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.ViewModel.Admin
{
    /// <summary>
    /// Properties For Pricing's Operations Viz. Add, Edit, Delete, List And Etc.
    /// </summary>
    public class PricingViewModel
    {
        public long PricingId { get; set; }
        public long BookingTypeId { get; set; }
        public long DayTypeId { get; set; }
        public long MemberTypeId { get; set; }
        public long HoleTypeId { get; set; }
        public long SlotId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal RangeFee { get; set; }
        public decimal GreenFee { get; set; }
        public decimal AddOnCaddie { get; set; }
        public decimal AddOnCart { get; set; }

        public List<PricingTaxMappingViewModel> PricingTaxMappingViewModels { get; set; }
        public bool IsSpecialPricing { get; set; }
        public ApiClientViewModel ApiClientViewModel { get; set; }
        public long[] RangeFeeTax { get; set; }
        public long[] GreenFeeTax { get; set; }
        public long[] AddOnCaddieTax { get; set; }
        public long[] AddOnCartTax { get; set; }
        public string SlotTime { get; set; }


        public string BookingTypeName { get; set; }
        public string DayTypeName { get; set; }
        public string MemberTypeName { get; set; }
        public string HoleTypeName { get; set; }
        public long TimeFormatId { get; set; }
        public string TimeFormatName { get; set; }

        public long[] DayArray { get; set; }
        public long[] MemberTypeArray { get; set; }
        public long[] HoleTypeArray { get; set; }
        public long[] SlotArray { get; set; }
        public long[] TimeFormatArray { get; set; }
    }
}
