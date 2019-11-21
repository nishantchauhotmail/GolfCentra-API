using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.ViewModel.Admin
{
   public class CoursePairingViewModel
    {
        public long CoursePairingId { get; set; }
        public long StartCourseNameId { get; set; }
        public Nullable<long> EndCourseNameId { get; set; }
        public long HoleTypeId { get; set; }
        public ApiClientViewModel ApiClientViewModel { get; set; }
    }
}
