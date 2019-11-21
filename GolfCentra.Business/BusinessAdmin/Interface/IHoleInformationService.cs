using GolfCentra.ViewModel.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.Business.BusinessAdmin.Interface
{
    public interface IHoleInformationService
    {
        List<HoleViewModel> GetHoleNumberList();
        HoleViewModel GetHoleDetailsByHoleNumberId(long holeNumberId);
        void SaveUpdateHoleInformation(HoleViewModel holeViewModel,long uniqueSessionId);
        List<HoleTypeViewModel> GetAllHoleType();
    }
}
