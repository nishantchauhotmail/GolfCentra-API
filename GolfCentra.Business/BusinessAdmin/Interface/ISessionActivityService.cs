using GolfCentra.ViewModel.Admin.LoginActivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.Business.BusinessAdmin.Interface
{
    public interface ISessionActivityService
    {
        void AddSessionActivity(SessionActivityPageViewModel sessionActivityPageViewModel, string uniqueSessionId);
        void AddSessionActivityPage(List<SessionActivityPageViewModel> sessionActivityPageViewModels);
        List<SessionDetailViewModel> GetSessionActivityBySearch(SessionDetailViewModel sessionDetailViewModel);
    }
}
