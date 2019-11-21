
using GolfCentra.Business.BusinessAdmin.Interface;
using GolfCentra.Core;
using GolfCentra.Core.DataBase;
using GolfCentra.Core.Helper;

using GolfCentra.ViewModel.Admin;
using GolfCentra.ViewModel.Admin.LoginActivity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GolfCentra.Core.Helper.Enum;

namespace GolfCentra.Business.BusinessAdmin.Implementation
{
    public class BookingService : IBookingService
    {
        private readonly UnitOfWork _unitOfWork;

        public BookingService()
        {
            _unitOfWork = new UnitOfWork();
        }

        /// <summary>
        /// Get Booking Deatils By No Of Records
        /// </summary>
        /// <param name="noOfRecord"></param>
        /// <returns></returns>
        public List<BookingViewModel> GetBookingDetailsByTake(int noOfRecord)
        {
            List<Booking> bookings = _unitOfWork.BookingRepository.GetMany(x => x.IsActive == true).OrderByDescending(x => x.CreatedOn).Take(noOfRecord).ToList();
            List<BookingViewModel> bookingViewModels = new List<BookingViewModel>();

            foreach (var booking in bookings)
            {
                CoursePairing coursePairing = _unitOfWork.CoursePairingRepository.Get(x => x.CoursePairingId == booking.CoursePairingId);

                int hole = 0;
                if (booking.BookingTypeId == (int)Core.Helper.Enum.EnumBookingType.BTT)
                    hole = booking.HoleType.Value;
                BookingViewModel bookingViewModel = new BookingViewModel()
                {
                    BookingDate = Core.Helper.DateHelper.ConvertSystemDateToCurrent(booking.BookingDate),
                    TeeOffDate = booking.TeeOffDate.ToShortDateString(),
                    BookingId = booking.BookingId,
                    UserName = booking.Golfer.Name,
                    BookingStatus = booking.BookingStatu.Value,
                    NoOfHole = hole,
                    Time = booking.TeeOffSlot,
                    BallFee = booking.BallFee.GetValueOrDefault(),
                    CaddieFee = booking.CaddieFee.GetValueOrDefault(),
                    CartFee = booking.CartFee.GetValueOrDefault(),
                    TotalAmount = booking.Amount.GetValueOrDefault(),
                    Discount = booking.Discount.GetValueOrDefault(),
                    GreenFee = booking.GreenFee.GetValueOrDefault(),
                    OnSpot = booking.OnSpot.GetValueOrDefault(),
                    NoOfBalls = booking.NoOfBalls.GetValueOrDefault(),
                    NoOfCaddie = booking.CaddieCount.GetValueOrDefault(),
                    NoOfCart = booking.CartCount.GetValueOrDefault(),
                    NoOfMemberPlayer = booking.NoOfMember.GetValueOrDefault(),
                    NoOfNonMemberPlayer = booking.NoOfNonMember.GetValueOrDefault(),
                    NoOfPlayer = booking.NoOfPlayer,
                    PaymentMode = booking.PaymentMode,
                    RangeFee = booking.RangeFee.GetValueOrDefault(),
                    CurrencyName = Core.Helper.Constants.Currency.CurrencyName,
                    CoursePairingId = booking.CoursePairingId.GetValueOrDefault(),
                    CoursePairingName = coursePairing != null ? (coursePairing.CourseName1.Value) + (coursePairing.EndCourseNameId != null ? " - " + coursePairing.CourseName.Value : "") : ""

                };
                if (booking.PromotionId != null)
                {
                    bookingViewModel.PromotionViewModel = GetPromotionById(booking.PromotionId.GetValueOrDefault());

                }

                bookingViewModels.Add(bookingViewModel);
            }
            return bookingViewModels;
        }

        /// <summary>
        /// Get Booking Details By Booking Id
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public BookingViewModel GetBookingDetailsByBookingId(long bookingId)
        {
            BookingViewModel bookingViewModel = new BookingViewModel();
            Booking booking = _unitOfWork.BookingRepository.Get(x => x.BookingId == bookingId && x.IsActive == true);
            if (booking != null)
            {
                bookingViewModel.UserName = booking.Golfer.Name;
                bookingViewModel.BookingId = booking.BookingId;
                bookingViewModel.Time = booking.TeeOffSlot;
                bookingViewModel.TeeOffDate = booking.TeeOffDate.ToShortDateString();
                bookingViewModel.NoOfPlayer = booking.NoOfPlayer;
                bookingViewModel.TotalAmount = booking.Amount.GetValueOrDefault();
                bookingViewModel.CurrencyName = Core.Helper.Constants.Currency.CurrencyName;
                bookingViewModel.BookingTypeId = booking.BookingTypeId;
                if (booking.BookingTypeId == (int)Core.Helper.Enum.EnumBookingType.BTT)
                    bookingViewModel.NoOfHole = booking.HoleType.Value;
                bookingViewModel.NoOfBalls = booking.NoOfBalls.GetValueOrDefault();
                bookingViewModel.BookingStatus = booking.BookingStatu.Value;
                bookingViewModel.BookingDate = Core.Helper.DateHelper.ConvertSystemDateToCurrent(booking.BookingDate);
                bookingViewModel.Time = booking.TeeOffSlot;
                bookingViewModel.BallFee = booking.BallFee.GetValueOrDefault();
                bookingViewModel.CaddieFee = booking.CaddieFee.GetValueOrDefault();
                bookingViewModel.CartFee = booking.CartFee.GetValueOrDefault();
                bookingViewModel.TotalAmount = booking.Amount.GetValueOrDefault();
                bookingViewModel.Discount = booking.Discount.GetValueOrDefault();
                bookingViewModel.GreenFee = booking.GreenFee.GetValueOrDefault();
                bookingViewModel.OnSpot = booking.OnSpot.GetValueOrDefault();
                bookingViewModel.NoOfBalls = booking.NoOfBalls.GetValueOrDefault();
                bookingViewModel.NoOfCaddie = booking.CaddieCount.GetValueOrDefault();
                bookingViewModel.NoOfCart = booking.CartCount.GetValueOrDefault();
                bookingViewModel.NoOfMemberPlayer = booking.NoOfMember.GetValueOrDefault();
                bookingViewModel.NoOfNonMemberPlayer = booking.NoOfNonMember.GetValueOrDefault();
                bookingViewModel.NoOfPlayer = booking.NoOfPlayer;
                bookingViewModel.PaymentMode = booking.PaymentMode;
                bookingViewModel.RangeFee = booking.RangeFee.GetValueOrDefault();
                bookingViewModel.MemberTypeViewModels = new List<MemberTypeViewModel>();
                bookingViewModel.BookingEquipmentMappingViewModels = new List<BookingEquipmentMappingViewModel>();
                bookingViewModel.CoursePairingId = booking.CoursePairingId.GetValueOrDefault();
                bookingViewModel.CoursePairingName = booking.CoursePairingId != null ? (booking.CoursePairing.CourseName1.Value) + (booking.CoursePairing.EndCourseNameId != null ? " - " + booking.CoursePairing.CourseName.Value : "") : "";
                bookingViewModel.PaidAmount = booking.PaidAmount.GetValueOrDefault();
                foreach (var item in booking.BookingPlayerMappings)
                {
                    MemberTypeViewModel bookingPlayerMapping = new MemberTypeViewModel()
                    {
                        Name = item.MemberType.Name,
                        PlayerCount = item.PlayerCount
                    };
                    bookingViewModel.MemberTypeViewModels.Add(bookingPlayerMapping);

                };

                foreach (var item in booking.BookingEquipmentMappings)
                {
                    BookingEquipmentMappingViewModel bookingEquipmentMappingViewModel = new BookingEquipmentMappingViewModel()
                    {
                        EquipmentName = item.EquipmentName,
                        EquipmentCount = item.EquipmentCount,
                        EquipmentPrice = item.EquipmentPrice
                    };
                    bookingViewModel.BookingEquipmentMappingViewModels.Add(bookingEquipmentMappingViewModel);

                };


                foreach (var item in booking.BookingPlayerDetails)
                {
                    BookingPlayerDetailViewModel bookingPlayerDetailViewModel = new BookingPlayerDetailViewModel()
                    {

                        // bookingPlayerDetail.Player1 = playerDetail.PlayerDetails;
                        Player1 = item.Player1,
                        Player2 = item.Player2,
                        Player3 = item.Player3,
                        Player4 = item.Player4


                    };
                    bookingViewModel.BookingPlayerDetailViewModel = bookingPlayerDetailViewModel;

                };

                if (booking.PromotionId != null)
                {
                    bookingViewModel.PromotionViewModel = GetPromotionById(booking.PromotionId.GetValueOrDefault());

                }

            }
            return bookingViewModel;
        }

