using GolfCentra.ViewModel.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.Business.BusinessAdmin.Interface
{
    public interface ISessionService
    {
        List<SessionMasterViewModel> GetSessionDetailsWithTime(long bookingTypeId);
        bool SaveSession(SessionMasterViewModel sessionMasterViewModel, long uniqueSessionId);
        bool UpdateSession(SessionMasterViewModel sessionMasterViewModel, long uniqueSessionId);
        bool UpdateSlotSessionDetailTeeTime(List<SessionMasterViewModel> sessionMasterViewModels, long uniqueSessionId);
        bool UpdateSlotSessionDetailDrivingRange(List<SessionMasterViewModel> sessionMasterViewModels, long uniqueSessionId);
        List<SlotViewModel> GetAllActiveSlotBySessionId(long sessionId);
        List<SessionMasterViewModel> GetAllSession();
        List<SlotViewModel> GetSlotDetailsByDateAndBookingTypeAndSessionId(DateTime date, long bookingTypeId, long sessionId, long? coursePairingId);
        bool DeleteSession(SessionMasterViewModel sessionMasterViewModel, long uniqueSessionId);
    }
}
