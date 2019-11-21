using GolfCentra.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.Business.Business.Interface
{
    public interface IPromotionsCouponService
    {

        CouponViewModel GetCouponAmountByCouponCode(string code);
        List<PromotionViewModel> GetAllActivePromotion();
    }
}
