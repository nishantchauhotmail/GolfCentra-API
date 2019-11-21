using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.ViewModel
{
    /// <summary>
    /// Properties For ScoreDetails's Operations Viz. Add, Edit, Delete, List And Etc.
    /// </summary>
    public class ScoreDetailsViewModel
    {
        public ScoreDetailsViewModel()
        {
            TotalScore = 0;
            GrossScore = 0;
            ScoreId = 0;
            ScoreViewModels = new List<ScoreViewModel>();
            BookingId = 0;
            CourseName = string.Empty;
            CoursePairingName = string.Empty;
            ENB = string.Empty;
        }

        public long ScoreId { get; set; }
        public long TotalScore { get; set; }
        public long GrossScore { get; set; }
        public List<ScoreViewModel> ScoreViewModels { get; set; }
        public long BookingId { get; set; }
        public string CourseName { get; set; }
        public string CoursePairingName { get; set; }
        public string ENB { get; set; }
    }
}
