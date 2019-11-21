using GolfCentra.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.Business.Business.Interface
{
    public interface IPaytmPaymentService
    {
        PaytmCheckSumViewModel GeneratePaytmCheckSum(PaytmCheckSumViewModel paytmCheckSumViewModel);
        PaytmCheckSumViewModel GetVerifyCheckSum(PaytmCheckSumViewModel paytmCheckSumViewModel);
    }
}
