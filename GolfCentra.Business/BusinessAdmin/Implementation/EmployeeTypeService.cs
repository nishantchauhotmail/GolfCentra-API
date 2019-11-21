using GolfCentra.Business.BusinessAdmin.Interface;
using GolfCentra.Core;
using GolfCentra.Core.DataBase;
using GolfCentra.ViewModel.Admin;
using GolfCentra.ViewModel.Admin.LoginActivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.Business.BusinessAdmin.Implementation
{
    public class EmployeeTypeService : IEmployeeTypeService
    {

        private readonly UnitOfWork _unitOfWork;

        public EmployeeTypeService()
        {
            _unitOfWork = new UnitOfWork();
        }

        /// <summary>
        /// Get All employee Type Detail
        /// </summary>
        /// <returns></returns>
        public List<EmployeeTypeViewModel> GetAllEmployeeType()
        {
            List<EmployeeType> employeeTypes = _unitOfWork.EmployeeTypeRepository.GetMany(x => x.IsActive == true).ToList();
            List<EmployeeTypeViewModel> employeeTypeViewModels = new List<EmployeeTypeViewModel>();
            foreach (var item in employeeTypes)
            {
                EmployeeTypeViewModel employeeViewModel = new EmployeeTypeViewModel()
                {
                    EmployeeTypeId = item.EmployeeTypeId,
                    Value = item.Value
                };
                employeeTypeViewModels.Add(employeeViewModel);
            }
            return employeeTypeViewModels;
        }

        /// <summary>
        /// Save Employee Detail
        /// </summary>
        /// <param name="employeeTypeViewModel"></param>
        /// <returns></returns>
        public bool SaveEmployeeType(EmployeeTypeViewModel employeeTypeViewModel,long uniqueSessionId)
        {
            List<EmployeeType> employeeTypes = _unitOfWork.EmployeeTypeRepository.GetMany(x => x.IsActive == true && x.Value.Trim().ToLower()==employeeTypeViewModel.Value.ToLower().Trim()).ToList();
            if (employeeTypes.Count() > 0)
                throw new Exception("Type Already Registered");

            EmployeeType employeeType = new EmployeeType()
            {
                IsActive = true,
                Value = employeeTypeViewModel.Value,
                CreatedOn = System.DateTime.UtcNow
            };

            _unitOfWork.EmployeeTypeRepository.Insert(employeeType);
            _unitOfWork.Save();

            try
            {

                SessionActivityPageViewModel sessionActivityPageViewModel = new SessionActivityPageViewModel()
                {
                    ControllerName = "Employee Type",
                    ActionName = "Save",
                    PerformOn = employeeType.EmployeeTypeId.ToString(),
                    LoginHistoryId = uniqueSessionId,

                    Info = "Created a Employee Type with Employeetypeid- " + employeeType.EmployeeTypeId.ToString() + ". EmployeeType had following details <br/>Name " + employeeType.Value
                };
                new Common.AddSessionActivity().SaveSessionActivity(sessionActivityPageViewModel);

            }
            catch (Exception)
            {

            }
            return true;
        }
    }
}
