using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.ViewModel.Admin
{
    /// <summary>
    /// Properties For UpdatePricing's Operations Viz. Add, Edit, Delete, List And Etc.
    /// </summary>
    public class UpdatePricingViewModel
    {
        public long PricingId { get; set; }
        public long MemberTypeId { get; set; }
        public long BookingTypeId { get; set; }
        public long DayTypeId { get; set; }
        public long HoleTypeId { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public decimal GreenFee { get; set; }
        public decimal TaxAndFee { get; set; }
        public decimal AddOnCart { get; set; }
        public decimal AddOnCaddie { get; set; }
        public decimal ConvenienceFee { get; set; }
        public decimal RangeFee { get; set; }
        public decimal BucketFeePerPlayer { get; set; }
        public int NumberOfAllowedSessions { get; set; }

        public bool Member { get; set; }
        public bool NonMember { get; set; }
        public long? SlotId { get; set; }

        public ApiClientViewModel ApiClientViewModel { get; set; }

        public WEH9UpdatePriceViewModel WEH9UpdatePriceViewModel { get; set; }
        public WEH18UpdatePriceViewModel WEH18UpdatePriceViewModel { get; set; }
        public WDH18UpdatePriceViewModel WDH18UpdatePriceViewModel { get; set; }
        public WDH9UpdatePriceViewModel WDH9UpdatePriceViewModel { get; set; }
        public WDDRUpdatePriceViewModel WDDRUpdatePriceViewModel { get; set; }
        public WEDRUpdatePriceViewModel WEDRUpdatePriceViewModel { get; set; }

        public WEH27UpdatePriceViewModel WEH27UpdatePriceViewModel { get; set; }
        public WDH27UpdatePriceViewModel WDH27UpdatePriceViewModel { get; set; }

        public string MemberType { get; set; }
        public string SlotTime { get; set; }

    }
}
