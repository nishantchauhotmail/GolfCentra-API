using GolfCentra.ViewModel.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.Business.BusinessAdmin.Interface
{
    public interface IEmployeeService
    {
        EmployeeViewModel LoginValidation(string userName, string password);
        bool Logout(string uniqueSessionId);
        List<EmployeeViewModel> GetAllActiveEmployeeForList();
        List<EmployeeViewModel> GetAllActiveEmployee();
        bool SaveEmployee(EmployeeViewModel employeeViewModel, long uniqueSessionId);
        bool EmployeeStatus(long employeeId, bool isStatus, long uniqueSessionId);
        bool UpdateEmployee(EmployeeViewModel employeeViewModel, long uniqueSessionId);
        EmployeeViewModel GetEmployeeById(long id);
        bool ChangePassword(EmployeeViewModel employeeViewModel, long uniqueSessionId);
    }
}
