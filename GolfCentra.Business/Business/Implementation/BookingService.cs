using GolfCentra.Business.Business.Interface;
using GolfCentra.Core;
using GolfCentra.Core.DataBase;
using GolfCentra.Core.Helper;
using GolfCentra.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using static GolfCentra.Core.Helper.Enum;


namespace GolfCentra.Business.Business.Implementation
{
    public class BookingService : IBookingService
    {
        private readonly UnitOfWork _unitOfWork;

        public object Response { get; private set; }

        public BookingService()
        {
            _unitOfWork = new UnitOfWork();
        }

        /// <summary>
        /// Get Booking's By Differnt Search Parameter
        /// </summary>
        /// <param name="golferId"></param>
        /// <param name="bookingSearchTypeId"></param>
        /// <returns></returns>
        public List<BookingViewModel> GetBookingsByBookingStatus(long golferId, long bookingSearchTypeId)
        {
            List<Booking> bookings = new List<Booking>();
            List<BookingViewModel> bookingViewModels = new List<BookingViewModel>();
            DateTime currentDateTime = Core.Helper.DateHelper.ConvertSystemDate();
            DateTime startDate = new DateTime(currentDateTime.Year, currentDateTime.Month, currentDateTime.Day, 0, 0, 0);
            DateTime endDate = new DateTime(currentDateTime.Year, currentDateTime.Month, currentDateTime.Day, 23, 59, 59);
            if (bookingSearchTypeId == (int)Core.Helper.Enum.EnumSearchBooking.Cancelled)
            {
                bookings = _unitOfWork.BookingRepository.GetMany(x => x.GolferId == golferId && x.BookingStatusId == (int)Core.Helper.Enum.EnumBookingStatus.Cancelled && x.IsActive == true).OrderByDescending(x => x.UpdatedOn).ToList();
            }
            else
            {
                if (bookingSearchTypeId == (int)Core.Helper.Enum.EnumSearchBooking.Completed)
                {
                    bookings = _unitOfWork.BookingRepository.GetMany(x => x.GolferId == golferId && x.TeeOffDate < currentDateTime && TimeSpan.Parse(x.TeeOffSlot) <= TimeSpan.Parse(currentDateTime.Hour.ToString() + ":" + currentDateTime.Minute.ToString()) && x.BookingStatusId == (int)Core.Helper.Enum.EnumBookingStatus.Confirm && x.IsActive == true).OrderByDescending(x => x.TeeOffDate).ToList();
                }
                else if (bookingSearchTypeId == (int)Core.Helper.Enum.EnumSearchBooking.Upcoming)
                {
                    DateTime nextDay = currentDateTime.AddDays(1);
                    DateTime date = new DateTime(nextDay.Year, nextDay.Month, nextDay.Day, 0, 0, 0);
                    bookings = _unitOfWork.BookingRepository.GetMany(x => x.GolferId == golferId && x.TeeOffDate >= date && x.BookingStatusId == (int)Core.Helper.Enum.EnumBookingStatus.Confirm && x.IsActive == true).OrderBy(x => x.TeeOffDate).ToList();

                }
                else if (bookingSearchTypeId == (int)Core.Helper.Enum.EnumSearchBooking.Today)
                {

                    bookings = _unitOfWork.BookingRepository.GetMany(x => x.GolferId == golferId && x.TeeOffDate >= startDate && TimeSpan.Parse(x.TeeOffSlot) > TimeSpan.Parse(currentDateTime.Hour.ToString() + ":" + currentDateTime.Minute.ToString()) && x.TeeOffDate <= endDate && x.BookingStatusId == (int)Core.Helper.Enum.EnumBookingStatus.Confirm && x.IsActive == true).OrderBy(x => x.TeeOffSlot).ToList();
                }
            }
            foreach (Booking booking in bookings)
            {
                BookingViewModel bookingViewModel = new BookingViewModel()
                {
                    BookingId = booking.BookingId,
                    Time = booking.TeeOffSlot,
                    Date = booking.TeeOffDate.ToShortDateString(),
                    NoOfPlayer = booking.NoOfPlayer,
                    PaymentGatewayBookingId = booking.PaymentGatewayBookingId,
                    ENB = Core.Helper.Crypto.EncryptStringAES(booking.BookingId.ToString())
                };
                bookingViewModels.Add(bookingViewModel);
            };


            return bookingViewModels;
        }

        /// <summary>
        /// Get Booking Detail By Booking Id And Golfer Id
        /// </summary>
        /// <param name="bookingId"></param>
        /// <param name="golferId"></param>
        /// <returns></returns>
        public BookingViewModel GetBookingDetailsByBookingIdAndGolferId(long bookingId, long golferId)
        {
            BookingViewModel bookingViewModel = new BookingViewModel();
            Booking booking = _unitOfWork.BookingRepository.Get(x => x.GolferId == golferId && x.BookingId == bookingId && x.IsActive == true);

            if (booking != null)
            {
                CoursePairing coursePairing = _unitOfWork.CoursePairingRepository.Get(x => x.CoursePairingId == booking.CoursePairingId);

                bookingViewModel.BookingId = booking.BookingId;
                bookingViewModel.Time = booking.TeeOffSlot;
                bookingViewModel.Date = booking.TeeOffDate.ToShortDateString();
                bookingViewModel.NoOfPlayer = booking.NoOfPlayer;
                bookingViewModel.TotalAmount = booking.Amount.GetValueOrDefault();
                bookingViewModel.CurrencyName = Core.Helper.Constants.Currency.CurrencyName;
                bookingViewModel.BookingTypeId = booking.BookingTypeId;
                if (booking.BookingTypeId == (int)Core.Helper.Enum.EnumBookingType.BTT)
                    bookingViewModel.NoOfHole = _unitOfWork.HoleTypeRepository.Get(x => x.HoleTypeId == booking.HoleTypeId && x.IsActive == true).Value;
                bookingViewModel.NoOfBalls = booking.NoOfBalls.GetValueOrDefault();
                bookingViewModel.PaymentGatewayBookingId = booking.PaymentGatewayBookingId;
                bookingViewModel.CoursePairingId = booking.CoursePairingId.GetValueOrDefault();
                bookingViewModel.CoursePairingName = coursePairing != null ? (coursePairing.CourseName1.Value) + (coursePairing.EndCourseNameId != null ? " - " + coursePairing.CourseName.Value : "") : "";


            }
            return bookingViewModel;
        }

        /// <summary>
        /// Cancel Booking  By Booking Id
        /// </summary>
        /// <param name="bookingId"></param>
        /// <param name="golferId"></param>
        /// <returns></returns>
        public bool CancelBookingByGolfer(long bookingId, long golferId)
        {
            Booking booking = _unitOfWork.BookingRepository.Get(x => x.GolferId == golferId && x.BookingId == bookingId && x.IsActive == true);

            if (booking != null)
            {
                DateTime date = Core.Helper.DateHelper.ConvertSystemDateToCurrent(System.DateTime.UtcNow);
                if (new DateTime(date.Year, date.Month, date.Day, 12, 00, 00) <= booking.BookingDate.AddDays(-1))
                {
                    booking.BookingStatusId = (int)Core.Helper.Enum.EnumBookingStatus.Cancelled;
                    booking.UpdatedOn = System.DateTime.UtcNow;
                    _unitOfWork.BookingRepository.Update(booking);
                    _unitOfWork.Save();

                    string emailBody = "";
                    if (booking.BookingTypeId == (int)Core.Helper.Enum.EnumBookingType.BTT)
                    {


                        emailBody = "Dear Member,<br/><br/>Your booking is cancelled.<br/><br/>We look forward to welcoming you back soon!<br/><br/>Warm regards,<br/>Classic Golf & Country Club";

                        //string coursePairingName = booking.CoursePairingId != null ? (booking.CoursePairing.CourseName1.Value) + (booking.CoursePairing.EndCourseNameId != null ? " - " + booking.CoursePairing.CourseName.Value : "") : "";
                        //emailBody = "Dear Member, your tee time booking is cancelled as follows -<br/> Booking ID - " + booking.PaymentGatewayBookingId + "<br/> Date - " + booking.TeeOffDate.ToString("dd/MM/yyyy") + "<br/> Time - " + DateTime.Parse(booking.TeeOffSlot).ToString("hh:mm tt", CultureInfo.GetCultureInfo("en-US")) + "<br/> No. of Holes - " + booking.HoleType.Value + "<br/> Course Name - " + coursePairingName + "<br/>";
                        //foreach (var item in booking.BookingPlayerDetails)
                        //{
                        //    if (item.Player1 == null || item.Player1 == "")
                        //    {
                        //        emailBody = emailBody + "Player Name/Membership Number : " + item.Player1 + "<br/>";
                        //    }


                        //}
                        //emailBody = emailBody + "<br/> We look forward to seeing you at " + Core.Helper.Constants.Common.AppName + "!";

                    }
                    else
                    {

                        emailBody = "Dear Member, Your driving range booking is cancelled as follows -<br/> Booking ID - " + booking.PaymentGatewayBookingId + "<br/> Date - " + booking.TeeOffDate.ToString("dd/MM/yyyy") + "<br/> Time - " + DateTime.Parse(booking.TeeOffSlot).ToString("hh:mm tt", CultureInfo.GetCultureInfo("en-US")) + "<br/>";

                        foreach (var item in booking.BookingPlayerDetails)
                        {
                            if (item.Player1 == null || item.Player1 == "")
                            {
                                emailBody = emailBody + "Player Name/Membership Number : " + item.Player1 + "<br/>";
                            }


                        }
                        emailBody = emailBody + "<br/> We look forward to seeing you at " + Core.Helper.Constants.Common.AppName + "!";
                    }
                    MailerViewModel mailerViewModel = new MailerViewModel()
                    {
                        ToEmails = booking.Golfer.Email,
                        From = Constants.MailId.FromMails,
                        CCMail = Constants.MailId.CCMails,
                        Subject = "Cancel Booking For " + Constants.Common.AppName + " ",
                        Body = emailBody
                    };
                    EmailNotification emailNotification = new EmailNotification();
                    bool status = emailNotification.SendMail(mailerViewModel);


                }
                else
                {
                    throw new Exception("You Can Not Cancel Booking Now.");

                }
            }
            else
            {
                throw new Exception("No booking found for booking Id");
            }

            return true;
        }

        /// <summary>
        /// Get Amount By Coupon Code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public decimal GetCouponAmountByCouponCode(string code)
        {
            Coupon coupon = _unitOfWork.CouponRepository.Get(x => x.Code == code && x.IsActive == true);

            if (coupon == null) { throw new Exception("Invalid CouponCode."); }
            else
            {
                return coupon.Amount;
            }
        }

