using System;
using GolfCentra.ViewModel.Admin;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.Business.BusinessAdmin.Interface
{
   public interface IMemberTypeService
    {
        List<MemberTypeViewModel> GetAllMemberType();
        bool SaveMemberTypeDetails(string memberType, string memberTypeValue, long uniqueSessionId);
        bool UpdateMemberTypeDetails(long id, String memberType, string valueToShow, long uniqueSessionId);
        bool DeleteMemberTypeDetails(long id, long uniqueSessionId);
    }
}
