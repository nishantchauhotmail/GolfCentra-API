using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.ViewModel.Admin
{
    /// <summary>
    /// Properties For Permission's Operations Viz. Add, Edit, Delete, List And Etc.
    /// </summary>
    public class PermissionMainViewModel
    {
        public long EmployeeId { get; set; }
        public List<PermissionViewModel> PermissionViewModels { get; set; }
        public ApiClientViewModel ApiClientViewModel { get; set; }
    }
}
