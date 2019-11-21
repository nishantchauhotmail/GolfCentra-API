using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.ViewModel
{
    /// <summary>
    /// Properties For CourseTee's Operations Viz. Add, Edit, Delete, List And Etc.
    /// </summary>
    public class CourseTeeViewModel
    {
        public CourseTeeViewModel()
        {
            this.TeeName = string.Empty;
            this.Yardage = Decimal.Zero;
        }
        public string TeeName { get; set; }
        public decimal Yardage { get; set; }
     
    }
}
