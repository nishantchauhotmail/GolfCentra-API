using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.ViewModel.Admin
{
    public class TeeSheetViewModel
    {
        public long BookingId { get; set; }
        public string CourseName { get; set; }
        public string TeeTime { get; set; }
        public string Player1 { get; set; }
        public string Player2 { get; set; }
        public string Player3 { get; set; }
        public string Player4 { get; set; }
        public string Status { get; set; }
        public string Date { get; set; }
        public string SessionSlotId { get; set; }
        public long SessionId { get; set; }
        public string HoleName { get; set; }

        //Search
        public DateTime SearchDate { get; set; }
        public long CoursePairingId { get; set; }
        public ApiClientViewModel ApiClientViewModel { get; set; }
    }
}