        /// <summary>
        /// Get Rate Card For BDT And BDR
        /// </summary>
        /// <param name="date"></param>
        /// <param name="slotId"></param>
        /// <param name="bookingTypeId"></param>
        /// <returns></returns>
        public List<String> GetRateCardByDateAndSlot(DateTime date, long slotId, long bookingTypeId, long golferId)
        {
            long timeFormatId = 0;
            Golfer golfer = _unitOfWork.GolferRepository.Get(x => x.GolferId == golferId);
            List<MemberType> memberTypes = new List<MemberType>();

            Course course = _unitOfWork.CourseRepository.Get(x => x.CourseId == 1);
            var time = _unitOfWork.SlotRepository.Get(x => x.SlotId == slotId && x.IsActive == true);
            if (time.Time >= TimeSpan.Parse("12:00")) { timeFormatId = (int)Core.Helper.Enum.EnumTimeFormatType.PM; }
            else { timeFormatId = (int)Core.Helper.Enum.EnumTimeFormatType.AM; }
            List<String> pricingString = new List<string>();

            long dayTypeId = GetDayTypeId(date);
            if (golfer.MemberTypeId == (int)Core.Helper.Enum.EnumMemberType.NonMember)
            {

                memberTypes = _unitOfWork.MemberTypeRepository.GetMany(x => x.IsActive == true && x.MemberTypeId == (int)Core.Helper.Enum.EnumMemberType.NonMember).ToList();
            }
            else
            {
                memberTypes = _unitOfWork.MemberTypeRepository.GetMany(x => x.IsActive == true && x.MemberTypeId != (int)Core.Helper.Enum.EnumMemberType.NonMember).ToList();
            }
            DayType dayType = _unitOfWork.DayTypeRepository.Get(x => x.IsActive == true && x.DayTypeId == dayTypeId);
            if (bookingTypeId == (int)EnumBookingType.BTT)
            {
                foreach (var memberType in memberTypes)
                {
                    if (course.HoleTypeId == (int)Core.Helper.Enum.EnumHoleType.Hole9)
                    {
                        Pricing Member9HolePrice = GetPricingDetails(date, slotId, (int)EnumHoleType.Hole9, (int)EnumBookingType.BTT, memberType.MemberTypeId, dayTypeId, timeFormatId);
                        pricingString.Add("(" + memberType.Name + ") (" + dayType.Name + ") 9 Holes - " + Core.Helper.Constants.Currency.CurrencyName + ". " + (Member9HolePrice != null ? Member9HolePrice.GreenFee : 0));

                    }
                    if (course.HoleTypeId == (int)Core.Helper.Enum.EnumHoleType.Hole18)
                    {
                        Pricing Member9HolePrice = GetPricingDetails(date, slotId, (int)EnumHoleType.Hole9, (int)EnumBookingType.BTT, memberType.MemberTypeId, dayTypeId, timeFormatId);
                        pricingString.Add("(" + memberType.Name + ") (" + dayType.Name + ") 9 Holes - " + Core.Helper.Constants.Currency.CurrencyName + ". " + (Member9HolePrice != null ? Member9HolePrice.GreenFee : 0));

                        Pricing Member18HolePrice = GetPricingDetails(date, slotId, (int)EnumHoleType.Hole18, (int)EnumBookingType.BTT, memberType.MemberTypeId, dayTypeId, timeFormatId);
                        pricingString.Add("(" + memberType.Name + ") (" + dayType.Name + ") 18Holes - " + Core.Helper.Constants.Currency.CurrencyName + ". " + (Member18HolePrice != null ? Member18HolePrice.GreenFee : 0));

                    }
                    if (course.HoleTypeId == (int)Core.Helper.Enum.EnumHoleType.Hole27)
                    {
                        Pricing Member9HolePrice = GetPricingDetails(date, slotId, (int)EnumHoleType.Hole9, (int)EnumBookingType.BTT, memberType.MemberTypeId, dayTypeId, timeFormatId);
                        pricingString.Add("(" + memberType.Name + ") (" + dayType.Name + ") 9 Holes - " + Core.Helper.Constants.Currency.CurrencyName + ". " + (Member9HolePrice != null ? Member9HolePrice.GreenFee : 0));

                        Pricing Member18HolePrice = GetPricingDetails(date, slotId, (int)EnumHoleType.Hole18, (int)EnumBookingType.BTT, memberType.MemberTypeId, dayTypeId, timeFormatId);
                        pricingString.Add("(" + memberType.Name + ") (" + dayType.Name + ") 18Holes - " + Core.Helper.Constants.Currency.CurrencyName + "." + (Member18HolePrice != null ? Member18HolePrice.GreenFee : 0));

                        //Pricing Member27HolePrice = GetPricingDetails(date, slotId, (int)EnumHoleType.Hole27, (int)EnumBookingType.BTT, memberType.MemberTypeId, dayTypeId, timeFormatId);
                        //pricingString.Add("(" + memberType.Name + ") (" + dayType.Name + ") 27Holes - " + Core.Helper.Constants.Currency.CurrencyName + ". " + (Member27HolePrice != null ? Member27HolePrice.GreenFee : 0));

                    }
                }
                Pricing login9MemberPricing = GetPricingDetails(date, slotId, (int)EnumHoleType.Hole9, (int)EnumBookingType.BTT, golfer.MemberTypeId.GetValueOrDefault(), dayTypeId, timeFormatId);
                Pricing login18MemberPricing = GetPricingDetails(date, slotId, (int)EnumHoleType.Hole18, (int)EnumBookingType.BTT, golfer.MemberTypeId.GetValueOrDefault(), dayTypeId, timeFormatId);
                //  Pricing login27MemberPricing = GetPricingDetails(date, slotId, (int)EnumHoleType.Hole27, (int)EnumBookingType.BTT, golfer.MemberTypeId.GetValueOrDefault(), dayTypeId, timeFormatId);

                pricingString.Add(" (" + dayType.Name + ") 9 Hole Cart- " + Core.Helper.Constants.Currency.CurrencyName + ". " + (login9MemberPricing != null ? login9MemberPricing.AddOnCart : 0));
                pricingString.Add(" (" + dayType.Name + ") 9 Hole Caddie - " + Core.Helper.Constants.Currency.CurrencyName + ". " + (login9MemberPricing != null ? login9MemberPricing.AddOnCaddie : 0));
                pricingString.Add(" (" + dayType.Name + ") 18 Hole Cart- " + Core.Helper.Constants.Currency.CurrencyName + ". " + (login18MemberPricing != null ? login18MemberPricing.AddOnCart : 0));
                pricingString.Add(" (" + dayType.Name + ") 18 Hole Caddie - " + Core.Helper.Constants.Currency.CurrencyName + ". " + (login18MemberPricing != null ? login18MemberPricing.AddOnCaddie : 0));
                //pricingString.Add(" (" + dayType.Name + ") 27 Hole Cart- " + Core.Helper.Constants.Currency.CurrencyName + ". " + (login27MemberPricing != null ? login27MemberPricing.AddOnCart : 0));
                //pricingString.Add(" (" + dayType.Name + ") 27 Hole Caddie - " + Core.Helper.Constants.Currency.CurrencyName + ". " + (login27MemberPricing != null ? login27MemberPricing.AddOnCaddie : 0));

                var equipments = GetAllEquipment(date, false);
                foreach (var equipment in equipments.OrderBy(x => x.EquipmentPrice))
                {
                    pricingString.Add(equipment.EquipmentName + " - " + Core.Helper.Constants.Currency.CurrencyName + ". " + (equipment != null ? equipment.EquipmentPrice : 0));

                }

            }
            else
            {
                List<BucketDetail> bucketDetails = _unitOfWork.BucketDetailRepository.GetMany(x => x.IsActive == true && x.MemberTypeId == golfer.MemberTypeId && x.DayTypeId == dayTypeId).ToList();
                foreach (BucketDetail bucket in bucketDetails.OrderBy(x => x.Balls))
                {
                    pricingString.Add("Fee Of " + bucket.Balls + " Balls - " + Core.Helper.Constants.Currency.CurrencyName + ". " + bucket.Price);
                }
                foreach (var memberType in memberTypes)
                {

                    Pricing Member9HolePrice = GetPricingDetails(date, slotId, (int)EnumHoleType.Hole9, (int)EnumBookingType.BDR, memberType.MemberTypeId, dayTypeId, 1, timeFormatId);
                    pricingString.Add("(" + memberType.Name + ") (" + dayType.Name + ") Range Fee - " + Core.Helper.Constants.Currency.CurrencyName + ". " + (Member9HolePrice != null ? Member9HolePrice.RangeFee : 0));


                }
            }
            return pricingString;
        }

        /// <summary>
        /// Get Pricing Details For BDR
        /// </summary>
        /// <param name="date"></param>
        /// <param name="slotId"></param>
        /// <param name="holeTypeId"></param>
        /// <param name="bookingTypeId"></param>
        /// <param name="memberTypeId"></param>
        /// <param name="dayTypeId"></param>
        /// <returns></returns>
        private Pricing GetPricingDetails(DateTime date, long slotId, long holeTypeId, long bookingTypeId, long memberTypeId, long dayTypeId, long timeFormatId)
        {
            List<Pricing> pricings = new List<Pricing>();
            DateTime startDate = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
            DateTime endDate = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
            if (bookingTypeId == (int)EnumBookingType.BTT)
            {
                pricings = _unitOfWork.PricingRepository.GetMany(x => x.SlotId == slotId && x.StartDate <= startDate && x.EndDate >= endDate && x.DayTypeId == null && x.HoleTypeId == holeTypeId && x.BookingTypeId == bookingTypeId && x.MemberTypeId == memberTypeId && x.IsActive == true).ToList();
                if (pricings.Count == 0)
                {
                    pricings = _unitOfWork.PricingRepository.GetMany(x => x.StartDate <= startDate && x.EndDate >= endDate && x.DayTypeId == null && x.HoleTypeId == holeTypeId && x.BookingTypeId == bookingTypeId && x.MemberTypeId == memberTypeId && x.IsActive == true && x.SlotId == null).ToList();
                    if (pricings.Count == 0)
                    {
                        pricings = _unitOfWork.PricingRepository.GetMany(x => x.StartDate <= startDate && x.EndDate >= endDate && x.DayTypeId == dayTypeId && x.HoleTypeId == holeTypeId && x.BookingTypeId == bookingTypeId && x.MemberTypeId == memberTypeId && x.IsActive == true && x.SlotId == null && x.TimeFormatId == timeFormatId).ToList();
                    }

                }

                Pricing pricings1 = pricings.OrderByDescending(x => x.CreatedOn).ToList().FirstOrDefault();
                return pricings1;
            }
            if (bookingTypeId == (int)EnumBookingType.BDR)
            {
                pricings = _unitOfWork.PricingRepository.GetMany(x => x.SlotId == slotId && x.StartDate <= startDate && x.EndDate >= endDate && x.DayTypeId == null && x.BookingTypeId == bookingTypeId && x.MemberTypeId == memberTypeId && x.IsActive == true).ToList();
                if (pricings.Count == 0)
                {
                    pricings = _unitOfWork.PricingRepository.GetMany(x => x.StartDate <= startDate && x.EndDate >= endDate && x.DayTypeId == null && x.BookingTypeId == bookingTypeId && x.MemberTypeId == memberTypeId && x.IsActive == true && x.SlotId == null).ToList();
                    if (pricings.Count == 0)
                    {
                        pricings = _unitOfWork.PricingRepository.GetMany(x => x.StartDate <= startDate && x.EndDate >= endDate && x.DayTypeId == dayTypeId && x.BookingTypeId == bookingTypeId && x.MemberTypeId == memberTypeId && x.IsActive == true && x.SlotId == null).ToList();
                    }
                }

                Pricing pricings1 = pricings.OrderByDescending(x => x.CreatedOn).ToList().FirstOrDefault();
                return pricings1;

            }
            return new Pricing();
        }

        /// <summary>
        /// Get Pricing Details For BDT
        /// </summary>
        /// <param name="date"></param>
        /// <param name="slotId"></param>
        /// <param name="holeTypeId"></param>
        /// <param name="bookingTypeId"></param>
        /// <param name="memberTypeId"></param>
        /// <param name="dayTypeId"></param>
        /// <param name="bucketId"></param>
        /// <returns></returns>
        private Pricing GetPricingDetails(DateTime date, long slotId, long holeTypeId, long bookingTypeId, long memberTypeId, long dayTypeId, long bucketId, long timeFormatId)
        {
            List<Pricing> pricings = new List<Pricing>();
            DateTime startDate = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
            DateTime endDate = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
            if (bookingTypeId == (int)EnumBookingType.BTT)
            {
                pricings = _unitOfWork.PricingRepository.GetMany(x => x.SlotId == slotId && x.StartDate <= startDate && x.EndDate >= endDate && x.DayTypeId == null && x.HoleTypeId == holeTypeId && x.BookingTypeId == bookingTypeId && x.MemberTypeId == memberTypeId && x.IsActive == true).ToList();
                if (pricings.Count == 0)
                {
                    pricings = _unitOfWork.PricingRepository.GetMany(x => x.StartDate <= startDate && x.EndDate >= endDate && x.DayTypeId == null && x.HoleTypeId == holeTypeId && x.BookingTypeId == bookingTypeId && x.MemberTypeId == memberTypeId && x.IsActive == true && x.SlotId == null).ToList();
                    if (pricings.Count == 0)
                    {
                        pricings = _unitOfWork.PricingRepository.GetMany(x => x.StartDate <= startDate && x.EndDate >= endDate && x.DayTypeId == dayTypeId && x.HoleTypeId == holeTypeId && x.BookingTypeId == bookingTypeId && x.MemberTypeId == memberTypeId && x.IsActive == true && x.SlotId == null).ToList();
                    }
                }

                Pricing pricings1 = pricings.OrderByDescending(x => x.CreatedOn).ToList().FirstOrDefault();
                return pricings1;
            }
            if (bookingTypeId == (int)EnumBookingType.BDR)
            {
                pricings = _unitOfWork.PricingRepository.GetMany(x => x.SlotId == slotId && x.StartDate <= startDate && x.EndDate >= endDate && x.DayTypeId == null && x.BookingTypeId == bookingTypeId && x.MemberTypeId == memberTypeId && x.IsActive == true).ToList();
                if (pricings.Count == 0)
                {
                    pricings = _unitOfWork.PricingRepository.GetMany(x => x.StartDate <= startDate && x.EndDate >= endDate && x.DayTypeId == null && x.BookingTypeId == bookingTypeId && x.MemberTypeId == memberTypeId && x.IsActive == true && x.SlotId == null).ToList();
                    if (pricings.Count == 0)
                    {
                        pricings = _unitOfWork.PricingRepository.GetMany(x => x.StartDate <= startDate && x.EndDate >= endDate && x.DayTypeId == dayTypeId && x.BookingTypeId == bookingTypeId && x.MemberTypeId == memberTypeId && x.IsActive == true && x.SlotId == null && x.TimeFormatId == timeFormatId).ToList();
                    }
                }


                Pricing pricings1 = pricings.OrderByDescending(x => x.CreatedOn).ToList().FirstOrDefault();
                return pricings1;

            }
            return new Pricing();
        }


