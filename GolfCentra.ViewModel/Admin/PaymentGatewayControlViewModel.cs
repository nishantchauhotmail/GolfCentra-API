using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.ViewModel.Admin
{
   public class PaymentGatewayControlViewModel
    {
        public long PaymentGatewayControlId { get; set; }
        public bool PaymentGatewayEnable { get; set; }
        public bool GreenFee { get; set; }
        public bool CartFee { get; set; }
        public bool CaddieFee { get; set; }
        public string EquipmentPriceOffIds { get; set; }
        public bool AllMembers { get; set; }
        public string SelectedGolferIds { get; set; }
        public string EquipmentName { get; set; }
        public string GolferNames { get; set; }
        public ApiClientViewModel ApiClientViewModel { get; set; }
    }
}
