using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.ViewModel.Admin
{
    /// <summary>
    /// Properties For DashboardTopBar's Operations Viz. Add, Edit, Delete, List And Etc.
    /// </summary>
    public class DashBoardTopBarViewModel
    {
        public int TotalBooking { get; set; }
        public int Hole9Booking { get; set; }
        public int Hole18Booking { get; set; }
        public int Hole27Booking { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
