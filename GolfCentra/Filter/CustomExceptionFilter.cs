using GolfCentra.Core.ResponseModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;

namespace GolfCentra.Filter
{
    public class CustomExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            ResponseViewModel<string> responseViewModel = new ResponseViewModel<string>();

            String message = String.Empty;
            var exceptionType = actionExecutedContext.Exception.GetType();

            responseViewModel = ResponseViewModel<string>.Failed(Core.ResponseModel.StatusCode.Not_Found, actionExecutedContext.Exception.Message, "Something Went Wrong", actionExecutedContext.Exception, "",string.Empty);
            JsonSerializerSettings jss = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };

            actionExecutedContext.Response = new HttpResponseMessage()
            {
                Content = new StringContent(JsonConvert.SerializeObject(responseViewModel, Formatting.Indented, jss), System.Text.Encoding.UTF8, "application/json"),
                StatusCode = HttpStatusCode.NotFound
            };
            base.OnException(actionExecutedContext);
        }
    }
}