using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.ViewModel.Admin
{
    /// <summary>
    /// Properties For Bucket's Operations Viz. Add, Edit, Delete, List And Etc.
    /// </summary>
    public class BucketViewModel
    {
        public long BucketDetailId { get; set; }
        public int Balls { get; set; }
        public decimal Price { get; set; }
        public Nullable<long> CurrencyId { get; set; }
        public Nullable<long> DayTypeId { get; set; }
        public Nullable<long> MemberTypeId { get; set; }
        public ApiClientViewModel ApiClientViewModel { get; set; }
        public List<BucketTaxMappingViewModel> BucketTaxMappingViewModels { get; set; }
        public long[] Taxs { get; set; }

        public string DayTypeName { get; set; }
        public string MemberTypeName{ get; set; }

        public DateTime Date { get; set; }
    }
}
