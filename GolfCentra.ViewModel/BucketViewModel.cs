using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.ViewModel
{
    /// <summary>
    /// Properties For Bucket's Operations Viz. Add, Edit, Delete, List And Etc.
    /// </summary>
    public class BucketViewModel
    {
        public BucketViewModel()
        {
            BucketDetailId = 0;
            Balls = 0;
        }
        public long BucketDetailId { get; set; }
        public int Balls { get; set; }
      
    }
}
