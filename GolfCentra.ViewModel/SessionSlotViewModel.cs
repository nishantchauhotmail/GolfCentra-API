using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.ViewModel
{
    /// <summary>
    /// Properties For SessionSlot's Operations Viz. Add, Edit, Delete, List And Etc.
    /// </summary>
    public class SessionSlotViewModel
    {
        public SessionSlotViewModel()
        {
            SessionName = string.Empty;
            SlotDetailsViewModelList = new List<SlotDetailsViewModel>();
        }
        public String SessionName { get; set; }
        public List<SlotDetailsViewModel> SlotDetailsViewModelList { get; set; }
    }
}