        /// <summary>
        /// Get Data For Top Bar Of Dashboard
        /// </summary>
        /// <returns></returns>
        public DashBoardTopBarViewModel GetDataForTopBar()
        {
            List<Booking> bookings = _unitOfWork.BookingRepository.GetMany(x => x.IsActive == true && x.BookingStatusId == (int)Core.Helper.Enum.EnumBookingStatus.Confirm).ToList();
            DashBoardTopBarViewModel dashBoardTopBarViewModel = new DashBoardTopBarViewModel()
            {
                TotalAmount = bookings.Sum(x => x.PaidAmount).GetValueOrDefault(),
                TotalBooking = bookings.Count(x => x.BookingTypeId == (int)Core.Helper.Enum.EnumBookingType.BTT),
                Hole9Booking = bookings.Count(x => x.BookingTypeId == (int)Core.Helper.Enum.EnumBookingType.BTT && x.HoleTypeId == (int)Core.Helper.Enum.EnumHoleType.Hole9),
                Hole18Booking = bookings.Count(x => x.BookingTypeId == (int)Core.Helper.Enum.EnumBookingType.BTT && x.HoleTypeId == (int)Core.Helper.Enum.EnumHoleType.Hole18),
                Hole27Booking = bookings.Count(x => x.BookingTypeId == (int)Core.Helper.Enum.EnumBookingType.BTT && x.HoleTypeId == (int)Core.Helper.Enum.EnumHoleType.Hole27),

            };
            return dashBoardTopBarViewModel;
        }

        /// <summary>
        /// Get All Booking Details
        /// </summary>
        /// <returns></returns>
        public List<BookingViewModel> GetBookingDetails()
        {
            List<Booking> bookings = _unitOfWork.BookingRepository.GetMany(x => x.IsActive == true).OrderBy(x => x.CreatedOn).ToList();
            List<BookingViewModel> bookingViewModels = new List<BookingViewModel>();

            foreach (var booking in bookings)
            {
                int hole = 0;
                if (booking.BookingTypeId == (int)Core.Helper.Enum.EnumBookingType.BTT)
                    hole = booking.HoleType.Value;
                BookingViewModel bookingViewModel = new BookingViewModel()
                {
                    BookingDate = booking.BookingDate,
                    TeeOffDate = booking.TeeOffDate.ToShortDateString(),
                    BookingId = booking.BookingId,
                    UserName = booking.Golfer.Name,
                    BookingStatus = booking.BookingStatu.Value,
                    NoOfHole = hole,
                    Time = booking.TeeOffSlot,
                    BallFee = booking.BallFee.GetValueOrDefault(),
                    CaddieFee = booking.CaddieFee.GetValueOrDefault(),
                    CartFee = booking.CartFee.GetValueOrDefault(),
                    TotalAmount = booking.PaidAmount.GetValueOrDefault(),
                    Discount = booking.Discount.GetValueOrDefault(),
                    GreenFee = booking.GreenFee.GetValueOrDefault(),
                    OnSpot = booking.OnSpot.GetValueOrDefault(),
                    NoOfBalls = booking.NoOfBalls.GetValueOrDefault(),
                    NoOfCaddie = booking.CaddieCount.GetValueOrDefault(),
                    NoOfCart = booking.CartCount.GetValueOrDefault(),
                    NoOfMemberPlayer = booking.NoOfMember.GetValueOrDefault(),
                    NoOfNonMemberPlayer = booking.NoOfNonMember.GetValueOrDefault(),
                    NoOfPlayer = booking.NoOfPlayer,
                    PaymentMode = booking.PaymentMode,
                    RangeFee = booking.RangeFee.GetValueOrDefault(),
                    CurrencyName = Core.Helper.Constants.Currency.CurrencyName,
                    BookingStatusId = booking.BookingStatusId,
                    PlayingDate = booking.TeeOffDate

                };
                bookingViewModels.Add(bookingViewModel);
            }
            return bookingViewModels;
        }

        /// <summary>
        /// Get Booking By Advance Serach
        /// </summary>
        /// <param name="bookingModel"></param>
        /// <returns></returns>
        public List<BookingViewModel> GetBookingDetailsBySearch(BookingViewModel bookingModel)
        {
            List<Booking> bookings = new List<Booking>();
            DateTime startDate = new DateTime(bookingModel.StartDate.Year, bookingModel.StartDate.Month, bookingModel.StartDate.Day, 0, 0, 0);
            DateTime endDate = new DateTime(bookingModel.EndDate.Year, bookingModel.EndDate.Month, bookingModel.EndDate.Day, 23, 59, 59);

            if (bookingModel.SearchTypeId == (int)Core.Helper.Enum.EnumAdminReportSearch.All)
            {
                bookings = _unitOfWork.BookingRepository.GetMany(x => x.IsActive == true && x.CreatedOn >= startDate && x.CreatedOn <= endDate).OrderBy(x => x.CreatedOn).ToList();
            }
            else if (bookingModel.SearchTypeId == (int)Core.Helper.Enum.EnumAdminReportSearch.FromApp)
            {
                bookings = _unitOfWork.BookingRepository.GetMany(x => x.IsActive == true && x.OnSpot == false && x.CreatedOn >= startDate && x.CreatedOn <= endDate).OrderBy(x => x.CreatedOn).ToList();

            }
            else if (bookingModel.SearchTypeId == (int)Core.Helper.Enum.EnumAdminReportSearch.OnSpot)
            {
                bookings = _unitOfWork.BookingRepository.GetMany(x => x.IsActive == true && x.OnSpot == true && x.CreatedOn >= startDate && x.CreatedOn <= endDate).OrderBy(x => x.CreatedOn).ToList();

            }


            List<BookingViewModel> bookingViewModels = new List<BookingViewModel>();

            foreach (var booking in bookings.OrderByDescending(x => x.TeeOffDate).ToList())
            {
                int hole = 0;
                if (booking.BookingTypeId == (int)Core.Helper.Enum.EnumBookingType.BTT)
                    hole = booking.HoleType.Value;
                BookingViewModel bookingViewModel = new BookingViewModel()
                {
                    BookingDate = booking.BookingDate,
                    TeeOffDate = booking.TeeOffDate.ToShortDateString(),
                    BookingId = booking.BookingId,
                    UserName = booking.Golfer.Name,
                    BookingStatus = booking.BookingStatu.Value,
                    NoOfHole = hole,
                    Time = booking.TeeOffSlot,
                    BallFee = booking.BallFee.GetValueOrDefault(),
                    CaddieFee = booking.CaddieFee.GetValueOrDefault(),
                    CartFee = booking.CartFee.GetValueOrDefault(),
                    TotalAmount = booking.PaidAmount.GetValueOrDefault(),
                    Discount = booking.Discount.GetValueOrDefault(),
                    GreenFee = booking.GreenFee.GetValueOrDefault(),
                    OnSpot = booking.OnSpot.GetValueOrDefault(),
                    NoOfBalls = booking.NoOfBalls.GetValueOrDefault(),
                    NoOfCaddie = booking.CaddieCount.GetValueOrDefault(),
                    NoOfCart = booking.CartCount.GetValueOrDefault(),
                    NoOfMemberPlayer = booking.NoOfMember.GetValueOrDefault(),
                    NoOfNonMemberPlayer = booking.NoOfNonMember.GetValueOrDefault(),
                    NoOfPlayer = booking.NoOfPlayer,
                    PaymentMode = booking.PaymentMode,
                    RangeFee = booking.RangeFee.GetValueOrDefault(),
                    CurrencyName = Core.Helper.Constants.Currency.CurrencyName,

                };
                bookingViewModels.Add(bookingViewModel);
            }
            return bookingViewModels;
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
                        pricings = _unitOfWork.PricingRepository.GetMany(x => x.StartDate <= startDate && x.EndDate >= endDate && x.DayTypeId == dayTypeId && x.BookingTypeId == bookingTypeId && x.MemberTypeId == memberTypeId && x.IsActive == true && x.SlotId == null && x.TimeFormatId == timeFormatId).ToList();
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
                        pricings = _unitOfWork.PricingRepository.GetMany(x => x.StartDate <= startDate && x.EndDate >= endDate && x.DayTypeId == dayTypeId && x.BookingTypeId == bookingTypeId && x.MemberTypeId == memberTypeId && x.IsActive == true && x.SlotId == null && x.TimeFormatId == timeFormatId).ToList();
                    }
                }


