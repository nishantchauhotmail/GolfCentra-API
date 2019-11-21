using System;
using System.Collections.Generic;
using GolfCentra.ViewModel.Admin;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.Business.BusinessAdmin.Interface
{
    public interface IBucketService
    {
        List<BucketViewModel> GetAllBucketType();
        bool SaveBucketDetails(BucketViewModel bucketViewModel, long uniqueSessionId);
        bool DeleteBucketDetails(long id, long uniqueSessionId);
        bool UpdateBucketDetails(BucketViewModel bucketViewModel, long uniqueSessionId);
    }
}
