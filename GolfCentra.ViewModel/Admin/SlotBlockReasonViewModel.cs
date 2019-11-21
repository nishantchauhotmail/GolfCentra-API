using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.ViewModel.Admin
{
   public class SlotBlockReasonViewModel
    {
        public long SlotBlockReasonId { get; set; }
        public string Value { get; set; }
        public ApiClientViewModel ApiClientViewModel { get; set; }
    }
}
