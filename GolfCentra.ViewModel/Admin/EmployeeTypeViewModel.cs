using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.ViewModel.Admin
{
    /// <summary>
    /// Properties For Employeetype's Operations Viz. Add, Edit, Delete, List And Etc.
    /// </summary>
    public class EmployeeTypeViewModel
    {
        public long EmployeeTypeId { get; set; }
        public string Value { get; set; }

        public ApiClientViewModel ApiClientViewModel { get; set; }
    }
}
