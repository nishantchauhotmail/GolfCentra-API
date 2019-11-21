using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.ViewModel.Admin
{
    /// <summary>
    /// Properties For Booking Type's Operations Viz. Add, Edit, Delete, List And Etc.
    /// </summary>
    public class BookingTypeViewModel
    {
        public long BookingTypeId { get; set; }
        public string Name { get; set; }
        public ApiClientViewModel ApiClientViewModel { get; set; }
    }
}
