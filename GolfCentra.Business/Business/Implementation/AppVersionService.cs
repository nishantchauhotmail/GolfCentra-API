using GolfCentra.Business.Business.Interface;
using GolfCentra.Core;
using GolfCentra.Core.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.Business.Business.Implementation
{
    public class AppVersionService : IAppVersionService
    {
        private readonly UnitOfWork _unitOfWork;

        public AppVersionService()
        {
            _unitOfWork = new UnitOfWork();
        }

        /// <summary>
        /// Check App Version Details For IOS And ANDIORD
        /// </summary>
        /// <param name="currentVersion">Current Version Of APP</param>
        /// <param name="platformTypeId">Platform Id[IOS/AND]</param>
        /// <returns>ForceUpdate[true/False],Message,Status[True/False]</returns>
        public Tuple<bool, string,bool> CheckAppVersion(decimal currentVersion, long platformTypeId)
        {
            bool forceUpdate = false;
            string forceMessage = "";
            bool update = false;
            string message = "";
            bool status = false;
            List<AppVersion> appVersions = _unitOfWork.AppVersionRepository.GetMany(x => x.IsActive == true && x.PlatformTypeId == platformTypeId && x.Version > currentVersion).ToList();
            if (appVersions.Count == 0)
                status = false;
            foreach (var item in appVersions.OrderBy(x => x.AppVersionId))
            {
                status = true;
                if (item.ForceUpdate == true)
                {
                    forceUpdate = item.ForceUpdate;
                    forceMessage = item.Message;
                }
                else
                {
                    update = item.ForceUpdate;
                    message = item.Message;
                }
            }


            if (forceUpdate == true)
            {
                return new Tuple<bool, string,bool>(forceUpdate, forceMessage,status);
            }
            else
            {
                return new Tuple<bool, string,bool>(update, message,status);

            }

        }
    }
}
