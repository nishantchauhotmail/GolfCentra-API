using GolfCentra.ViewModel.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.Business.BusinessAdmin.Interface
{
    public interface ITeeSheetService
    {
        List<TeeSheetViewModel> GetTeeTimeSheet(TeeSheetViewModel teeSheetViewModel);
        bool UpdateBooking(BookingViewModel bookingViewModel,long uniqueSessionId);
    }
}
