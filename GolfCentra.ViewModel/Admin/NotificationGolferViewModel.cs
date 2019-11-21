using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.ViewModel.Admin
{
 public   class NotificationGolferViewModel
    {
        public string NotificationGolferId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string GolferIds { get; set; }
        public DateTime SentOn { get; set; }

        public ApiClientViewModel ApiClientViewModel { get; set; }
    }
}