        /// <summary>
        /// Get Slot Details For BDR And BDT
        /// </summary>
        /// <param name="date"></param>
        /// <param name="bookingTypeId"></param>
        /// <returns></returns>
        public List<SessionSlotViewModel> GetSlotDetailsByDateAndBookingType(DateTime date, long bookingTypeId, long? coursePairingId)
        {
            List<long> removeSlot = new List<long>();
            List<long> disableSlot = new List<long>();
            if (bookingTypeId == (int)EnumBookingType.BTT)
            {
                long coursePairingId1 = 0;
                long coursePairingId2 = 0;
                if (coursePairingId == 1) { coursePairingId1 = 4; coursePairingId2 = 3; }
                if (coursePairingId == 2) { coursePairingId1 = 5; coursePairingId2 = 1; }
                if (coursePairingId == 3) { coursePairingId1 = 6; coursePairingId2 = 2; }
                if (coursePairingId == 4) { coursePairingId1 = 1; coursePairingId2 = 3; }
                if (coursePairingId == 5) { coursePairingId1 = 2; coursePairingId2 = 1; }
                if (coursePairingId == 6) { coursePairingId1 = 3; coursePairingId2 = 2; }

                List<BlockSlotRange> blockSlotRanges = _unitOfWork.BlockSlotRangeRepository.GetMany(x => x.StartDate <= date && x.EndDate >= date && x.IsActive == true && (x.CoursePairingId == coursePairingId || x.CoursePairingId == null || x.CoursePairingId == coursePairingId1 || x.CoursePairingId == coursePairingId2)).ToList();
                foreach (BlockSlotRange blockSlotRange in blockSlotRanges)
                {
                    List<BlockSlot> blockSlots = _unitOfWork.BlockSlotRepository.GetMany(x => x.IsActive == true && x.BlockSlotRangeId == blockSlotRange.BlockSlotRangeId).ToList();
                    if (blockSlotRange.IsCustomerAvailable != true)
                    {
                        removeSlot.AddRange(blockSlots.Select(x => x.SlotSessionWiseId).ToList());
                    }
                    else
                    {
                        if (blockSlotRange.IsBookingavailable != true)
                            disableSlot.AddRange(blockSlots.Select(x => x.SlotSessionWiseId).ToList());
                    }
                }
            }
            List<SessionSlotViewModel> sessionSlotViewModels = new List<SessionSlotViewModel>();
            DateTime currentTime = Core.Helper.DateHelper.ConvertSystemDate();
            List<Session> sessions = _unitOfWork.SessionRepository.GetMany(x => x.IsActive == true && x.BookingTypeId == bookingTypeId).ToList();
            var dayId = GetDayTypeId(date);
            var minPlayer = Core.Helper.Constants.PlayerCount.MinPlayerCount;
            if (dayId == (int)Core.Helper.Enum.EnumDayType.Saturday || dayId == (int)Core.Helper.Enum.EnumDayType.Sunday || CheckNationalHoliday(date) == true)
            {
                minPlayer = Core.Helper.Constants.PlayerCount.WeekendMinPlayerCount;
            }


            foreach (Session session in sessions)
            {
                SessionSlotViewModel sessionSlotViewModel = new SessionSlotViewModel()
                {
                    SessionName = session.Name + "(" + session.StartTime + " - " + session.EndTime + ")"
                };
                List<SlotSessionWise> slotSessionWises = _unitOfWork.SlotSessionWiseRepository.GetMany(x => x.SessionId == session.SessionId && x.IsActive == true && x.BookingTypeId == bookingTypeId).ToList();

                if (bookingTypeId == (int)EnumBookingType.BTT)
                {
                    foreach (var slot in removeSlot)
                    {
                        var itemToRemove = slotSessionWises.SingleOrDefault(r => r.SlotSessionWiseId == slot);
                        if (itemToRemove != null)
                            slotSessionWises.Remove(itemToRemove);
                    }
                }

                foreach (SlotSessionWise slotSessionWise in slotSessionWises.OrderBy(x => x.Slot.Time))
                {
                    List<Booking> bookings = new List<Booking>();
                    SlotDetailsViewModel slotDetailsViewModel = new SlotDetailsViewModel();
                    if (bookingTypeId == (int)EnumBookingType.BTT)
                    {
                        long coursePairingId1 = 0;
                        if (coursePairingId == 1) { coursePairingId1 = 4; }
                        if (coursePairingId == 2) { coursePairingId1 = 5; }
                        if (coursePairingId == 3) { coursePairingId1 = 6; }
                        if (coursePairingId == 4) { coursePairingId1 = 1; }
                        if (coursePairingId == 5) { coursePairingId1 = 2; }
                        if (coursePairingId == 6) { coursePairingId1 = 3; }
                        bookings = _unitOfWork.BookingRepository.GetMany(x => x.TeeOffDate == date && x.TeeOffSlot == slotSessionWise.Slot.Time.ToString("hh':'mm") && x.IsActive == true && x.BookingTypeId == bookingTypeId && (x.BookingStatusId == (int)EnumBookingStatus.Confirm || (x.BookingStatusId == (int)EnumBookingStatus.Pending && x.CreatedOn.AddMinutes(15) > DateTime.UtcNow)) && (x.CoursePairingId == coursePairingId || x.CoursePairingId == coursePairingId1)).ToList();
                        var itemToRemove = disableSlot.SingleOrDefault(r => r == slotSessionWise.SlotSessionWiseId);
                        if (itemToRemove != 0)
                        {
                            slotDetailsViewModel.SlotId = slotSessionWise.SlotId;
                            slotDetailsViewModel.SlotTime = slotSessionWise.Slot.Time.ToString("hh':'mm");
                            slotDetailsViewModel.IsAvailable = false;
                            sessionSlotViewModel.SlotDetailsViewModelList.Add(slotDetailsViewModel);
                            continue;
                        }
                    }
                    else
                    {
                        bookings = _unitOfWork.BookingRepository.GetMany(x => x.TeeOffDate == date && x.TeeOffSlot == slotSessionWise.Slot.Time.ToString("hh':'mm") && x.IsActive == true && x.BookingTypeId == bookingTypeId && (x.BookingStatusId == (int)EnumBookingStatus.Confirm || (x.BookingStatusId == (int)EnumBookingStatus.Pending && x.CreatedOn.AddMinutes(15) > DateTime.UtcNow))).ToList();
                    }
                    int noOfPlayer = bookings.Select(x => x.NoOfPlayer).Sum();


                    if (bookings.Count() == 0)
                    {
                        slotDetailsViewModel.SlotId = slotSessionWise.SlotId;
                        slotDetailsViewModel.SlotTime = slotSessionWise.Slot.Time.ToString("hh':'mm");
                        if ((slotSessionWise.Slot.Time >= currentTime.TimeOfDay && date.Month == currentTime.Month && date.Year == currentTime.Year && date.Day == currentTime.Day))
                        {
                            slotDetailsViewModel.IsAvailable = true;
                            slotDetailsViewModel.PlayerLeftCount = (4 - noOfPlayer);
                            if (slotDetailsViewModel.PlayerLeftCount < minPlayer)
                            { slotDetailsViewModel.IsAvailable = false; }
                            slotDetailsViewModel.MinPlayerCount = minPlayer;
                        }
                        else { slotDetailsViewModel.IsAvailable = false; }
                        if (date > currentTime)
                        {
                            slotDetailsViewModel.IsAvailable = true;
                            slotDetailsViewModel.PlayerLeftCount = (4 - noOfPlayer);
                            if (slotDetailsViewModel.PlayerLeftCount < minPlayer)
                            { slotDetailsViewModel.IsAvailable = false; }
                            slotDetailsViewModel.MinPlayerCount = minPlayer;
                        }
                    }
                    else
                    {

                        slotDetailsViewModel.SlotId = slotSessionWise.SlotId;
                        slotDetailsViewModel.SlotTime = slotSessionWise.Slot.Time.ToString("hh':'mm");
                        slotDetailsViewModel.IsAvailable = false;
                    }

                    sessionSlotViewModel.SlotDetailsViewModelList.Add(slotDetailsViewModel);
                }
                sessionSlotViewModels.Add(sessionSlotViewModel);
            }

            return sessionSlotViewModels;
        }

        /// <summary>
        /// Get Pricing  Details For BTT
        /// </summary>
        /// <param name="holeTypeId"></param>
        /// <param name="date"></param>
        /// <param name="slotId"></param>
        /// <param name="TotalNoOfPlayer"></param>
        /// <param name="noOfMemberPlayer"></param>
        /// <param name="noOfNonMemberPlayer"></param>
        /// <returns></returns>
        public BookingPricingViewModel GetPricingCalculation(long holeTypeId, DateTime date, long slotId, int TotalNoOfPlayer, int noOfMemberPlayer, int noOfNonMemberPlayer, List<MemberTypeViewModel> memberTypeViewModels, long golferId, long? coursePairingId, List<BookingPlayerDetailViewModel> bookingPlayerDetailViewModels, long promotionsId)
        {
            CheckBookingForGolfer(bookingPlayerDetailViewModels, date, 1);

            long timeFormatId = 0;

            if (date > DateHelper.ConvertSystemDate().AddDays((int)Core.Helper.Constants.BookingDetails.AdvanceBookingDays))
                throw new Exception("You Can Book Only Upto " + (int)Core.Helper.Constants.BookingDetails.AdvanceBookingDays + " Days");


            if (TotalNoOfPlayer != memberTypeViewModels.Sum(x => x.PlayerCount))
                throw new Exception("Invalid Player Details");
            Golfer golfer = _unitOfWork.GolferRepository.Get(x => x.GolferId == golferId);


            var time = _unitOfWork.SlotRepository.Get(x => x.SlotId == slotId && x.IsActive == true);
            var holename = _unitOfWork.HoleTypeRepository.Get(x => x.HoleTypeId == holeTypeId && x.IsActive == true);
            if (time.Time >= TimeSpan.Parse("12:00")) { timeFormatId = (int)Core.Helper.Enum.EnumTimeFormatType.PM; }
            else { timeFormatId = (int)Core.Helper.Enum.EnumTimeFormatType.AM; }
            CoursePairing coursePairing = _unitOfWork.CoursePairingRepository.Get(x => x.CoursePairingId == coursePairingId);

            BookingPricingViewModel bookingPricingViewModel = new BookingPricingViewModel()
            {
                HoleTypeId = holeTypeId,
                SlotId = slotId,
                SlotTime = time.Time.ToString("hh':'mm"),
                NoOfPlayer = TotalNoOfPlayer,
                //NoOfMemberPlayer = noOfMemberPlayer,
                //NoOfNonMemberPlayer = noOfNonMemberPlayer,
                Date = date.ToShortDateString(),
                HoleTypeName = holename.Value.ToString(),
                CurrencyName = Core.Helper.Constants.Currency.CurrencyName,
                CoursePairingId = coursePairingId.GetValueOrDefault(),
                CoursePairingName = coursePairing != null ? (coursePairing.CourseName1.Value) + (coursePairing.EndCourseNameId != null ? " - " + coursePairing.CourseName.Value : "") : ""


            };

            if (promotionsId == 0)
            {
                if (golfer.MemberTypeId == (int)Core.Helper.Enum.EnumMemberType.NonMember)

                {
                    bookingPricingViewModel.IsMember = false;
                }
                else
                {
                    bookingPricingViewModel.IsMember = true;
                }

                decimal total = 0;
                long dayTypeId = GetDayTypeId(date);
                Pricing login9MemberPricing = GetPricingDetails(date, slotId, (int)EnumHoleType.Hole9, (int)EnumBookingType.BTT, golfer.MemberTypeId.GetValueOrDefault(), dayTypeId, timeFormatId);
                Pricing login18MemberPricing = GetPricingDetails(date, slotId, (int)EnumHoleType.Hole18, (int)EnumBookingType.BTT, golfer.MemberTypeId.GetValueOrDefault(), dayTypeId, timeFormatId);
                Pricing login27MemberPricing = GetPricingDetails(date, slotId, (int)EnumHoleType.Hole27, (int)EnumBookingType.BTT, golfer.MemberTypeId.GetValueOrDefault(), dayTypeId, timeFormatId);

                foreach (var memberType in memberTypeViewModels)
                {
                    MemberTypeViewModel memberTypeViewModel = new MemberTypeViewModel();
                    memberTypeViewModel.CurrencyName = Core.Helper.Constants.Currency.CurrencyName;
                    if (memberType.PlayerCount != 0)
                    {
                        Pricing member9HolePricing = new Pricing();
                        Pricing member18HolePricing = new Pricing();
                        Pricing member27HolePricing = new Pricing();
                        member9HolePricing = GetPricingDetails(date, slotId, (int)EnumHoleType.Hole9, (int)EnumBookingType.BTT, memberType.MemberTypeId, dayTypeId, timeFormatId);
                        member18HolePricing = GetPricingDetails(date, slotId, (int)EnumHoleType.Hole18, (int)EnumBookingType.BTT, memberType.MemberTypeId, dayTypeId, timeFormatId);
                        member27HolePricing = GetPricingDetails(date, slotId, (int)EnumHoleType.Hole27, (int)EnumBookingType.BTT, memberType.MemberTypeId, dayTypeId, timeFormatId);
                        if (holeTypeId == (int)EnumHoleType.Hole9)
                        {
                            memberTypeViewModel.GreenFee = member9HolePricing != null ? member9HolePricing.GreenFee.GetValueOrDefault() : 0;
                            if (member9HolePricing != null)
                            {

                                decimal totalPrec = 0;

                                foreach (var tax in member9HolePricing.PriceTaxMappings.Where(x => x.FeeCategoryId == (int)Core.Helper.Enum.EnumFeeCategory.GreenFee && x.IsActive == true))
                                {
                                    totalPrec += tax.Tax.Percentage;

                                }
                                memberTypeViewModel.GreenFee = Math.Round(memberTypeViewModel.GreenFee + (memberTypeViewModel.GreenFee * (totalPrec / 100)), 2);

                            }
                        }
                        if (holeTypeId == (int)EnumHoleType.Hole18)
                        {
                            memberTypeViewModel.GreenFee = member18HolePricing != null ? member18HolePricing.GreenFee.GetValueOrDefault() : 0;
                            if (member18HolePricing != null)
                            {
                                decimal totalPrec = 0;
                                foreach (var tax in member18HolePricing.PriceTaxMappings.Where(x => x.FeeCategoryId == (int)Core.Helper.Enum.EnumFeeCategory.GreenFee && x.IsActive == true))
                                {
                                    totalPrec += tax.Tax.Percentage;
                                }
                                memberTypeViewModel.GreenFee = Math.Round(memberTypeViewModel.GreenFee + (memberTypeViewModel.GreenFee * (totalPrec / 100)), 2);

                            }
                        }
                        if (holeTypeId == (int)EnumHoleType.Hole27)
                        {

                            memberTypeViewModel.GreenFee = member27HolePricing != null ? member27HolePricing.GreenFee.GetValueOrDefault() : 0;
                            if (member27HolePricing != null)
                            {
                                decimal totalPrec = 0;
                                foreach (var tax in member27HolePricing.PriceTaxMappings.Where(x => x.FeeCategoryId == (int)Core.Helper.Enum.EnumFeeCategory.GreenFee && x.IsActive == true))
                                {
                                    totalPrec += tax.Tax.Percentage;
                                }
                                memberTypeViewModel.GreenFee = Math.Round(memberTypeViewModel.GreenFee + (memberTypeViewModel.GreenFee * (totalPrec / 100)), 2);

                            }
                        }


                        memberTypeViewModel.MemberTypeId = memberType.MemberTypeId;
                        memberTypeViewModel.Name = memberType.Name;
                        memberTypeViewModel.PlayerCount = memberType.PlayerCount;
                        bookingPricingViewModel.MemberTypeViewModels.Add(memberTypeViewModel);
                    }
                }


                bookingPricingViewModel.Caddie18HolePrice = login18MemberPricing != null ? login18MemberPricing.AddOnCaddie.GetValueOrDefault() : 0;
                bookingPricingViewModel.Caddie9HolePrice = login9MemberPricing != null ? login9MemberPricing.AddOnCaddie.GetValueOrDefault() : 0;
                bookingPricingViewModel.Cart18HolePrice = login18MemberPricing != null ? login18MemberPricing.AddOnCart.GetValueOrDefault() : 0;
                bookingPricingViewModel.Cart9HolePrice = login9MemberPricing != null ? login9MemberPricing.AddOnCart.GetValueOrDefault() : 0;

                bookingPricingViewModel.Caddie27HolePrice = login27MemberPricing != null ? login27MemberPricing.AddOnCaddie.GetValueOrDefault() : 0;
                bookingPricingViewModel.Cart27HolePrice = login27MemberPricing != null ? login27MemberPricing.AddOnCart.GetValueOrDefault() : 0;

                if (login9MemberPricing != null)
                {
                    decimal totalPrec = 0;
                    decimal totalPrec1 = 0;
                    foreach (var tax in login9MemberPricing.PriceTaxMappings.Where(x => x.FeeCategoryId == (int)Core.Helper.Enum.EnumFeeCategory.AddOnCart && x.IsActive == true))
                    {
                        totalPrec += tax.Tax.Percentage;
                    }
                    bookingPricingViewModel.Cart9HolePrice = Math.Round(bookingPricingViewModel.Cart9HolePrice + (bookingPricingViewModel.Cart9HolePrice * (totalPrec / 100)), 2);

                    foreach (var tax in login9MemberPricing.PriceTaxMappings.Where(x => x.FeeCategoryId == (int)Core.Helper.Enum.EnumFeeCategory.AddOnCaddie && x.IsActive == true))
                    {
                        totalPrec1 += tax.Tax.Percentage;
                    }
                    bookingPricingViewModel.Caddie9HolePrice = Math.Round(bookingPricingViewModel.Caddie9HolePrice + (bookingPricingViewModel.Caddie9HolePrice * (totalPrec1 / 100)), 2);

                }
                if (login18MemberPricing != null)
                {
                    decimal totalPrec = 0;
                    decimal totalPrec1 = 0;
                    foreach (var tax in login18MemberPricing.PriceTaxMappings.Where(x => x.FeeCategoryId == (int)Core.Helper.Enum.EnumFeeCategory.AddOnCart && x.IsActive == true))
                    {
                        totalPrec += tax.Tax.Percentage;

                    }
                    bookingPricingViewModel.Cart18HolePrice = Math.Round(bookingPricingViewModel.Cart18HolePrice + (bookingPricingViewModel.Cart18HolePrice * (totalPrec / 100)), 2);

                    foreach (var tax in login18MemberPricing.PriceTaxMappings.Where(x => x.FeeCategoryId == (int)Core.Helper.Enum.EnumFeeCategory.AddOnCaddie && x.IsActive == true))
                    {
                        totalPrec1 += tax.Tax.Percentage;

                    }
                    bookingPricingViewModel.Caddie18HolePrice = Math.Round(bookingPricingViewModel.Caddie18HolePrice + (bookingPricingViewModel.Caddie18HolePrice * (totalPrec1 / 100)), 2);

                }
                if (login27MemberPricing != null)
                {
                    decimal totalPrec = 0;
                    decimal totalPrec1 = 0;
                    foreach (var tax in login27MemberPricing.PriceTaxMappings.Where(x => x.FeeCategoryId == (int)Core.Helper.Enum.EnumFeeCategory.AddOnCart && x.IsActive == true))
                    {
                        totalPrec += tax.Tax.Percentage;


                    }
                    bookingPricingViewModel.Cart27HolePrice = Math.Round(bookingPricingViewModel.Cart27HolePrice + (bookingPricingViewModel.Cart27HolePrice * (totalPrec / 100)), 2);

                    foreach (var tax in login27MemberPricing.PriceTaxMappings.Where(x => x.FeeCategoryId == (int)Core.Helper.Enum.EnumFeeCategory.AddOnCaddie))
                    {
                        totalPrec1 += tax.Tax.Percentage;


                    }
                    bookingPricingViewModel.Caddie27HolePrice = Math.Round(bookingPricingViewModel.Caddie27HolePrice + (bookingPricingViewModel.Caddie27HolePrice * (totalPrec1 / 100)), 2);

                }

                bookingPricingViewModel.BookingEquipmentMappingViewModels = GetAllEquipment(date, true);
            }
            else
            {
                bookingPricingViewModel.PromotionPrice = PromotionPrice(promotionsId);
                bookingPricingViewModel.PromotionsId = promotionsId;
                bookingPricingViewModel.MemberTypeViewModels = memberTypeViewModels;
            }
            bookingPricingViewModel.CourseTaxMappingViewModels = GetAllCourseTaxs();
            bookingPricingViewModel.BookingPlayerDetailViewModels = bookingPlayerDetailViewModels;
            bookingPricingViewModel.PaymentGatewayControlViewModel = PaymentGatewayControl(golferId);
            return bookingPricingViewModel;
        }

