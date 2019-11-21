using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.ViewModel.Admin
{
    /// <summary>
    /// Properties For Slot's Operations Viz. Add, Edit, Delete, List And Etc.
    /// </summary>
    public class SlotViewModel
    {
        public long SlotId { get; set; }
        public System.TimeSpan Time { get; set; }
        public ApiClientViewModel ApiClientViewModel { get; set; }
        public long SessionId { get; set; }
        public long BookingTypeId { get; set; }
        public DateTime Date { get; set; }
        public long? CoursePairingId { get; set; }
        public int PlayerLeftCount { get; set; }
    }
}
