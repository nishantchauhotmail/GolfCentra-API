using GolfCentra.Business.BusinessAdmin.Interface;
using GolfCentra.Core;
using GolfCentra.Core.DataBase;
using GolfCentra.ViewModel.Admin;
using GolfCentra.ViewModel.Admin.LoginActivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GolfCentra.Core.Helper.Enum;

namespace GolfCentra.Business.BusinessAdmin.Implementation
{
    public class TeeSheetService : ITeeSheetService
    {
        private readonly UnitOfWork _unitOfWork;

        public TeeSheetService()
        {
            _unitOfWork = new UnitOfWork();
        }

        public List<TeeSheetViewModel> GetTeeTimeSheet(TeeSheetViewModel teeSheetViewModel)
        {
            long coursePairingId = teeSheetViewModel.CoursePairingId;
            List<Tuple<TimeSpan, long, long>> Teetimes = new List<Tuple<TimeSpan, long, long>>();
            DateTime searchedDate = new DateTime(teeSheetViewModel.SearchDate.Year, teeSheetViewModel.SearchDate.Month, teeSheetViewModel.SearchDate.Day);
            long bookingTypeId = (int)Core.Helper.Enum.EnumBookingType.BTT;
            List<TeeSheetViewModel> teeSheetViewModels = new List<TeeSheetViewModel>();
            List<Session> sessions = _unitOfWork.SessionRepository.GetMany(x => x.IsActive == true && x.BookingTypeId == bookingTypeId).ToList();
            foreach (Session session in sessions)
            {
                List<SlotSessionWise> slotSessionWises = _unitOfWork.SlotSessionWiseRepository.GetMany(x => x.SessionId == session.SessionId && x.IsActive == true && x.BookingTypeId == bookingTypeId).ToList();
                foreach (SlotSessionWise slotSessionWise in slotSessionWises.OrderBy(x => x.Slot.Time))
                {
                    Teetimes.Add(new Tuple<TimeSpan, long, long>(slotSessionWise.Slot.Time, slotSessionWise.SlotSessionWiseId, session.SessionId));
                }
            }
            foreach (Tuple<TimeSpan, long, long> teeTime in Teetimes.OrderBy(x => x.Item1))
            {
                CoursePairing coursePairing = _unitOfWork.CoursePairingRepository.Get(x => x.CoursePairingId == coursePairingId);
                TeeSheetViewModel teeSheetView = new TeeSheetViewModel()
                {
                    SessionSlotId = teeTime.Item2.ToString(),
                    TeeTime = teeTime.Item1.ToString("hh':'mm"),
                    CourseName = coursePairing != null ? (coursePairing.CourseName1.Value) + (coursePairing.EndCourseNameId != null ? " - " + coursePairing.CourseName.Value : "") : "",
                    CoursePairingId = coursePairingId,
                    SessionId = Convert.ToInt64(teeTime.Item3)
                };
                long coursePairingId1 = 0;
                if (coursePairingId == 1) { coursePairingId1 = 4; }
                if (coursePairingId == 2) { coursePairingId1 = 5; }
                if (coursePairingId == 3) { coursePairingId1 = 6; }
                if (coursePairingId == 4) { coursePairingId1 = 1; }
                if (coursePairingId == 5) { coursePairingId1 = 2; }
                if (coursePairingId == 6) { coursePairingId1 = 3; }
                List<Booking> bookings = _unitOfWork.BookingRepository.GetMany(x => x.TeeOffDate == searchedDate && x.TeeOffSlot == teeTime.Item1.ToString("hh':'mm") && x.IsActive == true && x.BookingTypeId == bookingTypeId && (x.BookingStatusId == (int)EnumBookingStatus.Confirm && (x.CoursePairingId == coursePairingId || x.CoursePairingId == coursePairingId1))).ToList();
                if (bookings.Count() != 0)
                {
                    foreach (Booking booking in bookings)
                    {
                        if (bookings.Sum(x => x.NoOfPlayer) >= 4)
                        {
                            teeSheetView.Status = "Booked";
                        }
                        else
                        {
                            teeSheetView.Status = (4 - bookings.Sum(x => x.NoOfPlayer)).ToString() + " Place Left";
                        }
                        foreach (var item in booking.BookingPlayerDetails)
                        {
                            teeSheetView.Player1 = item.Player1;
                            teeSheetView.Player2 = item.Player2;
                            teeSheetView.Player3 = item.Player3;
                            teeSheetView.Player4 = item.Player4;

                        }
                        teeSheetView.BookingId = booking.BookingId;
                        teeSheetViewModels.Add(teeSheetView);
                    }

                }
                else
                {
                    teeSheetView.Status = "Open";
                    teeSheetViewModels.Add(teeSheetView);
                }

            }

            return teeSheetViewModels;
        }


        public bool UpdateBooking(BookingViewModel bookingViewModel,long uniqueSessionId)
        {
            Booking booking = _unitOfWork.BookingRepository.Get(x => x.BookingId == bookingViewModel.BookingId);
            booking.NoOfPlayer = SaveBookingPlayerDetail(bookingViewModel.BookingId, bookingViewModel.BookingPlayerDetailViewModel);
            _unitOfWork.BookingRepository.Update(booking);
            _unitOfWork.Save();
            try
            {

                SessionActivityPageViewModel sessionActivityPageViewModel = new SessionActivityPageViewModel()
                {
                    ControllerName = "Tee Sheet Update",
                    ActionName = "Save",
                    PerformOn = bookingViewModel.BookingId.ToString(),
                    LoginHistoryId = uniqueSessionId,

                    Info = "Updated a booking on Tee Sheet with booking id- " + bookingViewModel.BookingId.ToString()
                };
                new Common.AddSessionActivity().SaveSessionActivity(sessionActivityPageViewModel);

            }
            catch (Exception)
            {

            }
            return true;
        }

        private Int32 SaveBookingPlayerDetail(long bookingId, BookingPlayerDetailViewModel playerDetail)
        {
            Int32 noofPlayer = 1;
            if (playerDetail != null)
            {
                BookingPlayerDetail bookingPlayerDetail = _unitOfWork.BookingPlayerDetailRepository.Get(x => x.BookingId == bookingId);


                if (playerDetail.PlayerSerialNumber.Trim() == "Player 2")
                {
                    if (playerDetail.MemberShipId != null && playerDetail.MemberShipId != "")
                    {
                        bookingPlayerDetail.Player2 = playerDetail.MemberShipId;
                        noofPlayer++;
                    }
                    else
                    {
                        if (playerDetail.Name != "")
                        {
                            bookingPlayerDetail.Player2 = playerDetail.Name;
                            noofPlayer++;
                        }
                        else
                        {
                            bookingPlayerDetail.Player2 = "";
                        }
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
                        if (playerDetail.Name != "")
                        {
                            bookingPlayerDetail.Player3 = playerDetail.Name;
                            noofPlayer++;
                        }
                        else
                        {
                            bookingPlayerDetail.Player3 = "";
                        }
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
                        if (playerDetail.Name != "")
                        {
                            bookingPlayerDetail.Player4 = playerDetail.Name;
                            noofPlayer++;
                        }
                        else
                        {
                            bookingPlayerDetail.Player4 = "";
                        }
                    }
                }

                _unitOfWork.BookingPlayerDetailRepository.Update(bookingPlayerDetail);
                _unitOfWork.Save();
            }


            return noofPlayer;
        }

    }
}