        /// <summary>
        /// Save Tee Time Booking
        /// </summary>
        /// <param name="commonViewModel"></param>
        /// <returns></returns>
        public BookingViewModel SaveTeeTimeBooking(CommonViewModel commonViewModel)
        {

            Core.Helper.Vaildation.TimeSpanValidation(commonViewModel.TeeOffSlot);
            CheckBookingForGolfer(commonViewModel.BookingPlayerDetailViewModels, commonViewModel.TeeOffDate, commonViewModel.BookingTypeId);
            if (commonViewModel.TeeOffDate > DateHelper.ConvertSystemDate().AddDays((int)Core.Helper.Constants.BookingDetails.AdvanceBookingDays))
                throw new Exception("You Can Book Only Upto " + ((int)Core.Helper.Constants.BookingDetails.AdvanceBookingDays).ToString() + " Days");
            if (commonViewModel.NoOfPlayer != commonViewModel.MemberTypeViewModels.Sum(x => x.PlayerCount))
                throw new Exception("Invalid Player Details");
            if (IsSlotAvailable(commonViewModel.TeeOffSlot, (int)EnumBookingType.BTT, commonViewModel.TeeOffDate, commonViewModel.NoOfPlayer, commonViewModel.CoursePairingId) == false)
                throw new Exception("Slot Is Already Booked");
            CheckEquipmentVaildation(commonViewModel);
            commonViewModel.BookingTypeId = (int)EnumBookingType.BTT;
            var paymentEnable = VaildateAmount(commonViewModel);
            Booking booking = new Booking()
            {
                NoOfPlayer = commonViewModel.NoOfPlayer,
                NoOfMember = commonViewModel.NoOfMemberPlayer,
                NoOfNonMember = commonViewModel.NoOfNonMemberPlayer,
                Discount = commonViewModel.Discount,
                CaddieCount = commonViewModel.CaddieCount,
                CartCount = commonViewModel.CartCount,
                CaddieFee = commonViewModel.CaddieFee,
                CartFee = commonViewModel.CartFee,
                BookingStatusId = commonViewModel.BookingStatusId,
                GreenFee = commonViewModel.GreenFee,
                HoleTypeId = commonViewModel.HoleTypeId,
                IsActive = true,
                CreatedOn = DateTime.UtcNow,
                BookingTypeId = (int)EnumBookingType.BTT,
                FeeAndTaxes = commonViewModel.FeeAndTaxes,
                TeeOffSlot = commonViewModel.TeeOffSlot,
                TeeOffDate = commonViewModel.TeeOffDate,
                OnSpot = commonViewModel.OnSpot,
                GolferId = commonViewModel.GolferId,
                BookingDate = DateTime.UtcNow,
                Amount = commonViewModel.Amount,
                PaymentStatusId = commonViewModel.PaymentStatusId,
                CoursePairingId = commonViewModel.CoursePairingId,
                PaidAmount = commonViewModel.PaidAmount,
                IsPaymentGatewaySkip = paymentEnable
            };
            if (commonViewModel.PromotionsId != 0)
            {
                booking.PromotionId = commonViewModel.PromotionsId;
            }
            _unitOfWork.BookingRepository.Insert(booking);
            _unitOfWork.Save();
            Booking bk = _unitOfWork.BookingRepository.Get(x => x.BookingId == booking.BookingId);
            bk.PaymentGatewayBookingId = Core.Helper.Constants.BookingKey.BookingKeyId + booking.BookingId.ToString();
            _unitOfWork.BookingRepository.Update(bk);
            _unitOfWork.Save();
            SaveMemberTypePlayerMapping(booking.BookingId, commonViewModel.MemberTypeViewModels);
            SaveCourseTaxMapping(booking.BookingId, commonViewModel.CourseTaxMappingViewModels);
            if (commonViewModel.PromotionsId == 0)
            {
                SaveBookingEquipmentMappingMapping(booking.BookingId, commonViewModel.BookingEquipmentMappingViewModels);
            }
            SaveBookingPlayerDetail(booking.BookingId, commonViewModel.BookingPlayerDetailViewModels);
            CoursePairing coursePairing = _unitOfWork.CoursePairingRepository.Get(x => x.CoursePairingId == booking.CoursePairingId);

            BookingViewModel bookingViewModel = new BookingViewModel()
            {
                BookingId = booking.BookingId,
                PaymentGatewayBookingId = booking.PaymentGatewayBookingId,
                TotalAmount = bk.PaidAmount.GetValueOrDefault(),
                ConvertedAmount = ConvertCurrency(bk.PaidAmount.GetValueOrDefault()),
                CoursePairingId = bk.CoursePairingId.GetValueOrDefault(),
                CoursePairingName = coursePairing != null ? (coursePairing.CourseName1.Value) + (coursePairing.EndCourseNameId != null ? " - " + coursePairing.CourseName.Value : "") : ""

            };
            return bookingViewModel;
        }

        /// <summary>
        /// Get All Bucket Details
        /// </summary>
        /// <returns></returns>
        public List<BucketViewModel> GetBucketDetailList(DateTime date, long golferId)
        {
            Golfer golfer = _unitOfWork.GolferRepository.Get(x => x.GolferId == golferId);
            long dayTypeId = GetDayTypeId(date);
            List<BucketViewModel> bucketViewModels = new List<BucketViewModel>();
            List<BucketDetail> bucketDetails = _unitOfWork.BucketDetailRepository.GetMany(x => x.IsActive == true && x.MemberTypeId == golfer.MemberTypeId && x.DayTypeId == dayTypeId).ToList();
            foreach (BucketDetail bucketDetail in bucketDetails.OrderBy(x => x.Balls))
            {
                BucketViewModel bucketViewModel = new BucketViewModel()
                {
                    Balls = bucketDetail.Balls,
                    BucketDetailId = bucketDetail.BucketDetailId
                };
                bucketViewModels.Add(bucketViewModel);
            }
            return bucketViewModels;
        }

        /// <summary>
        /// Get Pricing Detail For BDT
        /// </summary>
        /// <param name="bucketId"></param>
        /// <param name="date"></param>
        /// <param name="slotId"></param>
        /// <param name="TotalNoOfPlayer"></param>
        /// <param name="noOfMemberPlayer"></param>
        /// <param name="noOfNonMemberPlayer"></param>
        /// <returns></returns>
        public BookingPricingViewModel GetPricingCalculationBDT(long bucketId, DateTime date, long slotId, int TotalNoOfPlayer, int noOfMemberPlayer, int noOfNonMemberPlayer, List<MemberTypeViewModel> memberTypeViewModels, long golferId)
        {

            long timeFormatId = 0;
            if (date > DateHelper.ConvertSystemDate().AddDays((int)Core.Helper.Constants.BookingDetails.AdvanceBookingDays))
                throw new Exception("You Can Book Only Upto " + (int)Core.Helper.Constants.BookingDetails.AdvanceBookingDays + " Days");
            if (TotalNoOfPlayer != memberTypeViewModels.Sum(x => x.PlayerCount))
                throw new Exception("Invalid Player Details");
            Golfer golfer = _unitOfWork.GolferRepository.Get(x => x.GolferId == golferId);

            var time = _unitOfWork.SlotRepository.Get(x => x.SlotId == slotId && x.IsActive == true);
            if (time.Time >= TimeSpan.Parse("12:00")) { timeFormatId = (int)Core.Helper.Enum.EnumTimeFormatType.PM; }
            else { timeFormatId = (int)Core.Helper.Enum.EnumTimeFormatType.AM; }
            BucketDetail bucketDetail = _unitOfWork.BucketDetailRepository.Get(x => x.BucketDetailId == bucketId && x.IsActive == true);
            decimal totalBucketTax = 0;
            foreach (var tax in bucketDetail.BucketTaxMappings.Where(x => x.IsActive == true))
            {
                totalBucketTax += tax.Tax.Percentage;

            }
            var ballPrice = Math.Round(bucketDetail.Price + (bucketDetail.Price * (totalBucketTax / 100)), 2);

            BookingPricingViewModel bookingPricingViewModel = new BookingPricingViewModel()
            {
                SlotId = slotId,
                SlotTime = time.Time.ToString("hh':'mm"),
                NoOfPlayer = TotalNoOfPlayer,
                //NoOfMemberPlayer = noOfMemberPlayer,
                //NoOfNonMemberPlayer = noOfNonMemberPlayer,
                Date = date.ToShortDateString(),
                CurrencyName = Core.Helper.Constants.Currency.CurrencyName,
                BallPrice = ballPrice,
                NoOfBalls = bucketDetail.Balls

            };
            if (golfer.MemberTypeId == (int)Core.Helper.Enum.EnumMemberType.NonMember)
            {
                bookingPricingViewModel.IsMember = false;
            }
            else
            {
                bookingPricingViewModel.IsMember = true;
            }
            decimal total = 0;
            long dayTypeId = GetDayTypeId(date);
            foreach (var memberType in memberTypeViewModels)
            {
                MemberTypeViewModel memberTypeViewModel = new MemberTypeViewModel();
                memberTypeViewModel.CurrencyName = Core.Helper.Constants.Currency.CurrencyName;
                if (memberType.PlayerCount != 0)
                {
                    Pricing memberPricing = new Pricing();
                    memberPricing = GetPricingDetails(date, slotId, (int)EnumBookingType.BDR, (int)EnumBookingType.BDR, memberType.MemberTypeId, dayTypeId, bucketId, timeFormatId);


                    memberTypeViewModel.RangeFee = memberPricing != null ? memberPricing.RangeFee.GetValueOrDefault() : 0;
                    if (memberPricing != null)
                    {
                        decimal totalPrec = 0;
                        foreach (var tax in memberPricing.PriceTaxMappings.Where(x => x.FeeCategoryId == (int)Core.Helper.Enum.EnumFeeCategory.RangeFee && x.IsActive == true))
                        {
                            totalPrec += tax.Tax.Percentage;

                        }
                        memberTypeViewModel.RangeFee = Math.Round(memberTypeViewModel.RangeFee + (memberTypeViewModel.RangeFee * (totalPrec / 100)), 2);

                    }

                    bookingPricingViewModel.MemberTypeViewModels.Add(memberTypeViewModel);
                    memberTypeViewModel.MemberTypeId = memberType.MemberTypeId;
                    memberTypeViewModel.Name = memberType.Name;
                    memberTypeViewModel.PlayerCount = memberType.PlayerCount;
                }
            }

            bookingPricingViewModel.CourseTaxMappingViewModels = GetAllCourseTaxs();
            return bookingPricingViewModel;
        }


