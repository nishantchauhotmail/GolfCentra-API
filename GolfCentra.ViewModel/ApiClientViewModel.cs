using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.ViewModel
{
    /// <summary>
    /// Properties For API Client's Operations Viz. Add, Edit, Delete, List And Etc.
    /// </summary>
    public class ApiClientViewModel
    {
        public ApiClientViewModel()
        {
            this.ApiPassword = string.Empty;
            this.ApiUserName = string.Empty;
        }
        public string ApiUserName { get; set; }

        public string ApiPassword { get; set; }

    }
}
