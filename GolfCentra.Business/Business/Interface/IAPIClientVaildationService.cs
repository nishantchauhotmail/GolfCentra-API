using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.Business.Business.Interface
{
    public interface IAPIClientVaildationService
    {
        bool IsLoggedInClientVaild(string uniqueSessionId, string apiUserName, string apiPassword);
        bool IsClientVaild(string apiUserName, string apiPassword);
        bool IsLoggedInClientVaildWithGolferUnBlock(string uniqueSessionId, string apiUserName, string apiPassword);
    }
}
