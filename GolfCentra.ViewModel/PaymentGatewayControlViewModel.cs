using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.ViewModel
{
    public class PaymentGatewayControlViewModel
    {
        public PaymentGatewayControlViewModel()
        {
            PaymentGatewayControlId = 0;
            PaymentGatewayEnable = true;
            GreenFee = false;
            CartFee = false;
            CaddieFee = false;
            EquipmentPriceOffIds = new List<string>();
            AllMembers = true;
            SelectedGolferIds = SelectedGolferIds;
        }
        public long PaymentGatewayControlId { get; set; }
        public bool PaymentGatewayEnable { get; set; }
        public bool GreenFee { get; set; }
        public bool CartFee { get; set; }
        public bool CaddieFee { get; set; }
        public List<string> EquipmentPriceOffIds { get; set; }
        public bool AllMembers { get; set; }
        public string SelectedGolferIds { get; set; }
    }
}
