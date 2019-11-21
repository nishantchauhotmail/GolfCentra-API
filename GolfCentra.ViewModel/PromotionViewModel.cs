using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.ViewModel
{
    public class PromotionViewModel
    {
        public PromotionViewModel()
        {
            PromotionsId = 0;
            Name = string.Empty;
            GreenFee = false;
            CaddieFee = false;
            CartFee = false;
            Price = 0;
            Extra = string.Empty;
            EquipmentName = string.Empty;
            HoleTypeName = string.Empty;

        }
        public long PromotionsId { get; set; }
        public string Name { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public System.TimeSpan StartTime { get; set; }
        public System.TimeSpan EndTime { get; set; }
        public long HoleTypeId { get; set; }
        public Nullable<bool> GreenFee { get; set; }
        public Nullable<bool> CaddieFee { get; set; }
        public Nullable<bool> CartFee { get; set; }
        public Nullable<decimal> Price { get; set; }
        public string Extra { get; set; }
        public String EquipmentName { get; set; }
        public string HoleTypeName { get; set; }
    }
}
