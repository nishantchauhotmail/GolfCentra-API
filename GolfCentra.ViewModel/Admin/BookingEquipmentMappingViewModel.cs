using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.ViewModel.Admin
{
    public class BookingEquipmentMappingViewModel
    {
        public BookingEquipmentMappingViewModel()
        {
            BookingId = 0;
            BookingEquipmentMappingId = 0;
            EquipmentId = 0;
            EquipmentName = string.Empty;
            EquipmentPrice = 0;
            EquipmentCount = 0;
            EquipmentLeft = 0;
        }
        public long BookingEquipmentMappingId { get; set; }
        public long BookingId { get; set; }
        public long EquipmentId { get; set; }
        public string EquipmentName { get; set; }
        public Nullable<decimal> EquipmentPrice { get; set; }
        public int EquipmentCount { get; set; }
        public int EquipmentLeft { get; set; }
    }
}