        /// <summary>
        /// Save Driving Range Booking
        /// </summary>
        /// <param name="commonViewModel"></param>
        /// <returns></returns>
        public BookingViewModel SaveDrivingRangeBooking(CommonViewModel commonViewModel)
        {
            CheckBookingForGolfer(commonViewModel.BookingPlayerDetailViewModels, commonViewModel.TeeOffDate, commonViewModel.BookingTypeId);
            if (commonViewModel.TeeOffDate > DateHelper.ConvertSystemDate().AddDays((int)Core.Helper.Constants.BookingDetails.AdvanceBookingDays))
                throw new Exception("You Can Book Only Upto " + (int)Core.Helper.Constants.BookingDetails.AdvanceBookingDays + " Days");
            if (commonViewModel.NoOfPlayer != commonViewModel.MemberTypeViewModels.Sum(x => x.PlayerCount))
                throw new Exception("Invalid Player Details");
            if (IsSlotAvailable(commonViewModel.TeeOffSlot, (int)EnumBookingType.BDR, commonViewModel.TeeOffDate, commonViewModel.NoOfPlayer, commonViewModel.CoursePairingId) == false)
                throw new Exception("Slot Is Already Booked");
            Booking booking = new Booking()
            {
                NoOfPlayer = commonViewModel.NoOfPlayer,
                NoOfMember = commonViewModel.NoOfMemberPlayer,
                NoOfNonMember = commonViewModel.NoOfNonMemberPlayer,
                Discount = commonViewModel.Discount,
                BookingStatusId = commonViewModel.BookingStatusId,
                RangeFee = commonViewModel.RangeFee,
                IsActive = true,
                CreatedOn = DateTime.UtcNow,
                BookingTypeId = (int)EnumBookingType.BDR,
                FeeAndTaxes = commonViewModel.FeeAndTaxes,
                TeeOffSlot = commonViewModel.TeeOffSlot,
                TeeOffDate = commonViewModel.TeeOffDate,
                OnSpot = commonViewModel.OnSpot,
                GolferId = commonViewModel.GolferId,
                BookingDate = DateTime.UtcNow,
                Amount = commonViewModel.Amount,
                BallFee = commonViewModel.BallFee,
                NoOfBalls = commonViewModel.NoOfBalls,
                PaymentStatusId = commonViewModel.PaymentStatusId,
                PaidAmount = commonViewModel.PaidAmount
            };
            _unitOfWork.BookingRepository.Insert(booking);
            _unitOfWork.Save();
            Booking bk = _unitOfWork.BookingRepository.GetById(booking.BookingId);
            bk.PaymentGatewayBookingId = Core.Helper.Constants.BookingKey.BookingKeyId + booking.BookingId.ToString();
            _unitOfWork.BookingRepository.Update(bk);
            _unitOfWork.Save();
            SaveMemberTypePlayerMapping(booking.BookingId, commonViewModel.MemberTypeViewModels);
            SaveCourseTaxMapping(booking.BookingId, commonViewModel.CourseTaxMappingViewModels);
            BookingViewModel bookingViewModel = new BookingViewModel()
            {
                BookingId = booking.BookingId,
                PaymentGatewayBookingId = booking.PaymentGatewayBookingId,
                TotalAmount = bk.Amount.GetValueOrDefault(),
                ConvertedAmount = ConvertCurrency(bk.Amount.GetValueOrDefault())
            };
            return bookingViewModel;
        }

        /// <summary>
        /// Update Booking Status  And Send Mail If Payment Success
        /// </summary>
        /// <param name="commonViewModel"></param>
        /// <returns></returns>
        public Tuple<bool, string> UpdateBookingStatus(CommonViewModel commonViewModel)
        {

            Booking booking = _unitOfWork.BookingRepository.Get(x => x.PaymentGatewayBookingId == commonViewModel.PaymentGatewayBookingId);
            if (booking == null)
                throw new Exception("No Booking For Booking Id");
            if (booking.BookingStatusId != (int)Core.Helper.Enum.EnumBookingStatus.Pending)
            {
                throw new Exception("Booking Status Already Updated.");
            }

            booking.PaymentMode = "";
            booking.PaymentStatusId = commonViewModel.PaymentStatusId;
            if (commonViewModel.PaymentStatusId == (int)Core.Helper.Enum.EnumPaymentStatus.Success)
            {
                if (booking.PaidAmount <= 0 || booking.IsPaymentGatewaySkip == false) { }
                else
                {
                    checkPayment(commonViewModel.PaymentGatewayBookingId, booking.PaidAmount.GetValueOrDefault());
                }
                booking.BookingStatusId = (int)Core.Helper.Enum.EnumBookingStatus.Confirm;

            }
            else
            {
                booking.BookingStatusId = (int)Core.Helper.Enum.EnumBookingStatus.Failed;

            }
            booking.UpdatedOn = System.DateTime.UtcNow;
            _unitOfWork.BookingRepository.Update(booking);
            _unitOfWork.Save();
            if (commonViewModel.PaymentStatusId == (int)Core.Helper.Enum.EnumPaymentStatus.Success)
            {
                string emailBody = "";
                if (booking.BookingTypeId == (int)Core.Helper.Enum.EnumBookingType.BTT)
                {

                    string coursePairingName = booking.CoursePairingId != null ? (booking.CoursePairing.CourseName1.Value) + (booking.CoursePairing.EndCourseNameId != null ? " - " + booking.CoursePairing.CourseName.Value : "") : "";
                    emailBody = " Dear Member,<br/><br/>Thank you for choosing to book with us!<br/><br/>Your reservation is confirmed as follows:<br/><br/>";
                    emailBody = emailBody + "Name: " + booking.Golfer.Name + "<br/>Date: " + booking.TeeOffDate.ToString("dd/MM/yyyy") + "<br/>No.of golfers: " + booking.NoOfPlayer + "<br/>Golf course: " + coursePairingName + "<br/>Tee-time:" + DateTime.Parse(booking.TeeOffSlot).ToString("hh:mm tt", CultureInfo.GetCultureInfo("en-US")) + "<br/><br/>";
                    emailBody = emailBody + "*Payment to be made at the Pro - Shop on arrival.<br/><br/>We look forward to welcoming you!<br/><br/>Warm regards,<br/>Classic Golf & Country Club";


                    //emailBody = "Dear Member, your tee time booking is confirmed as follows -<br/> Booking ID - " + booking.PaymentGatewayBookingId + "<br/> Date - " + booking.TeeOffDate.ToString("dd/MM/yyyy") + "<br/> Time - " + DateTime.Parse(booking.TeeOffSlot).ToString("hh:mm tt", CultureInfo.GetCultureInfo("en-US")) + "<br/> No. of Holes - " + booking.HoleType.Value + "<br/> Course Name - " + coursePairingName + "<br/>";
                    //foreach (var item in booking.BookingPlayerDetails)
                    //{
                    //    if (item.Player1 == null || item.Player1 == "")
                    //    {
                    //        emailBody = emailBody + "Player Name/Membership Number : " + item.Player1 + "<br/>";
                    //    }


                    //}
                    //emailBody = emailBody + "<br/> We look forward to seeing you at " + Core.Helper.Constants.Common.AppName + "!";
                }
                else
                {

                    emailBody = "Dear Member, your driving range booking is confirmed as follows -<br/> Booking ID - " + booking.PaymentGatewayBookingId + "<br/> Date - " + booking.TeeOffDate.ToString("dd/MM/yyyy") + "<br/> Time - " + DateTime.Parse(booking.TeeOffSlot).ToString("hh:mm tt", CultureInfo.GetCultureInfo("en-US")) + "<br/>";
                    foreach (var item in booking.BookingPlayerDetails)
                    {
                        if (item.Player1 == null || item.Player1 == "")
                        {
                            emailBody = emailBody + "Player Name/Membership Number : " + item.Player1 + "<br/>";
                        }


                    }
                    emailBody = emailBody + "<br/> We look forward to seeing you at " + Core.Helper.Constants.Common.AppName + "!";
                }
                MailerViewModel mailerViewModel = new MailerViewModel()
                {
                    ToEmails = booking.Golfer.Email,
                    From = Constants.MailId.FromMails,
                    CCMail = Constants.MailId.CCMails,
                    Subject = "Booking For " + Constants.Common.AppName + " ",
                    Body = emailBody
                };
                EmailNotification emailNotification = new EmailNotification();
                bool status = emailNotification.SendMail(mailerViewModel);
                // if (status == false) { throw new Exception("Email Not Sent."); }
            }

            return new Tuple<bool, string>(true, (Crypto.EncryptStringAES(booking.BookingId.ToString())));
        }

        /// <summary>
        /// Get  Converted Amount  Using API
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        private decimal ConvertCurrency(decimal amount)
        {
            decimal convertedAmount = 0;
            if (Core.Helper.Constants.Currency.CurrencyExchangeEnable)
            {
                CurrencyExchangeViewModel model = new CurrencyExchangeViewModel();
                CommonViewModel commonViewModel = new CommonViewModel()
                {
                    baseCurrency = Core.Helper.Constants.Currency.CurrencyName,
                    currencyISO = Core.Helper.Constants.Currency.ConvertedCurrencyName
                };
                HttpClient client = new HttpClient
                {
                    BaseAddress = new Uri(Core.Helper.Constants.Currency.ConvertCurrencyBaseURL)
                };
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/Json"));
                HttpResponseMessage response = client.PostAsJsonAsync(Core.Helper.Constants.Currency.ConvertCurrencyRelativeURL, commonViewModel).Result;
                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    model = JsonConvert.DeserializeObject<ViewModel.CurrencyExchangeViewModel>(data);
                }
                convertedAmount = amount * model.Rate;
            }
            else
            {
                convertedAmount = amount;
            }
            return convertedAmount;
        }

