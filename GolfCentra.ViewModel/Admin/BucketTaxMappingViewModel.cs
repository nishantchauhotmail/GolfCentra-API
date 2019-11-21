using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.ViewModel.Admin
{
    /// <summary>
    /// Properties For BucketTax Mapping's Operations Viz. Add, Edit, Delete, List And Etc.
    /// </summary>
    public class BucketTaxMappingViewModel
    {
        public long BucketTaxMappingId { get; set; }
        public long TaxId { get; set; }
        public long BucketId { get; set; }
        public string TaxName { get; set; }
        public decimal TaxPercentage { get; set; }
    }
}
