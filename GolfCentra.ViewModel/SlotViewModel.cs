using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.ViewModel
{
    public class SlotViewModel
    {
        public SlotViewModel()
        {
            BookingLeft = 0;
            SessionSlotViewModels = new List<SessionSlotViewModel>();
        }

        public int BookingLeft { get; set; }
        public List<SessionSlotViewModel> SessionSlotViewModels { get; set; }
    }
}
