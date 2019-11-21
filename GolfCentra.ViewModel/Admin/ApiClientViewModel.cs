using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.ViewModel.Admin
{

    /// <summary>
    /// Properties For API Client View 's Operations Viz. Add, Edit, Delete, List And Etc.
    /// </summary>
    public class ApiClientViewModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string UniqueSessionId { get; set; }
    }
}
