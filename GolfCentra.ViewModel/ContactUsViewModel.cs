using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.ViewModel
{
    /// <summary>
    /// Properties For ContactUs's Operations Viz. Add, Edit, Delete, List And Etc.
    /// </summary>
    public class ContactUsViewModel
    {
        public ContactUsViewModel()
        {
            CourseName = string.Empty;
            Address = string.Empty;
            TechnicalSupportMailId = string.Empty;
            Phone = string.Empty;
            Email = string.Empty;
            Latitude = string.Empty;
            Longitude = string.Empty;
            Address2 = string.Empty;
            Address2Name = string.Empty;
            Address2Phone = string.Empty;
        }

        public string CourseName { get; set; }
        public string Address { get; set; }
        public string TechnicalSupportMailId { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Address2Name { get; set; }
        public string Address2 { get; set; }
        public string Address2Phone { get; set; }
    }
}
