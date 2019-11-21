using paytm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GolfCentra.Controllers
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            String merchantKey = "_RlqaRi&ndfVzUGJ";
            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                //{ "MID", "Studen73239122403895" },
                //{ "CHANNEL_ID", "WAP" },
                //{ "INDUSTRY_TYPE_ID", "Retail" },
                //{ "WEBSITE", "APP_STAGING" },
                //{ "EMAIL", "harvinder.athwal65@gmail.com" },
                //{ "MOBILE_NO", "8059120207" },
                //{ "CUST_ID", "3" },
                //{ "ORDER_ID", "124" },
                //{ "TXN_AMOUNT", "1" },
                //{ "CALLBACK_URL", "https://securegw-stage.paytm.in/theia/paytmCallback?ORDER_ID=124" }


                 { "MID", "Golfla03291372924605" },
                { "CHANNEL_ID", "WAP" },
                { "INDUSTRY_TYPE_ID", "Retail116" },
                { "WEBSITE", "GolflanTWAP" },
                { "EMAIL", "harvinder.athwal65@gmail.com" },
                { "MOBILE_NO", "8059120207" },
                { "CUST_ID", "TestingNGCRVS1531458702124" },
                { "ORDER_ID", "TedstingsNGCRVSq209" },
                { "TXN_AMOUNT", "100" },
                { "CALLBACK_URL", "http://localhost:58642/" }
            };
            //foreach (string key in parameters.Keys)
            //{
            //    if (parameters[key].ToUpper().Contains("REFUND") || parameters[key].ToUpper().Contains("|"))
            //    {
            //        parameters[key] = "";
            //    }
            //}

              var checksum = CheckSum.generateCheckSum(merchantKey, parameters);
            //var checksum = "bckV2QtoNBkoyfoOt1JCnwwQornQwBs2Y+C0nhxxlW1YmLoGH8GxyxLQNOjmV1ICXB7etN4NLSJoQ61tzP3Fz1pIEVVZEwDduMWmzue7hjM=";
            string paytmURL = "https://securegw.paytm.in/theia/processTransaction?orderid=" + "TedstingsNGCRVSq209";

            string outputHTML = "<html>";
            outputHTML += "<head>";
            outputHTML += "<title>Merchant Check Out Page</title>";
            outputHTML += "</head>";
            outputHTML += "<body>";
            outputHTML += "<center><h1>Please do not refresh this page...</h1></center>";
            outputHTML += "<form method='post' action='" + paytmURL + "' name='f1'>";
            outputHTML += "<table border='1'>";
            outputHTML += "<tbody>";
            foreach (string key in parameters.Keys)
            {
                outputHTML += "<input type='hidden' name='" + key + "' value='" + parameters[key] + "'>";
            }
            outputHTML += "<input type='hidden' name='CHECKSUMHASH' value='" + checksum + "'>";
            outputHTML += "</tbody>";
            outputHTML += "</table>";
            outputHTML += "<script type='text/javascript'>";
            outputHTML += "document.f1.submit();";
            outputHTML += "</script>";
            outputHTML += "</form>";
            outputHTML += "</body>";
            outputHTML += "</html>";
            Response.Write(outputHTML);
        }
    }
}