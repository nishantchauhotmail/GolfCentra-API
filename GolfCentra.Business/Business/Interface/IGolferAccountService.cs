using GolfCentra.Core.DataBase;
using GolfCentra.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.Business.Business.Interface
{
    public interface IGolferAccountService
    {
        List<PhoneCodeViewModel> GetPhoneCodeList();
        Golfer AddGolfer(CommonViewModel golferViewModel);
        GolferViewModel VerifyGolferAccount(string code, long golferId, int platformId);
        bool ResendVerficationCode(long golferId);
        GolferViewModel LogInUser(GolferViewModel golfer);
        long GetGolferIdFromLoginHistoryById(string loginHistoryId);
        LogInHistory GetLoginHistoryById(string loginHistoryId);
        bool ChangePassword(long golferId, string oldPassword, string newPassword, string uid);
        GolferViewModel GetGolferDetailByGolferId(long golferId);
        bool ForgetPassword(string clubMemberId);
        bool Logout(string uniqueSessionId);
        // GolferViewModel LoginMember(GolferViewModel golfer);
        List<SalutationViewModel> GetAllSalutation();
        GolferViewModel UpdateGolferProfile(GolferViewModel golferViewModel);
        List<GenderTypeViewModel> GetAllGenderType();
        bool ChangePassword(string ss, string newPassword);
        bool CheckPasswordLinkStatus(string ss);
    }
}
