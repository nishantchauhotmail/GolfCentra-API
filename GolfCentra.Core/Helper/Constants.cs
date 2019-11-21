using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.Core.Helper
{
    public class Constants
    {

        public static class Url
        {
            public static string WebApiUrlWithoutSlash = ConfigurationManager.AppSettings[nameof(WebApiUrlWithoutSlash)];
            public static string UserPanelUrlWithoutSlash = ConfigurationManager.AppSettings[nameof(UserPanelUrlWithoutSlash)];
        }

        public static class Common
        {
            public static string AppName = ConfigurationManager.AppSettings[nameof(AppName)];
        }

        public static class Smtp
        {
            public static string SmtpHost = ConfigurationManager.AppSettings[nameof(SmtpHost)];
            public static string SmtpEmail = ConfigurationManager.AppSettings[nameof(SmtpEmail)];
            public static string SmtpPassword = ConfigurationManager.AppSettings[nameof(SmtpPassword)];
            public static int SmtpPort = Convert.ToInt32(ConfigurationManager.AppSettings[nameof(SmtpPort)]);
            public static bool SmtpSSL = Convert.ToBoolean(ConfigurationManager.AppSettings[nameof(SmtpSSL)]);

        }

        public static class MailId
        {
            public static string FromMails = ConfigurationManager.AppSettings[nameof(FromMails)];
            public static string CCMails = ConfigurationManager.AppSettings["CCMail"];

        }

        public static class TimeZone
        {
            public static string TimeZoneName = ConfigurationManager.AppSettings[nameof(TimeZoneName)];

        }

        public static class BookingKey
        {
            public static string BookingKeyId = ConfigurationManager.AppSettings[nameof(BookingKey)];

        }

        public static class StrMessage
        {
            public static string InValidAccess = "Invalid access details, Please log-out.";

        }

        public static class Currency
        {
            public static string CurrencyName = ConfigurationManager.AppSettings[nameof(CurrencyName)];
            public static string ConvertedCurrencyName = ConfigurationManager.AppSettings["ConvertedCurrencyName"];
            public static string ConvertCurrencyBaseURL = ConfigurationManager.AppSettings["ConvertCurrencyBaseURL"];
            public static string ConvertCurrencyRelativeURL = ConfigurationManager.AppSettings["ConvertCurrencyRelativeURL"];
            public static bool CurrencyExchangeEnable = Convert.ToBoolean(ConfigurationManager.AppSettings["CurrencyExchangeEnable"]);

        }


        public static class PlayerCount
        {
            public static int MinPlayerCount = Convert.ToInt32(ConfigurationManager.AppSettings["MinPlayerCount"]);
            public static int WeekendMinPlayerCount = Convert.ToInt32(ConfigurationManager.AppSettings["WeekendMinPlayerCount"]);
        }

        public static class FireBase
        {
            public static string FireBaseServerKey = ConfigurationManager.AppSettings["FireBaseServerKey"];
        }
        public static class BookingDetails
        {
            public static int AdvanceBookingDays = Convert.ToInt32(ConfigurationManager.AppSettings["AdvanceBookingDays"]);
            public static string DayOffIds = ConfigurationManager.AppSettings["DayOffIds"];

        }

        /// <summary>
        ///  Security Key For Data Encrypt And Decrypt
        /// </summary>
        public static class Keys
        {
            public static string SaltKey = System.Configuration.ConfigurationManager.AppSettings["SaltKey"];
            public static string SharedKey = System.Configuration.ConfigurationManager.AppSettings["SharedKey"];
        }

        public static class ErrorDisplay
        {
            public static bool IsErrorDisplay = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["IsErrorDisplay"]);       
        }
    }
}
