using GolfCentra.ViewModel.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.Business.BusinessAdmin.Interface
{
    public interface IGolferService
    {
        List<GolferViewModel> GetAllGolferProfile();
        GolferViewModel GetGolferProfileByGolferId(long golferId);
        bool UpdateGolferProfile(GolferViewModel golferViewModel, long uniqueSessionId);
        bool SaveGolferDetails(GolferViewModel golferViewModel, long uniqueSessionId);
        List<GolferViewModel> GetGolferByAdvanceSearch(GolferViewModel golferViewModel);
        void BlockUnBlockOperation(long golferId, bool isBlocked, long uniqueSessionId);
        List<GolferViewModel> SearchGolferByMemberShipId(string memberShipId);
        bool ChangePassword(GolferViewModel golferViewModel, long uniqueSessionId);
        void Logout(string uniqueSessionId);
        List<SalutationViewModel> GetAllSalutation();
    }
}
