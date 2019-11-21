using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.ViewModel
{
    public class CouponViewModel
    {
        public CouponViewModel()
        {
            Value = 0;
            IsFlat = false;
        }
        public decimal Value { get; set; }
        public bool IsFlat { get; set; }
    }
}
