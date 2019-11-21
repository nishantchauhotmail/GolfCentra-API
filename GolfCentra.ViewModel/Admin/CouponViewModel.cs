using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.ViewModel.Admin
{
    /// <summary>
    /// Properties For Coupon's Operations Viz. Add, Edit, Delete, List And Etc.
    /// </summary>
    public class CouponViewModel
    {
        public long CouponId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public decimal Amount { get; set; }
        public bool Status { get; set; }
        public ApiClientViewModel ApiClientViewModel { get; set; }
        public long CouponTypeId { get; set; }
        public decimal Value { get; set; }
        public bool IsFlat { get; set; }

    }
}
