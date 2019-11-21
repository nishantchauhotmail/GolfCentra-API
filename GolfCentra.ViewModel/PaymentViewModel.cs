using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.ViewModel
{
    /// <summary>
    /// Properties For Payment's Operations Viz. Add, Edit, Delete, List And Etc.
    /// </summary>
    public class PaymentViewModel
    {
        public PaymentViewModel()
        {
            Message = string.Empty;
    
            Status = false;
            ENB = string.Empty;
        }

        public string Message { get; set; }
     
        public bool Status { get; set; }
        public string ENB { get; set; }
    }
}
