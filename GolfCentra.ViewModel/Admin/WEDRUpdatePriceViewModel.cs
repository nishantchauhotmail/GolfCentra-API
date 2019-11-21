using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.ViewModel.Admin
{
 public   class WEDRUpdatePriceViewModel
    {
        public decimal TaxAndFee { get; set; }
        public decimal BucketFeePerPlayer { get; set; }
        public decimal ConvenienceFee { get; set; }
        public decimal RangeFee { get; set; }
    }
}
