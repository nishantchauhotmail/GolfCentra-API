using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.ViewModel.Admin
{
 public   class TimeFormatViewModel
    {
        public long TimeFormatId { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public ApiClientViewModel ApiClientViewModel { get; set; }
    }
}
