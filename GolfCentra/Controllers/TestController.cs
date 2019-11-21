using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace GolfCentra.Controllers
{
    public class TestController : Controller
    {
        // GET: Test


        public ActionResult Index()
        {
            RootObject ngo = new RootObject();
            string baseurl = "https://ngodarpan.gov.in/ngodarpanws_ndci/apin/getngo/DL_2017_0165260";
            using (var client = new HttpClient())
            {
                //JSParser parse = new JSParser();

                client.BaseAddress = new Uri(baseurl);
                //client.DefaultRequestHeaders.Clear();
                //client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = client.GetAsync(baseurl).Result;
                //JsonObjectAttribute jobj = (JsonObjectAttribute)parse.Parse(Res);
                if (Res.IsSuccessStatusCode)
                {

                    var response = Res.Content.ReadAsStringAsync().Result;
                    JavaScriptSerializer jsserial = new JavaScriptSerializer();
                    ngo = (RootObject)jsserial.Deserialize(response, typeof(RootObject));
                    //ngo = JsonConvert.DeserializeObject<List<ngodarpan>>(response);
                    List<Ngodetail> nn = ngo.ngodetails.ToList();
                    foreach(Ngodetail ngolist in nn)
                    {
                        //ViewBag.json = ngolist;
                        //Response.Write("organisation name=" + ngolist.name);
                    }
                }
                return View(ngo);
            }
        }

    }

    public class Ngodetail
    {
        public string id { get; set; }
        public string name { get; set; }
        public string registrationNumber { get; set; }
        public string panNumber { get; set; }
        public string panVerifyStatus { get; set; }
    }

    public class Memberdetail
    {
        public string name { get; set; }
        public string designation { get; set; }
        public string aadhaarNumber { get; set; }
        public string aadhaarVerifyStatus { get; set; }
        public string panNumber { get; set; }
        public string panVerifyStatus { get; set; }
        public string LastUpdated { get; set; }
    }

    public class RootObject
    {
        public List<Ngodetail> ngodetails { get; set; }
        public List<Memberdetail> memberdetails { get; set; }
    }
}


