using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GolfCentra.Core.Helper
{
    public static class DateHelper
    {
        /// <summary>
        /// Get Day Id  From Date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static long GetDayTypeFromDate(DateTime date)
        {
            string dayName = date.DayOfWeek.ToString();
            switch (dayName.ToLower())
            {
                case "monday":
                    return (int)Core.Helper.Enum.EnumDayType.Monday;

                case "tuesday":
                    return (int)Core.Helper.Enum.EnumDayType.Tuesday;
                case "wednesday":
                    return (int)Core.Helper.Enum.EnumDayType.Wednesday;
                case "thursday":
                    return (int)Core.Helper.Enum.EnumDayType.Thursday;
                case "friday":
                    return (int)Core.Helper.Enum.EnumDayType.Friday;

                case "saturday":
                    return (int)Core.Helper.Enum.EnumDayType.Saturday;
                case "sunday":
                    return (int)Core.Helper.Enum.EnumDayType.Sunday;
            }
            return 0;
        }

        /// <summary>
        /// Convert DateTime To  TimeZone DateTime
        /// </summary>
        /// <returns></returns>
        public static DateTime ConvertSystemDate()
        {
            TimeZoneInfo Time_ZONE = TimeZoneInfo.FindSystemTimeZoneById(Constants.TimeZone.TimeZoneName);

            DateTime convertedTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, Time_ZONE);
            return convertedTime;
        }

        /// <summary>
        /// Convert DateTime To  Current time
        /// </summary>
        /// <returns></returns>
        public static DateTime ConvertSystemDateToCurrent(DateTime dateTime)
        {
            TimeZoneInfo Time_ZONE = TimeZoneInfo.FindSystemTimeZoneById(Constants.TimeZone.TimeZoneName);

            DateTime convertedTime = TimeZoneInfo.ConvertTimeFromUtc(dateTime, Time_ZONE);
            return convertedTime;
        }


        /// <summary>
        /// booking blocked day 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static bool BookingNotAllowedOnDay(DateTime date)
        {
            string dayName = date.DayOfWeek.ToString();
            if (dayName.ToLower() == "monday")
            {
                return false;
            }
            else { return true; }

        }


       
    }
}
