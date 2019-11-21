using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.ViewModel
{
    public class BookingConditionViewModel
    {
        public BookingConditionViewModel()
        {
            NoOfAdvanceDay = 0;
            DayOffIds =new long[0];

        }

        public long[] DayOffIds { get; set; }
        public int NoOfAdvanceDay { get; set; }
    }
}
