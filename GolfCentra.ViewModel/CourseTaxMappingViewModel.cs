using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.ViewModel
{
    /// <summary>
    /// Properties For CourseTaxMapping's Operations Viz. Add, Edit, Delete, List And Etc.
    /// </summary>
    public class CourseTaxMappingViewModel
    {
        public CourseTaxMappingViewModel()
        {
            TaxId = 0;
            Name = string.Empty;
            Percentage = 0;
        }
        public long TaxId { get; set; }
        public string Name { get; set; }
        public decimal Percentage { get; set; }
    }
}
