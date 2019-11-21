using GolfCentra.Business.BusinessAdmin.Interface;
using GolfCentra.Core;
using GolfCentra.Core.DataBase;
using GolfCentra.ViewModel.Admin;
using GolfCentra.ViewModel.Admin.LoginActivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.Business.BusinessAdmin.Implementation
{
    public class EmployeeService : IEmployeeService
    {
        private readonly UnitOfWork _unitOfWork;

        public EmployeeService()
        {
            _unitOfWork = new UnitOfWork();
        }

        /// <summary>
        /// Employee Login Vaildation
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public EmployeeViewModel LoginValidation(string userName, string password)
        {

            Employee employee = _unitOfWork.EmployeeRepository.Get(x => x.EmailId.ToLower() == userName.ToLower() && x.IsActive == true);
            if (employee == null)
                throw new Exception("No User Found");
            if (!Core.Helper.PasswordConvertor.ValidatePassword(password, employee.Password))
                throw new Exception("No User Found");

            LogoutOperation(employee.EmployeeId);


            List<PageRight> pageRights = _unitOfWork.PageRightRepository.GetManyWithInclude(x => x.EmployeeId == employee.EmployeeId && x.IsActive == true).ToList();
            List<PageViewModel> pageViewModels = new List<PageViewModel>();
            foreach (PageRight pageRight in pageRights)
            {
                PageViewModel pageViewModel = new PageViewModel()
                {
                    PageId = pageRight.PageId,
                    ActionName = pageRight.Page.ActionName,
                    ControllerName = pageRight.Page.ControllerName,
                    Icon = pageRight.Page.Icon,
                    Ordering = pageRight.Page.Ordering,
                    PageName = pageRight.Page.PageName,
                    ParentId = pageRight.Page.ParentId
                };
                pageViewModels.Add(pageViewModel);
            }

            List<Page> pages = _unitOfWork.PageRepository.GetManyWithInclude(x => x.IsActive == true).ToList();

            List<PageViewModel> pageViewModels1 = new List<PageViewModel>();
            foreach (Page pageRight in pages)
            {
                PageViewModel pageViewModel = new PageViewModel()
                {
                    PageId = pageRight.PageId,
                    ActionName = pageRight.ActionName,
                    ControllerName = pageRight.ControllerName,
                    Icon = pageRight.Icon,
                    Ordering = pageRight.Ordering,
                    PageName = pageRight.PageName,
                    ParentId = pageRight.ParentId
                };
                pageViewModels1.Add(pageViewModel);
            }

            EmployeeViewModel employeeViewModel = new EmployeeViewModel()
            {
                Name = employee.Name,
                EmailId = employee.EmailId,
                Mobile = employee.Mobile,
                PageViewModels = pageViewModels,
                EmployeeId = employee.EmployeeId,
                AllPageViewModels = pageViewModels1
            };
            LogInHistory logInHistory = SaveLogInHistory(employeeViewModel);

            employeeViewModel.UniqueSessionId = Core.Helper.Crypto.EncryptStringAES(logInHistory.LoginHistoryId.ToString());

            if (employee.IsPrimaryPasswordChanged == null || employee.IsPrimaryPasswordChanged == false)
            {

                employeeViewModel.IsFirstLogIn = true;
            }
            else
            {
                employeeViewModel.IsFirstLogIn = false;
            }

            return employeeViewModel;
        }

        /// <summary>
        /// Save Login History
        /// </summary>
        /// <param name="employeeViewModel"></param>
        /// <returns></returns>
        private LogInHistory SaveLogInHistory(EmployeeViewModel employeeViewModel)
        {
            LogInHistory logInHistory = new LogInHistory()
            {
                LoginStatusId = 1,
                CreatedOn = System.DateTime.UtcNow,
                IsActive = true,
                UserId = employeeViewModel.EmployeeId,
                IPAddress = "",
                PlatformTypeId = (int)Core.Helper.Enum.EnumPlatformType.Web,
                MACAddress = "",
                UserName = employeeViewModel.EmailId,
                UserTypeId = 2,
            };
            _unitOfWork.LogInHistoryRepository.Insert(logInHistory);
            _unitOfWork.Save();
            return logInHistory;
        }

        /// <summary>
        /// Logout Employee 
        /// </summary>
        /// <param name="uniqueSessionId"></param>
        /// <returns></returns>
        public bool Logout(string uniqueSessionId)
        {
            uniqueSessionId = Core.Helper.Crypto.DecryptStringAES(uniqueSessionId);
            long id = Convert.ToInt64(uniqueSessionId);
            LogInHistory logInHistory = _unitOfWork.LogInHistoryRepository.Get(x => x.LoginHistoryId == id && x.LoggedOutAt == null);
            if (logInHistory == null)
                throw new Exception("User Already Logout");
            logInHistory.LoggedOutAt = System.DateTime.UtcNow;
            logInHistory.UpdatedOn = System.DateTime.UtcNow;
            _unitOfWork.LogInHistoryRepository.Update(logInHistory);
            _unitOfWork.Save();
            return true;
        }

        /// <summary>
        /// Get All Active Employee List
        /// </summary>
        /// <returns></returns>
        public List<EmployeeViewModel> GetAllActiveEmployeeForList()
        {
            List<Employee> employees = _unitOfWork.EmployeeRepository.GetMany(x => x.IsActive == true).ToList();

            List<EmployeeViewModel> employeeViewModels = new List<EmployeeViewModel>();

            foreach (var item in employees)
            {
                EmployeeViewModel employeeViewModel = new EmployeeViewModel()
                {
                    EmployeeId = item.EmployeeId,
                    Name = item.Name
                };
                employeeViewModels.Add(employeeViewModel);
            }
            return employeeViewModels;
        }

        /// <summary>
        /// Get All Active Employee
        /// </summary>
        /// <returns></returns>
        public List<EmployeeViewModel> GetAllActiveEmployee()
        {
            List<Employee> employees = _unitOfWork.EmployeeRepository.GetAll().ToList();

            List<EmployeeViewModel> employeeViewModels = new List<EmployeeViewModel>();

            foreach (var item in employees)
            {
                EmployeeViewModel employeeViewModel = new EmployeeViewModel()
                {
                    EmployeeId = item.EmployeeId,
                    Name = item.Name,
                    EmailId = item.EmailId,
                    Mobile = item.Mobile,
                    EmployeeType = item.EmployeeType.Value,
                    GenderType = item.GenderType.Value,
                    IsActive = item.IsActive

                };
                employeeViewModels.Add(employeeViewModel);
            }
            return employeeViewModels;
        }

        /// <summary>
        /// Save Employee Details
        /// </summary>
        /// <param name="employeeViewModel"></param>
        /// <returns></returns>
        public bool SaveEmployee(EmployeeViewModel employeeViewModel, long uniqueSessionId)
        {

            Employee emailCheck = _unitOfWork.EmployeeRepository.Get(x => x.EmailId.Trim().ToLower() == employeeViewModel.EmailId.Trim().ToLower());
            if (emailCheck != null)
                throw new Exception("Email Is Already Registered.");
            Employee employee = new Employee()
            {
                Name = employeeViewModel.Name,
                EmailId = employeeViewModel.EmailId,
                GenderTypeId = employeeViewModel.GenderTypeId,
                EmployeeTypeId = employeeViewModel.EmployeeTypeId,
                IsActive = true,
                CreatedOn = System.DateTime.UtcNow,
                Password = Core.Helper.PasswordConvertor.Password(employeeViewModel.Password).ToString(),
                Mobile = employeeViewModel.Mobile
            };
            _unitOfWork.EmployeeRepository.Insert(employee);
            _unitOfWork.Save();

            try
            {

                SessionActivityPageViewModel sessionActivityPageViewModel = new SessionActivityPageViewModel()
                {
                    ControllerName = "Employee",
                    ActionName = "Save",
                    PerformOn = employee.EmployeeId.ToString(),
                    LoginHistoryId = uniqueSessionId,

                    Info = "Created a Employee with Employeeid- " + employee.EmployeeId.ToString()
                };
                new Common.AddSessionActivity().SaveSessionActivity(sessionActivityPageViewModel);

            }
            catch (Exception)
            {

            }
            return true;
        }

        /// <summary>
        /// Update Employee
        /// </summary>
        /// <param name="employeeViewModel"></param>
        /// <returns></returns>
        public bool UpdateEmployee(EmployeeViewModel employeeViewModel, long uniqueSessionId)
        {
            Employee employee1 = new Employee();
            Employee employee = _unitOfWork.EmployeeRepository.Get(x => x.EmployeeId == employeeViewModel.EmployeeId);
            employee1 = employee;
            Employee emailCheck = _unitOfWork.EmployeeRepository.Get(x => x.EmailId.Trim().ToLower() == employeeViewModel.EmailId.Trim().ToLower());

            if (emailCheck != null && emailCheck.EmailId.Trim().ToLower() != employee.EmailId.Trim().ToLower())
                throw new Exception("Email Is Already Registered.");
            employee.Name = employeeViewModel.Name;
            employee.EmailId = employeeViewModel.EmailId;
            //employee.GenderTypeId = employeeViewModel.GenderTypeId;
            //employee.EmployeeTypeId = employeeViewModel.EmployeeTypeId;
            //employee.IsActive = true;
            employee.UpdatedOn = System.DateTime.UtcNow;
            if (employeeViewModel.Password != "" && employeeViewModel.Password != null)
            {
                employee.Password = Core.Helper.PasswordConvertor.Password(employeeViewModel.Password).ToString();
            }
            employee.Mobile = employeeViewModel.Mobile;

            _unitOfWork.EmployeeRepository.Update(employee);
            _unitOfWork.Save();

            try
            {

                SessionActivityPageViewModel sessionActivityPageViewModel = new SessionActivityPageViewModel()
                {
                    ControllerName = "Employee",
                    ActionName = "Update",
                    PerformOn = employee.EmployeeId.ToString(),
                    LoginHistoryId = uniqueSessionId,

                    Info = "Updated  a Employee with Employeeid- " + employee.EmployeeId.ToString() + ".Employee have following details-<br/>Name " + employee1.Name + "<br/> email " + employee1.EmailId

                };
                new Common.AddSessionActivity().SaveSessionActivity(sessionActivityPageViewModel);

            }
            catch (Exception)
            {

            }
            return true;
        }

        /// <summary>
        /// Active/Deactive Employee 
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="isStatus"></param>
        /// <returns></returns>
        public bool EmployeeStatus(long employeeId, bool isStatus, long uniqueSessionId)
        {
            Employee employee = _unitOfWork.EmployeeRepository.Get(x => x.EmployeeId == employeeId);
            employee.IsActive = isStatus;
            employee.UpdatedOn = System.DateTime.UtcNow;
            _unitOfWork.EmployeeRepository.Update(employee);
            _unitOfWork.Save();
            try
            {

                SessionActivityPageViewModel sessionActivityPageViewModel = new SessionActivityPageViewModel()
                {
                    ControllerName = "Employee Status",
                    ActionName = "Update",
                    PerformOn = employee.EmployeeId.ToString(),
                    LoginHistoryId = uniqueSessionId,

                    Info = "Update a Employee Status with Employeeid- " + employee.EmployeeId.ToString() + ". Employee had following details <br/>Status " + (isStatus == true ? "Inactive" : "Active")
                };
                new Common.AddSessionActivity().SaveSessionActivity(sessionActivityPageViewModel);

            }
            catch (Exception)
            {

            }

            return true;
        }

        /// <summary>
        /// Get Employee Details By Employee Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public EmployeeViewModel GetEmployeeById(long id)
        {
            Employee employee = _unitOfWork.EmployeeRepository.Get(x => x.EmployeeId == id);


            EmployeeViewModel employeeViewModel = new EmployeeViewModel()
            {
                EmployeeId = employee.EmployeeId,
                Name = employee.Name,
                EmailId = employee.EmailId,
                Mobile = employee.Mobile,
                EmployeeType = employee.EmployeeType.Value,
                GenderType = employee.GenderType.Value,
                IsActive = employee.IsActive

            };

            return employeeViewModel;
        }


        public bool ChangePassword(EmployeeViewModel employeeViewModel, long uniqueSessionId)
        {
            Employee employee = _unitOfWork.EmployeeRepository.Get(x => x.EmployeeId == employeeViewModel.EmployeeId);
            if (employee == null)
                throw new Exception("No Employee Found");
            if (!Core.Helper.PasswordConvertor.ValidatePassword(employeeViewModel.OldPassword, employee.Password))
                throw new Exception("Old Password Is Not Correct");

            if (Core.Helper.PasswordConvertor.ValidatePassword(employeeViewModel.Password, employee.Password))
                throw new Exception("You Can't Use Old Password As New Password");


            employee.Password = Core.Helper.PasswordConvertor.Password(employeeViewModel.Password).ToString();
            employee.UpdatedOn = System.DateTime.UtcNow;
            employee.IsPrimaryPasswordChanged = true;
            _unitOfWork.EmployeeRepository.Update(employee);
            _unitOfWork.Save();
            try
            {

                SessionActivityPageViewModel sessionActivityPageViewModel = new SessionActivityPageViewModel()
                {
                    ControllerName = "Employee Password ",
                    ActionName = "Update",
                    PerformOn = employee.EmployeeId.ToString(),
                    LoginHistoryId = uniqueSessionId,

                    Info = "Updated a Employee Password with Employeeid- " + employee.EmployeeId.ToString()
                };
                new Common.AddSessionActivity().SaveSessionActivity(sessionActivityPageViewModel);

            }
            catch (Exception)
            {

            }
            return true;
        }

        private void LogoutOperation(long employeeId)
        {
            try
            {
                List<LogInHistory> logInHistory = _unitOfWork.LogInHistoryRepository.GetMany(x => x.UserId == employeeId && x.LoggedOutAt == null).ToList();

                foreach (var item in logInHistory)
                {
                    item.LoggedOutAt = System.DateTime.UtcNow;
                    item.UpdatedOn = System.DateTime.UtcNow;
                    _unitOfWork.LogInHistoryRepository.Update(item);
                    _unitOfWork.Save();
                }
            }
            catch (Exception ex)
            {

                //    throw;
            }
        }


        public void conver()
        {
            List<Golfer> golfers = _unitOfWork.GolferRepository.GetMany(x => x.IsActive == true).ToList();
            foreach (var golfer in golfers)
            {
                golfer.Password = Core.Helper.PasswordConvertor.Password(golfer.Password).ToString();
                _unitOfWork.GolferRepository.Update(golfer);
                _unitOfWork.Save();
            }
            List<Employee> em = _unitOfWork.EmployeeRepository.GetMany(x => x.IsActive == true).ToList();
            foreach (var golfer in em)
            {
                golfer.Password = Core.Helper.PasswordConvertor.Password(golfer.Password).ToString();
                _unitOfWork.EmployeeRepository.Update(golfer);
                _unitOfWork.Save();
            }
        }
    }
}
