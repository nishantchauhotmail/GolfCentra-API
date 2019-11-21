using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.ViewModel.Admin
{
    /// <summary>
    /// Properties For ScoreDetails's Operations Viz. Add, Edit, Delete, List And Etc.
    /// </summary>
    public class ScoreDetailsViewModel
    {
        public long ScoreId { get; set; }
        public long TotalScore { get; set; }
        public long GrossScore { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public DateTime SubmittedDate { get; set; }
        public TimeSpan Time { get; set; }
        public List<ScoreViewModel> ScoreViewModels { get; set; }

        public DateTime DateOfPlay { get; set; }
        public string PlayTime { get; set; }
        public string MemberShipId { get; set; }
        public string CoursePairingName { get; set; }

        public ApiClientViewModel ApiClientViewModel { get; set; }
    }
}
