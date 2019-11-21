using GolfCentra.Core;
using GolfCentra.Core.DataBase;
using GolfCentra.ViewModel.Admin.LoginActivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.Business.Common
{
    public class AddSessionActivity
    {
        private readonly UnitOfWork _unitOfWork;

        public AddSessionActivity()
        {
            _unitOfWork = new UnitOfWork();
        }
        public void SaveSessionActivity(SessionActivityPageViewModel sessionActivityPageViewModel)
        {
            SessionActivity sessionActivity1 = new SessionActivity
            {
                LoginHistoryId = sessionActivityPageViewModel.LoginHistoryId,
                SessionActivityPageId = GetPageId(sessionActivityPageViewModel.ControllerName, sessionActivityPageViewModel.ActionName),
                PerformedOn = sessionActivityPageViewModel.PerformOn,
                ArriveAt = System.DateTime.Now,
                CreatedOn = System.DateTime.Now,
                IsActive = true,
                Info = sessionActivityPageViewModel.Info
            };


            this._unitOfWork.SessionActivityRepository.Insert(sessionActivity1);
            this._unitOfWork.Save();
        }

        private long GetPageId(string controllerName, string action)
        {
            SessionActivityPage sessionActivityPagex = _unitOfWork.SessionActivityPageRepository.Get(x => x.Controller.ToLower().Equals(controllerName.ToLower()) && x.Action.ToLower().Equals(action.ToLower()));
            if (sessionActivityPagex != null)
            {
                return sessionActivityPagex.SessionActivityPageId;
            }
            else
            {

                SessionActivityPage sessionActivityPage = new SessionActivityPage()
                {
                    DisplayName = controllerName,
                    Controller = controllerName,
                    Action = action,
                    IsActive = true,
                    CreatedOn = System.DateTime.UtcNow,
                    IsShowable=true
                };
                this._unitOfWork.SessionActivityPageRepository.Insert(sessionActivityPage);
                this._unitOfWork.Save();
                return sessionActivityPage.SessionActivityPageId;
            }

        }

    }
}
