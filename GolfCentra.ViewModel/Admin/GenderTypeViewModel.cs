using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.ViewModel.Admin
{
    /// <summary>
    /// Properties For GenderType's Operations Viz. Add, Edit, Delete, List And Etc.
    /// </summary>
    public class GenderTypeViewModel
    {
        public long GenderTypeId { get; set; }
        public string Value { get; set; }

        public ApiClientViewModel ApiClientViewModel { get; set; }
    }
}
