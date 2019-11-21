using GolfCentra.Business.Business.Interface;
using GolfCentra.Core;
using GolfCentra.Core.DataBase;
using GolfCentra.Core.Helper;
using GolfCentra.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.Business.Business.Implementation
{
    public class BookingDetailService : IBookingDetailService
    {

        private readonly UnitOfWork _unitOfWork;

        public BookingDetailService()
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
            Golfer golfer = _unitOfWork.GolferRepository.Get(x => x.GolferId == golferId);
            List<Booking> bookings = new List<Booking>();
            List<BookingViewModel> bookingViewModels = new List<BookingViewModel>();
            DateTime currentDateTime = Core.Helper.DateHelper.ConvertSystemDate();
            DateTime startDate = new DateTime(currentDateTime.Year, currentDateTime.Month, currentDateTime.Day, 0, 0, 0);
            DateTime endDate = new DateTime(currentDateTime.Year, currentDateTime.Month, currentDateTime.Day, 23, 59, 59);
            if (bookingSearchTypeId == (int)Core.Helper.Enum.EnumSearchBooking.Cancelled)
            {
                List<Booking> otherBooking = GetAllOtherBookingDetails(golfer.ClubMemberId, bookingSearchTypeId);
                bookings = _unitOfWork.BookingRepository.GetMany(x => x.GolferId == golferId && x.BookingStatusId == (int)Core.Helper.Enum.EnumBookingStatus.Cancelled && x.IsActive == true).OrderByDescending(x => x.UpdatedOn).ToList();
                bookings.AddRange(otherBooking);
                bookings.OrderByDescending(x => x.UpdatedOn).ToList();
            }
            else
            {
                if (bookingSearchTypeId == (int)Core.Helper.Enum.EnumSearchBooking.Completed)
                {
                    List<Booking> otherBooking = GetAllOtherBookingDetails(golfer.ClubMemberId, bookingSearchTypeId);
                    bookings = _unitOfWork.BookingRepository.GetMany(x => x.GolferId == golferId && x.TeeOffDate < currentDateTime && TimeSpan.Parse(x.TeeOffSlot) <= TimeSpan.Parse(currentDateTime.Hour.ToString() + ":" + currentDateTime.Minute.ToString()) && x.BookingStatusId == (int)Core.Helper.Enum.EnumBookingStatus.Confirm && x.IsActive == true).OrderByDescending(x => x.TeeOffDate).ToList();
                    bookings.AddRange(otherBooking);
                    bookings.OrderByDescending(x => x.TeeOffDate).ToList();
                }
                else if (bookingSearchTypeId == (int)Core.Helper.Enum.EnumSearchBooking.Upcoming)
                {
                    DateTime nextDay = currentDateTime.AddDays(1);
                    DateTime date = new DateTime(nextDay.Year, nextDay.Month, nextDay.Day, 0, 0, 0);
                    bookings = _unitOfWork.BookingRepository.GetMany(x => x.GolferId == golferId && x.TeeOffDate >= date && x.BookingStatusId == (int)Core.Helper.Enum.EnumBookingStatus.Confirm && x.IsActive == true).OrderBy(x => x.TeeOffDate).ToList();
                    List<Booking> otherBooking = GetAllOtherBookingDetails(golfer.ClubMemberId, bookingSearchTypeId);
                    bookings.AddRange(otherBooking);
                    bookings.OrderByDescending(x => x.TeeOffDate).ToList();
                }
                else if (bookingSearchTypeId == (int)Core.Helper.Enum.EnumSearchBooking.Today)
                {
                    List<Booking> otherBooking = GetAllOtherBookingDetails(golfer.ClubMemberId, bookingSearchTypeId);
                    bookings = _unitOfWork.BookingRepository.GetMany(x => x.GolferId == golferId && x.TeeOffDate >= startDate && TimeSpan.Parse(x.TeeOffSlot) > TimeSpan.Parse(currentDateTime.Hour.ToString() + ":" + currentDateTime.Minute.ToString()) && x.TeeOffDate <= endDate && x.BookingStatusId == (int)Core.Helper.Enum.EnumBookingStatus.Confirm && x.IsActive == true).OrderBy(x => x.TeeOffSlot).ToList();
                    bookings.AddRange(otherBooking);
                    bookings.OrderByDescending(x => x.TeeOffSlot).ToList();
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
                    BookedBy = booking.Golfer.Name + " " + booking.Golfer.LastName,
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
        public BookingViewModel GetBookingDetailsByBookingId(string bookingId, long golferId)
        {
            long id = Convert.ToInt64(Core.Helper.Crypto.DecryptStringAES(bookingId));
            BookingViewModel bookingViewModel = new BookingViewModel();
            Booking booking = _unitOfWork.BookingRepository.Get(x => x.BookingId == id && x.IsActive == true);

            if (booking != null)
            {
                CoursePairing coursePairing = _unitOfWork.CoursePairingRepository.Get(x => x.CoursePairingId == booking.CoursePairingId);

                bookingViewModel.BookingId = booking.BookingId;
                bookingViewModel.Time = booking.TeeOffSlot;
                bookingViewModel.Date = booking.TeeOffDate.ToShortDateString();
                bookingViewModel.NoOfPlayer = booking.NoOfPlayer;
                bookingViewModel.TotalAmount = booking.PaidAmount.GetValueOrDefault();
                bookingViewModel.CurrencyName = Core.Helper.Constants.Currency.CurrencyName;
                bookingViewModel.BookingTypeId = booking.BookingTypeId;
                if (booking.BookingTypeId == (int)Core.Helper.Enum.EnumBookingType.BTT)
                    bookingViewModel.NoOfHole = _unitOfWork.HoleTypeRepository.Get(x => x.HoleTypeId == booking.HoleTypeId && x.IsActive == true).Value;
                bookingViewModel.NoOfBalls = booking.NoOfBalls.GetValueOrDefault();
                bookingViewModel.PaymentGatewayBookingId = booking.PaymentGatewayBookingId;
                bookingViewModel.CoursePairingId = booking.CoursePairingId.GetValueOrDefault();
                bookingViewModel.CoursePairingName = coursePairing != null ? (coursePairing.CourseName1.Value) + (coursePairing.EndCourseNameId != null ? " - " + coursePairing.CourseName.Value : "") : "";
                bookingViewModel.BookedBy = booking.Golfer.Name + " " + booking.Golfer.LastName;
                bookingViewModel.ENB = Core.Helper.Crypto.EncryptStringAES(booking.BookingId.ToString());
                bookingViewModel.BookingStatus = booking.BookingStatu.Value;
                if (booking.GolferId == golferId && booking.BookingStatusId == (int)Core.Helper.Enum.EnumBookingStatus.Confirm)
                {
                    DateTime date = Core.Helper.DateHelper.ConvertSystemDateToCurrent(System.DateTime.UtcNow);
                    if (new DateTime(date.Year, date.Month, date.Day, 12, 00, 00) <= booking.TeeOffDate.AddDays(-1))
                    {
                        bookingViewModel.CanCancelBooking = true;
                    }
                    else
                    {
                        bookingViewModel.CanCancelBooking = false;
                    }
                    if (booking.ScoreId == null)
                    {
                        bookingViewModel.CanSubmitScore = true;
                    }
                    else
                    {
                        bookingViewModel.CanSubmitScore = false;
                    }
                }
                else
                {
                    bookingViewModel.CanCancelBooking = false;
                    bookingViewModel.CanSubmitScore = CanPostScore(booking, golferId);
                }


                foreach (var item in booking.BookingPlayerDetails)
                {

                    if (item.Player1 != null && item.Player1 != "")
                    {
                        BookingPlayerDetailViewModel bookingPlayerDetailViewModel = new BookingPlayerDetailViewModel();

                        bookingPlayerDetailViewModel.PlayerDetails = item.Player1;
                        bookingPlayerDetailViewModel.PlayerSerialNumber = "Player 1";
                        bookingViewModel.BookingPlayerDetailViewModels.Add(bookingPlayerDetailViewModel);
                    }
                    if (item.Player2 != null && item.Player2 != "")
                    {
                        BookingPlayerDetailViewModel bookingPlayerDetailViewModel = new BookingPlayerDetailViewModel();

                        bookingPlayerDetailViewModel.PlayerDetails = item.Player2;
                        bookingPlayerDetailViewModel.PlayerSerialNumber = "Player 2";
                        bookingViewModel.BookingPlayerDetailViewModels.Add(bookingPlayerDetailViewModel);
                    }
                    if (item.Player3 != null && item.Player3 != "")
                    {
                        BookingPlayerDetailViewModel bookingPlayerDetailViewModel = new BookingPlayerDetailViewModel();

                        bookingPlayerDetailViewModel.PlayerDetails = item.Player3;
                        bookingPlayerDetailViewModel.PlayerSerialNumber = "Player 3";
                        bookingViewModel.BookingPlayerDetailViewModels.Add(bookingPlayerDetailViewModel);
                    }
                    if (item.Player4 != null && item.Player4 != "")
                    {
                        BookingPlayerDetailViewModel bookingPlayerDetailViewModel = new BookingPlayerDetailViewModel();

                        bookingPlayerDetailViewModel.PlayerDetails = item.Player4;
                        bookingPlayerDetailViewModel.PlayerSerialNumber = "Player 4";
                        bookingViewModel.BookingPlayerDetailViewModels.Add(bookingPlayerDetailViewModel);
                    }
                };
            }
            return bookingViewModel;
        }

        /// <summary>
        /// Cancel Booking  By Booking Id
        /// </summary>
        /// <param name="bookingId"></param>
        /// <param name="golferId"></param>
        /// <returns></returns>
        public bool CancelBookingByGolfer(string bookingId, long golferId)
        {
            long id = Convert.ToInt64(Core.Helper.Crypto.DecryptStringAES(bookingId));
            Booking booking = _unitOfWork.BookingRepository.Get(x => x.GolferId == golferId && x.BookingId == id && x.IsActive == true);

            if (booking != null)
            {
                DateTime date = Core.Helper.DateHelper.ConvertSystemDateToCurrent(System.DateTime.UtcNow);
                if (new DateTime(date.Year, date.Month, date.Day, 12, 00, 00) <= booking.TeeOffDate.AddDays(-1))
                {
                    booking.BookingStatusId = (int)Core.Helper.Enum.EnumBookingStatus.Cancelled;
                    booking.UpdatedOn = System.DateTime.UtcNow;
                    _unitOfWork.BookingRepository.Update(booking);
                    _unitOfWork.Save();

                    string emailBody = "";
                    if (booking.BookingTypeId == (int)Core.Helper.Enum.EnumBookingType.BTT)
                    {

                        emailBody = "Dear Member,<br/><br/>Your booking is cancelled.<br/><br/>We look forward to welcoming you back soon!<br/><br/>Warm regards,<br/>Classic Golf &Country Club";

                        //string coursePairingName = booking.CoursePairingId != null ? (booking.CoursePairing.CourseName1.Value) + (booking.CoursePairing.EndCourseNameId != null ? " - " + booking.CoursePairing.CourseName.Value : "") : "";
                        //emailBody = "Dear Member, your tee time booking is cancelled as follows - -<br/> Booking ID - " + booking.PaymentGatewayBookingId + "<br/> Date - " + booking.TeeOffDate.ToString("dd/MM/yyyy") + "<br/> Time - " + DateTime.Parse(booking.TeeOffSlot).ToString("hh:mm tt", CultureInfo.GetCultureInfo("en-US")) + "<br/> No. of Holes - " + booking.HoleType.Value + "<br/> Course Name - " + coursePairingName + "<br/>";
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

                        emailBody = "Dear Member, your driveing range booking is cancelled as follows - -<br/> Booking ID - " + booking.PaymentGatewayBookingId + "<br/> Date - " + booking.TeeOffDate.ToString("dd/MM/yyyy") + "<br/> Time - " + DateTime.Parse(booking.TeeOffSlot).ToString("hh:mm tt", CultureInfo.GetCultureInfo("en-US")) + "<br/>";

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

        private List<Booking> GetAllOtherBookingDetails(string MId, long bookingSearchTypeId)
        {
            List<Booking> bookings = new List<Booking>();
            DateTime currentDateTime = Core.Helper.DateHelper.ConvertSystemDate();
            DateTime startDate = new DateTime(currentDateTime.Year, currentDateTime.Month, currentDateTime.Day, 0, 0, 0);
            DateTime endDate = new DateTime(currentDateTime.Year, currentDateTime.Month, currentDateTime.Day, 23, 59, 59);

            List<BookingPlayerDetail> bookingPlayerDetails = _unitOfWork.BookingPlayerDetailRepository.GetMany(x => (x.Player2 == MId || x.Player3 == MId || x.Player4 == MId) && x.IsActive == true).ToList();

            foreach (var booking in bookingPlayerDetails)
            {
                if (bookingSearchTypeId == (int)Core.Helper.Enum.EnumSearchBooking.Cancelled)
                {
                    bookings.AddRange(_unitOfWork.BookingRepository.GetMany(x => x.BookingId == booking.BookingId && x.BookingStatusId == (int)Core.Helper.Enum.EnumBookingStatus.Cancelled && x.IsActive == true).OrderByDescending(x => x.UpdatedOn).ToList());
                }
                else
                {
                    if (bookingSearchTypeId == (int)Core.Helper.Enum.EnumSearchBooking.Completed)
                    {
                        bookings.AddRange(_unitOfWork.BookingRepository.GetMany(x => x.BookingId == booking.BookingId && x.TeeOffDate < currentDateTime && TimeSpan.Parse(x.TeeOffSlot) <= TimeSpan.Parse(currentDateTime.Hour.ToString() + ":" + currentDateTime.Minute.ToString()) && x.BookingStatusId == (int)Core.Helper.Enum.EnumBookingStatus.Confirm && x.IsActive == true).OrderByDescending(x => x.TeeOffDate).ToList());
                    }
                    else if (bookingSearchTypeId == (int)Core.Helper.Enum.EnumSearchBooking.Upcoming)
                    {
                        DateTime nextDay = currentDateTime.AddDays(1);
                        DateTime date = new DateTime(nextDay.Year, nextDay.Month, nextDay.Day, 0, 0, 0);
                        bookings.AddRange(_unitOfWork.BookingRepository.GetMany(x => x.BookingId == booking.BookingId && x.TeeOffDate >= date && x.BookingStatusId == (int)Core.Helper.Enum.EnumBookingStatus.Confirm && x.IsActive == true).OrderBy(x => x.TeeOffDate).ToList());

                    }
                    else if (bookingSearchTypeId == (int)Core.Helper.Enum.EnumSearchBooking.Today)
                    {

                        bookings.AddRange(_unitOfWork.BookingRepository.GetMany(x => x.BookingId == booking.BookingId && x.TeeOffDate >= startDate && TimeSpan.Parse(x.TeeOffSlot) > TimeSpan.Parse(currentDateTime.Hour.ToString() + ":" + currentDateTime.Minute.ToString()) && x.TeeOffDate <= endDate && x.BookingStatusId == (int)Core.Helper.Enum.EnumBookingStatus.Confirm && x.IsActive == true).OrderBy(x => x.TeeOffSlot).ToList());
                    }
                }


            }
            return bookings;
        }

        private bool CanPostScore(Booking booking, long golferId)
        {
            Golfer golfer = _unitOfWork.GolferRepository.Get(x => x.GolferId == golferId);

            bool canPostScore = false;

            foreach (var item in booking.BookingPlayerDetails)
            {
                if (item.Player2 == golfer.ClubMemberId)
                {
                    if (item.Player2ScoreId == null)
                    {
                        canPostScore = true;
                    }

                }
                if (item.Player3 == golfer.ClubMemberId)
                {
                    if (item.Player3ScoreId == null)
                    {
                        canPostScore = true;
                    }
                }
                if (item.Player4 == golfer.ClubMemberId)
                {
                    if (item.Player4ScoreId == null)
                    {
                        canPostScore = true;
                    }
                }
            }

            return canPostScore;


        }

        public List<BookingViewModel> GetBookingsByBookingTypeId(long golferId, long bookingTypeId)
        {
            Golfer golfer = _unitOfWork.GolferRepository.Get(x => x.GolferId == golferId);
            List<Booking> bookings = new List<Booking>();
            List<BookingViewModel> bookingViewModels = new List<BookingViewModel>();
            DateTime currentDateTime = Core.Helper.DateHelper.ConvertSystemDate();
            DateTime startDate = new DateTime(currentDateTime.Year, currentDateTime.Month, currentDateTime.Day, 0, 0, 0);
            DateTime endDate = new DateTime(currentDateTime.Year, currentDateTime.Month, currentDateTime.Day, 23, 59, 59);

            List<Booking> otherBooking = GetAllOtherBookingDetails(golfer.ClubMemberId, (int)Core.Helper.Enum.EnumBookingStatus.Cancelled);
            bookings.AddRange(_unitOfWork.BookingRepository.GetMany(x => x.GolferId == golferId && x.BookingStatusId == (int)Core.Helper.Enum.EnumBookingStatus.Cancelled && x.IsActive == true).OrderByDescending(x => x.UpdatedOn).ToList());
            bookings.AddRange(otherBooking);
            bookings.OrderByDescending(x => x.UpdatedOn).ToList();



            List<Booking> otherBooking1 = GetAllOtherBookingDetails(golfer.ClubMemberId, (int)Core.Helper.Enum.EnumBookingStatus.Confirm);
            bookings.AddRange(_unitOfWork.BookingRepository.GetMany(x => x.GolferId == golferId && x.TeeOffDate < currentDateTime && TimeSpan.Parse(x.TeeOffSlot) <= TimeSpan.Parse(currentDateTime.Hour.ToString() + ":" + currentDateTime.Minute.ToString()) && x.BookingStatusId == (int)Core.Helper.Enum.EnumBookingStatus.Confirm && x.IsActive == true).OrderByDescending(x => x.TeeOffDate).ToList());
            bookings.AddRange(otherBooking1);
            bookings.OrderByDescending(x => x.TeeOffDate).ToList();

            DateTime nextDay = currentDateTime.AddDays(1);
            DateTime date = new DateTime(nextDay.Year, nextDay.Month, nextDay.Day, 0, 0, 0);
            bookings.AddRange(_unitOfWork.BookingRepository.GetMany(x => x.GolferId == golferId && x.TeeOffDate >= date && x.BookingStatusId == (int)Core.Helper.Enum.EnumBookingStatus.Confirm && x.IsActive == true).OrderBy(x => x.TeeOffDate).ToList());
            List<Booking> otherBooking2 = GetAllOtherBookingDetails(golfer.ClubMemberId, (int)Core.Helper.Enum.EnumBookingStatus.Confirm);
            bookings.AddRange(otherBooking2);
            bookings.OrderByDescending(x => x.TeeOffDate).ToList();

            List<Booking> otherBooking3 = GetAllOtherBookingDetails(golfer.ClubMemberId, (int)Core.Helper.Enum.EnumBookingStatus.Confirm);
            bookings.AddRange(_unitOfWork.BookingRepository.GetMany(x => x.GolferId == golferId && x.TeeOffDate >= startDate && TimeSpan.Parse(x.TeeOffSlot) > TimeSpan.Parse(currentDateTime.Hour.ToString() + ":" + currentDateTime.Minute.ToString()) && x.TeeOffDate <= endDate && x.BookingStatusId == (int)Core.Helper.Enum.EnumBookingStatus.Confirm && x.IsActive == true).OrderBy(x => x.TeeOffSlot).ToList());
            bookings.AddRange(otherBooking3);
            bookings.OrderByDescending(x => x.TeeOffSlot).ToList();



            foreach (Booking booking in bookings.Where(x => x.BookingTypeId == bookingTypeId).OrderByDescending(x => x.TeeOffDate))
            {
                BookingViewModel bookingViewModel = new BookingViewModel()
                {
                    BookingId = booking.BookingId,
                    Time = booking.TeeOffSlot,
                    Date = booking.TeeOffDate.ToShortDateString(),
                    NoOfPlayer = booking.NoOfPlayer,
                    PaymentGatewayBookingId = booking.PaymentGatewayBookingId,
                    BookedBy = booking.Golfer.Name + " " + booking.Golfer.LastName,
                    IsUpcomingBookiong = false,
                    BookingStatusId = booking.BookingStatusId,
                    ENB = Core.Helper.Crypto.EncryptStringAES(booking.BookingId.ToString())
                };
                if (booking.TeeOffDate >= date)
                {
                    bookingViewModel.IsUpcomingBookiong = true;
                }
                bookingViewModels.Add(bookingViewModel);
            };

            return bookingViewModels;
        }

    }
}
