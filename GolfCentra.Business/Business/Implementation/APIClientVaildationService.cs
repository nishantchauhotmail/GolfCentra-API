using GolfCentra.Business.Business.Interface;
using GolfCentra.Core;
using GolfCentra.Core.DataBase;
using GolfCentra.Core.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.Business.Business.Implementation
{
    public class APIClientVaildationService : IAPIClientVaildationService
    {
        private readonly UnitOfWork _unitOfWork;

        public APIClientVaildationService()
        {
            _unitOfWork = new UnitOfWork();
        }

        /// <summary>
        /// Check API Client Is Vaild Or Not
        /// </summary>
        /// <param name="uniqueSessionId"></param>
        /// <param name="apiUserName"></param>
        /// <param name="apiPassword"></param>
        /// <returns></returns>
        public bool IsLoggedInClientVaild(string uniqueSessionId, string apiUserName, string apiPassword)

        {
            apiUserName.TryValidate("Api User Name");
            apiPassword.TryValidate("Api Password");
            long id = Convert.ToInt64(Core.Helper.Crypto.DecryptStringAES(uniqueSessionId.ToString()));
            APIClient aPIClient = _unitOfWork.APIClientRepository.Get(x => x.UserName == apiUserName && x.Password == apiPassword && x.IsActive == true);
            if (aPIClient == null)
                return false;
            LogInHistory logInHistory = _unitOfWork.LogInHistoryRepository.Get(x => x.LoginHistoryId == id);
            if (logInHistory == null)
                return false;

            if (logInHistory.LoggedOutAt != null)
                return false;
            return true;
        }

        /// <summary>
        /// Check Api Access Detail Is Vaild Or Not
        /// </summary>
        /// <param name="apiUserName"></param>
        /// <param name="apiPassword"></param>
        /// <returns></returns>
        public bool IsClientVaild(string apiUserName, string apiPassword)
        {
            apiUserName.TryValidate("Api User Name");
            apiPassword.TryValidate("Api Password");
            APIClient aPIClient = _unitOfWork.APIClientRepository.Get(x => x.UserName == apiUserName && x.Password == apiPassword && x.IsActive == true);
            if (aPIClient == null)
                return false;
            return true;
        }

        public bool IsLoggedInClientVaildWithGolferUnBlock(string uniqueSessionId, string apiUserName, string apiPassword)

        {
            apiUserName.TryValidate("Api User Name");
            apiPassword.TryValidate("Api Password");
            long id = Convert.ToInt64(Core.Helper.Crypto.DecryptStringAES(uniqueSessionId.ToString()));
            APIClient aPIClient = _unitOfWork.APIClientRepository.Get(x => x.UserName == apiUserName && x.Password == apiPassword && x.IsActive == true);
            if (aPIClient == null)
                return false;
            LogInHistory logInHistory = _unitOfWork.LogInHistoryRepository.Get(x => x.LoginHistoryId == id);
            if (logInHistory == null)
                return false;

            if (logInHistory.LoggedOutAt != null)
                return false;

            Golfer golfer = _unitOfWork.GolferRepository.Get(x => x.IsBlocked == true && x.GolferId == logInHistory.UserId && x.IsActive==true);
            if (golfer != null)
                throw new Exception("Your account is blocked.");

            return true;
        }

    }
}
