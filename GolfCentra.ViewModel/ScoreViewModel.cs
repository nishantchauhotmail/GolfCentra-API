using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.ViewModel
{
    /// <summary>
    /// Properties For Score's Operations Viz. Add, Edit, Delete, List And Etc.
    /// </summary>
    public class ScoreViewModel
    {
        public ScoreViewModel()
        {
            GolferId = 0;
            HoleNumber = 0;
            Storkes = 0;
            Putts = 0;
            Drive = 0;
            Chips = 0;
            Clubs = 0;
            Sand = 0;
            Saves = 0;
            Penalty = 0;
            Time = string.Empty;
            ScoreDate = string.Empty;
            ScoreId = 0;
            HoleName = string.Empty;
            ENS = string.Empty;
        }

        public long GolferId { get; set; }
        public long HoleNumber { get; set; }
        public Nullable<int> Storkes { get; set; }
        public Nullable<int> Putts { get; set; }
        public Nullable<int> Drive { get; set; }
        public Nullable<int> Clubs { get; set; }
        public Nullable<int> Chips { get; set; }
        public Nullable<int> Sand { get; set; }
        public Nullable<int> Saves { get; set; }
        public Nullable<int> Penalty { get; set; }
        public string Time { get; set; }
        public string ScoreDate { get; set; }
        public long Par { get; set; }
        public long StorkeIndex { get; set; }
        public long ScoreId { get; set; }

        public string HoleName { get; set; }
        public string ENS { get; set; }
    }
}
