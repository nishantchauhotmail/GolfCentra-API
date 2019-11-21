using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.Core.ResponseModel
{
    public class IdentityError
    {
        /// <summary>
        /// Gets or sets the code for this error.
        /// </summary>
        /// <value>
        /// The code for this error.
        /// </value>
        public StatusCode Code { get; set; }

        /// <summary>
        /// Gets or sets the description for this error.
        /// </summary>
        /// <value>
        /// The description for this error.
        /// </value>
        public string Message { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Exception Exception { get; set; }
    }

    public enum StatusCode
    {
        OK = 200,
        Bad_Request = 400,
        Unauthorized = 401,
        Payment_Required = 402,
        Forbidden = 403,
        Not_Found = 404,
        Internal_Server_Error = 500,
        Service_Unavailable = 503,
        Not_Implemented = 501,
        Bad_Gateway = 502,
        Network_Connect_Timeout_Error = 599,
        No_Record_Found = 201
    }
}
