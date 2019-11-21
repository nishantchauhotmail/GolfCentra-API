using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.Business.Business.Interface
{
    public interface IAppVersionService
    {
        Tuple<bool, string,bool> CheckAppVersion(decimal currentVersion, long platformTypeId);
    }
}
