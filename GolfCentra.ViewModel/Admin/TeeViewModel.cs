using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.ViewModel.Admin
{
    /// <summary>
    /// Properties For Tee's Operations Viz. Add, Edit, Delete, List And Etc.
    /// </summary>
    public class TeeViewModel
    {
        public long TeeId { get; set; }
        public long HoleTeeYardageId { get; set; }
        public string TeeName { get; set; }
        public decimal Yardage { get; set; }
    }
}
