using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.ViewModel.Admin.FireBase
{
    public class FireBaseModel
    {
        public FireBaseModel()
        {
            data = new Data();
            notification = new Notification();
        }
        public string[] registration_ids { get; set; }
        public bool content_available { get; set; }
        public bool mutable_content { get; set; }
        public string priority { get; set; }
        public Data data { get; set; }
        public Notification notification { get; set; }
    }
    public class Data
    {
        public string message { get; set; }
        public string attachment { get; set; }
        public string media_type { get; set; }
        public string title { get; set; }
    }

    public class Notification
    {
        public string body { get; set; }
        public string sound { get; set; }
        public string title { get; set; }
    }

    public class FireBaseOperation
    {
        public string Operation { get; set; }
        public string notification_key_name { get; set; }
        public string registration_ids { get; set; }
     
    }
}
