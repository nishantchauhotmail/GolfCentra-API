using GolfCentra.Business.BusinessAdmin.Interface;
using GolfCentra.Core;
using GolfCentra.Core.DataBase;
using GolfCentra.ViewModel.Admin.LoginActivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.Business.BusinessAdmin.Implementation
{
    public class SessionActivityService : ISessionActivityService
    {
        private readonly UnitOfWork _unitOfWork;

        public SessionActivityService()
        {
            _unitOfWork = new UnitOfWork();
        }

        public void AddSessionActivity(SessionActivityPageViewModel sessionActivityPageViewModel, string uniqueSessionId)
        {
            uniqueSessionId = Core.Helper.Crypto.DecryptStringAES(uniqueSessionId);
            long id = Convert.ToInt64(uniqueSessionId);
            SessionActivity sessionActivity = new SessionActivity
            {
                LoginHistoryId = id,
                SessionActivityPageId = GetPageId(sessionActivityPageViewModel.ControllerName, sessionActivityPageViewModel.ActionName),
                PerformedOn = sessionActivityPageViewModel.PerformOn,
                ArriveAt = System.DateTime.Now,
                CreatedOn = System.DateTime.Now,
                IsActive = true
            };


            this._unitOfWork.SessionActivityRepository.Insert(sessionActivity);
            this._unitOfWork.Save();
        }

        private long GetPageId(string controllerName, string action)
        {
            SessionActivityPage sessionActivityPage = _unitOfWork.SessionActivityPageRepository.Get(x => x.Controller.ToLower().Equals(controllerName.ToLower()) && x.Action.ToLower().Equals(action.ToLower()));
            if (sessionActivityPage != null)
            {
                return sessionActivityPage.SessionActivityPageId;
            }
            else
            {
                SessionActivityPage sessionActivityPagex = new SessionActivityPage()
                {
                    DisplayName = controllerName,
                    Controller = controllerName,
                    Action = action,
                    IsActive = true,
                    CreatedOn = System.DateTime.UtcNow,
                    IsShowable = true
                };
                this._unitOfWork.SessionActivityPageRepository.Insert(sessionActivityPagex);
                this._unitOfWork.Save();
                return sessionActivityPagex.SessionActivityPageId;
            }
            return 1;
        }

        public void AddSessionActivityPage(List<SessionActivityPageViewModel> sessionActivityPageViewModels)
        {
            string value = "controller";
            foreach (var sessionPage in sessionActivityPageViewModels)
            {

                string controller = sessionPage.ControllerName.ToLower().Substring(0, (sessionPage.ControllerName.ToLower().Length - value.Length));

                long id = GetPageId(controller, sessionPage.ActionName);
                if (id == 1)
                {
                    SessionActivityPage sessionActivityPage = new SessionActivityPage()
                    {
                        DisplayName = controller,
                        Controller = controller,
                        Action = sessionPage.ActionName,
                        IsActive = true,
                        CreatedOn = System.DateTime.UtcNow
                    };
                    this._unitOfWork.SessionActivityPageRepository.Insert(sessionActivityPage);
                    this._unitOfWork.Save();
                }

            }



        }



        public List<SessionDetailViewModel> GetSessionActivityBySearch(SessionDetailViewModel sessionDetailViewModel)
        {
            List<SessionDetailViewModel> sessionDetailViewModels = new List<SessionDetailViewModel>();
            List<LogInHistory> logInHistories = new List<LogInHistory>();
            DateTime startDate = new DateTime(sessionDetailViewModel.StartDate.Year, sessionDetailViewModel.StartDate.Month, sessionDetailViewModel.StartDate.Day, 0, 0, 0);
            DateTime endDate = new DateTime(sessionDetailViewModel.EndDate.Year, sessionDetailViewModel.EndDate.Month, sessionDetailViewModel.EndDate.Day, 23, 59, 59);
            logInHistories = _unitOfWork.LogInHistoryRepository.GetMany(x => x.IsActive == true && x.CreatedOn >= startDate && x.CreatedOn <= endDate && x.PlatformTypeId == (int)Core.Helper.Enum.EnumPlatformType.Web).OrderBy(x => x.CreatedOn).ToList();
            if (sessionDetailViewModel.EmployeeId != 0)
            {
                logInHistories = logInHistories.Where(x => x.UserId == sessionDetailViewModel.EmployeeId).ToList();
            }

            foreach (LogInHistory loginHistory in logInHistories)
            {
                SessionDetailViewModel sessionDetailView = new SessionDetailViewModel()
                {
                    UserName = loginHistory.UserName,
                    LoggedIn = loginHistory.CreatedOn,
                    LoggedOutAt = loginHistory.LoggedOutAt,

                };
                sessionDetailView.SessionActivityViewModels = GetAllActivityByLoginHistoryId(loginHistory.LoginHistoryId);
                sessionDetailViewModels.Add(sessionDetailView);
            }
            return sessionDetailViewModels;
        }

        private List<SessionActivityViewModel> GetAllActivityByLoginHistoryId(long loginHistoryId)
        {
            List<SessionActivityViewModel> sessionActivityViewModels = new List<SessionActivityViewModel>();
            List<SessionActivity> sessionActivities = _unitOfWork.SessionActivityRepository.GetMany(x => x.IsActive == true && x.LoginHistoryId == loginHistoryId && x.SessionActivityPage.IsShowable == true).ToList();
            foreach (var item in sessionActivities)
            {
                SessionActivityViewModel sessionActivityViewModel = new SessionActivityViewModel();
                //if (item.SessionActivityPage.SessionActivityPageId == 318 || item.SessionActivityPage.SessionActivityPageId == 319)
                //{
                //    sessionActivityViewModel.DisplayName = item.SessionActivityPage.DisplayName + " Booking Id-" + item.PerformedOn;
                //    sessionActivityViewModel.ArriveAt = item.ArriveAt;
                //}
                //else
                //{
                sessionActivityViewModel.DisplayName = item.SessionActivityPage.DisplayName;
                sessionActivityViewModel.ArriveAt = item.ArriveAt;
                sessionActivityViewModel.Info = item.Info;
                // }

                sessionActivityViewModels.Add(sessionActivityViewModel);

            }
            return sessionActivityViewModels;
        }



        public List<SessionDetailViewModel> GetDailySessionActivityBySearch(SessionDetailViewModel sessionDetailViewModel)
        {
            List<SessionDetailViewModel> sessionDetailViewModels = new List<SessionDetailViewModel>();
            List<LogInHistory> logInHistories = new List<LogInHistory>();
            DateTime startDate = new DateTime(sessionDetailViewModel.StartDate.Year, sessionDetailViewModel.StartDate.Month, sessionDetailViewModel.StartDate.Day, 0, 0, 0);
            DateTime endDate = new DateTime(sessionDetailViewModel.StartDate.Year, sessionDetailViewModel.StartDate.Month, sessionDetailViewModel.StartDate.Day, 23, 59, 59);
            logInHistories = _unitOfWork.LogInHistoryRepository.GetMany(x => x.IsActive == true && x.CreatedOn >= startDate && x.CreatedOn <= endDate && x.PlatformTypeId == (int)Core.Helper.Enum.EnumPlatformType.Web).OrderBy(x => x.CreatedOn).ToList();
            if (sessionDetailViewModel.EmployeeId != 0)
            {
                logInHistories = logInHistories.Where(x => x.UserId == sessionDetailViewModel.EmployeeId).ToList();
            }

            foreach (LogInHistory loginHistory in logInHistories)
            {
                SessionDetailViewModel sessionDetailView = new SessionDetailViewModel()
                {
                    UserName = loginHistory.UserName,
                    LoggedIn = loginHistory.CreatedOn,
                    LoggedOutAt = loginHistory.LoggedOutAt,
                };
                sessionDetailView.SessionActivityViewModels = GetAllActivityByLoginHistoryId(loginHistory.LoginHistoryId);
                sessionDetailViewModels.Add(sessionDetailView);
            }
            return sessionDetailViewModels;
        }
    }
}
