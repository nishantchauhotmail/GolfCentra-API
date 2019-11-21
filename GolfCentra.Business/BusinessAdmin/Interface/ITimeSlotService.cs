using GolfCentra.ViewModel.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.Business.BusinessAdmin.Interface
{
    public interface ITimeSlotService
    {
        List<SlotViewModel> GetAllTimeSlot();
        bool SaveTimeSlotDetails(TimeSpan slotTime, long uniqueSessionId);
        bool UpdateTimeSlotDetails(long id, TimeSpan slotTime, long uniqueSessionId);
        bool DeleteTimeSlotDetails(long id, long uniqueSessionId);
    }
}
