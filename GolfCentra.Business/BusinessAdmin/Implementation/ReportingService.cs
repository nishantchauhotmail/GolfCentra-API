using GolfCentra.Business.BusinessAdmin.Interface;
using GolfCentra.Core;
using GolfCentra.Core.DataBase;
using GolfCentra.ViewModel.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.Business.BusinessAdmin.Implementation
{
    public class ReportingService : IReportingService
    {
        private readonly UnitOfWork _unitOfWork;

        public ReportingService()
        {
            _unitOfWork = new UnitOfWork();
        }

        public List<ReportingViewModel> GetBookingDetailsBySearch(ReportingViewModel reportingViewModel)
        {
            List<string> times = new List<string>();
            List<Booking> bookings = new List<Booking>();
            DateTime startDate = new DateTime(reportingViewModel.StartDate.Year, reportingViewModel.StartDate.Month, reportingViewModel.StartDate.Day, 0, 0, 0);
            DateTime endDate = new DateTime(reportingViewModel.EndDate.Year, reportingViewModel.EndDate.Month, reportingViewModel.EndDate.Day, 23, 59, 59);


            bookings = _unitOfWork.BookingRepository.GetMany(x => x.IsActive == true && x.CreatedOn >= startDate && x.CreatedOn <= endDate).OrderBy(x => x.CreatedOn).ToList();
            if (reportingViewModel.CoursePairingId != 0)
            {
                bookings = bookings.Where(x => x.CoursePairingId == reportingViewModel.CoursePairingId).ToList();
            }
            if (reportingViewModel.MemberTypeId != 0)
            {
                bookings = bookings.Where(x => x.Golfer.MemberTypeId == reportingViewModel.MemberTypeId).ToList();
            }
            if (reportingViewModel.MembershipId != "" && reportingViewModel.MembershipId != null)
            {
                bookings = bookings.Where(x => x.Golfer.ClubMemberId == reportingViewModel.MembershipId).ToList();

            }
            if (reportingViewModel.BookingStatusId != 0)
            {
                bookings = bookings.Where(x => x.BookingStatusId == reportingViewModel.BookingStatusId).ToList();
            }
            if (reportingViewModel.SessionTypeId != 0)
            {
                List<SlotSessionWise> slotSessionWises = _unitOfWork.SlotSessionWiseRepository.GetMany(x => x.IsActive == true && x.SessionId == reportingViewModel.SessionTypeId).ToList();
                foreach(SlotSessionWise ss in slotSessionWises)
                {
                    TimeSpan sp = ss.Slot.Time;
                    string s = sp.ToString("hh':'mm");
                    times.Add(s);
                }
              //  times = slotSessionWises.Select(x => x.Slot.Time.ToString("HH:mm")).ToList();


            }

            List<ReportingViewModel> reportingViewModels = new List<ReportingViewModel>();

            foreach (var booking in bookings.OrderByDescending(x => x.TeeOffDate).ToList())
            {
                if (times != null && times.Count() != 0)
                {
                    if (!times.Contains(booking.TeeOffSlot))
                    {
                        continue;
                    }
                }


                int hole = 0;
                if (booking.BookingTypeId == (int)Core.Helper.Enum.EnumBookingType.BTT)
                    hole = booking.HoleType.Value;
                ReportingViewModel bookingViewModel = new ReportingViewModel()
                {
                    TeeOffDate = booking.TeeOffDate.ToShortDateString(),
                    BookingId = booking.BookingId,
                    BookingStatus = booking.BookingStatu.Value,
                    Time = booking.TeeOffSlot,
                    BallFee = booking.BallFee.GetValueOrDefault(),
                    CaddieFee = booking.CaddieFee.GetValueOrDefault(),
                    CartFee = booking.CartFee.GetValueOrDefault(),
                    Total = booking.Amount.GetValueOrDefault(),
                    Discount = booking.Discount.GetValueOrDefault(),
                    GreenFee = booking.GreenFee.GetValueOrDefault(),
                    OnSpot = booking.OnSpot.GetValueOrDefault(),
                    NoOfCaddie = booking.CaddieCount.GetValueOrDefault(),
                    NoOfCart = booking.CartCount.GetValueOrDefault(),

                    RangeFee = booking.RangeFee.GetValueOrDefault(),
                    CurrencyName = Core.Helper.Constants.Currency.CurrencyName,
                    Player1 = booking.Golfer.Name,
                    Paid = booking.PaidAmount.GetValueOrDefault(),
                    CoursePairingName = booking.CoursePairingId != null ? (booking.CoursePairing.CourseName1.Value) + (booking.CoursePairing.EndCourseNameId != null ? " - " + booking.CoursePairing.CourseName.Value : "") : "",

                    BookingDate= booking.BookingDate
                };
                foreach (var item in booking.BookingPlayerDetails)
                {
                    bookingViewModel.Player2 = item.Player2;
                    bookingViewModel.Player3 = item.Player3;
                    bookingViewModel.Player4 = item.Player4;
                }
                reportingViewModels.Add(bookingViewModel);
            }
            return reportingViewModels;
        }

        public List<ReportingViewModel> GetMoneyDetailsBySearch(ReportingViewModel reportingViewModel)
        {
            List<Booking> bookings = new List<Booking>();
            DateTime startDate = new DateTime(reportingViewModel.StartDate.Year, reportingViewModel.StartDate.Month, reportingViewModel.StartDate.Day, 0, 0, 0);
            DateTime endDate = new DateTime(reportingViewModel.EndDate.Year, reportingViewModel.EndDate.Month, reportingViewModel.EndDate.Day, 23, 59, 59);


            bookings = _unitOfWork.BookingRepository.GetMany(x => x.IsActive == true && x.CreatedOn >= startDate && x.CreatedOn <= endDate).OrderBy(x => x.CreatedOn).ToList();
            if (reportingViewModel.CoursePairingId != 0)
            {
                bookings = bookings.Where(x => x.CoursePairingId == reportingViewModel.CoursePairingId).ToList();
            }
            if (reportingViewModel.MemberTypeId != 0)
            {
                bookings = bookings.Where(x => x.Golfer.MemberTypeId == reportingViewModel.MemberTypeId).ToList();
            }

            if (reportingViewModel.BookingStatusId != 0)
            {
                bookings = bookings.Where(x => x.BookingStatusId == reportingViewModel.BookingStatusId).ToList();
            }
            if (reportingViewModel.PackageId != 0)
            {
                bookings = bookings.Where(x => x.PromotionId == reportingViewModel.PackageId).ToList();
            }

            List<ReportingViewModel> reportingViewModels = new List<ReportingViewModel>();

            foreach (var booking in bookings.OrderByDescending(x => x.TeeOffDate).ToList())
            {
                int hole = 0;
                if (booking.BookingTypeId == (int)Core.Helper.Enum.EnumBookingType.BTT)
                    hole = booking.HoleType.Value;
                ReportingViewModel bookingViewModel = new ReportingViewModel()
                {
                    TeeOffDate = booking.TeeOffDate.ToShortDateString(),
                    BookingId = booking.BookingId,
                    BookingStatus = booking.BookingStatu.Value,
                    Time = booking.TeeOffSlot,
                    BallFee = booking.BallFee.GetValueOrDefault(),
                    CaddieFee = booking.CaddieFee.GetValueOrDefault(),
                    CartFee = booking.CartFee.GetValueOrDefault(),
                    Total = booking.Amount.GetValueOrDefault(),
                    Discount = booking.Discount.GetValueOrDefault(),
                    GreenFee = booking.GreenFee.GetValueOrDefault(),
                    OnSpot = booking.OnSpot.GetValueOrDefault(),
                    NoOfCaddie = booking.CaddieCount.GetValueOrDefault(),
                    NoOfCart = booking.CartCount.GetValueOrDefault(),
                    NoOfPlayer= booking.NoOfPlayer,
                    RangeFee = booking.RangeFee.GetValueOrDefault(),
                    CurrencyName = Core.Helper.Constants.Currency.CurrencyName,
                    Player1 = booking.Golfer.Name,
                    Paid = booking.PaidAmount.GetValueOrDefault(),
                    CoursePairingName = booking.CoursePairingId != null ? (booking.CoursePairing.CourseName1.Value) + (booking.CoursePairing.EndCourseNameId != null ? " - " + booking.CoursePairing.CourseName.Value : "") : "",
                    BookingDate = booking.BookingDate

                };

                reportingViewModels.Add(bookingViewModel);
            }
            return reportingViewModels;
        }
    }
}
