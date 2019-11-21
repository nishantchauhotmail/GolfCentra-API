using GolfCentra.Business.BusinessAdmin.Interface;
using GolfCentra.Core;
using GolfCentra.Core.DataBase;
using GolfCentra.ViewModel.Admin;
using GolfCentra.ViewModel.Admin.FireBase;
using GolfCentra.ViewModel.Admin.LoginActivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.Business.BusinessAdmin.Implementation
{
    public class SendPushNotificationService : ISendPushNotificationService
    {
        private readonly UnitOfWork _unitOfWork;

        public SendPushNotificationService()
        {
            _unitOfWork = new UnitOfWork();
        }

        public bool SendNotification(NotificationGolferViewModel fireBaseViewModel,long uniqueSessionId)
        {
            List<LogInHistory> loginHistories = _unitOfWork.LogInHistoryRepository.GetMany(x => x.IsActive == true).ToList();

            FireBaseModel fireBaseModel = new FireBaseModel
            {
                registration_ids = loginHistories.Where(x => x.DeviceTokenId != "" && x.DeviceTokenId != null && x.PlatformTypeId == 1).Select(x => x.DeviceTokenId).Distinct().ToArray(),
                content_available = true,
                mutable_content = true,
                priority = "high"
            };
            fireBaseModel.notification.sound = "default";
            fireBaseModel.notification.title = fireBaseViewModel.Title;
            fireBaseModel.notification.body = fireBaseViewModel.Message;
            fireBaseModel.data.attachment = "";
            fireBaseModel.data.media_type = "";
            fireBaseModel.data.message = "";
            fireBaseModel.data.title = "";

            FireBaseModel fireBaseModelAND = new FireBaseModel
            {
                registration_ids = loginHistories.Where(x => x.DeviceTokenId != "" && x.DeviceTokenId != null && x.PlatformTypeId==2).Select(x => x.DeviceTokenId).Distinct().ToArray(),
                content_available = true,
                mutable_content = true,
                priority = "high"
            };
             fireBaseModelAND.data.attachment = "";
            fireBaseModelAND.data.media_type = "";
            fireBaseModelAND.data.message = fireBaseViewModel.Message;
            fireBaseModelAND.data.title = fireBaseViewModel.Title;

            HttpClient client = new HttpClient
            {
                BaseAddress = new Uri("https://fcm.googleapis.com/")
            };
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/Json"));
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", Core.Helper.Constants.FireBase.FireBaseServerKey);
            HttpResponseMessage response1 = client.PostAsJsonAsync("fcm/send", fireBaseModelAND).Result;
            HttpResponseMessage response = client.PostAsJsonAsync("fcm/send", fireBaseModel).Result;
            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                GolferNotification golferNotification = new GolferNotification()
                {
                    Title = fireBaseViewModel.Title,
                    Message = fireBaseViewModel.Message,
                    GolferId = string.Join(",", loginHistories.Where(x => x.DeviceTokenId != "" && x.DeviceTokenId != null).Select(x => x.UserId).Distinct().ToArray()),
                    IsActive = true,
                    CreatedOn = System.DateTime.UtcNow
                };

                _unitOfWork.GolferNotificationRepository.Insert(golferNotification);
                _unitOfWork.Save();
                try
                {

                    SessionActivityPageViewModel sessionActivityPageViewModel = new SessionActivityPageViewModel()
                    {
                        ControllerName = "Push Notification",
                        ActionName = "Save",
                        PerformOn = golferNotification.GolferNotificationId.ToString(),
                        LoginHistoryId = uniqueSessionId,

                        Info = "Send a Notification with id- " + golferNotification.GolferNotificationId.ToString() + ".Notification have following details"+
                          Environment.NewLine + "Tittle " + golferNotification.Title + "" 
                          + Environment.NewLine + "Message " + golferNotification.Message + ""
                           + Environment.NewLine + "GolferId " + golferNotification.GolferId + ""
                    };
                    new Common.AddSessionActivity().SaveSessionActivity(sessionActivityPageViewModel);

                }
                catch (Exception)
                {

                }

                return true;
            }
            else
            { return false; }

        }


        public string FindNotificationKey()
        {
            HttpClient client = new HttpClient
            {
                BaseAddress = new Uri(" https://fcm.googleapis.com/fcm/notification?notification_key_name=ClassicGolfCentra")
            };
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/Json"));
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "key=AAAAqFNaR9U:APA91bEhrX7f3KpHnVQeqEQ_nLhwQCdKonq5elfVdKvO7rIZ45arbxAMlzjGgkb0chU-5TPoZfW-tWe0wkNJbQqtNVsDXIHccxGiEam4jAoBwKkdqLkjF9gXbQwKSE0UzqncS0lEFqYq");
            client.DefaultRequestHeaders.TryAddWithoutValidation("project_id", "722952931285");

            HttpResponseMessage response = client.GetAsync("fcm/send").Result;
            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                return data;
            }
            else
            {
                List<LogInHistory> loginHistories = _unitOfWork.LogInHistoryRepository.GetMany(x => x.IsActive == true && x.LoggedOutAt == null).ToList();

                HttpClient client1 = new HttpClient
                {
                    BaseAddress = new Uri("https://fcm.googleapis.com/")
                };
                client1.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/Json"));
                client1.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "key=AAAAqFNaR9U:APA91bEhrX7f3KpHnVQeqEQ_nLhwQCdKonq5elfVdKvO7rIZ45arbxAMlzjGgkb0chU-5TPoZfW-tWe0wkNJbQqtNVsDXIHccxGiEam4jAoBwKkdqLkjF9gXbQwKSE0UzqncS0lEFqYq");

                FireBaseOperation fireBaseOperation = new FireBaseOperation()
                {

                    Operation = "create",
                    notification_key_name = "ClassicGolfCentra",
                    registration_ids = String.Join(",", loginHistories.Where(x => x.DeviceTokenId != "" && x.DeviceTokenId != null).Select(x => x.DeviceTokenId))
                };

                HttpResponseMessage response1 = client.PostAsJsonAsync("fcm/notification", fireBaseOperation).Result;
                if (response1.IsSuccessStatusCode)
                {
                    var data = response1.Content.ReadAsStringAsync().Result;
                    return data;
                }
                else
                { return ""; }


            }

        }


        public List<NotificationGolferViewModel> FindAllNotification()
        {
            List<NotificationGolferViewModel> notificationGolferViewModels = new List<NotificationGolferViewModel>();
            List<GolferNotification> golferNotifications = _unitOfWork.GolferNotificationRepository.GetMany(x => x.IsActive == true).OrderByDescending(x=>x.CreatedOn).ToList();
            foreach (var item in golferNotifications)
            {
                NotificationGolferViewModel notificationGolferViewModel = new NotificationGolferViewModel()
                {
                    Title = item.Title,
                    Message = item.Message,
                    SentOn = Core.Helper.DateHelper.ConvertSystemDateToCurrent(item.CreatedOn)
                };
                notificationGolferViewModels.Add(notificationGolferViewModel);
            };
            return notificationGolferViewModels.ToList();
        }

    }
}
