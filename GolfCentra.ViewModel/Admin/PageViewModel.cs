using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.ViewModel.Admin
{
    /// <summary>
    /// Properties For Page's Operations Viz. Add, Edit, Delete, List And Etc.
    /// </summary>
    public class PageViewModel
    {
        public long PageId { get; set; }
        public string PageName { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string Icon { get; set; }
        public int ParentId { get; set; }
        public int Ordering { get; set; }
      
        public ApiClientViewModel ApiClientViewModel { get; set; }
    }
}