                Pricing pricings1 = pricings.OrderByDescending(x => x.CreatedOn).ToList().FirstOrDefault();
                return pricings1;

            }
            return new Pricing();
        }


        /// <summary>
        /// Get Pricing  Details For BDR
        /// </summary>
        /// <param name="holeTypeId"></param>
        /// <param name="date"></param>
        /// <param name="slotId"></param>
        /// <param name="TotalNoOfPlayer"></param>
        /// <param name="noOfMemberPlayer"></param>
        /// <param name="noOfNonMemberPlayer"></param>
        /// <returns></returns>
        private BookingPricingViewModel GetPricingCalculation(long holeTypeId, DateTime date, long slotId, int TotalNoOfPlayer, int noOfMemberPlayer, int noOfNonMemberPlayer, List<MemberTypeViewModel> memberTypeViewModels, long memberTypeId)
        {

            long timeFormatId = 0;
            var time = _unitOfWork.SlotRepository.Get(x => x.SlotId == slotId && x.IsActive == true);
            if (time.Time >= TimeSpan.Parse("12:00")) { timeFormatId = (int)Core.Helper.Enum.EnumTimeFormatType.PM; }
            else { timeFormatId = (int)Core.Helper.Enum.EnumTimeFormatType.AM; }
            var holename = _unitOfWork.HoleTypeRepository.Get(x => x.HoleTypeId == holeTypeId && x.IsActive == true);

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
                CurrencyName = Core.Helper.Constants.Currency.CurrencyName

            };


            decimal total = 0;
            long dayTypeId = GetDayTypeId(date);

            Pricing login9MemberPricing = GetPricingDetails(date, slotId, (int)EnumHoleType.Hole9, (int)EnumBookingType.BTT, memberTypeId, dayTypeId, timeFormatId);
            Pricing login18MemberPricing = GetPricingDetails(date, slotId, (int)EnumHoleType.Hole18, (int)EnumBookingType.BTT, memberTypeId, dayTypeId, timeFormatId);
            Pricing login27MemberPricing = GetPricingDetails(date, slotId, (int)EnumHoleType.Hole27, (int)EnumBookingType.BTT, memberTypeId, dayTypeId, timeFormatId);

            foreach (var memberType in memberTypeViewModels)
            {
                MemberTypeViewModel memberTypeViewModel = new MemberTypeViewModel();
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
                        bookingPricingViewModel.GreenFee += member9HolePricing != null ? member9HolePricing.GreenFee.GetValueOrDefault() * memberType.PlayerCount : 0;
                        if (member9HolePricing != null)
                        {
                            decimal totalPrec = 0;

                            foreach (var tax in member9HolePricing.PriceTaxMappings.Where(x => x.FeeCategoryId == (int)Core.Helper.Enum.EnumFeeCategory.GreenFee && x.IsActive == true))
                            {
                                totalPrec += tax.Tax.Percentage;

                            }
                            bookingPricingViewModel.GreenFee = Math.Round(bookingPricingViewModel.GreenFee + (bookingPricingViewModel.GreenFee * (totalPrec / 100)), 2);


                        }
                    }
                    if (holeTypeId == (int)EnumHoleType.Hole18)
                    {
                        bookingPricingViewModel.GreenFee += member18HolePricing != null ? member18HolePricing.GreenFee.GetValueOrDefault() * memberType.PlayerCount : 0;
                        if (member18HolePricing != null)
                        {

                            decimal totalPrec = 0;
                            foreach (var tax in member18HolePricing.PriceTaxMappings.Where(x => x.FeeCategoryId == (int)Core.Helper.Enum.EnumFeeCategory.GreenFee && x.IsActive == true))
                            {
                                totalPrec += tax.Tax.Percentage;
                            }
                            bookingPricingViewModel.GreenFee = Math.Round(bookingPricingViewModel.GreenFee + (bookingPricingViewModel.GreenFee * (totalPrec / 100)), 2);


                        }
                    }
                    if (holeTypeId == (int)EnumHoleType.Hole27)
                    {

                        bookingPricingViewModel.GreenFee += member27HolePricing != null ? member27HolePricing.GreenFee.GetValueOrDefault() * memberType.PlayerCount : 0;
                        if (member27HolePricing != null)
                        {
                            decimal totalPrec = 0;
                            foreach (var tax in member27HolePricing.PriceTaxMappings.Where(x => x.FeeCategoryId == (int)Core.Helper.Enum.EnumFeeCategory.GreenFee && x.IsActive == true))
                            {
                                totalPrec += tax.Tax.Percentage;
                            }
                            bookingPricingViewModel.GreenFee = Math.Round(bookingPricingViewModel.GreenFee + (bookingPricingViewModel.GreenFee * (totalPrec / 100)), 2);


                        }
                    }


                    //memberTypeViewModel.MemberTypeId = memberType.MemberTypeId;
                    //memberTypeViewModel.Name = memberType.Name;
                    //memberTypeViewModel.PlayerCount = memberType.PlayerCount;
                    //bookingPricingViewModel.MemberTypeViewModels.Add(memberTypeViewModel);
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
                bookingPricingViewModel.Cart27HolePrice = bookingPricingViewModel.Cart27HolePrice + (bookingPricingViewModel.Cart27HolePrice * (totalPrec / 100));

                foreach (var tax in login27MemberPricing.PriceTaxMappings.Where(x => x.FeeCategoryId == (int)Core.Helper.Enum.EnumFeeCategory.AddOnCaddie))
                {
                    totalPrec1 += tax.Tax.Percentage;
                }
                bookingPricingViewModel.Caddie27HolePrice = Math.Round(bookingPricingViewModel.Caddie27HolePrice + (bookingPricingViewModel.Caddie27HolePrice * (totalPrec1 / 100)), 2);

            }
            bookingPricingViewModel.CourseTaxMappingViewModels = GetAllCourseTaxs();
            bookingPricingViewModel.BookingEquipmentMappingViewModels = GetAllEquipment(date);
            return bookingPricingViewModel;
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
        private BookingPricingViewModel GetPricingCalculationBDT(long bucketId, DateTime date, long slotId, int TotalNoOfPlayer, int noOfMemberPlayer, int noOfNonMemberPlayer, List<MemberTypeViewModel> memberTypeViewModels, long memberTypeId)
        {
            long timeFormatId = 0;
            var time = _unitOfWork.SlotRepository.Get(x => x.SlotId == slotId && x.IsActive == true);
            if (time.Time >= TimeSpan.Parse("12:00")) { timeFormatId = (int)Core.Helper.Enum.EnumTimeFormatType.PM; }
            else { timeFormatId = (int)Core.Helper.Enum.EnumTimeFormatType.AM; }

            BucketDetail bucketDetail = _unitOfWork.BucketDetailRepository.Get(x => x.BucketDetailId == bucketId && x.IsActive == true);
            decimal totalBucketTax = 0;
            foreach (var tax in bucketDetail.BucketTaxMappings.Where(x => x.IsActive == true))
            {
                totalBucketTax += tax.Tax.Percentage;

            }
            var ballPrice = bucketDetail.Price + (bucketDetail.Price * (totalBucketTax / 100));

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

            decimal total = 0;
            long dayTypeId = GetDayTypeId(date);
            foreach (var memberType in memberTypeViewModels)
            {
                MemberTypeViewModel memberTypeViewModel = new MemberTypeViewModel();
                if (memberType.PlayerCount != 0)
                {
                    Pricing memberPricing = new Pricing();
                    memberPricing = GetPricingDetails(date, slotId, (int)EnumBookingType.BDR, (int)EnumBookingType.BDR, memberType.MemberTypeId, dayTypeId, bucketId, timeFormatId);


                    bookingPricingViewModel.RangeFee += memberPricing != null ? memberPricing.RangeFee.GetValueOrDefault() * memberType.PlayerCount : 0;
                    if (memberPricing != null)
                    {
                        decimal totalPrec = 0;
                        foreach (var tax in memberPricing.PriceTaxMappings.Where(x => x.FeeCategoryId == (int)Core.Helper.Enum.EnumFeeCategory.RangeFee && x.IsActive == true))
                        {
                            totalPrec += tax.Tax.Percentage;

                        }
                        bookingPricingViewModel.RangeFee = Math.Round(bookingPricingViewModel.RangeFee + (bookingPricingViewModel.RangeFee * (totalPrec / 100)), 2);


                    }

                    //bookingPricingViewModel.MemberTypeViewModels.Add(memberTypeViewModel);
                    //memberTypeViewModel.MemberTypeId = memberType.MemberTypeId;
                    //memberTypeViewModel.Name = memberType.Name;
                    //memberTypeViewModel.PlayerCount = memberType.PlayerCount;
                }
            }

            bookingPricingViewModel.CourseTaxMappingViewModels = GetAllCourseTaxs();
            return bookingPricingViewModel;
        }

        /// <summary>
        /// Get Pricing Details For Pricing
        /// </summary>
        /// <param name="bookingPricingViewModel"></param>
        /// <returns></returns>
        public BookingPricingViewModel GetPricingCalculation(BookingPricingViewModel bookingPricingViewModel)
        {
            if (bookingPricingViewModel.SearchDate > DateHelper.ConvertSystemDate().AddDays(7))
                throw new Exception("You Can Book Only Upto 7 Days");
            if (bookingPricingViewModel.BookingTypeId == (int)Core.Helper.Enum.EnumBookingType.BTT)
            {
                return GetPricingCalculation(bookingPricingViewModel.HoleTypeId, bookingPricingViewModel.SearchDate, bookingPricingViewModel.SlotId, bookingPricingViewModel.NoOfPlayer, bookingPricingViewModel.NoOfMemberPlayer, bookingPricingViewModel.NoOfNonMemberPlayer, bookingPricingViewModel.MemberTypeViewModels, bookingPricingViewModel.MemberTypeId);
            }
            else
            {
                return GetPricingCalculationBDT(bookingPricingViewModel.BucketTypeId, bookingPricingViewModel.SearchDate, bookingPricingViewModel.SlotId, bookingPricingViewModel.NoOfPlayer, bookingPricingViewModel.NoOfMemberPlayer, bookingPricingViewModel.NoOfNonMemberPlayer, bookingPricingViewModel.MemberTypeViewModels, bookingPricingViewModel.MemberTypeId);
            }

        }

        /// <summary>
        /// Get Bucket List Details
        /// </summary>
        /// <returns></returns>
        public List<BucketViewModel> GetBucketDetailList(DateTime date, long memberTypeId)
        {
            long dayTypeId = GetDayTypeId(date);
            List<BucketViewModel> bucketViewModels = new List<BucketViewModel>();
            List<BucketDetail> bucketDetails = _unitOfWork.BucketDetailRepository.GetMany(x => x.IsActive == true && x.MemberTypeId == memberTypeId && x.DayTypeId == dayTypeId).ToList();
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
        /// Save Tee Time Booking Details
        /// </summary>
        /// <param name="bookingViewModel"></param>
        /// <returns></returns>
        private BookingViewModel SaveTeeTimeBooking(SaveBookingViewModel bookingViewModel)
        {
            Slot slot = _unitOfWork.SlotRepository.Get(x => x.IsActive == true && x.SlotId == bookingViewModel.SlotId);
            if (slot == null) { throw new Exception("Invalid Slot Time"); }
            if (IsSlotAvailable(slot.Time.ToString("hh':'mm"), (int)EnumBookingType.BTT, bookingViewModel.TeeOffDate, bookingViewModel.NoOfPlayer, bookingViewModel.CoursePairingId, bookingViewModel.BookingPlayerDetailViewModels) == false)
                throw new Exception("Slot Is Already Booked");
            Booking booking = new Booking()
            {
                NoOfPlayer = bookingViewModel.NoOfPlayer,
                NoOfMember = bookingViewModel.NoOfMemberPlayer,
                NoOfNonMember = bookingViewModel.NoOfNonMemberPlayer,
                Discount = bookingViewModel.Discount,
                CaddieCount = bookingViewModel.CaddieCount,
                CartCount = bookingViewModel.CartCount,
                CaddieFee = bookingViewModel.CaddieFee,
                CartFee = bookingViewModel.CartFee,
                BookingStatusId = (int)Core.Helper.Enum.EnumBookingStatus.Confirm,
                GreenFee = bookingViewModel.GreenFee,
                HoleTypeId = bookingViewModel.HoleTypeId,
                IsActive = true,
                CreatedOn = DateTime.UtcNow,
                BookingTypeId = (int)EnumBookingType.BTT,
                FeeAndTaxes = bookingViewModel.FeeAndTaxes,
                TeeOffSlot = slot.Time.ToString("hh':'mm"),
                TeeOffDate = bookingViewModel.TeeOffDate,
                OnSpot = true,
                GolferId = bookingViewModel.GolferId,
                BookingDate = DateTime.UtcNow,
                Amount = bookingViewModel.Amount,
                CurrencyId = 1,
                CoursePairingId = bookingViewModel.CoursePairingId,
                PaidAmount = bookingViewModel.Amount
            };
            _unitOfWork.BookingRepository.Insert(booking);
            _unitOfWork.Save();
            Booking bk = _unitOfWork.BookingRepository.GetById(booking.BookingId);
            bk.PaymentGatewayBookingId = Core.Helper.Constants.BookingKey.BookingKeyId + booking.BookingId.ToString();
            _unitOfWork.BookingRepository.Update(bk);
            _unitOfWork.Save();
            SaveMemberTypePlayerMapping(booking.BookingId, bookingViewModel.MemberTypeViewModels);
            SaveCourseTaxMapping(booking.BookingId, GetAllCourseTaxs());
            SaveBookingEquipmentMappingMapping(booking.BookingId, bookingViewModel.BookingEquipmentMappingViewModels);
            SaveBookingPlayerDetail(booking.BookingId, bookingViewModel.BookingPlayerDetailViewModels);
            CoursePairing coursePairing = _unitOfWork.CoursePairingRepository.Get(x => x.CoursePairingId == booking.CoursePairingId);

            BookingViewModel bookingViewModel1 = new BookingViewModel()
            {
                BookingId = booking.BookingId,
                PaymentGatewayBookingId = booking.PaymentGatewayBookingId,
                Time = slot.Time.ToString("hh':'mm"),
                NoOfPlayer = booking.NoOfPlayer,
                UserName = booking.Golfer.Name,
                CoursePairingName = coursePairing != null ? (coursePairing.CourseName1.Value) + (coursePairing.EndCourseNameId != null ? " - " + coursePairing.CourseName.Value : "") : ""

            };
            if (bk.HoleTypeId == (int)Core.Helper.Enum.EnumHoleType.Hole18)
            {
                bookingViewModel1.NoOfHole = 18;
            }
            else if (bk.HoleTypeId == (int)Core.Helper.Enum.EnumHoleType.Hole9)
            {
                bookingViewModel1.NoOfHole = 9;
            }
            else
            {

                bookingViewModel1.NoOfHole = 27;
            }
            return bookingViewModel1;
        }


        /// <summary>
        /// Save Driving Range Booking 
        /// </summary>
        /// <param name="bookingViewModel"></param>
        /// <returns></returns>
        private ViewModel.Admin.BookingViewModel SaveDrivingRangeBooking(SaveBookingViewModel bookingViewModel)
        {
            Slot slot = _unitOfWork.SlotRepository.Get(x => x.IsActive == true && x.SlotId == bookingViewModel.SlotId);

            BucketDetail bucketDetail = _unitOfWork.BucketDetailRepository.Get(x => x.BucketDetailId == bookingViewModel.BucketId && x.IsActive == true);
            if (IsSlotAvailable(slot.Time.ToString("hh':'mm"), (int)EnumBookingType.BDR, bookingViewModel.TeeOffDate, bookingViewModel.NoOfPlayer, null, bookingViewModel.BookingPlayerDetailViewModels) == false)
                throw new Exception("Slot Is Already Booked");
            Booking booking = new Booking()
            {
                NoOfPlayer = bookingViewModel.NoOfPlayer,
                NoOfMember = bookingViewModel.NoOfMemberPlayer,
                NoOfNonMember = bookingViewModel.NoOfNonMemberPlayer,
                Discount = bookingViewModel.Discount,
                BookingStatusId = (int)Core.Helper.Enum.EnumBookingStatus.Confirm,
                RangeFee = bookingViewModel.RangeFee,
                IsActive = true,
                CreatedOn = DateTime.UtcNow,
                BookingTypeId = (int)EnumBookingType.BDR,
                FeeAndTaxes = bookingViewModel.FeeAndTaxes,
                TeeOffSlot = slot.Time.ToString("hh':'mm"),
                TeeOffDate = bookingViewModel.TeeOffDate,
                OnSpot = true,
                GolferId = bookingViewModel.GolferId,
                BookingDate = DateTime.UtcNow,
                Amount = bookingViewModel.Amount,
                BallFee = bookingViewModel.BallFee,
                NoOfBalls = bucketDetail.Balls,
                CurrencyId = 1,
                PaidAmount = bookingViewModel.Amount
            };
            _unitOfWork.BookingRepository.Insert(booking);
            _unitOfWork.Save();
            Booking bk = _unitOfWork.BookingRepository.GetById(booking.BookingId);
            bk.PaymentGatewayBookingId = Core.Helper.Constants.BookingKey.BookingKeyId + booking.BookingId.ToString();
            _unitOfWork.BookingRepository.Update(bk);
            _unitOfWork.Save();
            SaveMemberTypePlayerMapping(booking.BookingId, bookingViewModel.MemberTypeViewModels);
            SaveCourseTaxMapping(booking.BookingId, GetAllCourseTaxs());
            SaveBookingPlayerDetail(booking.BookingId, bookingViewModel.BookingPlayerDetailViewModels);
            BookingViewModel bookingViewModel1 = new BookingViewModel()
            {
                BookingId = booking.BookingId,
                PaymentGatewayBookingId = booking.PaymentGatewayBookingId,
                Time = slot.Time.ToString("hh':'mm")

            };
            return bookingViewModel1;
        }


        /// <summary>
        /// Check Slot Is Available Or Not
        /// </summary>
        /// <param name="slotTime"></param>
        /// <param name="bookingTypeId"></param>
        /// <param name="teeOffDate"></param>
        /// <returns></returns>
        private bool IsSlotAvailable(string slotTime, long bookingTypeId, DateTime teeOffDate, int noOfPlayerBooking, long? coursePairingId, List<BookingPlayerDetailViewModel> bookingPlayerDetailViewModels)
        {

            DateTime currentTime = Core.Helper.DateHelper.ConvertSystemDate(); List<Booking> bookings = new List<Booking>();
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
                int i = 1;
                foreach (var item in bookingPlayerDetailViewModels)
                {
                    int bookingCount = 0;
                    var golferBookings1 = _unitOfWork.BookingRepository.GetMany(x => x.TeeOffDate == teeOffDate && x.IsActive == true && x.BookingTypeId == bookingTypeId && (x.BookingStatusId == (int)EnumBookingStatus.Confirm || (x.BookingStatusId == (int)EnumBookingStatus.Pending && x.CreatedOn.AddMinutes(15) > DateTime.UtcNow))).ToList();
                    foreach (var golf in golferBookings1)
                    {
                        var bookingPalyerDetails = golf.BookingPlayerDetails.ToList();
                        if (bookingPalyerDetails != null && bookingPalyerDetails.Count != 0 && item.MemberShipId != null)
                        {
                            if (i != 1)
                            {
                                if (bookingPalyerDetails[0].Player1 == item.MemberShipId)
                                    bookingCount++;
                            }
                            if (bookingPalyerDetails[0].Player2 == item.MemberShipId)
                                bookingCount++;
                            if (bookingPalyerDetails[0].Player3 == item.MemberShipId)
                                bookingCount++;
                            if (bookingPalyerDetails[0].Player4 == item.MemberShipId)
                                bookingCount++;
                        }
                    }
                    var golferBookings = new List<Booking>();
                    if (item.MemberShipId != "" && item.MemberShipId != null)
                    {
                        golferBookings = _unitOfWork.BookingRepository.GetMany(x => x.TeeOffDate == teeOffDate && x.Golfer.ClubMemberId == item.MemberShipId && x.IsActive == true && x.BookingTypeId == bookingTypeId && (x.BookingStatusId == (int)EnumBookingStatus.Confirm || (x.BookingStatusId == (int)EnumBookingStatus.Pending && x.CreatedOn.AddMinutes(15) > DateTime.UtcNow))).ToList();

                    }//Max Booking Allowed Per User
                    int bookingLeft = 2 - (golferBookings.Count() + bookingCount);
                    if (bookingLeft <= 0)
                        throw new Exception("Max Two Booking Per Day.Please Check Golfer " + i.ToString());
                    i++;
                }

            }
            else
            {
                bookings = _unitOfWork.BookingRepository.GetMany(x => x.TeeOffDate == teeOffDate && x.TeeOffSlot == slotTime && x.IsActive == true && x.BookingTypeId == bookingTypeId && (x.BookingStatusId == (int)EnumBookingStatus.Confirm || (x.BookingStatusId == (int)EnumBookingStatus.Pending && x.CreatedOn.AddMinutes(15) > DateTime.UtcNow))).ToList();
                int i = 1;
                foreach (var item in bookingPlayerDetailViewModels)
                {
                    int bookingCount = 0;
                    var golferBookings1 = _unitOfWork.BookingRepository.GetMany(x => x.TeeOffDate == teeOffDate && x.IsActive == true && x.BookingTypeId == bookingTypeId && (x.BookingStatusId == (int)EnumBookingStatus.Confirm || (x.BookingStatusId == (int)EnumBookingStatus.Pending && x.CreatedOn.AddMinutes(15) > DateTime.UtcNow))).ToList();
                    foreach (var golf in golferBookings1)
                    {
                        var bookingPalyerDetails = golf.BookingPlayerDetails.ToList();
                        if (bookingPalyerDetails != null && bookingPalyerDetails.Count != 0 && item.MemberShipId != null)
                        {
                            if (i != 1)
                            {
                                if (bookingPalyerDetails[0].Player1 == item.MemberShipId)
                                    bookingCount++;
                            }
                            if (bookingPalyerDetails[0].Player2 == item.MemberShipId)
                                bookingCount++;
                            if (bookingPalyerDetails[0].Player3 == item.MemberShipId)
                                bookingCount++;
                            if (bookingPalyerDetails[0].Player4 == item.MemberShipId)
                                bookingCount++;
                        }
                    }
                    var golferBookings = new List<Booking>();
                    if (item.MemberShipId != "" && item.MemberShipId != null)
                    {
                        golferBookings = _unitOfWork.BookingRepository.GetMany(x => x.TeeOffDate == teeOffDate && x.Golfer.ClubMemberId == item.MemberShipId && x.IsActive == true && x.BookingTypeId == bookingTypeId && (x.BookingStatusId == (int)EnumBookingStatus.Confirm || (x.BookingStatusId == (int)EnumBookingStatus.Pending && x.CreatedOn.AddMinutes(15) > DateTime.UtcNow))).ToList();

                    }//Max Booking Allowed Per User
                    int bookingLeft = 2 - (golferBookings.Count() + bookingCount);
                    if (bookingLeft <= 0)
                        throw new Exception("Max Two Booking Per Day.Please Check Golfer " + i.ToString());
                    i++;
                }
            }
            int noOfPlayer = bookings.Select(x => x.NoOfPlayer).Sum();
            int playerLeft = 4 - (noOfPlayer + noOfPlayerBooking);
            //if (noOfPlayerBooking <= 4 && playerLeft >= 0)
            //{
            if (bookings.Count() == 0)
            {

                return true;
            }
            return false;
        }


        /// <summary>
        /// Save Booking 
        /// </summary>
        /// <param name="bookingViewModel"></param>
        /// <returns></returns>
        public bool SaveBooking(SaveBookingViewModel bookingViewModel)
        {

            BookingViewModel bvM = new BookingViewModel();
            long id = SaveOrGetGolferProfile(bookingViewModel);
            bookingViewModel.GolferId = id;
            string emailBody = "";
            if (bookingViewModel.TeeOffDate > DateHelper.ConvertSystemDate().AddDays(7))
                throw new Exception("You Can Book Only Upto 7 Days");
            if (bookingViewModel.BookingTypeId == (int)Core.Helper.Enum.EnumBookingType.BTT)
            {
                CheckEquipmentVaildation(bookingViewModel);
                bvM = SaveTeeTimeBooking(bookingViewModel);
                string emailAddress = "";
                string coursePairingName = bvM.CoursePairingName;
                emailBody = " Dear Member,<br/><br/>Thank you for choosing to book with us!<br/><br/>Your reservation is confirmed as follows:<br/><br/>";
                emailBody = emailBody + "Name: " + bvM.NoOfPlayer + "<br/>Date: " + bookingViewModel.TeeOffDate.ToString("dd/MM/yyyy") + "<br/>No.of golfers: " + bvM.NoOfPlayer + "<br/>Golf course: " + coursePairingName + "<br/>Tee-time:" + DateTime.Parse(bvM.Time).ToString("hh:mm tt", CultureInfo.GetCultureInfo("en-US")) + "<br/><br/>";
                emailBody = emailBody + "*Payment to be made at the Pro - Shop on arrival.<br/><br/>We look forward to welcoming you!<br/><br/>Warm regards,<br/>Classic Golf & Country Club";



                //emailBody = "Dear Member, your tee time booking is confirmed as follows -<br/>" +
                //       " Booking ID - " + bvM.PaymentGatewayBookingId +
                //       "<br/> Date - " + bookingViewModel.TeeOffDate.ToString("dd/MM/yyyy") +
                //       "<br/> Time - " + DateTime.Parse(bvM.Time).ToString("hh:mm tt", CultureInfo.GetCultureInfo("en-US")) + "<br/> No. of Holes - " + bvM.NoOfHole +
                //       "<br/> Course Name - " + bvM.CoursePairingName +
                //       "<br/>";
                //foreach (var item in bookingViewModel.BookingPlayerDetailViewModels)
                //{
                //    if (item.MemberShipId == null || item.MemberShipId == "")
                //    {
                //        emailBody = emailBody + "Player Name/Membership Number : " + item.Name + "<br/>";
                //    }
                //    else
                //    {
                //        emailBody = emailBody + "Player Name/Membership Number : " + item.MemberShipId + "<br/>";

                //    }
                //    if (emailAddress == "")
                //    {
                //        emailAddress = item.EmailAddress;
                //    }
                //    else
                //    {
                //        emailAddress = emailAddress + "," + item.EmailAddress;
                //    }
                //}
                //emailBody = emailBody + "<br/> We look forward to seeing you at " + Core.Helper.Constants.Common.AppName + "!";
                GolfCentra.ViewModel.MailerViewModel mailerViewModel = new GolfCentra.ViewModel.MailerViewModel()
                {
                    ToEmails = emailAddress,
                    From = Constants.MailId.FromMails,
                    CCMail = Constants.MailId.CCMails,
                    Subject = "Booking For " + Constants.Common.AppName + " ",
                    Body = emailBody
                };
                EmailNotification emailNotification = new EmailNotification();
                bool status = emailNotification.SendMail(mailerViewModel);



            }
            else
            {
                bvM = SaveDrivingRangeBooking(bookingViewModel);
                string emailAddress = "";
                emailBody = "Dear Member, your driving range booking is confirmed as follows -<br/>" +
                       " Booking ID - " + bvM.PaymentGatewayBookingId +
                       "<br/> Date - " + bookingViewModel.TeeOffDate.ToString("dd/MM/yyyy") +
                       "<br/> Time - " + DateTime.Parse(bvM.Time).ToString("hh:mm tt", CultureInfo.GetCultureInfo("en-US")) + "<br/> No. of Holes - " + bvM.NoOfHole +
                       "<br/> Course Name - " + bvM.CoursePairingName +
                       "<br/>";
                foreach (var item in bookingViewModel.BookingPlayerDetailViewModels)
                {
                    if (item.MemberShipId == null || item.MemberShipId == "")
                    {
                        emailBody = emailBody + "Player Name/Membership Number : " + item.Name + "<br/>";
                    }
                    else
                    {
                        emailBody = emailBody + "Player Name/Membership Number : " + item.MemberShipId + "<br/>";

                    }
                    if (emailAddress == "")
                    {
                        emailAddress = item.EmailAddress;
                    }
                    else
                    {
                        emailAddress = emailAddress + "," + item.EmailAddress;
                    }
                }
                emailBody = emailBody + "<br/> We look forward to seeing you at " + Core.Helper.Constants.Common.AppName + "!";
                GolfCentra.ViewModel.MailerViewModel mailerViewModel = new GolfCentra.ViewModel.MailerViewModel()
                {
                    ToEmails = emailAddress,
                    From = Constants.MailId.FromMails,
                    CCMail = Constants.MailId.CCMails,
                    Subject = "Booking For " + Constants.Common.AppName + " ",
                    Body = emailBody
                };
                EmailNotification emailNotification = new EmailNotification();
                bool status = emailNotification.SendMail(mailerViewModel);
            }
            try
            {
                SessionActivityPageViewModel sessionActivityPageViewModel = new SessionActivityPageViewModel()
                {
                    ControllerName = "CreateBooking",
                    ActionName = "Create",
                    PerformOn = bvM.BookingId.ToString(),
                    Info = "Created a Booking with booking id-" + bvM.BookingId.ToString(),
                    LoginHistoryId = Convert.ToInt64(Core.Helper.Crypto.DecryptStringAES(bookingViewModel.ApiClientViewModel.UniqueSessionId)),

                };
                new Common.AddSessionActivity().SaveSessionActivity(sessionActivityPageViewModel);
            }
            catch (Exception ex)
            {


            }



            return true;
        }

        /// <summary>
        /// Save Golfer Profile Is not Exist
        /// </summary>
        /// <param name="bookingViewModel"></param>
        /// <returns></returns>
        private long SaveOrGetGolferProfile(SaveBookingViewModel bookingViewModel)
        {

            Golfer golfer = _unitOfWork.GolferRepository.Get(x => x.IsActive == true && x.Email.Trim().ToLower().Equals(bookingViewModel.BookingPlayerDetailViewModels[0].EmailAddress.Trim().ToLower()));
            if (golfer == null)
            {
                Golfer golf = new Golfer()
                {
                    Name = bookingViewModel.BookingPlayerDetailViewModels[0].Name,
                    Email = bookingViewModel.BookingPlayerDetailViewModels[0].EmailAddress,

                    ClubMemberId = "",
                    IsActive = true,
                    PhoneCode = "",
                    PlatformTypeId = (int)Core.Helper.Enum.EnumPlatformType.Web,
                    CreatedOn = System.DateTime.UtcNow,
                    MemberTypeId = (int)Core.Helper.Enum.EnumMemberType.NonMember,
                    Password = Core.Helper.RamdomNumber.RandomNumber()

                };
                if (bookingViewModel.BookingPlayerDetailViewModels[0].Contact != null)
                {
                    golf.Mobile = bookingViewModel.BookingPlayerDetailViewModels[0].Contact;
                }
                else
                {
                    golf.Mobile = "";
                }
                _unitOfWork.GolferRepository.Insert(golf);
                _unitOfWork.Save();
                return golf.GolferId;

            }
            else
            {
                return golfer.GolferId;
            }

        }

        /// <summary>
        /// Get All Booking Type
        /// </summary>
        /// <returns></returns>
        public List<BookingTypeViewModel> GetAllBookingType()
        {
            List<BookingTypeViewModel> bookingTypeViewModels = new List<BookingTypeViewModel>();
            List<BookingType> bookingTypes = _unitOfWork.BookingTypeRepository.GetMany(x => x.IsActive == true).ToList();
            foreach (BookingType btype in bookingTypes)
            {
                BookingTypeViewModel bookingTypeViewModel = new BookingTypeViewModel()
                {
                    Name = btype.Value,
                    BookingTypeId = btype.BookingTypeId
                };
                bookingTypeViewModels.Add(bookingTypeViewModel);

            }
            return bookingTypeViewModels;
        }

        /// <summary>
        /// Get All Day type Detail
        /// </summary>
        /// <returns></returns>
        public List<DayTypeViewModel> GetAllDayType()
        {
            List<DayTypeViewModel> dayTypeViewModels = new List<DayTypeViewModel>();
            List<DayType> dayTypes = _unitOfWork.DayTypeRepository.GetMany(x => x.IsActive == true).ToList();
            foreach (DayType btype in dayTypes)
            {
                DayTypeViewModel dayTypeViewModel = new DayTypeViewModel()
                {
                    Name = btype.Name,
                    DayTypeId = btype.DayTypeId
                };
                dayTypeViewModels.Add(dayTypeViewModel);

            }
            return dayTypeViewModels;
        }

        /// <summary>
        /// Get Booking Details For GolferId
        /// </summary>
        /// <param name="golferId"></param>
        /// <returns></returns>
        public List<BookingViewModel> GetBookingDetailsByGolferId(long golferId)
        {
            List<Booking> bookings = _unitOfWork.BookingRepository.GetMany(x => x.IsActive == true && x.GolferId == golferId).OrderByDescending(x => x.CreatedOn).ToList();
            List<BookingViewModel> bookingViewModels = new List<BookingViewModel>();

            foreach (var booking in bookings)
            {
                int hole = 0;
                if (booking.BookingTypeId == (int)Core.Helper.Enum.EnumBookingType.BTT)
                    hole = booking.HoleType.Value;
                BookingViewModel bookingViewModel = new BookingViewModel()
                {
                    BookingDate = booking.BookingDate,
                    TeeOffDate = booking.TeeOffDate.ToShortDateString(),
                    BookingId = booking.BookingId,
                    UserName = booking.Golfer.Name,
                    BookingStatus = booking.BookingStatu.Value,
                    NoOfHole = hole,
                    Time = booking.TeeOffSlot,
                    BallFee = booking.BallFee.GetValueOrDefault(),
                    CaddieFee = booking.CaddieFee.GetValueOrDefault(),
                    CartFee = booking.CartFee.GetValueOrDefault(),
                    TotalAmount = booking.PaidAmount.GetValueOrDefault(),
                    Discount = booking.Discount.GetValueOrDefault(),
                    GreenFee = booking.GreenFee.GetValueOrDefault(),
                    OnSpot = booking.OnSpot.GetValueOrDefault(),
                    NoOfBalls = booking.NoOfBalls.GetValueOrDefault(),
                    NoOfCaddie = booking.CaddieCount.GetValueOrDefault(),
                    NoOfCart = booking.CartCount.GetValueOrDefault(),
                    NoOfMemberPlayer = booking.NoOfMember.GetValueOrDefault(),
                    NoOfNonMemberPlayer = booking.NoOfNonMember.GetValueOrDefault(),
                    NoOfPlayer = booking.NoOfPlayer,
                    PaymentMode = booking.PaymentMode,
                    RangeFee = booking.RangeFee.GetValueOrDefault(),
                    CurrencyName = Core.Helper.Constants.Currency.CurrencyName,

                };
                bookingViewModels.Add(bookingViewModel);
            }
            return bookingViewModels;
        }

        /// <summary>
        /// Get All Member Type Detail
        /// </summary>
        /// <returns></returns>
        public List<MemberTypeViewModel> GetAllMemberType()
        {
            List<MemberTypeViewModel> dayTypeViewModels = new List<MemberTypeViewModel>();
            List<MemberType> dayTypes = _unitOfWork.MemberTypeRepository.GetMany(x => x.IsActive == true).ToList();
            foreach (MemberType btype in dayTypes)
            {
                MemberTypeViewModel dayTypeViewModel = new MemberTypeViewModel()
                {
                    Name = btype.Name,
                    MemberTypeId = btype.MemberTypeId
                };
                dayTypeViewModels.Add(dayTypeViewModel);

            }
            return dayTypeViewModels;
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
        /// Save MemberTypePlayer Mapping For Booking
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
        /// Save CourseTaxMapping For Booking
        /// </summary>
        /// <param name="bookingId"></param>
        /// <param name="courseTaxMappingViewModels"></param>
        private void SaveCourseTaxMapping(long bookingId, List<CourseTaxMappingViewModel> courseTaxMappingViewModels)
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

        /// <summary>
        /// Get All Course Taxs
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

        private List<BookingEquipmentMappingViewModel> GetAllEquipment(DateTime date)
        {
            List<Booking> bookings = new List<Booking>();

            bookings = _unitOfWork.BookingRepository.GetMany(x => x.TeeOffDate == date && x.IsActive == true && x.BookingTypeId == (int)EnumBookingType.BTT && (x.BookingStatusId == (int)EnumBookingStatus.Confirm || (x.BookingStatusId == (int)EnumBookingStatus.Pending && x.CreatedOn.AddMinutes(15) > DateTime.UtcNow))).ToList();

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


                //if (bookingEquipmentMappingViewModel.EquipmentLeft > 0)
                //{
                    bookingEquipmentMappingViewModels.Add(bookingEquipmentMappingViewModel);
                //}
            }
            return bookingEquipmentMappingViewModels;
        }


        public bool CancelBooking(long bookingId, long uniqueSessionId)
        {
            Booking booking = _unitOfWork.BookingRepository.Get(x => x.BookingId == bookingId && x.IsActive == true);

            if (booking != null)
            {
                var time = booking.TeeOffSlot.Split(':');
                DateTime date = new DateTime(booking.TeeOffDate.Year, booking.TeeOffDate.Month, booking.TeeOffDate.Day, Convert.ToInt32(time[0]), Convert.ToInt32(time[1]), 0);
                if (booking.BookingStatusId == (int)Core.Helper.Enum.EnumBookingStatus.Cancelled || booking.BookingStatusId == (int)Core.Helper.Enum.EnumBookingStatus.Pending || booking.BookingStatusId == (int)Core.Helper.Enum.EnumBookingStatus.Failed || date < Core.Helper.DateHelper.ConvertSystemDateToCurrent(DateTime.UtcNow))
                    throw new Exception("You can not cancel that booking.");
                //DateTime date = Core.Helper.DateHelper.ConvertSystemDateToCurrent(System.DateTime.UtcNow);
                //if (date <= booking.BookingDate.AddDays(-1))
                //{
                booking.BookingStatusId = (int)Core.Helper.Enum.EnumBookingStatus.Cancelled;
                booking.UpdatedOn = System.DateTime.UtcNow;
                _unitOfWork.BookingRepository.Update(booking);
                _unitOfWork.Save();

                string emailBody = "";
                if (booking.BookingTypeId == (int)Core.Helper.Enum.EnumBookingType.BTT)
                {

                    // string coursePairingName = booking.CoursePairingId != null ? (booking.CoursePairing.CourseName1.Value) + (booking.CoursePairing.EndCourseNameId != null ? " - " + booking.CoursePairing.CourseName.Value : "") : "";

                    emailBody = "Dear Member,<br/><br/>Your booking is cancelled.<br/><br/>We look forward to welcoming you back soon!<br/><br/>Warm regards,<br/>Classic Golf &Country Club";

                    //emailBody = "Dear Member, your tee time booking is cancelled as follows -<br/> Booking ID - " + booking.PaymentGatewayBookingId + "<br/> Date - " + booking.TeeOffDate.ToString("dd/MM/yyyy") + "<br/> Time - " + DateTime.Parse(booking.TeeOffSlot).ToString("hh:mm tt", CultureInfo.GetCultureInfo("en-US"))  + "<br/> No. of Holes - " + booking.HoleType.Value + "<br/> Course Name - " + coursePairingName + "<br/>";
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

                    emailBody = "Dear Member, your driving range booking is cancelled as follows -<br/> Booking ID - " + booking.PaymentGatewayBookingId + "<br/> Date - " + booking.TeeOffDate.ToString("dd/MM/yyyy") + "<br/> Time - " + DateTime.Parse(booking.TeeOffSlot).ToString("hh:mm tt", CultureInfo.GetCultureInfo("en-US")) + "<br/>";


                    foreach (var item in booking.BookingPlayerDetails)
                    {
                        if (item.Player1 == null || item.Player1 == "")
                        {
                            emailBody = emailBody + "Player Name/Membership Number : " + item.Player1 + "<br/>";
                        }


                    }
                    emailBody = emailBody + "<br/> We look forward to seeing you at " + Core.Helper.Constants.Common.AppName + "!";
                }
                GolfCentra.ViewModel.MailerViewModel mailerViewModel = new GolfCentra.ViewModel.MailerViewModel()
                {
                    ToEmails = booking.Golfer.Email,
                    From = Constants.MailId.FromMails,
                    CCMail = Constants.MailId.CCMails,
                    Subject = "Cancel Booking For " + Constants.Common.AppName + " ",
                    Body = emailBody
                };
                EmailNotification emailNotification = new EmailNotification();
                bool status = emailNotification.SendMail(mailerViewModel);
                try
                {

                    SessionActivityPageViewModel sessionActivityPageViewModel = new SessionActivityPageViewModel()
                    {
                        ControllerName = "CancelBooking",
                        ActionName = "Cancel",
                        PerformOn = bookingId.ToString(),
                        LoginHistoryId = uniqueSessionId,

                        Info = "Canceled a Booking with booking id- " + bookingId.ToString()
                    };
                    new Common.AddSessionActivity().SaveSessionActivity(sessionActivityPageViewModel);

                }
                catch (Exception)
                {

                }


            }
            else
            {
                throw new Exception("No booking found for booking Id");
            }

            return true;
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
                    if (playerDetail.PlayerSerialNumber.Trim() == "Player 1")
                    {
                        if (playerDetail.MemberShipId != null && playerDetail.MemberShipId != "")
                        {
                            bookingPlayerDetail.Player1 = playerDetail.MemberShipId;
                        }
                        else
                        {
                            bookingPlayerDetail.Player1 = playerDetail.Name;
                        }
                    }
                    else if (playerDetail.PlayerSerialNumber.Trim() == "Player 2")
                    {
                        if (playerDetail.MemberShipId != null && playerDetail.MemberShipId != "")
                        {
                            bookingPlayerDetail.Player2 = playerDetail.MemberShipId;
                        }
                        else
                        {
                            bookingPlayerDetail.Player2 = playerDetail.Name;
                        }
                    }
                    else if (playerDetail.PlayerSerialNumber.Trim() == "Player 3")
                    {
                        if (playerDetail.MemberShipId != null && playerDetail.MemberShipId != "")
                        {
                            bookingPlayerDetail.Player3 = playerDetail.MemberShipId;
                        }
                        else
                        {
                            bookingPlayerDetail.Player3 = playerDetail.Name;
                        }
                    }
                    else if (playerDetail.PlayerSerialNumber.Trim() == "Player 4")
                    {
                        if (playerDetail.MemberShipId != null && playerDetail.MemberShipId != "")
                        {
                            bookingPlayerDetail.Player4 = playerDetail.MemberShipId;
                        }
                        else
                        {
                            bookingPlayerDetail.Player4 = playerDetail.Name;
                        }
                    }


                }
                _unitOfWork.BookingPlayerDetailRepository.Insert(bookingPlayerDetail);
                _unitOfWork.Save();
            }
        }


        public List<BookingStatusViewModel> GetAllBookingStatus()
        {
            List<BookingStatusViewModel> bookingStatusViewModels = new List<BookingStatusViewModel>();
            List<BookingStatu> bookingStatus = _unitOfWork.BookingStatusRepository.GetMany(x => x.IsActive == true).ToList();
            foreach (var status in bookingStatus)
            {
                BookingStatusViewModel bookingStatu = new BookingStatusViewModel()
                {
                    BookingStatusId = status.BookingStatusId,
                    Name = status.Value,

                };
                bookingStatusViewModels.Add(bookingStatu);
            }
            return bookingStatusViewModels;
        }

        private PromotionViewModel GetPromotionById(long id)
        {
            List<PromotionViewModel> promotionViewModels = new List<PromotionViewModel>();
            DateTime date = Core.Helper.DateHelper.ConvertSystemDate();
            Promotion promotion = _unitOfWork.PromotionRepository.Get(x => x.PromotionsId == id);

            PromotionViewModel promotionViewModel = new PromotionViewModel()
            {
                PromotionsId = promotion.PromotionsId,
                Name = promotion.Name,
                StartDate = promotion.StartDate,
                EndDate = promotion.EndDate,
                StartTime = promotion.StartTime.GetValueOrDefault(),
                EndTime = promotion.EndTime.GetValueOrDefault(),
                Extra = promotion.Extra,
                HoleTypeId = promotion.HoleTypeId,
                Price = promotion.Price,
                CaddieFee = promotion.CaddieFee,
                GreenFee = promotion.GreenFee,
                CartFee = promotion.CartFee,
                HoleTypeName = promotion.HoleType.Value.ToString()
            };
            foreach (var equ in promotion.PromotionsEquipmentMappings)
            {
                if (promotionViewModel.EquipmentName != "" && promotionViewModel.EquipmentName != null)
                {
                    promotionViewModel.EquipmentName = promotionViewModel.EquipmentName + "," + equ.Equipment.Name;
                }
                else
                {
                    promotionViewModel.EquipmentName = equ.Equipment.Name;
                }
            }


            return promotionViewModel;
        }


        public CouponViewModel GetCouponAmountByCouponCode(string code)
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

        public bool CheckMemberShipId(string memberShipId)
        {

            Golfer golferCheck = _unitOfWork.GolferRepository.Get(x => x.ClubMemberId == memberShipId && x.IsActive == true);
            if (golferCheck != null)
            {
                return true;
            }
            return false;
        }


        public void CheckEquipmentVaildation(SaveBookingViewModel bookingViewModel)
        {
            List<Booking> bookings = new List<Booking>();
            bookings = _unitOfWork.BookingRepository.GetMany(x => x.TeeOffDate == bookingViewModel.TeeOffDate && x.IsActive == true && x.BookingTypeId == (int)EnumBookingType.BTT && (x.BookingStatusId == (int)EnumBookingStatus.Confirm || (x.BookingStatusId == (int)EnumBookingStatus.Pending && x.CreatedOn.AddMinutes(15) > DateTime.UtcNow))).ToList();

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
                //if ((4 - count) < 0)
                //{
                //    throw new Exception(equipment.Name + " can not be selected");
                //}
                if (bookingViewModel.BookingEquipmentMappingViewModels != null)
                {
                    BookingEquipmentMappingViewModel bookingEquipmentMappingView = bookingViewModel.BookingEquipmentMappingViewModels.Where(x => x.EquipmentId == equipment.EquipmentId).FirstOrDefault();
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
