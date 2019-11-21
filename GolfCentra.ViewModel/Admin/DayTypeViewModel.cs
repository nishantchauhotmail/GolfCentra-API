using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.ViewModel.Admin
{
    /// <summary>
    /// Properties For DayType's Operations Viz. Add, Edit, Delete, List And Etc.
    /// </summary>
    public class DayTypeViewModel
    {
        public long DayTypeId { get; set; }
        public string Name { get; set; }
        public ApiClientViewModel ApiClientViewModel { get; set; }
    }
}
