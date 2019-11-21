using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.ViewModel.Admin
{
    /// <summary>
    /// Properties For Employee's Operations Viz. Add, Edit, Delete, List And Etc.
    /// </summary>
    public class EmployeeViewModel
    {

        public long EmployeeId { get; set; }
        public string Name { get; set; }
        public string EmailId { get; set; }
        public string Mobile { get; set; }
        public long GenderTypeId { get; set; }
        public long EmployeeTypeId { get; set; }
        public string Password { get; set; }
        public string GenderType { get; set; }
        public string EmployeeType { get; set; }
        public bool IsActive { get;set; }
        public ApiClientViewModel ApiClientViewModel { get; set; }
        public List<PageViewModel> PageViewModels { get; set; }
   
        public string UniqueSessionId { get; set; }
        public string OldPassword { get; set; }
        public bool IsFirstLogIn { get; set; }
        public List<PageViewModel> AllPageViewModels { get; set; }
    }
}
