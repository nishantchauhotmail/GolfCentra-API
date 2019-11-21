using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.ViewModel.Admin
{
    /// <summary>
    /// Properties For SessionMaster's Operations Viz. Add, Edit, Delete, List And Etc.
    /// </summary>
    public class SessionMasterViewModel
    {
        public long BookingTypeId { get; set; }
        public long SessionId { get; set; }
        public string SessionName { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public List<TimeSpan> SlotTime { get; set; }
        public List<TimeSpan> ExtraSlotTime { get; set; }
        public ApiClientViewModel ApiClientViewModel { get; set; }
        public string TeeTimeSlot { get; set; }
        public string DrivingSlot { get; set; }
        public string BookingTypeName { get; set; }
    }
}
