using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.Core.Helper
{
    /// <summary>
    /// Connstant File For DB Master Table & Other Constant Type
    /// </summary>
    public static class Enum
    {
        /// <summary>
        /// Booking Type Id
        /// </summary>
        public enum EnumBookingType
        {
            BTT = 1,
            BDR = 2,
        }

        /// <summary>
        /// booking Status Type Id 
        /// </summary>
        public enum EnumBookingStatus
        {
            Confirm = 1,
            Pending = 2,
            Cancelled = 3,
            Failed = 4,
        }

        /// <summary>
        /// Search booking Id for APP
        /// </summary>
        public enum EnumSearchBooking
        {
            Completed = 1,
            Upcoming = 2,
            Today = 3,
            Cancelled = 4,
        }


        /// <summary>
        /// Day Type Id
        /// </summary>
        public enum EnumDayType
        {

            Monday = 1,
            Tuesday = 2,
            Wednesday = 3,
            Thursday = 4,
            Friday = 5,
            Saturday = 6,
            Sunday = 7,
            NationalHoliday = 8,
            WeekDay = 10,
            Weekend = 11,
        }

        /// <summary>
        /// Hole Type Id
        /// </summary>
        public enum EnumHoleType
        {
            Hole9 = 1,
            Hole18 = 2,
            Hole27 = 3
        }

        /// <summary>
        /// Member Type
        /// </summary>
        public enum EnumMemberType
        {
            Member = 1,
            NonMember = 2,
        }

        /// <summary>
        /// Socre Search Enum from Admi Panel
        /// </summary>
        public enum EnumAdminScoreSearchType
        {
            Date = 1,
            EmailDate = 2,
            DateFilter = 3,
            EmailArray = 4
        }

        /// <summary>
        /// Payment Status Id
        /// </summary>
        public enum EnumPaymentStatus
        {
            Pending = 1,
            Success = 2,
            Failed = 3,
        }

        /// <summary>
        /// Platform Type Id
        /// </summary>
        public enum EnumPlatformType
        {
            IOS = 1,
            AND = 2,
            Web = 3,
            UserPanel = 5
        }

        /// <summary>
        /// Admin Report From Admin Panel
        /// </summary>
        public enum EnumAdminReportSearch
        {
            All = 1,
            FromApp = 2,
            OnSpot = 3

        }

        /// <summary>
        /// Course nameing for different tee
        /// </summary>
        public enum EnumCourseHoleTypeId
        {
            RidgeValley = 1,
            ValleyCanyon = 2
        }

        /// <summary>
        /// Fee Category Id
        /// </summary>
        public enum EnumFeeCategory
        {
            GreenFee = 1,
            RangeFee = 2,
            AddOnCaddie = 3,
            AddOnCart = 4
        }

        /// <summary>
        /// Time Format Type Id
        /// </summary>
        public enum EnumTimeFormatType
        {
            AM = 1,
            PM = 2,
        }

        public enum EnumCourseNameTypeId
        {
            Ridge = 1,
            Valley = 2,
            Canyon = 3
        }
    }
}
