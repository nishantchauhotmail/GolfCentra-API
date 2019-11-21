using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.ViewModel
{
    /// <summary>
    /// Properties For SlotDetail's Operations Viz. Add, Edit, Delete, List And Etc.
    /// </summary>
    public class SlotDetailsViewModel
    {

        public SlotDetailsViewModel()
        {
            SlotTime = string.Empty;
            SlotId = 0;
            IsAvailable = false;
            MinPlayerCount = 0;
            PlayerLeftCount = 0;
        }
        public string SlotTime { get; set; }
        public long SlotId { get; set; }
        public bool IsAvailable { get; set; }
        public int MinPlayerCount { get; set; }
        public int PlayerLeftCount { get; set; }
    }
}
