using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.ViewModel
{
  public  class BookingPlayerDetailViewModel
    {
        public BookingPlayerDetailViewModel()
        {
            PlayerSerialNumber = string.Empty;
            PlayerDetails = string.Empty;
        }
        public string PlayerSerialNumber { get; set; }
        public string PlayerDetails { get; set; }
    }
}
