using GolfCentra.Core.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GolfCentra.Controllers
{
    public class ErrorController : ApiController
    {
        /// <summary>
        /// Error Page for API
        /// </summary>
        /// <returns> Returns</returns>
        [HttpGet, HttpPost, HttpPut, HttpDelete, HttpHead, HttpOptions, AcceptVerbs("PATCH")]
        public IHttpActionResult Handle404()
        {

            // changes

            ResponseViewModel<string> responseViewModel = new ResponseViewModel<string>();

            responseViewModel = ResponseViewModel<string>.Failed(Core.ResponseModel.StatusCode.Bad_Request, "The requested resource is not found", "Something Goes Wrong", null, "");
            return Content(HttpStatusCode.BadRequest, responseViewModel);


        }

    }
}
