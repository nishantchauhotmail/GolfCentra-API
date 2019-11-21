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
    public class PermissionViewModel
    {
        public long PageId { get; set; }
        public string PageName { get; set; }
        public long PageRightId { get; set; }
        public bool IsPermisson { get; set; }
        public bool IsSideMenuPage { get; set; }
       
    }
}