        /// <summary>
        /// Check Slot Is Available Or Not
        /// </summary>
        /// <param name="slotTime"></param>
        /// <param name="bookingTypeId"></param>
        /// <param name="teeOffDate"></param>
        /// <returns></returns>
        private bool IsSlotAvailable(string slotTime, long bookingTypeId, DateTime teeOffDate, int noOfPlayerBooking, long coursePairingId)
        {
            List<Booking> bookings = new List<Booking>();
            if (bookingTypeId == (int)EnumBookingType.BTT)
            {
                long coursePairingId1 = 0;
                if (coursePairingId == 1) { coursePairingId1 = 4; }
                if (coursePairingId == 2) { coursePairingId1 = 5; }
                if (coursePairingId == 3) { coursePairingId1 = 6; }
                if (coursePairingId == 4) { coursePairingId1 = 1; }
                if (coursePairingId == 5) { coursePairingId1 = 2; }
                if (coursePairingId == 6) { coursePairingId1 = 3; }
                bookings = _unitOfWork.BookingRepository.GetMany(x => x.TeeOffDate == teeOffDate && x.TeeOffSlot == slotTime && x.IsActive == true && x.BookingTypeId == bookingTypeId && (x.BookingStatusId == (int)EnumBookingStatus.Confirm || (x.BookingStatusId == (int)EnumBookingStatus.Pending && x.CreatedOn.AddMinutes(15) > DateTime.UtcNow)) && (x.CoursePairingId == coursePairingId || x.CoursePairingId == coursePairingId1)).ToList();

            }
            else
            {
                bookings = _unitOfWork.BookingRepository.GetMany(x => x.TeeOffDate == teeOffDate && x.TeeOffSlot == slotTime && x.IsActive == true && x.BookingTypeId == bookingTypeId && (x.BookingStatusId == (int)EnumBookingStatus.Confirm || (x.BookingStatusId == (int)EnumBookingStatus.Pending && x.CreatedOn.AddMinutes(15) > DateTime.UtcNow))).ToList();

            }
            DateTime currentTime = Core.Helper.DateHelper.ConvertSystemDate();
            int noOfPlayer = bookings.Select(x => x.NoOfPlayer).Sum();
            int playerLeft = 4 - (noOfPlayer + noOfPlayerBooking);
            if (bookings.Count() == 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Get Day Type From Date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        private long GetDayTypeId(DateTime date)
        {
            List<NationalHoliday> nationalHolidays = _unitOfWork.NationalHolidayRepository.GetMany(x => x.Day == date.Day && x.Month == date.Month && x.Year == date.Year && x.IsActive == true).ToList();
            if (nationalHolidays.Count == 0)
            {
                return Core.Helper.DateHelper.GetDayTypeFromDate(date);
            }
            else
            {
                return (int)Core.Helper.Enum.EnumDayType.NationalHoliday;
            }

        }

        /// <summary>
        /// Check Date is National Holiday or Weekend 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public bool CheckNationalHolidayOrWeekend(DateTime date)
        {
            List<NationalHoliday> nationalHolidays = _unitOfWork.NationalHolidayRepository.GetMany(x => x.Day == date.Day && x.Month == date.Month && x.Year == date.Year && x.IsActive == true).ToList();
            if (nationalHolidays.Count == 0)
            {
                if ((int)Core.Helper.Enum.EnumDayType.Weekend == Core.Helper.DateHelper.GetDayTypeFromDate(date))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }

        }

        /// <summary>
        /// Find All Member Type
        /// </summary>
        /// <param name="golferId"></param>
        /// <returns></returns>
        public List<MemberTypeViewModel> GetAllMemberType(long golferId)
        {

            Golfer golfer = _unitOfWork.GolferRepository.Get(x => x.GolferId == golferId);
            List<MemberTypeViewModel> memberTypeViewModels = new List<MemberTypeViewModel>();
            List<MemberType> memberTypes = new List<MemberType>();
            if (golfer.MemberTypeId == (int)Core.Helper.Enum.EnumMemberType.NonMember)
            {
                memberTypes = _unitOfWork.MemberTypeRepository.GetMany(x => x.IsActive == true && x.MemberTypeId == (int)Core.Helper.Enum.EnumMemberType.NonMember).ToList();
            }
            else
            {
                memberTypes = _unitOfWork.MemberTypeRepository.GetMany(x => x.IsActive == true && x.MemberTypeId != (int)Core.Helper.Enum.EnumMemberType.NonMember).ToList();
            }

            foreach (var item in memberTypes)
            {
                MemberTypeViewModel memberTypeViewModel = new MemberTypeViewModel()
                {
                    MemberTypeId = item.MemberTypeId,
                    Name = item.Name,
                    ValueToShow = item.ValueToShow,
                    PlayerCount = 0
                };
                memberTypeViewModels.Add(memberTypeViewModel);
            }
            return memberTypeViewModels;
        }

        /// <summary>
        /// Save Member Type Player Mapping For Booking
        /// </summary>
        /// <param name="bookingId"></param>
        /// <param name="memberTypeViewModels"></param>
        private void SaveMemberTypePlayerMapping(long bookingId, List<MemberTypeViewModel> memberTypeViewModels)
        {
            foreach (var memberType in memberTypeViewModels)
            {
                if (memberType.PlayerCount != 0)
                {
                    BookingPlayerMapping bookingPlayerMapping = new BookingPlayerMapping()
                    {
                        BookingId = bookingId,
                        PlayerCount = memberType.PlayerCount,
                        MemberTypeId = memberType.MemberTypeId,
                        IsActive = true,
                        CreatedOn = DateTime.UtcNow,
                        MemberTypeName = memberType.Name

                    };
                    _unitOfWork.BookingPlayerMappingRepository.Insert(bookingPlayerMapping);
                }
            }
            _unitOfWork.Save();
        }

        /// <summary>
        /// Save Course Tax Mapping For Booking
        /// </summary>
        /// <param name="bookingId"></param>
        /// <param name="courseTaxMappingViewModels"></param>
        private void SaveCourseTaxMapping(long bookingId, List<CourseTaxMappingViewModel> courseTaxMappingViewModels)
        {
            if (courseTaxMappingViewModels != null)
            {
                foreach (var tax in courseTaxMappingViewModels)
                {

                    BookingCourseTaxMapping bookingCourseTaxMapping = new BookingCourseTaxMapping()
                    {
                        BookingId = bookingId,
                        TaxId = tax.TaxId,
                        IsActive = true,
                        CreatedOn = DateTime.UtcNow,
                        TaxName = tax.Name,
                        Percentage = tax.Percentage

                    };
                    _unitOfWork.BookingCourseTaxMappingRepository.Insert(bookingCourseTaxMapping);

                }
                _unitOfWork.Save();
            }
        }

        /// <summary>
        /// Get All CourseTax For Course
        /// </summary>
        /// <returns></returns>
        private List<CourseTaxMappingViewModel> GetAllCourseTaxs()
        {
            List<CourseTaxMappingViewModel> courseTaxMappingViewModels = new List<CourseTaxMappingViewModel>();
            List<CourseTaxMapping> courseTaxMappings = _unitOfWork.CourseTaxMappingRepository.GetMany(x => x.IsActive == true).ToList();
            foreach (var tax in courseTaxMappings)
            {
                CourseTaxMappingViewModel courseTaxMappingViewModel = new CourseTaxMappingViewModel()
                {
                    TaxId = tax.Tax.TaxId,
                    Name = tax.Tax.Name,
                    Percentage = tax.Tax.Percentage

                };
                courseTaxMappingViewModels.Add(courseTaxMappingViewModel);
            }
            return courseTaxMappingViewModels;
        }

        private List<BookingEquipmentMappingViewModel> GetAllEquipment(DateTime date, bool checkMinCondition)
        {
            List<Booking> bookings = new List<Booking>();
            if (checkMinCondition == true)
            {
                bookings = _unitOfWork.BookingRepository.GetMany(x => x.TeeOffDate == date && x.IsActive == true && x.BookingTypeId == (int)EnumBookingType.BTT && (x.BookingStatusId == (int)EnumBookingStatus.Confirm || (x.BookingStatusId == (int)EnumBookingStatus.Pending && x.CreatedOn.AddMinutes(15) > DateTime.UtcNow))).ToList();
            }

            List<BookingEquipmentMappingViewModel> bookingEquipmentMappingViewModels = new List<BookingEquipmentMappingViewModel>();
            List<Equipment> equipments = _unitOfWork.EquipmentRepository.GetMany(x => x.IsActive == true).ToList();
            foreach (var equipment in equipments)
            {
                var count = 0;
                if (checkMinCondition == true)
                {
                    foreach (var booking in bookings)
                    {
                        foreach (var item in booking.BookingEquipmentMappings)
                        {
                            if (item.EquipmentId == equipment.EquipmentId)
                            {
                                count = count + item.EquipmentCount;
                            }
                        };
                    }
                }

                BookingEquipmentMappingViewModel bookingEquipmentMappingViewModel = new BookingEquipmentMappingViewModel()
                {
                    EquipmentId = equipment.EquipmentId,
                    EquipmentName = equipment.Name,
                    EquipmentPrice = equipment.Price,
                    EquipmentCount = 0,
                    EquipmentLeft = 4 - count
                };
                decimal totalPrec = 0;

                foreach (var tax in equipment.EquipmentTaxMappings.Where(x => x.IsActive == true))
                {
                    totalPrec += tax.Tax.Percentage;

                }
                bookingEquipmentMappingViewModel.EquipmentPrice = Math.Round(bookingEquipmentMappingViewModel.EquipmentPrice.GetValueOrDefault() + (bookingEquipmentMappingViewModel.EquipmentPrice.GetValueOrDefault() * (totalPrec / 100)), 2);

                if (bookingEquipmentMappingViewModel.EquipmentLeft > 0)
                {
                    bookingEquipmentMappingViewModels.Add(bookingEquipmentMappingViewModel);
                }
            }
            return bookingEquipmentMappingViewModels;
        }


        private void SaveBookingEquipmentMappingMapping(long bookingId, List<BookingEquipmentMappingViewModel> bookingEquipmentMappingViewModels)
        {
            if (bookingEquipmentMappingViewModels != null)
            {
                foreach (var bookingEquipmentMappingViewModel in bookingEquipmentMappingViewModels)
                {
                    if (bookingEquipmentMappingViewModel.EquipmentCount != 0)
                    {
                        BookingEquipmentMapping bookingEquipmentMapping = new BookingEquipmentMapping()
                        {
                            BookingId = bookingId,
                            EquipmentCount = bookingEquipmentMappingViewModel.EquipmentCount,
                            EquipmentId = bookingEquipmentMappingViewModel.EquipmentId,
                            IsActive = true,
                            CreatedOn = DateTime.UtcNow,
                            EquipmentName = bookingEquipmentMappingViewModel.EquipmentName,
                            EquipmentPrice = bookingEquipmentMappingViewModel.EquipmentPrice,

                        };
                        _unitOfWork.BookingEquipmentMappingRepository.Insert(bookingEquipmentMapping);

                    }
                }
                _unitOfWork.Save();
            }
        }


        public bool CheckNationalHoliday(DateTime date)
        {
            List<NationalHoliday> nationalHolidays = _unitOfWork.NationalHolidayRepository.GetMany(x => x.Day == date.Day && x.Month == date.Month && x.Year == date.Year && x.IsActive == true).ToList();
            if (nationalHolidays.Count == 0)
            {
                return false;

            }
            else
            {
                return true;
            }

        }

        private void SaveBookingPlayerDetail(long bookingId, List<BookingPlayerDetailViewModel> bookingPlayerDetailViewModels)
        {
            if (bookingPlayerDetailViewModels != null)
            {
                BookingPlayerDetail bookingPlayerDetail = new BookingPlayerDetail
                {
                    BookingId = bookingId,
                    IsActive = true,
                    CreatedOn = System.DateTime.UtcNow
                };

                foreach (var playerDetail in bookingPlayerDetailViewModels)
                {
                    if (playerDetail.PlayerSerialNumber.Trim() == "Player1")
                    {
                        bookingPlayerDetail.Player1 = playerDetail.PlayerDetails;
                    }
                    else if (playerDetail.PlayerSerialNumber.Trim() == "Player2")
                    {
                        bookingPlayerDetail.Player2 = playerDetail.PlayerDetails;
                    }
                    else if (playerDetail.PlayerSerialNumber.Trim() == "Player3")
                    {
                        bookingPlayerDetail.Player3 = playerDetail.PlayerDetails;
                    }
                    else if (playerDetail.PlayerSerialNumber.Trim() == "Player4")
                    {
                        bookingPlayerDetail.Player4 = playerDetail.PlayerDetails;
                    }


                }
                _unitOfWork.BookingPlayerDetailRepository.Insert(bookingPlayerDetail);
                _unitOfWork.Save();
            }
        }

        /// <summary>
        /// Get Slot Details For BDR And BDT
        /// </summary>
        /// <param name="date"></param>
        /// <param name="bookingTypeId"></param>
        /// <returns></returns>
        public SlotViewModel GetSlotDetailsByDateAndBookingType(DateTime date, long bookingTypeId, long? coursePairingId, long golferId, long? promotionId)
        {
            Promotion promotion = new Promotion();
            if (promotionId != null && promotionId != 0)
            {
                promotion = PromotionData(promotionId.GetValueOrDefault());
            }
            SlotViewModel slotViewModel = new SlotViewModel();
            List<long> removeSlot = new List<long>();
            List<long> disableSlot = new List<long>();
            if (bookingTypeId == (int)EnumBookingType.BTT)
            {
                long coursePairingId1 = 0;
                long coursePairingId2 = 0;
                List<BlockSlotRange> blockSlotRanges = new List<BlockSlotRange>();
                if (coursePairingId==1 || coursePairingId==2 || coursePairingId == 3)
                {
                    if (coursePairingId == 1) { coursePairingId1 = 4; coursePairingId2 = 5; }
                    if (coursePairingId == 2) { coursePairingId1 = 5; coursePairingId2 = 6; }
                    if (coursePairingId == 3) { coursePairingId1 = 6; coursePairingId2 = 4; }
                    blockSlotRanges = _unitOfWork.BlockSlotRangeRepository.GetMany(x => x.StartDate <= date && x.EndDate >= date && x.IsActive == true && ( x.CoursePairingId == null || x.CoursePairingId == coursePairingId1 || x.CoursePairingId == coursePairingId2)).ToList();

                }
                else
                {
                  
                    blockSlotRanges = _unitOfWork.BlockSlotRangeRepository.GetMany(x => x.StartDate <= date && x.EndDate >= date && x.IsActive == true && (x.CoursePairingId == coursePairingId || x.CoursePairingId == null)).ToList();

                }



                foreach (BlockSlotRange blockSlotRange in blockSlotRanges)
                {
                    List<BlockSlot> blockSlots = _unitOfWork.BlockSlotRepository.GetMany(x => x.IsActive == true && x.BlockSlotRangeId == blockSlotRange.BlockSlotRangeId).ToList();
                    if (blockSlotRange.IsCustomerAvailable != true)
                    {
                        removeSlot.AddRange(blockSlots.Select(x => x.SlotSessionWiseId).ToList());
                    }
                    else
                    {
                        if (blockSlotRange.IsBookingavailable != true)
                            disableSlot.AddRange(blockSlots.Select(x => x.SlotSessionWiseId).ToList());
                    }
                }
            }
            List<SessionSlotViewModel> sessionSlotViewModels = new List<SessionSlotViewModel>();
            DateTime currentTime = Core.Helper.DateHelper.ConvertSystemDate();
            List<Session> sessions = _unitOfWork.SessionRepository.GetMany(x => x.IsActive == true && x.BookingTypeId == bookingTypeId).ToList();
            var dayId = GetDayTypeId(date);
            //Min Max Player Condition
            var minPlayer = Core.Helper.Constants.PlayerCount.MinPlayerCount;
            if (dayId == (int)Core.Helper.Enum.EnumDayType.Saturday || dayId == (int)Core.Helper.Enum.EnumDayType.Sunday || CheckNationalHoliday(date) == true)
            {
                minPlayer = Core.Helper.Constants.PlayerCount.WeekendMinPlayerCount;
            }
            int bookingCount = 0;
            var golfer = _unitOfWork.GolferRepository.Get(x => x.GolferId == golferId);
            var golferBookings1 = _unitOfWork.BookingRepository.GetMany(x => x.TeeOffDate == date && x.IsActive == true && x.BookingTypeId == bookingTypeId && (x.BookingStatusId == (int)EnumBookingStatus.Confirm || (x.BookingStatusId == (int)EnumBookingStatus.Pending && x.CreatedOn.AddMinutes(15) > DateTime.UtcNow))).ToList();
            foreach (var golf in golferBookings1)
            {
                var bookingPalyerDetails = golf.BookingPlayerDetails.ToList();
                if (bookingPalyerDetails != null && bookingPalyerDetails.Count != 0)
                {
                    if (bookingPalyerDetails[0].Player2 == golfer.ClubMemberId)
                        bookingCount++;
                    if (bookingPalyerDetails[0].Player3 == golfer.ClubMemberId)
                        bookingCount++;
                    if (bookingPalyerDetails[0].Player4 == golfer.ClubMemberId)
                        bookingCount++;
                }
            }

            var golferBookings = _unitOfWork.BookingRepository.GetMany(x => x.TeeOffDate == date && x.GolferId == golferId && x.IsActive == true && x.BookingTypeId == bookingTypeId && (x.BookingStatusId == (int)EnumBookingStatus.Confirm || (x.BookingStatusId == (int)EnumBookingStatus.Pending && x.CreatedOn.AddMinutes(15) > DateTime.UtcNow))).ToList();
            //Max Booking Allowed Per User
            int bookingLeft = 2 - (golferBookings.Count() + bookingCount);
            var isBookingEnable = true;
            if (bookingLeft <= 0)
            {
                isBookingEnable = false;
                bookingLeft = 0;
            }


            foreach (Session session in sessions)
            {
                SessionSlotViewModel sessionSlotViewModel = new SessionSlotViewModel()
                {
                    SessionName = session.Name + "(" + session.StartTime + " - " + session.EndTime + ")"
                };
                List<SlotSessionWise> slotSessionWises = _unitOfWork.SlotSessionWiseRepository.GetMany(x => x.SessionId == session.SessionId && x.IsActive == true && x.BookingTypeId == bookingTypeId).ToList();

                if (bookingTypeId == (int)EnumBookingType.BTT)
                {
                    foreach (var slot in removeSlot)
                    {
                        var itemToRemove = slotSessionWises.SingleOrDefault(r => r.SlotSessionWiseId == slot);
                        if (itemToRemove != null)
                            slotSessionWises.Remove(itemToRemove);
                    }
                }

                foreach (SlotSessionWise slotSessionWise in slotSessionWises.OrderBy(x => x.Slot.Time))
                {
                    if (isBookingEnable)
                    {
                        List<Booking> bookings = new List<Booking>();
                        SlotDetailsViewModel slotDetailsViewModel = new SlotDetailsViewModel();
                        if (bookingTypeId == (int)EnumBookingType.BTT)
                        {
                            long coursePairingId1 = 0;
                            if (coursePairingId == 1) { coursePairingId1 = 4; }
                            if (coursePairingId == 2) { coursePairingId1 = 5; }
                            if (coursePairingId == 3) { coursePairingId1 = 6; }
                            if (coursePairingId == 4) { coursePairingId1 = 1; }
                            if (coursePairingId == 5) { coursePairingId1 = 2; }
                            if (coursePairingId == 6) { coursePairingId1 = 3; }
                            bookings = _unitOfWork.BookingRepository.GetMany(x => x.TeeOffDate == date && x.TeeOffSlot == slotSessionWise.Slot.Time.ToString("hh':'mm") && x.IsActive == true && x.BookingTypeId == bookingTypeId && (x.BookingStatusId == (int)EnumBookingStatus.Confirm || (x.BookingStatusId == (int)EnumBookingStatus.Pending && x.CreatedOn.AddMinutes(15) > DateTime.UtcNow)) && (x.CoursePairingId == coursePairingId || x.CoursePairingId == coursePairingId1)).ToList();
                            var itemToRemove = disableSlot.SingleOrDefault(r => r == slotSessionWise.SlotSessionWiseId);
                            if (itemToRemove != 0)
                            {
                                slotDetailsViewModel.SlotId = slotSessionWise.SlotId;
                                slotDetailsViewModel.SlotTime = slotSessionWise.Slot.Time.ToString("hh':'mm");
                                slotDetailsViewModel.IsAvailable = false;
                                sessionSlotViewModel.SlotDetailsViewModelList.Add(slotDetailsViewModel);
                                continue;
                            }
                        }
                        else
                        {
                            bookings = _unitOfWork.BookingRepository.GetMany(x => x.TeeOffDate == date && x.TeeOffSlot == slotSessionWise.Slot.Time.ToString("hh':'mm") && x.IsActive == true && x.BookingTypeId == bookingTypeId && (x.BookingStatusId == (int)EnumBookingStatus.Confirm || (x.BookingStatusId == (int)EnumBookingStatus.Pending && x.CreatedOn.AddMinutes(15) > DateTime.UtcNow))).ToList();
                        }
                        int noOfPlayer = bookings.Select(x => x.NoOfPlayer).Sum();
                        if (promotionId != null && promotionId != 0)
                        {
                            if (promotion.StartTime != null && promotion.EndTime != null && promotion.StartTime != new TimeSpan() && promotion.EndTime != new TimeSpan())
                            {
                                if (promotion.StartTime <= slotSessionWise.Slot.Time && promotion.EndTime >= slotSessionWise.Slot.Time)
                                {
                                    if (bookings.Count() == 0)
                                    {
                                        slotDetailsViewModel.SlotId = slotSessionWise.SlotId;
                                        slotDetailsViewModel.SlotTime = slotSessionWise.Slot.Time.ToString("hh':'mm");
                                        if ((slotSessionWise.Slot.Time >= currentTime.TimeOfDay && date.Month == currentTime.Month && date.Year == currentTime.Year && date.Day == currentTime.Day))
                                        {
                                            slotDetailsViewModel.IsAvailable = true;
                                            slotDetailsViewModel.PlayerLeftCount = (4 - noOfPlayer);
                                            if (slotDetailsViewModel.PlayerLeftCount < minPlayer)
                                            { slotDetailsViewModel.IsAvailable = false; }
                                            slotDetailsViewModel.MinPlayerCount = minPlayer;
                                        }
                                        else { slotDetailsViewModel.IsAvailable = false; }
                                        if (date > currentTime)
                                        {
                                            slotDetailsViewModel.IsAvailable = true;
                                            slotDetailsViewModel.PlayerLeftCount = (4 - noOfPlayer);
                                            if (slotDetailsViewModel.PlayerLeftCount < minPlayer)
                                            { slotDetailsViewModel.IsAvailable = false; }
                                            slotDetailsViewModel.MinPlayerCount = minPlayer;
                                        }
                                    }
                                    else
                                    {

                                        slotDetailsViewModel.SlotId = slotSessionWise.SlotId;
                                        slotDetailsViewModel.SlotTime = slotSessionWise.Slot.Time.ToString("hh':'mm");
                                        slotDetailsViewModel.IsAvailable = false;
                                    }
                                }
                                else
                                {
                                    slotDetailsViewModel.SlotId = slotSessionWise.SlotId;
                                    slotDetailsViewModel.SlotTime = slotSessionWise.Slot.Time.ToString("hh':'mm");
                                    slotDetailsViewModel.IsAvailable = false;
                                }
                            }
                            else
                            {
                                if (bookings.Count() == 0)
                                {
                                    slotDetailsViewModel.SlotId = slotSessionWise.SlotId;
                                    slotDetailsViewModel.SlotTime = slotSessionWise.Slot.Time.ToString("hh':'mm");
                                    if ((slotSessionWise.Slot.Time >= currentTime.TimeOfDay && date.Month == currentTime.Month && date.Year == currentTime.Year && date.Day == currentTime.Day))
                                    {
                                        slotDetailsViewModel.IsAvailable = true;
                                        slotDetailsViewModel.PlayerLeftCount = (4 - noOfPlayer);
                                        if (slotDetailsViewModel.PlayerLeftCount < minPlayer)
                                        { slotDetailsViewModel.IsAvailable = false; }
                                        slotDetailsViewModel.MinPlayerCount = minPlayer;
                                    }
                                    else { slotDetailsViewModel.IsAvailable = false; }
                                    if (date > currentTime)
                                    {
                                        slotDetailsViewModel.IsAvailable = true;
                                        slotDetailsViewModel.PlayerLeftCount = (4 - noOfPlayer);
                                        if (slotDetailsViewModel.PlayerLeftCount < minPlayer)
                                        { slotDetailsViewModel.IsAvailable = false; }
                                        slotDetailsViewModel.MinPlayerCount = minPlayer;
                                    }
                                }
                                else
                                {

                                    slotDetailsViewModel.SlotId = slotSessionWise.SlotId;
                                    slotDetailsViewModel.SlotTime = slotSessionWise.Slot.Time.ToString("hh':'mm");
                                    slotDetailsViewModel.IsAvailable = false;
                                }
                            }
                        }
                        else
                        {
                            if (bookings.Count() == 0)
                            {
                                slotDetailsViewModel.SlotId = slotSessionWise.SlotId;
                                slotDetailsViewModel.SlotTime = slotSessionWise.Slot.Time.ToString("hh':'mm");
                                if ((slotSessionWise.Slot.Time >= currentTime.TimeOfDay && date.Month == currentTime.Month && date.Year == currentTime.Year && date.Day == currentTime.Day))
                                {
                                    slotDetailsViewModel.IsAvailable = true;
                                    slotDetailsViewModel.PlayerLeftCount = (4 - noOfPlayer);
                                    if (slotDetailsViewModel.PlayerLeftCount < minPlayer)
                                    { slotDetailsViewModel.IsAvailable = false; }
                                    slotDetailsViewModel.MinPlayerCount = minPlayer;
                                }
                                else { slotDetailsViewModel.IsAvailable = false; }
                                if (date > currentTime)
                                {
                                    slotDetailsViewModel.IsAvailable = true;
                                    slotDetailsViewModel.PlayerLeftCount = (4 - noOfPlayer);
                                    if (slotDetailsViewModel.PlayerLeftCount < minPlayer)
                                    { slotDetailsViewModel.IsAvailable = false; }
                                    slotDetailsViewModel.MinPlayerCount = minPlayer;
                                }
                            }
                            else
                            {

                                slotDetailsViewModel.SlotId = slotSessionWise.SlotId;
                                slotDetailsViewModel.SlotTime = slotSessionWise.Slot.Time.ToString("hh':'mm");
                                slotDetailsViewModel.IsAvailable = false;
                            }
                        }

                        sessionSlotViewModel.SlotDetailsViewModelList.Add(slotDetailsViewModel);
                    }
                    else
                    {
                        SlotDetailsViewModel slotDetailsViewModel = new SlotDetailsViewModel();
                        slotDetailsViewModel.SlotId = slotSessionWise.SlotId;
                        slotDetailsViewModel.SlotTime = slotSessionWise.Slot.Time.ToString("hh':'mm");
                        slotDetailsViewModel.IsAvailable = false;
                    }
                }
                sessionSlotViewModels.Add(sessionSlotViewModel);
            }

            slotViewModel.BookingLeft = bookingLeft;
            slotViewModel.SessionSlotViewModels = sessionSlotViewModels;
            return slotViewModel;
        }

        private void CheckBookingForGolfer(List<BookingPlayerDetailViewModel> bookingPlayerDetailViewModels, DateTime teeOffDate, long bookingTypeId)
        {
            if (bookingPlayerDetailViewModels != null)
            {

                foreach (var playerDetail in bookingPlayerDetailViewModels)
                {
                    if (playerDetail.PlayerSerialNumber.Trim() == "Player1")
                    {
                        int bookingCount = 0;
                        var golfer = _unitOfWork.GolferRepository.Get(x => x.ClubMemberId == playerDetail.PlayerDetails);
                        if (golfer != null)
                        {
                            var golferBookings1 = _unitOfWork.BookingRepository.GetMany(x => x.TeeOffDate == teeOffDate && x.IsActive == true && x.BookingTypeId == bookingTypeId && (x.BookingStatusId == (int)EnumBookingStatus.Confirm || (x.BookingStatusId == (int)EnumBookingStatus.Pending && x.CreatedOn.AddMinutes(15) > DateTime.UtcNow))).ToList();
                            foreach (var golf in golferBookings1)
                            {
                                var bookingPalyerDetails = golf.BookingPlayerDetails.ToList();
                                if (bookingPalyerDetails != null && bookingPalyerDetails.Count != 0)
                                {
                                    if (bookingPalyerDetails[0].Player2 == golfer.ClubMemberId)
                                        bookingCount++;
                                    if (bookingPalyerDetails[0].Player3 == golfer.ClubMemberId)
                                        bookingCount++;
                                    if (bookingPalyerDetails[0].Player4 == golfer.ClubMemberId)
                                        bookingCount++;
                                }
                            }
                        }
                        var golferBookings = _unitOfWork.BookingRepository.GetMany(x => x.TeeOffDate == teeOffDate && x.Golfer.ClubMemberId == playerDetail.PlayerDetails && x.IsActive == true && x.BookingTypeId == bookingTypeId && (x.BookingStatusId == (int)EnumBookingStatus.Confirm || (x.BookingStatusId == (int)EnumBookingStatus.Pending && x.CreatedOn.AddMinutes(15) > DateTime.UtcNow))).ToList();
                        //Max Booking Allowed Per User
                        int bookingLeft = 2 - (golferBookings.Count() + bookingCount);
                        if (bookingLeft <= 0)
                            throw new Exception("Max Two Booking Per Day.Please Check Golfer 1");
                    }
                    else if (playerDetail.PlayerSerialNumber.Trim() == "Player2")
                    {
                        int bookingCount = 0;
                        var golfer = _unitOfWork.GolferRepository.Get(x => x.ClubMemberId == playerDetail.PlayerDetails);
                        if (golfer != null)
                        {
                            var golferBookings1 = _unitOfWork.BookingRepository.GetMany(x => x.TeeOffDate == teeOffDate && x.IsActive == true && x.BookingTypeId == bookingTypeId && (x.BookingStatusId == (int)EnumBookingStatus.Confirm || (x.BookingStatusId == (int)EnumBookingStatus.Pending && x.CreatedOn.AddMinutes(15) > DateTime.UtcNow))).ToList();
                            foreach (var golf in golferBookings1)
                            {
                                var bookingPalyerDetails = golf.BookingPlayerDetails.ToList();
                                if (bookingPalyerDetails != null && bookingPalyerDetails.Count != 0)
                                {
                                    if (bookingPalyerDetails[0].Player2 == golfer.ClubMemberId)
                                        bookingCount++;
                                    if (bookingPalyerDetails[0].Player3 == golfer.ClubMemberId)
                                        bookingCount++;
                                    if (bookingPalyerDetails[0].Player4 == golfer.ClubMemberId)
                                        bookingCount++;
                                }
                            }
                        }
                        var golferBookings = _unitOfWork.BookingRepository.GetMany(x => x.TeeOffDate == teeOffDate && x.Golfer.ClubMemberId == playerDetail.PlayerDetails && x.IsActive == true && x.BookingTypeId == bookingTypeId && (x.BookingStatusId == (int)EnumBookingStatus.Confirm || (x.BookingStatusId == (int)EnumBookingStatus.Pending && x.CreatedOn.AddMinutes(15) > DateTime.UtcNow))).ToList();
                        //Max Booking Allowed Per User
                        int bookingLeft = 2 - (golferBookings.Count() + bookingCount);
                        if (bookingLeft <= 0)
                            throw new Exception("Max Two Booking Per Day.Please Check Golfer 2");
                    }
                    else if (playerDetail.PlayerSerialNumber.Trim() == "Player3")
                    {
                        int bookingCount = 0;
                        var golfer = _unitOfWork.GolferRepository.Get(x => x.ClubMemberId == playerDetail.PlayerDetails);
                        if (golfer != null)
                        {
                            var golferBookings1 = _unitOfWork.BookingRepository.GetMany(x => x.TeeOffDate == teeOffDate && x.IsActive == true && x.BookingTypeId == bookingTypeId && (x.BookingStatusId == (int)EnumBookingStatus.Confirm || (x.BookingStatusId == (int)EnumBookingStatus.Pending && x.CreatedOn.AddMinutes(15) > DateTime.UtcNow))).ToList();
                            foreach (var golf in golferBookings1)
                            {
                                var bookingPalyerDetails = golf.BookingPlayerDetails.ToList();
                                if (bookingPalyerDetails != null && bookingPalyerDetails.Count != 0)
                                {
                                    if (bookingPalyerDetails[0].Player2 == golfer.ClubMemberId)
                                        bookingCount++;
                                    if (bookingPalyerDetails[0].Player3 == golfer.ClubMemberId)
                                        bookingCount++;
                                    if (bookingPalyerDetails[0].Player4 == golfer.ClubMemberId)
                                        bookingCount++;
                                }
                            }
                        }
                        var golferBookings = _unitOfWork.BookingRepository.GetMany(x => x.TeeOffDate == teeOffDate && x.Golfer.ClubMemberId == playerDetail.PlayerDetails && x.IsActive == true && x.BookingTypeId == bookingTypeId && (x.BookingStatusId == (int)EnumBookingStatus.Confirm || (x.BookingStatusId == (int)EnumBookingStatus.Pending && x.CreatedOn.AddMinutes(15) > DateTime.UtcNow))).ToList();
                        //Max Booking Allowed Per User
                        int bookingLeft = 2 - (golferBookings.Count() + bookingCount);
                        if (bookingLeft <= 0)
                            throw new Exception("Max Two Booking Per Day.Please Check Golfer 3");
                    }
                    else if (playerDetail.PlayerSerialNumber.Trim() == "Player4")
                    {
                        int bookingCount = 0;
                        var golfer = _unitOfWork.GolferRepository.Get(x => x.ClubMemberId == playerDetail.PlayerDetails);
                        if (golfer != null)
                        {
                            var golferBookings1 = _unitOfWork.BookingRepository.GetMany(x => x.TeeOffDate == teeOffDate && x.IsActive == true && x.BookingTypeId == bookingTypeId && (x.BookingStatusId == (int)EnumBookingStatus.Confirm || (x.BookingStatusId == (int)EnumBookingStatus.Pending && x.CreatedOn.AddMinutes(15) > DateTime.UtcNow))).ToList();
                            foreach (var golf in golferBookings1)
                            {
                                var bookingPalyerDetails = golf.BookingPlayerDetails.ToList();
                                if (bookingPalyerDetails != null && bookingPalyerDetails.Count != 0)
                                {
                                    if (bookingPalyerDetails[0].Player2 == golfer.ClubMemberId)
                                        bookingCount++;
                                    if (bookingPalyerDetails[0].Player3 == golfer.ClubMemberId)
                                        bookingCount++;
                                    if (bookingPalyerDetails[0].Player4 == golfer.ClubMemberId)
                                        bookingCount++;
                                }
                            }
                        }
                        var golferBookings = _unitOfWork.BookingRepository.GetMany(x => x.TeeOffDate == teeOffDate && x.Golfer.ClubMemberId == playerDetail.PlayerDetails && x.IsActive == true && x.BookingTypeId == bookingTypeId && (x.BookingStatusId == (int)EnumBookingStatus.Confirm || (x.BookingStatusId == (int)EnumBookingStatus.Pending && x.CreatedOn.AddMinutes(15) > DateTime.UtcNow))).ToList();
                        //Max Booking Allowed Per User
                        int bookingLeft = 2 - (golferBookings.Count() + bookingCount);
                        if (bookingLeft <= 0)
                            throw new Exception("Max Two Booking Per Day.Please Check Golfer 4");
                    }


                }

            }
        }



        public decimal PromotionPrice(long id)
        {
            Promotion promotion = _unitOfWork.PromotionRepository.Get(x => x.PromotionsId == id && x.IsActive == true);

            return promotion.Price.GetValueOrDefault();
        }

        public PaymentGatewayControlViewModel PaymentGatewayControl(long id)
        {
            PaymentGatewayControlViewModel paymentGateway = new PaymentGatewayControlViewModel();
            List<PaymentGatewayControl> paymentGatewayControl = _unitOfWork.PaymentGatewayControlRepository.GetMany(x => x.IsActive == true).OrderByDescending(x => x.CreatedOn).ToList();
            foreach (var item in paymentGatewayControl)
            {
                if (item.AllMembers == true)
                {
                    paymentGateway.PaymentGatewayEnable = item.PaymentGatewayEnable;
                    paymentGateway.GreenFee = item.GreenFee;
                    paymentGateway.CaddieFee = item.CaddieFee;
                    paymentGateway.CartFee = item.CartFee;
                    paymentGateway.EquipmentPriceOffIds = item.EquipmentOffIds != null ? item.EquipmentOffIds.Split(',').ToList() : new List<string>();
                    paymentGateway.PaymentGatewayControlId = item.PaymentGatewayControlId;
                    break;
                }
                else
                {
                    string[] ids = item.SelectedGolferIds.Split(',');
                    if (ids.Contains(id.ToString()))
                    {
                        paymentGateway.PaymentGatewayEnable = item.PaymentGatewayEnable;
                        paymentGateway.GreenFee = item.GreenFee;
                        paymentGateway.CaddieFee = item.CaddieFee;
                        paymentGateway.CartFee = item.CartFee;
                        paymentGateway.EquipmentPriceOffIds = item.EquipmentOffIds != null ? item.EquipmentOffIds.Split(',').ToList() : new List<string>();
                        paymentGateway.PaymentGatewayControlId = item.PaymentGatewayControlId;
                        break;
                    }
                }
            }


            return paymentGateway;

        }

        private Promotion PromotionData(long id)
        {
            Promotion promotion = _unitOfWork.PromotionRepository.Get(x => x.PromotionsId == id && x.IsActive == true);

            return promotion;
        }

        public BookingConditionViewModel BookingSetting(long bookingTypeId)
        {
            BookingConditionViewModel bookingConditionViewModel = new BookingConditionViewModel()
            {
                DayOffIds = Core.Helper.Constants.BookingDetails.DayOffIds.Split(',').Select(Int64.Parse).ToArray(),
                NoOfAdvanceDay = Core.Helper.Constants.BookingDetails.AdvanceBookingDays,
            };
            return bookingConditionViewModel;
        }


        private bool VaildateAmount(CommonViewModel commonViewModel)
        {
            Golfer golfer = _unitOfWork.GolferRepository.Get(x => x.GolferId == commonViewModel.GolferId);
            if (commonViewModel.BookingTypeId == (int)Core.Helper.Enum.EnumBookingType.BTT)
            {
                BookingPricingViewModel bookingPricingViewModel = GetPricingCalculation(commonViewModel.HoleTypeId, commonViewModel.TeeOffDate, commonViewModel.SlotId, commonViewModel.NoOfPlayer, 1, 1, commonViewModel.MemberTypeViewModels, commonViewModel.GolferId, commonViewModel.CoursePairingId, commonViewModel.BookingPlayerDetailViewModels, commonViewModel.PromotionsId);

                if (commonViewModel.PromotionsId == 0)
                {
                    decimal amount = 0;
                    decimal paidAmount = 0;
                    decimal greenFee = 0;
                    decimal cartFee = 0;
                    decimal caddieFee = 0;
                    decimal equPrice = 0;

                    foreach (var item in bookingPricingViewModel.MemberTypeViewModels)
                    {
                        if (item.PlayerCount != 0)
                        {
                            greenFee = greenFee + (item.PlayerCount * item.GreenFee);
                            if (bookingPricingViewModel.PaymentGatewayControlViewModel.GreenFee)
                            {
                                if (golfer.MemberTypeId == item.MemberTypeId)
                                {
                                    if (item.PlayerCount != 0)
                                    {
                                        paidAmount = paidAmount + ((item.PlayerCount - 1) * item.GreenFee);
                                    }
                                }
                                else
                                {
                                    paidAmount = paidAmount + ((item.PlayerCount) * item.GreenFee);
                                }
                            }
                            else
                            {
                                paidAmount = paidAmount + ((item.PlayerCount) * item.GreenFee);
                            }
                        }
                    }


                    if (commonViewModel.HoleTypeId == (int)Core.Helper.Enum.EnumHoleType.Hole9)
                    {
                        caddieFee = bookingPricingViewModel.Caddie9HolePrice;
                        cartFee = bookingPricingViewModel.Cart9HolePrice;
                    }
                    else if (commonViewModel.HoleTypeId == (int)Core.Helper.Enum.EnumHoleType.Hole18)
                    {
                        caddieFee = bookingPricingViewModel.Caddie18HolePrice;
                        cartFee = bookingPricingViewModel.Cart18HolePrice;
                    }
                    else
                    {
                        throw new Exception("Invalid Request");
                    }

                    cartFee = cartFee * commonViewModel.CartCount;
                    caddieFee = caddieFee * commonViewModel.CaddieCount;
                    if (!bookingPricingViewModel.PaymentGatewayControlViewModel.CartFee) { paidAmount += cartFee; }
                    if (!bookingPricingViewModel.PaymentGatewayControlViewModel.CaddieFee) { paidAmount += caddieFee; }

                    if (commonViewModel.BookingEquipmentMappingViewModels != null)
                    {
                        foreach (var item in commonViewModel.BookingEquipmentMappingViewModels)
                        {
                            if (bookingPricingViewModel.BookingEquipmentMappingViewModels == null) { throw new Exception("Invalid Request"); }
                            var equDetail = bookingPricingViewModel.BookingEquipmentMappingViewModels.Where(x => x.EquipmentId == item.EquipmentId).First();
                            if (equDetail != null)
                            {
                                if (item.EquipmentPrice != equDetail.EquipmentPrice) { throw new Exception("Invalid Request"); }
                                else
                                {
                                    var equPrice1 = (item.EquipmentCount * item.EquipmentPrice).GetValueOrDefault();
                                    if (bookingPricingViewModel.PaymentGatewayControlViewModel.EquipmentPriceOffIds.Contains(item.EquipmentId.ToString()))
                                    {

                                    }
                                    else
                                    {
                                        paidAmount += equPrice1;
                                    }
                                    equPrice += equPrice1;
                                }
                            }
                            else
                            {
                                throw new Exception("Invalid Request");
                            }
                        }
                    }
                    amount = greenFee + cartFee + caddieFee + equPrice;
                    if (commonViewModel.Discount != 0)
                    {
                        if (commonViewModel.CouponCode == "" || commonViewModel.CouponCode == null) { throw new Exception("Invalid Request"); }
                        CouponViewModel couponViewModel = GetCouponCouponCode(commonViewModel.CouponCode);
                        if (couponViewModel == null) { throw new Exception("Invalid Request"); }
                        if (couponViewModel.IsFlat)
                        {
                            if (commonViewModel.Discount != couponViewModel.Value)
                            {
                                throw new Exception("Invalid Request");
                            }
                            else
                            {
                                paidAmount -= couponViewModel.Value;
                            }
                        }
                        else
                        {
                            var dA = (paidAmount * couponViewModel.Value) / 100;
                            if (commonViewModel.Discount != dA)
                            {
                                throw new Exception("Invalid Request");
                            }
                            else
                            {
                                paidAmount -= dA;
                            }
                        }
                    }

                    decimal percentage = 0;
                    if (commonViewModel.CourseTaxMappingViewModels != null)
                    {

                        foreach (var tax in commonViewModel.CourseTaxMappingViewModels)
                        {
                            percentage = percentage + tax.Percentage;

                        }
                        if (percentage != 0)
                            paidAmount = Math.Round(paidAmount + (paidAmount * (percentage / 100)), 0);
                    }
                    if (commonViewModel.PaidAmount != Math.Round(paidAmount, 0)) { throw new Exception("Invalid Request"); }
                    if (commonViewModel.Amount != Math.Round(amount, 0)) { throw new Exception("Invalid Request"); }
                    //Greenfee  // add of all green fee
                    //CartFee // mulplr with count
                    //Caddie Fee //multpe with count
                    // equipment fee and count
                    // Discount // Coupon detail with coupon name
                    //add tax // course gps fee

                    // check amount to pay or not
                    return bookingPricingViewModel.PaymentGatewayControlViewModel.PaymentGatewayEnable;
                }
                else
                {
                    var amount = bookingPricingViewModel.PromotionPrice * commonViewModel.NoOfPlayer;
                    decimal percentage = 0;
                    if (commonViewModel.CourseTaxMappingViewModels != null)
                    {

                        foreach (var tax in commonViewModel.CourseTaxMappingViewModels)
                        {
                            percentage = percentage + tax.Percentage;

                        }
                        if (percentage != 0)
                            amount = Math.Round(amount + (amount * (percentage / 100)), 0);
                    }

                    if (amount != commonViewModel.PaidAmount)
                    {
                        throw new Exception("Invalid Request");
                    }
                    //    bookingPricingViewModel.PromotionPrice // Mulitple with no of player
                    //Tax 
                    //commonViewModel.CourseTaxMappingViewModels //add taxes
                    // commonViewModel.PaidAmount//tax ke sath
                    // commonViewModel.Amount //tax ke sth
                }
                return bookingPricingViewModel.PaymentGatewayControlViewModel.PaymentGatewayEnable;
            }
            else
            {


            }

            return true;
        }




        private CouponViewModel GetCouponCouponCode(string code)
        {
            CouponViewModel couponViewModel = new CouponViewModel();
            Coupon coupon = _unitOfWork.CouponRepository.Get(x => x.Code == code && x.IsActive == true);

            if (coupon == null) { throw new Exception("Invalid CouponCode."); }
            else
            {
                couponViewModel.Value = coupon.Amount;
                if (coupon.CouponTypeId == 1)
                {
                    couponViewModel.IsFlat = true;
                }
                else
                {
                    couponViewModel.IsFlat = false;
                }
                return couponViewModel;
            }
        }


        private void checkPayment(string bookingId, decimal taxAmount)
        {

            String transactionURL = "https://securegw.paytm.in/order/status";
            String merchantKey = "sVI@svK6LujZ!swv";
            String merchantMid = "Golfla03291372924605";
            String orderId = bookingId;
            Dictionary<String, String> paytmParams = new Dictionary<String, String>();
            paytmParams.Add("MID", merchantMid);
            paytmParams.Add("ORDERID", orderId);
            try
            {
                string paytmChecksum = paytm.CheckSum.generateCheckSum(merchantKey, paytmParams);
                paytmParams.Add("CHECKSUMHASH", paytmChecksum);
                String postData = "JsonData=" + new JavaScriptSerializer().Serialize(paytmParams);
                HttpWebRequest connection = (HttpWebRequest)WebRequest.Create(transactionURL);
                connection.Headers.Add("ContentType", "application/json");
                connection.Method = "POST";
                using (StreamWriter requestWriter = new StreamWriter(connection.GetRequestStream()))
                {
                    requestWriter.Write(postData);
                }
                string responseData = string.Empty;
                using (StreamReader responseReader = new StreamReader(connection.GetResponse().GetResponseStream()))
                {

                    responseData = responseReader.ReadToEnd();

                    var model = JsonConvert.DeserializeObject<dynamic>(responseData);
                    if (model.RESPCODE == "01")
                    {
                        if (model.TXNAMOUNT == taxAmount)
                        { }
                        else
                        {
                            throw new Exception("Invalid Request -A");
                        }
                    }
                    else
                    {
                        throw new Exception("Invalid Request - Not Found");
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception("Invalid Request- d");
                // Response.Write("Exception message: " + ex.Message.ToString());
            }

        }



        public bool CheckMemberShipId(CommonViewModel golferViewModel)
        {

            Golfer golferCheck = _unitOfWork.GolferRepository.Get(x => x.ClubMemberId == golferViewModel.MemberShipId && x.IsActive == true);
            if (golferCheck != null)
            {
                return true;
            }
            return false;
        }


        public void CheckEquipmentVaildation(CommonViewModel commonViewModel)
        {
            List<Booking> bookings = new List<Booking>();
            bookings = _unitOfWork.BookingRepository.GetMany(x => x.TeeOffDate == commonViewModel.TeeOffDate && x.IsActive == true && x.BookingTypeId == (int)EnumBookingType.BTT && (x.BookingStatusId == (int)EnumBookingStatus.Confirm || (x.BookingStatusId == (int)EnumBookingStatus.Pending && x.CreatedOn.AddMinutes(15) > DateTime.UtcNow))).ToList();

            List<BookingEquipmentMappingViewModel> bookingEquipmentMappingViewModels = new List<BookingEquipmentMappingViewModel>();
            List<Equipment> equipments = _unitOfWork.EquipmentRepository.GetMany(x => x.IsActive == true).ToList();
            foreach (var equipment in equipments)
            {
                var count = 0;

                foreach (var booking in bookings)
                {
                    foreach (var item in booking.BookingEquipmentMappings)
                    {
                        if (item.EquipmentId == equipment.EquipmentId)
                        {
                            count = count + item.EquipmentCount;
                        }
                    };
                }
                //if ((4 - count) <= 0)
                //{
                //    throw new Exception(equipment.Name + " can not be selected");
                //}
                if (commonViewModel.BookingEquipmentMappingViewModels != null)
                {
                    BookingEquipmentMappingViewModel bookingEquipmentMappingView = commonViewModel.BookingEquipmentMappingViewModels.Where(x => x.EquipmentId == equipment.EquipmentId).FirstOrDefault();
                    if (bookingEquipmentMappingView != null)
                    {
                        if (bookingEquipmentMappingView.EquipmentCount > 0)
                        {

                            if (bookingEquipmentMappingView.EquipmentCount > (4 - count))
                            {
                                throw new Exception("Only " + (4 - count) + " " + equipment.Name + " is  left");

                            }
                        }
                    }
                }

            }

        }
    }
}