using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.ViewModel.Admin.LoginActivity
{
    public class SessionDetailViewModel
    {
        public long LoginHistoryId { get; set; }
        public long EmployeeId { get; set; }
        public string UserName { get; set; }
        public Nullable<System.DateTime> LoggedIn { get; set; }
        public Nullable<System.DateTime> LoggedOutAt { get; set; }
        public Nullable<long> PlatformTypeId { get; set; }
        public List<SessionActivityViewModel> SessionActivityViewModels { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public ApiClientViewModel ApiClientViewModel { get; set; }
    }
}
