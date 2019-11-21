using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.ViewModel
{
    /// <summary>
    /// Properties For AboutUs's Operations Viz. Add, Edit, Delete, List And Etc.
    /// </summary>
    public class AboutUsViewModel
    {
        public AboutUsViewModel()
        {
            Name = string.Empty;
            ImageURL = string.Empty;
            URL = string.Empty;
        }

        public string Name { get; set; }
        public string ImageURL { get; set; }
        public string URL { get; set; }
    }
}
