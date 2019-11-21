using GolfCentra.ViewModel.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.Business.BusinessAdmin.Interface
{
    public interface ISlotReservationService
    {
        List<BlockSlotRangeViewModel> GetSlotBlockRangeDetails();
        List<BlockSlotViewModel> GetAllSlotDetails(DateTime startDate, DateTime endDate);
        void SaveBlockSlotRangeDetails(BlockSlotRangeViewModel blockSlotRangeViewModel, long uniqueSessionId);
        bool DeleteBlockSlotRangeDetails(long id, long uniqueSessionId);
    }
}
