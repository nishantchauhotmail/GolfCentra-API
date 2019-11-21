using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.ViewModel.Admin.LoginActivity
{
    public class SessionActivityViewModel
    {
        public long SessionActivityPageId { get; set; }
        public Nullable<System.DateTime> ArriveAt { get; set; }
        public string PerformedOn { get; set; }

        public System.DateTime CreatedOn { get; set; }
        public string DisplayName { get; set; }
   
        public long LoginHistoryId { get; set; }
        public string Info { get; set; }
    }
}
