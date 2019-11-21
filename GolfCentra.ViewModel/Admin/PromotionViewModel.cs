using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.ViewModel.Admin
{
  public  class PromotionViewModel
    {
        public long PromotionsId { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public System.TimeSpan StartTime { get; set; }
        public System.TimeSpan EndTime { get; set; }
        public long HoleTypeId { get; set; }
        public Nullable<bool> GreenFee { get; set; }
        public Nullable<bool> CaddieFee { get; set; }
        public Nullable<bool> CartFee { get; set; }
        public Nullable<decimal> Price { get; set; }
        public string Extra { get; set; }
        public String EquipmentName { get; set; }
        public long[] Taxs { get; set; }
      
        public long[] EquipmentIds { get; set; }

        public ApiClientViewModel ApiClientViewModel { get; set; }
      
        public string HoleTypeName { get; set; }
    }
}
