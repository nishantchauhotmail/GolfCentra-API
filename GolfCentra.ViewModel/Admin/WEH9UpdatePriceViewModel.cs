using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.ViewModel.Admin
{
    public class WEH9UpdatePriceViewModel
    {
        public decimal GreenFee { get; set; }
        public decimal TaxAndFee { get; set; }
        public decimal AddOnCart { get; set; }
        public decimal AddOnCaddie { get; set; }
        public decimal ConvenienceFee { get; set; }
        public decimal RangeFee { get; set; }
    }
}
