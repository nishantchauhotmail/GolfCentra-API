using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http.ExceptionHandling;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Results;
using GolfCentra.Core.ResponseModel;

namespace GolfCentra.Filter
{
    public class GlobalExceptionHandler : ExceptionHandler
    {
        public async override Task HandleAsync(ExceptionHandlerContext context, CancellationToken cancellationToken)
        {
            // Access Exception using context.Exception;  
            const string errorMessage = "An unexpected error occured";
           
            ResponseViewModel<string> responseViewModel = new ResponseViewModel<string>();
            responseViewModel = ResponseViewModel<string>.Failed(Core.ResponseModel.StatusCode.Not_Found, "An unexpected error occured", "Something Goes Wrong", null, "",string.Empty);
            var response = context.Request.CreateResponse(HttpStatusCode.InternalServerError,
               new
               {
                   Message = responseViewModel
               });
            response.Headers.Add("X-Error", errorMessage);
            context.Result = new ResponseMessageResult(response);



        }
    }
}
