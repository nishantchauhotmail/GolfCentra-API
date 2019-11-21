using GolfCentra.ViewModel.Admin;
using GolfCentra.ViewModel.Admin.FireBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.Business.BusinessAdmin.Interface
{
    public interface ISendPushNotificationService
    {
        bool SendNotification(NotificationGolferViewModel fireBaseViewModel,long uniqueSessionId);
        List<NotificationGolferViewModel> FindAllNotification();
    }
}
