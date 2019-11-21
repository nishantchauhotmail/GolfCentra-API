using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.ViewModel
{
    /// <summary>
    /// Properties For AppVersion's Operations Viz. Add, Edit, Delete, List And Etc.
    /// </summary>
    public class AppVersionViewModel
    {
        public AppVersionViewModel()
        {
            Message = string.Empty;
            ForceUpdate = false;

            Status = false;
        }

        public string Message { get; set; }
        public bool ForceUpdate { get; set; }
        public bool Status { get; set; }
    }
}
