using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.ViewModel.Admin
{
    /// <summary>
    /// Properties For ScoreSearch's Operations Viz. Add, Edit, Delete, List And Etc.
    /// </summary>
    public class ScoreSearchViewModel
    {
        public DateTime Date { get; set; }
        public string Email { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public long SearchTypeId { get; set; }
        public string[] EmailList { get; set; }
        public ApiClientViewModel ApiClientViewModel { get; set; }

        public long CoursePairingId { get; set; }
        public string GolferName { get; set; }
        public string MemberShipId { get; set; }
        public long ScoreId { get; set; }
    }
}
