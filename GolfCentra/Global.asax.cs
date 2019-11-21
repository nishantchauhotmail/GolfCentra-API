using GolfCentra.Core.ResponseModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace GolfCentra
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            // GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.All;

            //GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects;
            //GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            //GlobalConfiguration.Configuration.Formatters.Remove(GlobalConfiguration.Configuration.Formatters.XmlFormatter);
            //        GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings =
            //new JsonSerializerSettings
            //{
            //    DateFormatHandling = DateFormatHandling.IsoDateFormat,
            //    DateTimeZoneHandling = DateTimeZoneHandling.Unspecified,
            //    Culture = CultureInfo.GetCultureInfo("ta-IN")
            //};
            CultureInfo cInfo = new CultureInfo("en-IN");
            cInfo.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            cInfo.DateTimeFormat.DateSeparator = "/";
            Thread.CurrentThread.CurrentCulture = cInfo;
            Thread.CurrentThread.CurrentUICulture = cInfo;
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings =
  new JsonSerializerSettings
  {
      Culture = cInfo
  };
            MvcHandler.DisableMvcResponseHeader = true;

        }

        protected void Application_PreSendRequestHeaders()
        {
            Request.Headers.Remove("X-AspNetMvc-Version");
            Request.Headers.Remove("Server");
            Request.Headers.Remove("X-AspNet-Version");
            Request.Headers.Remove("X-Powered-By");
            Response.Headers.Remove("Server");
            Response.Headers.Remove("X-AspNet-Version");
            Response.Headers.Remove("X-Powered-By");
            Response.Headers.Remove("X-AspNetMvc-Version");
        }


        protected void Application_Error(object sender, EventArgs e)
        {
            // Get the exception object.
            Exception exc = Server.GetLastError();



            ResponseViewModel<string> responseViewModel = new ResponseViewModel<string>();

            String message = String.Empty;

            responseViewModel = ResponseViewModel<string>.Failed(Core.ResponseModel.StatusCode.Not_Found, exc.Message, "Something Goes Wrong", exc, "", string.Empty);

            Server.ClearError();
        }
    }
}
