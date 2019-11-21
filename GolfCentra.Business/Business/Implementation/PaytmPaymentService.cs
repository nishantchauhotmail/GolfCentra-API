using GolfCentra.Business.Business.Interface;
using GolfCentra.ViewModel;
using paytm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.Business.Business.Implementation
{
    public class PaytmPaymentService : IPaytmPaymentService
    {
        /// <summary>
        /// Generate CheckSum For Paytm
        /// </summary>
        /// <param name="paytmCheckSumViewModel"></param>
        /// <returns></returns>
        public PaytmCheckSumViewModel GeneratePaytmCheckSum(PaytmCheckSumViewModel paytmCheckSumViewModel)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                { "MID", paytmCheckSumViewModel.MID },
                { "CHANNEL_ID", paytmCheckSumViewModel.CHANNEL_ID },
                { "INDUSTRY_TYPE_ID", paytmCheckSumViewModel.INDUSTRY_TYPE_ID },
                { "WEBSITE", paytmCheckSumViewModel.WEBSITE },
                { "CALLBACK_URL", paytmCheckSumViewModel.CALLBACK_URL },
                { "EMAIL", paytmCheckSumViewModel.EMAIL },
                { "MOBILE_NO", paytmCheckSumViewModel.MOBILE_NO },
                { "CUST_ID", paytmCheckSumViewModel.CUST_ID },
                { "ORDER_ID", paytmCheckSumViewModel.ORDER_ID },
                { "TXN_AMOUNT", paytmCheckSumViewModel.TXN_AMOUNT }
            };
            PaytmCheckSumViewModel cc = new PaytmCheckSumViewModel();

            foreach (string key in parameters.Keys)
            {
                if (parameters[key].ToUpper().Contains("REFUND") || parameters[key].ToUpper().Contains("|"))
                {
                    parameters[key] = "";
                }
            }

            cc.PaytmCheckSum = CheckSum.generateCheckSum(paytmCheckSumViewModel.MERCHANT_KEY, parameters);
            cc.ORDER_ID = paytmCheckSumViewModel.ORDER_ID;
            cc.parameter = parameters;
            return cc;
        }

        /// <summary>
        /// Verfied CheckSum For Paytm
        /// </summary>
        /// <param name="paytmCheckSumViewModel"></param>
        /// <returns></returns>
        public PaytmCheckSumViewModel GetVerifyCheckSum(PaytmCheckSumViewModel paytmCheckSumViewModel)
        {
            PaytmCheckSumViewModel paytmCheckSumView = new PaytmCheckSumViewModel();

            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                { "MID", paytmCheckSumViewModel.MID },
                { "CHANNEL_ID", paytmCheckSumViewModel.CHANNEL_ID },
                { "INDUSTRY_TYPE_ID", paytmCheckSumViewModel.INDUSTRY_TYPE_ID },
                { "WEBSITE", paytmCheckSumViewModel.WEBSITE },
                { "EMAIL", paytmCheckSumViewModel.EMAIL },
                { "MOBILE_NO", paytmCheckSumViewModel.MOBILE_NO },
                { "CUST_ID", paytmCheckSumViewModel.CUST_ID },
                { "ORDER_ID", paytmCheckSumViewModel.ORDER_ID },
                { "TXN_AMOUNT", paytmCheckSumViewModel.TXN_AMOUNT }
            };

            paytmCheckSumView.VerifyCheckSum = CheckSum.verifyCheckSum(paytmCheckSumViewModel.MERCHANT_KEY, parameters, paytmCheckSumViewModel.PaytmCheckSum);
            paytmCheckSumView.ORDER_ID = paytmCheckSumViewModel.ORDER_ID;
            return paytmCheckSumView;
        }
    }
}
