using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.ViewModel
{
    /// <summary>
    /// Properties For Hole's Operations Viz. Add, Edit, Delete, List And Etc.
    /// </summary>
    public class HoleViewModel
    {
        public HoleViewModel()
        {
            this.HoleNumber = 0;
            this.CourseTeeViewList = new List<CourseTeeViewModel>();
            this.ImgURL = string.Empty;
            this.Latitude = decimal.Zero;
            this.Longitude = decimal.Zero;
            this.Par = 0;
            this.Storke = 0;

        }
        public int HoleNumber { get; set; }
        public int Par { get; set; }
        public int Storke { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string ImgURL { get; set; }
        public List<CourseTeeViewModel> CourseTeeViewList { get; set; }
    }
}
