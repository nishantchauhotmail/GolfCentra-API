using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.ViewModel.Admin
{
    /// <summary>
    /// Properties For EquipmentTaxMappig's Operations Viz. Add, Edit, Delete, List And Etc.
    /// </summary>
    public class EquipmentTaxMappingViewModel
    {
        public long EquipmentTaxMappingId { get; set; }
        public long TaxId { get; set; }
        public long EquipmentId { get; set; }
        public string TaxName { get; set; }
        public decimal TaxPercentage { get; set; }
    }
}
