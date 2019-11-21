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
    public class SessionService : ISessionService
    {
        private readonly UnitOfWork _unitOfWork;

        public SessionService()
        {
            _unitOfWork = new UnitOfWork();
        }

        /// <summary>
        /// Get Session Details With Time Slot Details
        /// </summary>
        /// <param name="bookingTypeId"></param>
        /// <returns></returns>
        public List<SessionMasterViewModel> GetSessionDetailsWithTime(long bookingTypeId)
        {
            List<SessionMasterViewModel> sessionMasterViewModels = new List<SessionMasterViewModel>();
            List<Session> sessions = _unitOfWork.SessionRepository.GetMany(x => x.IsActive == true && x.BookingTypeId == bookingTypeId).ToList();

            List<Slot> slots = _unitOfWork.SlotRepository.GetMany(x => x.IsActive == true).ToList();
            foreach (var session in sessions)
            {
                SessionMasterViewModel sessionMasterViewModel = new SessionMasterViewModel()
                {
                    SessionId = session.SessionId,
                    SessionName = session.Name,
                    StartTime = session.StartTime,
                    EndTime = session.EndTime,
                    SlotTime = new List<TimeSpan>()

                };

                List<SlotSessionWise> slotSessionWises = _unitOfWork.SlotSessionWiseRepository.GetMany(x => x.IsActive == true && x.SessionId == session.SessionId && x.BookingTypeId == bookingTypeId).ToList();
                List<TimeSpan> timeSpans = slotSessionWises.Select(x => x.Slot.Time).ToList();
                if (timeSpans.Count != 0)
                {
                    sessionMasterViewModel.SlotTime = new List<TimeSpan>();
                    sessionMasterViewModel.SlotTime.AddRange(timeSpans);
                }
                sessionMasterViewModels.Add(sessionMasterViewModel);

                foreach (var slotSession in slotSessionWises)
                {
                    var itemToRemove = slots.SingleOrDefault(r => r.SlotId == slotSession.SlotId);
                    if (itemToRemove != null)
                        slots.Remove(itemToRemove);
                }
            }
            sessionMasterViewModels[0].ExtraSlotTime = slots.Select(x => x.Time).ToList();
            return sessionMasterViewModels;
        }

        /// <summary>
        /// Save Session NAME
        /// </summary>
        /// <param name="sessionMasterViewModel"></param>
        /// <returns></returns>
        public bool SaveSession(SessionMasterViewModel sessionMasterViewModel, long uniqueSessionId)
        {

            Session session = new Session()
            {
                Name = sessionMasterViewModel.SessionName,
                StartTime = sessionMasterViewModel.StartTime,
                EndTime = sessionMasterViewModel.EndTime,
                IsActive = true,
                CreatedOn = System.DateTime.UtcNow,
                BookingTypeId = sessionMasterViewModel.BookingTypeId

            };
            _unitOfWork.SessionRepository.Insert(session);
            _unitOfWork.Save();
            try
            {

                SessionActivityPageViewModel sessionActivityPageViewModel = new SessionActivityPageViewModel()
                {
                    ControllerName = "Create Session",
                    ActionName = "Create",
                    PerformOn = "",
                    LoginHistoryId = uniqueSessionId,

                    Info = "Created Session Details with id" + session.Name
                };
                new Common.AddSessionActivity().SaveSessionActivity(sessionActivityPageViewModel);

            }
            catch (Exception)
            {

            }
            return true;

        }

        /// <summary>
        /// Update Session Details
        /// </summary>
        /// <param name="sessionMasterViewModel"></param>
        /// <returns></returns>
        public bool UpdateSession(SessionMasterViewModel sessionMasterViewModel, long uniqueSessionId)
        {
            Session session = _unitOfWork.SessionRepository.Get(x => x.SessionId == sessionMasterViewModel.SessionId);

            session.Name = sessionMasterViewModel.SessionName;
            session.StartTime = sessionMasterViewModel.StartTime;
            session.EndTime = sessionMasterViewModel.EndTime;

            _unitOfWork.SessionRepository.Update(session);
            _unitOfWork.Save();
            try
            {

                SessionActivityPageViewModel sessionActivityPageViewModel = new SessionActivityPageViewModel()
                {
                    ControllerName = "Update Session",
                    ActionName = "Update",
                    PerformOn = "",
                    LoginHistoryId = uniqueSessionId,

                    Info = "Updated Session Details with id-" + session.SessionId
                };
                new Common.AddSessionActivity().SaveSessionActivity(sessionActivityPageViewModel);

            }
            catch (Exception)
            {

            }
            return true;

        }

        /// <summary>
        /// Save/Update Session Slot Mapping For BTT
        /// </summary>
        /// <param name="sessionMasterViewModels"></param>
        /// <returns></returns>
        public bool UpdateSlotSessionDetailTeeTime(List<SessionMasterViewModel> sessionMasterViewModels, long uniqueSessionId)
        {

            List<SlotSessionWise> slotSessionWises = _unitOfWork.SlotSessionWiseRepository.GetMany(x => x.BookingTypeId == (int)Core.Helper.Enum.EnumBookingType.BTT).ToList();
            foreach (SlotSessionWise slotsw in slotSessionWises)
            {
                slotsw.IsActive = false;
                _unitOfWork.SlotSessionWiseRepository.Update(slotsw);
                _unitOfWork.Save();
            }

            foreach (var sessionMasterViewModel in sessionMasterViewModels)
            {
                string[] items;
                List<TimeSpan> slots = new List<TimeSpan>();
                if (sessionMasterViewModel.TeeTimeSlot != null && sessionMasterViewModel.TeeTimeSlot != "")
                {
                    items = sessionMasterViewModel.TeeTimeSlot.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                }
                else
                {
                    items = new string[0];
                }

                foreach (var item in items)
                {

                    SlotSessionWise slotSessionWise = _unitOfWork.SlotSessionWiseRepository.Get(x => x.Slot.Time == TimeSpan.Parse(item) && x.SessionId == sessionMasterViewModel.SessionId && x.BookingTypeId == (int)Core.Helper.Enum.EnumBookingType.BTT);
                    if (slotSessionWise != null)
                    {
                        Slot slot = _unitOfWork.SlotRepository.Get(x => x.IsActive == true && x.Time == TimeSpan.Parse(item));
                        if (slot == null) { }
                        else
                        {
                            slotSessionWise.SlotId = slot.SlotId;
                            slotSessionWise.IsActive = true;
                            _unitOfWork.SlotSessionWiseRepository.Update(slotSessionWise);
                            _unitOfWork.Save();
                        }
                    }
                    else
                    {
                        Slot slot = _unitOfWork.SlotRepository.Get(x => x.IsActive == true && x.Time == TimeSpan.Parse(item));
                        if (slot == null) { }
                        else
                        {
                            SlotSessionWise sessionWise = new SlotSessionWise()
                            {
                                SlotId = slot.SlotId,
                                SessionId = sessionMasterViewModel.SessionId,
                                IsActive = true,
                                CreatedOn = System.DateTime.UtcNow,
                                BookingTypeId = (int)Core.Helper.Enum.EnumBookingType.BTT
                            };
                            _unitOfWork.SlotSessionWiseRepository.Insert(sessionWise);
                            _unitOfWork.Save();

                        }
                    }

                }

            }

            List<SlotSessionWise> slotSessionWises1 = _unitOfWork.SlotSessionWiseRepository.GetMany(x => x.BookingTypeId == (int)Core.Helper.Enum.EnumBookingType.BTT && x.IsActive == false).ToList();
            foreach (SlotSessionWise slotsw in slotSessionWises1)
            {
                List<BlockSlot> blockSlots = _unitOfWork.BlockSlotRepository.GetMany(x => x.SlotSessionWiseId == slotsw.SlotSessionWiseId).ToList();
                foreach (BlockSlot blockSlot in blockSlots)
                {
                    blockSlot.IsActive = false;
                    blockSlot.UpdatedOn = System.DateTime.UtcNow;
                    _unitOfWork.BlockSlotRepository.Update(blockSlot);
                    _unitOfWork.Save();

                }
            }

            try
            {

                SessionActivityPageViewModel sessionActivityPageViewModel = new SessionActivityPageViewModel()
                {
                    ControllerName = "BTT SessionTime Slot",
                    ActionName = "Update",
                    PerformOn = "",
                    LoginHistoryId = uniqueSessionId,

                    Info = "Updated BTT SessionTime Slot"
                };
                new Common.AddSessionActivity().SaveSessionActivity(sessionActivityPageViewModel);

            }
            catch (Exception)
            {

            }

            return true;
        }

        /// <summary>
        /// Save/update Session Slot Mapping For BDR
        /// </summary>
        /// <param name="sessionMasterViewModels"></param>
        /// <returns></returns>  
        public bool UpdateSlotSessionDetailDrivingRange(List<SessionMasterViewModel> sessionMasterViewModels, long uniqueSessionId)
        {
            List<SlotSessionWise> slotSessionWises = _unitOfWork.SlotSessionWiseRepository.GetMany(x => x.BookingTypeId == (int)Core.Helper.Enum.EnumBookingType.BDR).ToList();
            foreach (SlotSessionWise slotsw in slotSessionWises)
            {
                slotsw.IsActive = false;
                _unitOfWork.SlotSessionWiseRepository.Update(slotsw);
                _unitOfWork.Save();
            }
            foreach (var sessionMasterViewModel in sessionMasterViewModels)
            {
                string[] items;
                List<TimeSpan> slots = new List<TimeSpan>();
                if (sessionMasterViewModel.TeeTimeSlot != null && sessionMasterViewModel.TeeTimeSlot != "")
                {
                    items = sessionMasterViewModel.TeeTimeSlot.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                }
                else
                {
                    items = new string[0];
                }

                foreach (var item in items)
                {
                    SlotSessionWise slotSessionWise = _unitOfWork.SlotSessionWiseRepository.Get(x => x.Slot.Time == TimeSpan.Parse(item) && x.BookingTypeId == (int)Core.Helper.Enum.EnumBookingType.BDR && x.SessionId == sessionMasterViewModel.SessionId);
                    if (slotSessionWise != null)
                    {
                        slotSessionWise.IsActive = true;
                        _unitOfWork.SlotSessionWiseRepository.Update(slotSessionWise);
                        _unitOfWork.Save();
                    }
                    else
                    {
                        Slot slot = _unitOfWork.SlotRepository.Get(x => x.IsActive == true && x.Time == TimeSpan.Parse(item));
                        if (slot == null) { }
                        else
                        {
                            SlotSessionWise sessionWise = new SlotSessionWise()
                            {
                                SlotId = slot.SlotId,
                                SessionId = sessionMasterViewModel.SessionId,
                                IsActive = true,
                                CreatedOn = System.DateTime.UtcNow,
                                BookingTypeId = (int)Core.Helper.Enum.EnumBookingType.BDR
                            };
                            _unitOfWork.SlotSessionWiseRepository.Insert(sessionWise);
                            _unitOfWork.Save();

                        }
                    }

                }

            }

            return true;
        }

        /// <summary>
        /// Get All Active Slot By Session Id
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        public List<SlotViewModel> GetAllActiveSlotBySessionId(long sessionId)
        {
            List<SlotViewModel> slotViewModels = new List<SlotViewModel>();
            List<SlotSessionWise> slotSessionWises = _unitOfWork.SlotSessionWiseRepository.GetMany(x => x.IsActive == true && x.SessionId == sessionId && x.BookingTypeId == (int)Core.Helper.Enum.EnumBookingType.BTT).ToList();

            foreach (var item in slotSessionWises)
            {
                SlotViewModel slotViewModel = new SlotViewModel()
                {
                    SlotId = item.SlotId,
                    Time = item.Slot.Time
                };
                slotViewModels.Add(slotViewModel);
            }


            return slotViewModels;
        }

        /// <summary>
        /// Get All Session Detail
        /// </summary>
        /// <returns></returns>
        public List<SessionMasterViewModel> GetAllSession()
        {
            List<SessionMasterViewModel> sessionMasterViewModels = new List<SessionMasterViewModel>();
            List<Session> sessions = _unitOfWork.SessionRepository.GetMany(x => x.IsActive == true).ToList();
            foreach (var item in sessions)
            {
                SessionMasterViewModel sessionMasterViewModel = new SessionMasterViewModel()
                {
                    SessionId = item.SessionId,
                    //  SessionName = item.Name + "(" + item.StartTime + "-" + item.EndTime + ")",
                    SessionName = item.Name,
                    BookingTypeName = item.BookingType.ValueToShow,
                    BookingTypeId = item.BookingTypeId.GetValueOrDefault(),
                    StartTime = item.StartTime,
                    EndTime = item.EndTime,
                };
                sessionMasterViewModels.Add(sessionMasterViewModel);
            }
            return sessionMasterViewModels;
        }


        /// <summary>
        /// Get Slot Detail by Date,BookingTypeId and SessionId
        /// </summary>
        /// <param name="date"></param>
        /// <param name="bookingTypeId"></param>
        /// <param name="sessionId"></param>
        /// <returns></returns>

        public List<SlotViewModel> GetSlotDetailsByDateAndBookingTypeAndSessionId(DateTime date, long bookingTypeId, long sessionId, long? coursePairingId)
        {
            List<SlotViewModel> slotViewModels = new List<SlotViewModel>();
            DateTime currentTime = Core.Helper.DateHelper.ConvertSystemDate();
            Session session = _unitOfWork.SessionRepository.Get(x => x.IsActive == true && x.SessionId == sessionId);

            List<SlotSessionWise> slotSessionWises = _unitOfWork.SlotSessionWiseRepository.GetMany(x => x.SessionId == session.SessionId && x.IsActive == true && x.BookingTypeId == bookingTypeId).ToList();
            foreach (SlotSessionWise slotSessionWise in slotSessionWises)
            {
                List<Booking> bookings = new List<Booking>();
                SlotViewModel slotViewModel = new SlotViewModel();
                if (bookingTypeId == (int)EnumBookingType.BTT)
                {
                    long coursePairingId1 = 0;
                    if (coursePairingId == 1) { coursePairingId1 = 4;  }
                    if (coursePairingId == 2) { coursePairingId1 = 5;  }
                    if (coursePairingId == 3) { coursePairingId1 = 6;  }
                    if (coursePairingId == 4) { coursePairingId1 = 1; }
                    if (coursePairingId == 5) { coursePairingId1 = 2;  }
                    if (coursePairingId == 6) { coursePairingId1 = 3;  }
                    bookings = _unitOfWork.BookingRepository.GetMany(x => x.TeeOffDate == date && x.TeeOffSlot == slotSessionWise.Slot.Time.ToString("hh':'mm") && x.IsActive == true && x.BookingTypeId == bookingTypeId && (x.BookingStatusId == (int)Core.Helper.Enum.EnumBookingStatus.Confirm || (x.BookingStatusId == (int)EnumBookingStatus.Pending && x.CreatedOn.AddMinutes(15) > DateTime.UtcNow)) && (x.CoursePairingId == coursePairingId || x.CoursePairingId == coursePairingId1 )).ToList();
                }
                else { bookings = _unitOfWork.BookingRepository.GetMany(x => x.TeeOffDate == date && x.TeeOffSlot == slotSessionWise.Slot.Time.ToString("hh':'mm") && x.IsActive == true && x.BookingTypeId == bookingTypeId && (x.BookingStatusId == (int)Core.Helper.Enum.EnumBookingStatus.Confirm || (x.BookingStatusId == (int)EnumBookingStatus.Pending && x.CreatedOn.AddMinutes(15) > DateTime.UtcNow))).ToList(); }
                int noOfPlayer = bookings.Select(x => x.NoOfPlayer).Sum();

                if (bookings.Count() == 0)
                {
                    slotViewModel.SlotId = slotSessionWise.SlotId;
                    slotViewModel.Time = slotSessionWise.Slot.Time;
                    slotViewModel.PlayerLeftCount = (4 - noOfPlayer);
                    if ((slotSessionWise.Slot.Time >= currentTime.TimeOfDay && date.Month == currentTime.Month && date.Year == currentTime.Year && date.Day == currentTime.Day))
                    {
                        slotViewModels.Add(slotViewModel);
                    }
                    else { }
                    if (date > currentTime)
                        slotViewModels.Add(slotViewModel);
                }
                else
                {
                }

                // slotViewModels.Add(slotViewModel);
            }
            return slotViewModels;
        }


        public bool DeleteSession(SessionMasterViewModel sessionMasterViewModel, long uniqueSessionId)
        {

            Session session = _unitOfWork.SessionRepository.Get(x => x.SessionId == sessionMasterViewModel.SessionId);
            session.IsActive = false;
            _unitOfWork.SessionRepository.Update(session);
            _unitOfWork.Save();

            try
            {

                SessionActivityPageViewModel sessionActivityPageViewModel = new SessionActivityPageViewModel()
                {
                    ControllerName = "Session",
                    ActionName = "Save",
                    PerformOn = session.SessionId.ToString(),
                    LoginHistoryId = uniqueSessionId,

                    Info = "Deleted a Session with id- " + session.SessionId.ToString()
                };
                new Common.AddSessionActivity().SaveSessionActivity(sessionActivityPageViewModel);

            }
            catch (Exception)
            {

            }
            return true;

        }
    }
}
