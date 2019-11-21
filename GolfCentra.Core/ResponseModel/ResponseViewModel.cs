using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.Core.ResponseModel
{
    public class ResponseViewModel<T>
    {
        public ResponseViewModel()
        {

        }
        /// <summary>
        /// IdentityResult Class For Common Response
        /// </summary>
        public IdentityResult IdentityResult { get; set; }

        /// <summary>
        /// Dynamic Content
        /// </summary>
        public T Content { get; set; }

        /// <summary>
        /// When API Failed
        /// </summary>
        /// <param name="code"></param>
        /// <param name="errorDescription"></param>
        /// <param name="message"></param>
        /// <param name="exp"></param>
        /// <param name="accessToken"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static ResponseViewModel<T> Failed(StatusCode code = StatusCode.Bad_Request, string errorDescription = "Internal Server Error", string message = "", Exception exp = null, string accessToken = null, T content = default(T))
        {
            IdentityError error = new IdentityError();
            if (Core.Helper.Constants.ErrorDisplay.IsErrorDisplay)
            {
                error = new IdentityError { Code = code, Message = errorDescription, Exception = exp };
            }
            else {
                error = new IdentityError { Code = code, Message = errorDescription };
                    }
            var identityResult = IdentityResult.Failed(error);
            identityResult.Message = message;
            return new ResponseViewModel<T>() { IdentityResult = identityResult, Content = content };
        }

        /// <summary>
        /// When API Successfully Hit
        /// </summary>
        /// <param name="content"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ResponseViewModel<T> Succeeded(T content, string message)
        {
            var identityResult = IdentityResult.Success;
            identityResult.Message = message;
          
            return new ResponseViewModel<T>() { Content = content, IdentityResult = identityResult };
        }
    }
}