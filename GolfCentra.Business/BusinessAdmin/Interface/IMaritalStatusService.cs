using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GolfCentra.ViewModel.Admin;

namespace GolfCentra.Business.BusinessAdmin.Interface
{
   public interface IMaritalStatusService
    {
        List<MaritalStatusViewModel> GetAllMaritalStatusType();
    }
}
