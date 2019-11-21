using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.ViewModel
{
    /// <summary>
    /// Properties For PhoneCode's Operations Viz. Add, Edit, Delete, List And Etc.
    /// </summary>
    public class PhoneCodeViewModel
    {
        public PhoneCodeViewModel()
        {
            CountryName = string.Empty;
            PhoneCode = 0;
        }
        public string CountryName { get; set; }
        public int PhoneCode { get; set; }
    }
}
