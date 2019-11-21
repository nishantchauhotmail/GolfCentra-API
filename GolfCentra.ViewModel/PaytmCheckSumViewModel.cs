using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.ViewModel
{
    /// <summary>
    /// Properties For Paytm Check Sum's Operations Viz. Add, Edit, Delete, List And Etc.
    /// </summary>
    public class PaytmCheckSumViewModel
    {
        public PaytmCheckSumViewModel()
        {
            MID = string.Empty;
            MERCHANT_KEY = string.Empty;
            PaytmCheckSum = string.Empty;
            CUST_ID = string.Empty;
            ORDER_ID = string.Empty;
            EMAIL = string.Empty;
            TXN_AMOUNT = string.Empty;
            MOBILE_NO = string.Empty;
            CHANNEL_ID = string.Empty;
            INDUSTRY_TYPE_ID = string.Empty;
            WEBSITE = string.Empty;
            CALLBACK_URL = string.Empty;
            parameter = new Dictionary<string, string>();
            ApiPassword = string.Empty;
            ApiUserName = string.Empty;
        }
        public string MID { get; set; }
        public string MERCHANT_KEY { get; set; }
        public string PaytmCheckSum { get; set; }
        public bool VerifyCheckSum { get; set; }
        public string CUST_ID { get; set; }
        public string ORDER_ID { get; set; }
        public string TXN_AMOUNT { get; set; }
        public string EMAIL { get; set; }
        public string MOBILE_NO { get; set; }
        public string CHANNEL_ID { get; set; }
        public string INDUSTRY_TYPE_ID { get; set; }
        public string WEBSITE { get; set; }
        public string CALLBACK_URL { get; set; }

        public Dictionary<string, string> parameter { get; set; }
        public string ApiUserName { get; set; }
        public string ApiPassword { get; set; }
    }
}
