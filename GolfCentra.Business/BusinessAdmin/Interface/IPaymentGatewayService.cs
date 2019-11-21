using GolfCentra.ViewModel.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.Business.BusinessAdmin.Interface
{
  public  interface IPaymentGatewayService
    {
        void SavePaymentGatewayControl(PaymentGatewayControlViewModel paymentGatewayControlViewModel, long uniqueSessionId);
        List<PaymentGatewayControlViewModel> SearchAllPaymentGatewayControl();
        bool DeletePaymentGatewayControl(long id, long uniqueSessionId);
    }
}
