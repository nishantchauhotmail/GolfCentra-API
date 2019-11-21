using GolfCentra.ViewModel.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.Business.BusinessAdmin.Interface
{
  public  interface ICouponService
    {
        bool SaveCouponDetail(CouponViewModel couponViewModel, long uniqueSessionId);
        bool DeleteCoupon(long couponId, long uniqueSessionId);
        List<CouponViewModel> GetAllActiveCoupon();
    }
}
