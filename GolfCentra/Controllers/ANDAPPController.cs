using GolfCentra.Business.Business.Implementation;
using GolfCentra.Business.Business.Interface;
using GolfCentra.Core.DataBase;
using GolfCentra.Core.Helper;
using GolfCentra.Core.ResponseModel;
using GolfCentra.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Routing;
using static GolfCentra.Filter.MyAttribute;

namespace GolfCentra.Controllers
{
    public class ANDAPPController : ApiController
    {
        #region Private member variables...
        private readonly ICourseDetailsService _courseDetailsService;
        private readonly IGolferAccountService _golferAccountService;
        private readonly IAPIClientVaildationService _apiClientVaildationService;
        private readonly IScoreService _scoreService;
        private readonly IBookingService _bookingService;
        private readonly IStaticPageService _staticPageService;
        private readonly IPaytmPaymentService _paytmPaymentService;
        private readonly IAppVersionService _appVersionService;
        private readonly IPromotionsCouponService _promotionsCouponService;
        private readonly IBookingDetailService _bookingDetailService;
        #endregion

        #region Public Constructor...
        public ANDAPPController()
        {
            this._courseDetailsService = new CourseDetailsService();
            this._golferAccountService = new GolferAccountService();
            this._apiClientVaildationService = new APIClientVaildationService();
            this._scoreService = new ScoreService();
            this._bookingService = new BookingService();
            this._staticPageService = new StaticPageService();
            this._paytmPaymentService = new PaytmPaymentService();
            _appVersionService = new AppVersionService();
            _promotionsCouponService = new PromotionsCouponService();
            _bookingDetailService = new BookingDetailService();
        }
        #endregion

        #region Login Register
        /// <summary>
        /// Phone Code List
        /// </summary>
        /// <returns>Return Phone Code List</returns> 
        [HttpPost]
        public IHttpActionResult GetPhoneCodeList([FromBody]GolferViewModel golferViewModel)
        {
            ResponseViewModel<List<PhoneCodeViewModel>> responseViewModel = new ResponseViewModel<List<PhoneCodeViewModel>>();
            try
            {
                if (_apiClientVaildationService.IsClientVaild(golferViewModel.ApiUserName, golferViewModel.ApiPassword))
                {
                    var data = _golferAccountService.GetPhoneCodeList();
                    responseViewModel = data.Any() ? ResponseViewModel<List<PhoneCodeViewModel>>.Succeeded(data, "") : ResponseViewModel<List<PhoneCodeViewModel>>.Succeeded(data, "No Record Found");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<List<PhoneCodeViewModel>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new List<PhoneCodeViewModel>());
                    return Ok(responseViewModel);
                }

            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<List<PhoneCodeViewModel>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new List<PhoneCodeViewModel>());
                return Ok(responseViewModel);
            }

        }

        /// <summary>Save Golfer Deatials
        /// 
        /// </summary>
        /// <param name="golferViewModel">Golfer Details</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult Register([FromBody]CommonViewModel golferViewModel)
        {
            ResponseViewModel<Dictionary<string, long>> responseViewModel = new ResponseViewModel<Dictionary<string, long>>();

            try
            {
                if (_apiClientVaildationService.IsClientVaild(golferViewModel.ApiUserName, golferViewModel.ApiPassword))
                {
                    golferViewModel.Name.TryValidate("Name");
                    golferViewModel.EmailAddress.TryValidateEmail("Email Address");
                    golferViewModel.MobileNumber.TryValidate("Mobile Number");
                    golferViewModel.Password.TryValidate("Password");

                    golferViewModel.PlatformTypeId = (int)Core.Helper.Enum.EnumPlatformType.AND;
                    Golfer golfer = _golferAccountService.AddGolfer(golferViewModel);
                    var data = new Dictionary<string, long>
                    {
                        { "GolferId", golfer.GolferId }
                    };
                    responseViewModel = ResponseViewModel<Dictionary<string, long>>.Succeeded(data, "Verfication Email Sent Successfully. Please Check Your Email.");
                }
                else
                {
                    responseViewModel = ResponseViewModel<Dictionary<string, long>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new Dictionary<string, long> { { "GolferId", 0 } });
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<Dictionary<string, long>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new Dictionary<string, long> { { "GolferId", 0 } });
                return Ok(responseViewModel);
            }
            return Ok(responseViewModel);
        }

        /// <summary>
        /// Resend Verification Code
        /// </summary>
        /// <param name="golferId"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult ResendVerificationCode([FromBody]GolferViewModel golferViewModel)
        {
            ResponseViewModel<Dictionary<string, bool>> responseViewModel = new ResponseViewModel<Dictionary<string, bool>>();
            try
            {
                if (_apiClientVaildationService.IsClientVaild(golferViewModel.ApiUserName, golferViewModel.ApiPassword))
                {
                    golferViewModel.GolferId.TryValidate("Golfer Id");
                    bool status = _golferAccountService.ResendVerficationCode(golferViewModel.GolferId);
                    var data = new Dictionary<string, bool>
                    {
                        { "Status", status }
                    };
                    responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Succeeded(data, "OTP Sent Successfully");
                }
                else
                {
                    responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new Dictionary<string, bool> { { "Status", false } });
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new Dictionary<string, bool> { { "Status", false } });
                return Ok(responseViewModel);
            }
            return Ok(responseViewModel);
        }

        /// <summary>
        /// Verify Golfer Account
        /// </summary>
        /// <param name="golferId"></param>
        /// <param name="verificationCode"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult VerifyGolferAccount([FromBody]GolferViewModel golferViewModel)
        {
            ResponseViewModel<GolferViewModel> responseViewModel = new ResponseViewModel<GolferViewModel>();
            try
            {
                if (_apiClientVaildationService.IsClientVaild(golferViewModel.ApiUserName, golferViewModel.ApiPassword))
                {
                    golferViewModel.GolferId.TryValidate("Golfer Id");
                    golferViewModel.OTP.TryValidate("Verification Code");
                    var data = _golferAccountService.VerifyGolferAccount(golferViewModel.OTP, golferViewModel.GolferId, 1);

                    responseViewModel = ResponseViewModel<GolferViewModel>.Succeeded(data, "Verified Successfully");
                }
                else
                {
                    responseViewModel = ResponseViewModel<GolferViewModel>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new GolferViewModel());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<GolferViewModel>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new GolferViewModel());
                return Ok(responseViewModel);
            }
            return Ok(responseViewModel);
        }

        /// <summary>
        /// Login For Golfer
        /// </summary>
        /// <param name="golferViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult Login([FromBody]GolferViewModel golferViewModel)
        {
            ResponseViewModel<GolferViewModel> responseViewModel = new ResponseViewModel<GolferViewModel>();

            try
            {
                if (_apiClientVaildationService.IsClientVaild(golferViewModel.ApiUserName, golferViewModel.ApiPassword))
                {
                    golferViewModel.MemberShipId.TryValidate("MemberShipId");
                    golferViewModel.Password.TryValidate("Password");
                    var data = _golferAccountService.LogInUser(golferViewModel);

                    responseViewModel = ResponseViewModel<GolferViewModel>.Succeeded(data, "Logged In Successfully");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<GolferViewModel>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new GolferViewModel());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<GolferViewModel>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new GolferViewModel());
                return Ok(responseViewModel);
            }
        }

        /// <summary>
        /// Logout For Golfer
        /// </summary>
        /// <param name="commonViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult Logout([FromBody]CommonViewModel commonViewModel)
        {
            ResponseViewModel<Dictionary<string, bool>> responseViewModel = new ResponseViewModel<Dictionary<string, bool>>();

            try
            {
                if (_apiClientVaildationService.IsClientVaild(commonViewModel.ApiUserName, commonViewModel.ApiPassword))
                {
                    var data = _golferAccountService.Logout(commonViewModel.UniqueSessionId);
                    var model = new Dictionary<string, bool>
            {
                { "Status", true }
            };
                    responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "Logged Out Successfully");

                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new Dictionary<string, bool> { { "Status", false } });
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new Dictionary<string, bool> { { "Status", false } });
                return Ok(responseViewModel);

            }
        }

        [HttpPost]
        public IHttpActionResult GetSalutationList([FromBody]GolferViewModel golferViewModel)
        {
            ResponseViewModel<List<SalutationViewModel>> responseViewModel = new ResponseViewModel<List<SalutationViewModel>>();
            try
            {
                if (_apiClientVaildationService.IsClientVaild(golferViewModel.ApiUserName, golferViewModel.ApiPassword))
                {
                    var data = _golferAccountService.GetAllSalutation();
                    responseViewModel = data.Any() ? ResponseViewModel<List<SalutationViewModel>>.Succeeded(data, "") : ResponseViewModel<List<SalutationViewModel>>.Succeeded(data, "No Record Found");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<List<SalutationViewModel>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new List<SalutationViewModel>());
                    return Ok(responseViewModel);
                }

            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<List<SalutationViewModel>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new List<SalutationViewModel>());
                return Ok(responseViewModel);
            }

        }

        [Throttle(Name = "TestThrottle", Message = "You must wait {n} seconds before accessing this url again.", Seconds = 1)]
        /// <summary>
        /// Get Password By Email 
        /// </summary>
        /// <param name="golferViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult ForgetPassword([FromBody]GolferViewModel golferViewModel)
        {
            ResponseViewModel<Dictionary<string, bool>> responseViewModel = new ResponseViewModel<Dictionary<string, bool>>();
            try
            {
                if (_apiClientVaildationService.IsClientVaild(golferViewModel.ApiUserName, golferViewModel.ApiPassword))
                {
                    golferViewModel.MemberShipId.TryValidate("MemberShipId");
                    bool status = _golferAccountService.ForgetPassword(golferViewModel.MemberShipId);
                    var data = new Dictionary<string, bool>
                    {
                        { "Status", status }
                    };
                    responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Succeeded(data, "Reset password link has been shared on your email id.");
                }
                else
                {
                    responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new Dictionary<string, bool> { { "Status", false } });
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new Dictionary<string, bool> { { "Status", false } });
                return Ok(responseViewModel);
            }
            return Ok(responseViewModel);
        }
        #endregion

        #region Course GPS
        /// <summary>
        /// Course GPS Details
        /// </summary>
        /// <returns>Return Course Details like Tee,Hole information</returns>
        [HttpPost]
        public IHttpActionResult GetCourseGPSDetail([FromBody]CommonViewModel commonViewModel)
        {
            ResponseViewModel<List<HoleViewModel>> responseViewModel = new ResponseViewModel<List<HoleViewModel>>();
            try
            {
                commonViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(commonViewModel.UniqueSessionId, commonViewModel.ApiUserName, commonViewModel.ApiPassword))
                {
                    var data = _courseDetailsService.CourseGPSDetails(commonViewModel.CourseNameTypeId);
                    responseViewModel = data.Any() ? ResponseViewModel<List<HoleViewModel>>.Succeeded(data, "") : ResponseViewModel<List<HoleViewModel>>.Succeeded(data, "No Record Found");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<List<HoleViewModel>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new List<HoleViewModel>());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<List<HoleViewModel>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new List<HoleViewModel>());
                return Ok(responseViewModel);
            }
        }
        #endregion

        #region CourseAboutUs
        /// <summary>
        /// Course About Us Details
        /// </summary>
        /// <param name="commonViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult CourseAboutUsDetail([FromBody]CommonViewModel commonViewModel)
        {
            ResponseViewModel<List<AboutUsViewModel>> responseViewModel = new ResponseViewModel<List<AboutUsViewModel>>();
            try
            {
                commonViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(commonViewModel.UniqueSessionId, commonViewModel.ApiUserName, commonViewModel.ApiPassword))
                {
                    var data = _courseDetailsService.CourseAboutUsDetails();
                    responseViewModel = data.Any() ? ResponseViewModel<List<AboutUsViewModel>>.Succeeded(data, "") : ResponseViewModel<List<AboutUsViewModel>>.Succeeded(data, "");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<List<AboutUsViewModel>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new List<AboutUsViewModel>());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<List<AboutUsViewModel>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new List<AboutUsViewModel>());
                return Ok(responseViewModel);
            }
        }
        #endregion

        #region SubmitScore
        /// <summary>
        /// Get Course Details For Score Post
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult CourseDetailForScorPost([FromBody]CommonViewModel commonViewModel)
        {
            ResponseViewModel<List<ScoreViewModel>> responseViewModel = new ResponseViewModel<List<ScoreViewModel>>();
            try
            {
                commonViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaildWithGolferUnBlock(commonViewModel.UniqueSessionId, commonViewModel.ApiUserName, commonViewModel.ApiPassword))
                {
                    var data = _courseDetailsService.GetCourseDetailForScorePost(commonViewModel.CourseHoleTypeId);
                    responseViewModel = data.Any() ? ResponseViewModel<List<ScoreViewModel>>.Succeeded(data, "") : ResponseViewModel<List<ScoreViewModel>>.Succeeded(data, "No Record Found");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<List<ScoreViewModel>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new List<ScoreViewModel>());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<List<ScoreViewModel>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new List<ScoreViewModel>());
                return Ok(responseViewModel);
            }
        }

        /// <summary>
        /// Save Golfer Score
        /// </summary>
        /// <returns></returns>
        public IHttpActionResult SaveScore([FromBody]CommonViewModel commonViewModel)
        {
            ResponseViewModel<Dictionary<string, bool>> responseViewModel = new ResponseViewModel<Dictionary<string, bool>>();

            try
            {
                commonViewModel.UniqueSessionId.TryValidate("Token");

                //JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                //  ScoreDetailsViewModel routes_list = Newtonsoft.Json.JsonConvert.DeserializeObject<ScoreDetailsViewModel>( commonViewModel.CurrencyName);
                if (_apiClientVaildationService.IsLoggedInClientVaildWithGolferUnBlock(commonViewModel.UniqueSessionId, commonViewModel.ApiUserName, commonViewModel.ApiPassword))
                {
                    long id = _golferAccountService.GetGolferIdFromLoginHistoryById(commonViewModel.UniqueSessionId);
                    if (commonViewModel.ScoreViewModels.Count == 0)
                        throw new Exception("No Score Details Found");
                    bool status = _scoreService.SaveScore(commonViewModel.ScoreViewModels, id, commonViewModel.GrossScore, commonViewModel.TotalScore);
                    var data = new Dictionary<string, bool>
                    {
                        { "Status", status }
                    };
                    responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Succeeded(data, "Score Saved Successfully");
                }
                else
                {
                    responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new Dictionary<string, bool> { { "Status", false } });
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new Dictionary<string, bool> { { "Status", false } });
                return Ok(responseViewModel);
            }
            return Ok(responseViewModel);

        }
        #endregion

        #region Score History
        /// <summary>
        /// Get Score List For Different Search Type
        /// </summary>
        /// <param name="commonViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult ScoresData([FromBody]CommonViewModel commonViewModel)
        {
            ResponseViewModel<List<ScoreViewModel>> responseViewModel = new ResponseViewModel<List<ScoreViewModel>>();
            try
            {
                commonViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(commonViewModel.UniqueSessionId, commonViewModel.ApiUserName, commonViewModel.ApiPassword))
                {
                    long id = _golferAccountService.GetGolferIdFromLoginHistoryById(commonViewModel.UniqueSessionId);
                    var data = _scoreService.GetScoreList(id);
                    responseViewModel = data.Any() ? ResponseViewModel<List<ScoreViewModel>>.Succeeded(data, "") : ResponseViewModel<List<ScoreViewModel>>.Succeeded(data, "No Score Found");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<List<ScoreViewModel>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new List<ScoreViewModel>());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<List<ScoreViewModel>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new List<ScoreViewModel>());
                return Ok(responseViewModel);
            }
        }

        [HttpPost]
        public IHttpActionResult GetScoreCardDetail([FromBody]CommonViewModel commonViewModel)
        {
            ResponseViewModel<ScoreDetailsViewModel> responseViewModel = new ResponseViewModel<ScoreDetailsViewModel>();
            try
            {
                commonViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(commonViewModel.UniqueSessionId, commonViewModel.ApiUserName, commonViewModel.ApiPassword))
                {
                  
                    long id = _golferAccountService.GetGolferIdFromLoginHistoryById(commonViewModel.UniqueSessionId);
                    Tuple<long, long, List<ScoreViewModel>> data = _scoreService.GetScoreCardDetailsByScoreId(commonViewModel.ENS, id);
                    ScoreDetailsViewModel model = new ScoreDetailsViewModel()
                    {
                        ScoreId = commonViewModel.ScoreId,
                        ScoreViewModels = data.Item3,
                        GrossScore = data.Item2,
                        TotalScore = data.Item1
                    };
                    responseViewModel = model == null ? ResponseViewModel<ScoreDetailsViewModel>.Succeeded(model, "") : ResponseViewModel<ScoreDetailsViewModel>.Succeeded(model, "");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<ScoreDetailsViewModel>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new ScoreDetailsViewModel());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<ScoreDetailsViewModel>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new ScoreDetailsViewModel());
                return Ok(responseViewModel);
            }
        }
        #endregion

        #region ProfileDetails
        [HttpPost]
        public IHttpActionResult GetGolferDetail([FromBody]CommonViewModel commonViewModel)
        {
            ResponseViewModel<GolferViewModel> responseViewModel = new ResponseViewModel<GolferViewModel>();
            try
            {
                commonViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(commonViewModel.UniqueSessionId, commonViewModel.ApiUserName, commonViewModel.ApiPassword))
                {
                    long id = _golferAccountService.GetGolferIdFromLoginHistoryById(commonViewModel.UniqueSessionId);
                    var data = _golferAccountService.GetGolferDetailByGolferId(id);
                    responseViewModel = data != null ? ResponseViewModel<GolferViewModel>.Succeeded(data, "") : ResponseViewModel<GolferViewModel>.Succeeded(data, "No Record Found");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<GolferViewModel>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new GolferViewModel());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<GolferViewModel>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new GolferViewModel());
                return Ok(responseViewModel);
            }
        }

        [HttpPost]
        public IHttpActionResult ChangePassword([FromBody]CommonViewModel commonViewModel)
        {
            ResponseViewModel<Dictionary<string, bool>> responseViewModel = new ResponseViewModel<Dictionary<string, bool>>();
            try
            {
                commonViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(commonViewModel.UniqueSessionId, commonViewModel.ApiUserName, commonViewModel.ApiPassword))
                {
                    commonViewModel.NewPassword.TryValidate("New Password");
                    commonViewModel.OldPassword.TryValidate("Old Password");
                    long id = _golferAccountService.GetGolferIdFromLoginHistoryById(commonViewModel.UniqueSessionId);
                    var status = _golferAccountService.ChangePassword(id, commonViewModel.OldPassword, commonViewModel.NewPassword,commonViewModel.UniqueSessionId);
                    var data = new Dictionary<string, bool>
                    {
                        { "Status", status }
                    };
                    responseViewModel = status != null ? ResponseViewModel<Dictionary<string, bool>>.Succeeded(data, "Password Changed Successfully") : ResponseViewModel<Dictionary<string, bool>>.Succeeded(data, "Password Changed Successfully");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new Dictionary<string, bool> { { "Status", false } });
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new Dictionary<string, bool> { { "Status", false } });
                return Ok(responseViewModel);
            }
        }

        [HttpPost]
        public IHttpActionResult UpdateGolferDetail([FromBody]GolferViewModel golferViewModel)
        {
            ResponseViewModel<GolferViewModel> responseViewModel = new ResponseViewModel<GolferViewModel>();
            try
            {
                golferViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(golferViewModel.UniqueSessionId, golferViewModel.ApiUserName, golferViewModel.ApiPassword))
                {
                    long id = _golferAccountService.GetGolferIdFromLoginHistoryById(golferViewModel.UniqueSessionId);
                    golferViewModel.GolferId = id;
                    var data = _golferAccountService.UpdateGolferProfile(golferViewModel);
                    responseViewModel = data != null ? ResponseViewModel<GolferViewModel>.Succeeded(data, "") : ResponseViewModel<GolferViewModel>.Succeeded(data, "No Record Found");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<GolferViewModel>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new GolferViewModel());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<GolferViewModel>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new GolferViewModel());
                return Ok(responseViewModel);
            }
        }

        #endregion

        #region Booking Details
        [HttpPost]
        public IHttpActionResult CancelBookingByGolfer([FromBody]CommonViewModel commonViewModel)
        {
            ResponseViewModel<Dictionary<string, bool>> responseViewModel = new ResponseViewModel<Dictionary<string, bool>>();
            try
            {
                commonViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(commonViewModel.UniqueSessionId, commonViewModel.ApiUserName, commonViewModel.ApiPassword))
                {
                    commonViewModel.BookingId.TryValidate("Booking Id");
                    long id = _golferAccountService.GetGolferIdFromLoginHistoryById(commonViewModel.UniqueSessionId);
                    var status = _bookingService.CancelBookingByGolfer(commonViewModel.BookingId, id);
                    var data = new Dictionary<string, bool> { { "Status", status } };
                    responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Succeeded(data, "Booking Is Cancelled");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new Dictionary<string, bool> { { "Status", false } });
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new Dictionary<string, bool> { { "Status", false } });
                return Ok(responseViewModel);
            }
        }

        [HttpPost]
        public IHttpActionResult GetBookingDetail([FromBody]CommonViewModel commonViewModel)
        {
            ResponseViewModel<BookingViewModel> responseViewModel = new ResponseViewModel<BookingViewModel>();
            try
            {
                commonViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(commonViewModel.UniqueSessionId, commonViewModel.ApiUserName, commonViewModel.ApiPassword))
                {
                    long id = _golferAccountService.GetGolferIdFromLoginHistoryById(commonViewModel.UniqueSessionId);
                    var model = _bookingService.GetBookingDetailsByBookingIdAndGolferId(commonViewModel.BookingId, id);

                    responseViewModel = model == null ? ResponseViewModel<BookingViewModel>.Succeeded(model, "") : ResponseViewModel<BookingViewModel>.Succeeded(model, "");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<BookingViewModel>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new BookingViewModel());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<BookingViewModel>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new BookingViewModel());
                return Ok(responseViewModel);
            }
        }

        [HttpPost]
        public IHttpActionResult GetBookingList([FromBody]CommonViewModel commonViewModel)
        {
            ResponseViewModel<List<BookingViewModel>> responseViewModel = new ResponseViewModel<List<BookingViewModel>>();
            try
            {
                commonViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(commonViewModel.UniqueSessionId, commonViewModel.ApiUserName, commonViewModel.ApiPassword))
                {
                    long id = _golferAccountService.GetGolferIdFromLoginHistoryById(commonViewModel.UniqueSessionId);
                    var model = _bookingService.GetBookingsByBookingStatus(id, commonViewModel.BookingSearchTypeId);

                    responseViewModel = model.Any() ? ResponseViewModel<List<BookingViewModel>>.Succeeded(model, "") : ResponseViewModel<List<BookingViewModel>>.Succeeded(model, "No Booking Found");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<List<BookingViewModel>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new List<BookingViewModel>());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<List<BookingViewModel>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new List<BookingViewModel>());
                return Ok(responseViewModel);
            }
        }


        #endregion

        #region StartBooking
        [HttpPost]
        public IHttpActionResult GetBookingRateCard([FromBody]CommonViewModel commonViewModel)
        {
            ResponseViewModel<List<String>> responseViewModel = new ResponseViewModel<List<String>>();
            try
            {
                commonViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaildWithGolferUnBlock(commonViewModel.UniqueSessionId, commonViewModel.ApiUserName, commonViewModel.ApiPassword))
                {
                    commonViewModel.SlotId.TryValidate("Solt Id");
                    commonViewModel.BookingTypeId.TryValidate("Booking Type Id");
                    commonViewModel.Date.TryValidate("Date");
                    long id = _golferAccountService.GetGolferIdFromLoginHistoryById(commonViewModel.UniqueSessionId);

                    var model = _bookingService.GetRateCardByDateAndSlot(commonViewModel.Date, commonViewModel.SlotId, commonViewModel.BookingTypeId, id);

                    responseViewModel = model.Count == 0 ? ResponseViewModel<List<String>>.Succeeded(model, "") : ResponseViewModel<List<String>>.Succeeded(model, "");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<List<String>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new List<string>());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<List<String>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new List<string>());
                return Ok(responseViewModel);
            }
        }

        [HttpPost]
        public IHttpActionResult GetCouponDetails([FromBody]CommonViewModel commonViewModel)
        {
            ResponseViewModel<Dictionary<string, decimal>> responseViewModel = new ResponseViewModel<Dictionary<string, decimal>>();
            try
            {
                commonViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(commonViewModel.UniqueSessionId, commonViewModel.ApiUserName, commonViewModel.ApiPassword))
                {
                    commonViewModel.CouponCode.TryValidate("Coupon Code");
                    var data = _bookingService.GetCouponAmountByCouponCode(commonViewModel.CouponCode);
                    var model = new Dictionary<string, decimal> { { "Amount", data } };
                    responseViewModel = ResponseViewModel<Dictionary<string, decimal>>.Succeeded(model, "");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<Dictionary<string, decimal>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new Dictionary<string, decimal> { { "Amount", 0 } });
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<Dictionary<string, decimal>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new Dictionary<string, decimal> { { "Amount", 0 } });
                return Ok(responseViewModel);
            }
        }

        [HttpPost]
        public IHttpActionResult GetSlotDetails([FromBody]CommonViewModel commonViewModel)
        {
            ResponseViewModel<List<SessionSlotViewModel>> responseViewModel = new ResponseViewModel<List<SessionSlotViewModel>>();
            try
            {
                commonViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaildWithGolferUnBlock(commonViewModel.UniqueSessionId, commonViewModel.ApiUserName, commonViewModel.ApiPassword))
                {
                    commonViewModel.BookingTypeId.TryValidate("Booking Type Id");
                    commonViewModel.Date.TryValidate("Date");
                    var model = _bookingService.GetSlotDetailsByDateAndBookingType(commonViewModel.Date, commonViewModel.BookingTypeId, commonViewModel.CoursePairingId);

                    responseViewModel = ResponseViewModel<List<SessionSlotViewModel>>.Succeeded(model, "");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<List<SessionSlotViewModel>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new List<SessionSlotViewModel>());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<List<SessionSlotViewModel>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new List<SessionSlotViewModel>());
                return Ok(responseViewModel);
            }
        }

        [HttpPost]
        public IHttpActionResult GetPricingCalculationBTT([FromBody]CommonViewModel commonViewModel)
        {
            ResponseViewModel<BookingPricingViewModel> responseViewModel = new ResponseViewModel<BookingPricingViewModel>();
            try
            {
                commonViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaildWithGolferUnBlock(commonViewModel.UniqueSessionId, commonViewModel.ApiUserName, commonViewModel.ApiPassword))
                {
                    commonViewModel.HoleTypeId.TryValidate("Hole Type Id");
                    commonViewModel.Date.TryValidate("Date");
                    commonViewModel.SlotId.TryValidate("Slot Id");
                    commonViewModel.NoOfPlayer.TryValidate("No Of Player");
                    long id = _golferAccountService.GetGolferIdFromLoginHistoryById(commonViewModel.UniqueSessionId);

                    //if (commonViewModel.NoOfPlayer != (commonViewModel.NoOfMemberPlayer + commonViewModel.NoOfNonMemberPlayer)) { throw new Exception("Invalid Player Details."); }
                    var model = _bookingService.GetPricingCalculation(commonViewModel.HoleTypeId, commonViewModel.Date, commonViewModel.SlotId, commonViewModel.NoOfPlayer, commonViewModel.NoOfMemberPlayer, commonViewModel.NoOfNonMemberPlayer, commonViewModel.MemberTypeViewModels, id, commonViewModel.CoursePairingId, commonViewModel.BookingPlayerDetailViewModels,commonViewModel.PromotionsId);

                    responseViewModel = ResponseViewModel<BookingPricingViewModel>.Succeeded(model, "");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<BookingPricingViewModel>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new BookingPricingViewModel());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<BookingPricingViewModel>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new BookingPricingViewModel());
                return Ok(responseViewModel);
            }
        }

        [HttpPost]
        public IHttpActionResult GetTeeTimeBookingId([FromBody]CommonViewModel commonViewModel)
        {
            ResponseViewModel<BookingViewModel> responseViewModel = new ResponseViewModel<BookingViewModel>();
            try
            {
                commonViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaildWithGolferUnBlock(commonViewModel.UniqueSessionId, commonViewModel.ApiUserName, commonViewModel.ApiPassword))
                {
                    commonViewModel.HoleTypeId.TryValidate("Hole Type Id");
                    commonViewModel.Date.TryValidate("Date");
                    commonViewModel.SlotId.TryValidate("Slot Id");
                    commonViewModel.NoOfPlayer.TryValidate("No Of Player");
                    //       if (commonViewModel.NoOfPlayer != (commonViewModel.NoOfMemberPlayer + commonViewModel.NoOfNonMemberPlayer)) { throw new Exception("Invalid Player Details."); }
                    long id = _golferAccountService.GetGolferIdFromLoginHistoryById(commonViewModel.UniqueSessionId);
                    commonViewModel.BookingStatusId = (int)Core.Helper.Enum.EnumBookingStatus.Pending;
                    commonViewModel.GolferId = id;
                    commonViewModel.PaymentStatusId = (int)Core.Helper.Enum.EnumPaymentStatus.Pending;
                    var model = _bookingService.SaveTeeTimeBooking(commonViewModel);
                    responseViewModel = ResponseViewModel<BookingViewModel>.Succeeded(model, "");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<BookingViewModel>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new BookingViewModel());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<BookingViewModel>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new BookingViewModel());
                return Ok(responseViewModel);
            }
        }

        [HttpPost]
        public IHttpActionResult GetBucketDetailList([FromBody]CommonViewModel commonViewModel)
        {
            ResponseViewModel<List<BucketViewModel>> responseViewModel = new ResponseViewModel<List<BucketViewModel>>();
            try
            {
                commonViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaildWithGolferUnBlock(commonViewModel.UniqueSessionId, commonViewModel.ApiUserName, commonViewModel.ApiPassword))
                {
                    long id = _golferAccountService.GetGolferIdFromLoginHistoryById(commonViewModel.UniqueSessionId);
                    var model = _bookingService.GetBucketDetailList(commonViewModel.Date, id);
                    responseViewModel = model.Count == 0 ? ResponseViewModel<List<BucketViewModel>>.Succeeded(model, "") : ResponseViewModel<List<BucketViewModel>>.Succeeded(model, "");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<List<BucketViewModel>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new List<BucketViewModel>());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<List<BucketViewModel>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new List<BucketViewModel>());
                return Ok(responseViewModel);
            }
        }

        [HttpPost]
        public IHttpActionResult GetPricingCalculationBDT([FromBody]CommonViewModel commonViewModel)
        {
            ResponseViewModel<BookingPricingViewModel> responseViewModel = new ResponseViewModel<BookingPricingViewModel>();
            try
            {
                commonViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaildWithGolferUnBlock(commonViewModel.UniqueSessionId, commonViewModel.ApiUserName, commonViewModel.ApiPassword))
                {
                    commonViewModel.Date.TryValidate("Date");
                    commonViewModel.SlotId.TryValidate("Slot Id");
                    long id = _golferAccountService.GetGolferIdFromLoginHistoryById(commonViewModel.UniqueSessionId);

                    commonViewModel.NoOfPlayer.TryValidate("No Of Player");
                    //if (commonViewModel.NoOfPlayer != (commonViewModel.NoOfMemberPlayer + commonViewModel.NoOfNonMemberPlayer)) { throw new Exception("Invalid Player Details."); }
                    var model = _bookingService.GetPricingCalculationBDT(commonViewModel.BucketId, commonViewModel.Date, commonViewModel.SlotId, commonViewModel.NoOfPlayer, commonViewModel.NoOfMemberPlayer, commonViewModel.NoOfNonMemberPlayer, commonViewModel.MemberTypeViewModels, id);

                    responseViewModel = ResponseViewModel<BookingPricingViewModel>.Succeeded(model, "");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<BookingPricingViewModel>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new BookingPricingViewModel());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<BookingPricingViewModel>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new BookingPricingViewModel());
                return Ok(responseViewModel);
            }
        }

        [HttpPost]
        public IHttpActionResult GetDrivingRangeBookingId([FromBody]CommonViewModel commonViewModel)
        {
            ResponseViewModel<BookingViewModel> responseViewModel = new ResponseViewModel<BookingViewModel>();
            try
            {
                commonViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(commonViewModel.UniqueSessionId, commonViewModel.ApiUserName, commonViewModel.ApiPassword))
                {
                    commonViewModel.Date.TryValidate("Date");
                    commonViewModel.SlotId.TryValidate("Slot Id");
                    commonViewModel.NoOfPlayer.TryValidate("No Of Player");
                    //  if (commonViewModel.NoOfPlayer != (commonViewModel.NoOfMemberPlayer + commonViewModel.NoOfNonMemberPlayer)) { throw new Exception("Invalid Player Details."); }

                    long id = _golferAccountService.GetGolferIdFromLoginHistoryById(commonViewModel.UniqueSessionId);
                    commonViewModel.BookingStatusId = (int)Core.Helper.Enum.EnumBookingStatus.Pending;
                    commonViewModel.GolferId = id;
                    commonViewModel.PaymentStatusId = (int)Core.Helper.Enum.EnumBookingStatus.Pending;
                    var model = _bookingService.SaveDrivingRangeBooking(commonViewModel);
                    responseViewModel = ResponseViewModel<BookingViewModel>.Succeeded(model, "");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<BookingViewModel>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new BookingViewModel());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<BookingViewModel>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new BookingViewModel());
                return Ok(responseViewModel);
            }
        }

        [HttpPost]
        public IHttpActionResult GetMemberTypes([FromBody]CommonViewModel commonViewModel)
        {
            ResponseViewModel<List<MemberTypeViewModel>> responseViewModel = new ResponseViewModel<List<MemberTypeViewModel>>();
            try
            {
                commonViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaildWithGolferUnBlock(commonViewModel.UniqueSessionId, commonViewModel.ApiUserName, commonViewModel.ApiPassword))
                {
                    long id = _golferAccountService.GetGolferIdFromLoginHistoryById(commonViewModel.UniqueSessionId);
                    var model = _bookingService.GetAllMemberType(id);

                    responseViewModel = ResponseViewModel<List<MemberTypeViewModel>>.Succeeded(model, "");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<List<MemberTypeViewModel>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new List<MemberTypeViewModel>());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<List<MemberTypeViewModel>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new List<MemberTypeViewModel>());
                return Ok(responseViewModel);
            }
        }

        #endregion

        #region Static Page

        [HttpPost]
        public IHttpActionResult GetRuleAndRegulation([FromBody]CommonViewModel commonViewModel)
        {
            ResponseViewModel<Dictionary<string, string>> responseViewModel = new ResponseViewModel<Dictionary<string, string>>();
            try
            {
                commonViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(commonViewModel.UniqueSessionId, commonViewModel.ApiUserName, commonViewModel.ApiPassword))
                {

                    var data = _courseDetailsService.GetRuleAndRegulationPageURL();
                    var model = new Dictionary<string, string>
                    {
                        { "URL", data ??string.Empty }
                    };
                    responseViewModel = model.Count == 0 ? ResponseViewModel<Dictionary<string, string>>.Succeeded(model, "") : ResponseViewModel<Dictionary<string, string>>.Succeeded(model, "");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<Dictionary<string, string>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new Dictionary<string, string> { { "URL", "" } });
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<Dictionary<string, string>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new Dictionary<string, string> { { "URL", "" } });
                return Ok(responseViewModel);
            }
        }

        [HttpPost]
        public IHttpActionResult GetCourseContactUsDetails([FromBody]CommonViewModel commonViewModel)
        {
            ResponseViewModel<ContactUsViewModel> responseViewModel = new ResponseViewModel<ContactUsViewModel>();
            try
            {
                commonViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(commonViewModel.UniqueSessionId, commonViewModel.ApiUserName, commonViewModel.ApiPassword))
                {
                    var model = _courseDetailsService.GetCourseContactUsDetails();
                    responseViewModel = model == null ? ResponseViewModel<ContactUsViewModel>.Succeeded(model, "") : ResponseViewModel<ContactUsViewModel>.Succeeded(model, "");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<ContactUsViewModel>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new ContactUsViewModel());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<ContactUsViewModel>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new ContactUsViewModel());
                return Ok(responseViewModel);
            }
        }


        [HttpPost]
        public IHttpActionResult GetPrivacyPolicyPageURL([FromBody]CommonViewModel commonViewModel)
        {
            ResponseViewModel<Dictionary<string, string>> responseViewModel = new ResponseViewModel<Dictionary<string, string>>();
            try
            {
                commonViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(commonViewModel.UniqueSessionId, commonViewModel.ApiUserName, commonViewModel.ApiPassword))
                {

                    var data = _courseDetailsService.GetPrivacyPolicyPageURL();
                    var model = new Dictionary<string, string>
                    {
                        { "URL", data ??string.Empty }
                    };
                    responseViewModel = model.Count == 0 ? ResponseViewModel<Dictionary<string, string>>.Succeeded(model, "") : ResponseViewModel<Dictionary<string, string>>.Succeeded(model, "");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<Dictionary<string, string>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new Dictionary<string, string> { { "URL", "" } });
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<Dictionary<string, string>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new Dictionary<string, string> { { "URL", "" } });
                return Ok(responseViewModel);
            }
        }

        [HttpPost]
        public IHttpActionResult GetTermAndConditionPageURL([FromBody]CommonViewModel commonViewModel)
        {
            ResponseViewModel<Dictionary<string, string>> responseViewModel = new ResponseViewModel<Dictionary<string, string>>();
            try
            {
                if (_apiClientVaildationService.IsClientVaild(commonViewModel.ApiUserName, commonViewModel.ApiPassword))
                {

                    var data = _courseDetailsService.GetTermAndConditionPageURL();
                    var model = new Dictionary<string, string>
                    {
                        { "URL", data ??string.Empty }
                    };
                    responseViewModel = model.Count == 0 ? ResponseViewModel<Dictionary<string, string>>.Succeeded(model, "") : ResponseViewModel<Dictionary<string, string>>.Succeeded(model, "");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<Dictionary<string, string>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new Dictionary<string, string> { { "URL", "" } });
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<Dictionary<string, string>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new Dictionary<string, string> { { "URL", "" } });
                return Ok(responseViewModel);
            }
        }
        #endregion


        [HttpPost]
        public IHttpActionResult CheckAppUpdate([FromBody]CommonViewModel commonViewModel)
        {
            ResponseViewModel<AppVersionViewModel> responseViewModel = new ResponseViewModel<AppVersionViewModel>();
            try
            {
                if (_apiClientVaildationService.IsClientVaild(commonViewModel.ApiUserName, commonViewModel.ApiPassword))
                {
                    if (commonViewModel.AppVersion == 0 || commonViewModel.AppVersion == null)
                        throw new Exception("App Version Can't Be Null Or Zero");
                    Tuple<bool, string, bool> model = _appVersionService.CheckAppVersion(commonViewModel.AppVersion, (int)Core.Helper.Enum.EnumPlatformType.AND);
                    AppVersionViewModel appVersionViewModel = new AppVersionViewModel()
                    {
                        ForceUpdate = model.Item1,
                        Message = model.Item2,
                        Status = model.Item3
                    };

                    responseViewModel = ResponseViewModel<AppVersionViewModel>.Succeeded(appVersionViewModel, "");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<AppVersionViewModel>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new AppVersionViewModel());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<AppVersionViewModel>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new AppVersionViewModel());
                return Ok(responseViewModel);
            }
        }


        [HttpPost]
        public IHttpActionResult UpdateBookingStatus([FromBody]CommonViewModel commonViewModel)
        {
            ResponseViewModel<PaymentViewModel> responseViewModel = new ResponseViewModel<PaymentViewModel>();
            try
            {
                commonViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaildWithGolferUnBlock(commonViewModel.UniqueSessionId, commonViewModel.ApiUserName, commonViewModel.ApiPassword))
                {
                    commonViewModel.PaymentGatewayBookingId.TryValidate("Booking Id");
                    commonViewModel.PaymentStatusId.GetValueOrDefault().TryValidate("Payment Status Id");
                    long id = _golferAccountService.GetGolferIdFromLoginHistoryById(commonViewModel.UniqueSessionId);
                    Tuple<bool,string> status = _bookingService.UpdateBookingStatus(commonViewModel);
                    PaymentViewModel paymentViewModel = new PaymentViewModel();




                    if (commonViewModel.PaymentStatusId == (int)Core.Helper.Enum.EnumPaymentStatus.Success)
                    {
                        paymentViewModel.Message = "Payment  is successful";
                        paymentViewModel.Status = true;
                        paymentViewModel.ENB = status.Item2;
                        responseViewModel = status != null ? ResponseViewModel<PaymentViewModel>.Succeeded(paymentViewModel, "Payment  is successful") : ResponseViewModel<PaymentViewModel>.Succeeded(paymentViewModel, "Payment is successful");

                    }
                    else
                    {
                        paymentViewModel.Message = "Payment  is unsuccessful";
                        paymentViewModel.Status = false;
                        responseViewModel = status != null ? ResponseViewModel<PaymentViewModel>.Succeeded(paymentViewModel, "Payment  is unsuccessful") : ResponseViewModel<PaymentViewModel>.Succeeded(paymentViewModel, "Payment is unsuccessful");
                    }
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<PaymentViewModel>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new PaymentViewModel());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<PaymentViewModel>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new PaymentViewModel());
                return Ok(responseViewModel);
            }
        }


        #region Paytm
        [HttpPost]
        public IHttpActionResult GeneratePaytmCheckSum([FromBody]PaytmCheckSumViewModel paytmCheckSumViewModel)
        {
            ResponseViewModel<PaytmCheckSumViewModel> responseViewModel = new ResponseViewModel<PaytmCheckSumViewModel>();
            try
            {
                if (_apiClientVaildationService.IsClientVaild(paytmCheckSumViewModel.ApiUserName, paytmCheckSumViewModel.ApiPassword))
                {

                    var model = _paytmPaymentService.GeneratePaytmCheckSum(paytmCheckSumViewModel);

                    responseViewModel = ResponseViewModel<PaytmCheckSumViewModel>.Succeeded(model, "");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<PaytmCheckSumViewModel>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new PaytmCheckSumViewModel());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<PaytmCheckSumViewModel>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new PaytmCheckSumViewModel());
                return Ok(responseViewModel);
            }
        }

        [HttpPost]
        public IHttpActionResult VerifyPaytmCheckSum([FromBody]PaytmCheckSumViewModel paytmCheckSumViewModel)
        {
            ResponseViewModel<PaytmCheckSumViewModel> responseViewModel = new ResponseViewModel<PaytmCheckSumViewModel>();
            try
            {
                if (_apiClientVaildationService.IsClientVaild(paytmCheckSumViewModel.ApiUserName, paytmCheckSumViewModel.ApiPassword))
                {

                    var model = _paytmPaymentService.GetVerifyCheckSum(paytmCheckSumViewModel);

                    responseViewModel = ResponseViewModel<PaytmCheckSumViewModel>.Succeeded(model, "");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<PaytmCheckSumViewModel>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new PaytmCheckSumViewModel());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<PaytmCheckSumViewModel>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new PaytmCheckSumViewModel());
                return Ok(responseViewModel);
            }
        }

        #endregion


        [HttpPost]
        public IHttpActionResult CheckNationalHolidayOrWeekend([FromBody]CommonViewModel commonViewModel)
        {
            ResponseViewModel<Dictionary<string, bool>> responseViewModel = new ResponseViewModel<Dictionary<string, bool>>();
            try
            {
                if (_apiClientVaildationService.IsClientVaild(commonViewModel.ApiUserName, commonViewModel.ApiPassword))
                {
                    commonViewModel.Date.TryValidate("Date");
                    bool status = _bookingService.CheckNationalHolidayOrWeekend(commonViewModel.Date);
                    var data = new Dictionary<string, bool>
                    {
                        { "Status", status }
                    };
                    responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Succeeded(data, "");
                }
                else
                {
                    responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new Dictionary<string, bool> { { "Status", false } });
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new Dictionary<string, bool> { { "Status", false } });
                return Ok(responseViewModel);
            }
            return Ok(responseViewModel);
        }


        [HttpPost]
        public IHttpActionResult GetSlotDetailsV1([FromBody]CommonViewModel commonViewModel)
        {
            ResponseViewModel<SlotViewModel> responseViewModel = new ResponseViewModel<SlotViewModel>();
            try
            {
                commonViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaildWithGolferUnBlock(commonViewModel.UniqueSessionId, commonViewModel.ApiUserName, commonViewModel.ApiPassword))
                {
                    commonViewModel.BookingTypeId.TryValidate("Booking Type Id");
                    commonViewModel.Date.TryValidate("Date");
                    long id = _golferAccountService.GetGolferIdFromLoginHistoryById(commonViewModel.UniqueSessionId);

                    var model = _bookingService.GetSlotDetailsByDateAndBookingType(commonViewModel.Date, commonViewModel.BookingTypeId, commonViewModel.CoursePairingId, id, commonViewModel.PromotionsId);

                    responseViewModel = ResponseViewModel<SlotViewModel>.Succeeded(model, "");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<SlotViewModel>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new SlotViewModel());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<SlotViewModel>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new SlotViewModel());
                return Ok(responseViewModel);
            }
        }




        [HttpPost]
        public IHttpActionResult GetPromotionList([FromBody]CommonViewModel commonViewModel)
        {
            ResponseViewModel<List<PromotionViewModel>> responseViewModel = new ResponseViewModel<List<PromotionViewModel>>();
            try
            {
                commonViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaildWithGolferUnBlock(commonViewModel.UniqueSessionId, commonViewModel.ApiUserName, commonViewModel.ApiPassword))
                {
                    var model = _promotionsCouponService.GetAllActivePromotion();
                    responseViewModel = model.Count == 0 ? ResponseViewModel<List<PromotionViewModel>>.Succeeded(model, "") : ResponseViewModel<List<PromotionViewModel>>.Succeeded(model, "");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<List<PromotionViewModel>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new List<PromotionViewModel>());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<List<PromotionViewModel>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new List<PromotionViewModel>());
                return Ok(responseViewModel);
            }
        }





        [HttpPost]
        public IHttpActionResult CourseDetailForScorPostV1([FromBody]CommonViewModel commonViewModel)
        {
            ResponseViewModel<ScoreDetailsViewModel> responseViewModel = new ResponseViewModel<ScoreDetailsViewModel>();
            try
            {
                commonViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaildWithGolferUnBlock(commonViewModel.UniqueSessionId, commonViewModel.ApiUserName, commonViewModel.ApiPassword))
                {
                    var data = _scoreService.GetScoreCardDataForScorePost(commonViewModel.ENB);
                    responseViewModel = data != null ? ResponseViewModel<ScoreDetailsViewModel>.Succeeded(data, "") : ResponseViewModel<ScoreDetailsViewModel>.Succeeded(data, "No Record Found");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<ScoreDetailsViewModel>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new ScoreDetailsViewModel());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<ScoreDetailsViewModel>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new ScoreDetailsViewModel());
                return Ok(responseViewModel);
            }
        }

      
        /// <summary>
        /// Save Golfer Score
        /// </summary>
        /// <returns></returns>
        public IHttpActionResult SaveScoreV1([FromBody]CommonViewModel commonViewModel)
        {
            ResponseViewModel<Dictionary<string, bool>> responseViewModel = new ResponseViewModel<Dictionary<string, bool>>();

            try
            {
                commonViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaildWithGolferUnBlock(commonViewModel.UniqueSessionId, commonViewModel.ApiUserName, commonViewModel.ApiPassword))
                {
                    long id = _golferAccountService.GetGolferIdFromLoginHistoryById(commonViewModel.UniqueSessionId);
                    if (commonViewModel.ScoreViewModels.Count == 0)
                        throw new Exception("No Score Details Found");
                    bool status = _scoreService.SaveScore(commonViewModel.ScoreViewModels, id, commonViewModel.GrossScore, commonViewModel.TotalScore, commonViewModel.ENB);
                    var data = new Dictionary<string, bool>
                    {
                        { "Status", status }
                    };
                    responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Succeeded(data, "Score Saved Successfully");
                }
                else
                {
                    responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new Dictionary<string, bool> { { "Status", false } });
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new Dictionary<string, bool> { { "Status", false } });
                return Ok(responseViewModel);
            }
            return Ok(responseViewModel);

        }


        [HttpPost]
        public IHttpActionResult GetBookingListForScorePost([FromBody]CommonViewModel commonViewModel)
        {
            ResponseViewModel<List<BookingViewModel>> responseViewModel = new ResponseViewModel<List<BookingViewModel>>();
            try
            {
                commonViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(commonViewModel.UniqueSessionId, commonViewModel.ApiUserName, commonViewModel.ApiPassword))
                {
                    long id = _golferAccountService.GetGolferIdFromLoginHistoryById(commonViewModel.UniqueSessionId);
                    var model = _scoreService.GetBookingListForScorePost(id);

                    responseViewModel = model == null ? ResponseViewModel<List<BookingViewModel>>.Succeeded(model, "") : ResponseViewModel<List<BookingViewModel>>.Succeeded(model, "");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<List<BookingViewModel>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new List<BookingViewModel>());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<List<BookingViewModel>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new List<BookingViewModel>());
                return Ok(responseViewModel);
            }
        }

        #region Booking Details
       
        [HttpPost]
        public IHttpActionResult CancelBookingByGolferV1([FromBody]CommonViewModel commonViewModel)
        {
            ResponseViewModel<Dictionary<string, bool>> responseViewModel = new ResponseViewModel<Dictionary<string, bool>>();
            try
            {
                commonViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(commonViewModel.UniqueSessionId, commonViewModel.ApiUserName, commonViewModel.ApiPassword))
                {
                   
                    long id = _golferAccountService.GetGolferIdFromLoginHistoryById(commonViewModel.UniqueSessionId);
                    var status = _bookingDetailService.CancelBookingByGolfer(commonViewModel.ENB, id);
                    var data = new Dictionary<string, bool> { { "Status", status } };
                    responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Succeeded(data, "Booking Is Cancelled");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new Dictionary<string, bool> { { "Status", false } });
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new Dictionary<string, bool> { { "Status", false } });
                return Ok(responseViewModel);
            }
        }

 
        [HttpPost]
        public IHttpActionResult GetBookingDetailV1([FromBody]CommonViewModel commonViewModel)
        {
            ResponseViewModel<BookingViewModel> responseViewModel = new ResponseViewModel<BookingViewModel>();
            try
            {
                commonViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(commonViewModel.UniqueSessionId, commonViewModel.ApiUserName, commonViewModel.ApiPassword))
                {
                    long id = _golferAccountService.GetGolferIdFromLoginHistoryById(commonViewModel.UniqueSessionId);
                    var model = _bookingDetailService.GetBookingDetailsByBookingId(commonViewModel.ENB, id);

                    responseViewModel = model == null ? ResponseViewModel<BookingViewModel>.Succeeded(model, "") : ResponseViewModel<BookingViewModel>.Succeeded(model, "");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<BookingViewModel>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new BookingViewModel());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<BookingViewModel>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new BookingViewModel());
                return Ok(responseViewModel);
            }
        }

    
        [HttpPost]
        public IHttpActionResult GetBookingListV1([FromBody]CommonViewModel commonViewModel)
        {
            ResponseViewModel<List<BookingViewModel>> responseViewModel = new ResponseViewModel<List<BookingViewModel>>();
            try
            {
                commonViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(commonViewModel.UniqueSessionId, commonViewModel.ApiUserName, commonViewModel.ApiPassword))
                {
                    long id = _golferAccountService.GetGolferIdFromLoginHistoryById(commonViewModel.UniqueSessionId);
                    var model = _bookingDetailService.GetBookingsByBookingStatus(id, commonViewModel.BookingSearchTypeId);

                    responseViewModel = model.Any() ? ResponseViewModel<List<BookingViewModel>>.Succeeded(model, "") : ResponseViewModel<List<BookingViewModel>>.Succeeded(model, "No Booking Found");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<List<BookingViewModel>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new List<BookingViewModel>());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<List<BookingViewModel>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new List<BookingViewModel>());
                return Ok(responseViewModel);
            }
        }

        #endregion


        [HttpPost]
        public IHttpActionResult GetCouponDetailsV1([FromBody]CommonViewModel commonViewModel)
        {
            ResponseViewModel<CouponViewModel> responseViewModel = new ResponseViewModel<CouponViewModel>();
            try
            {
                commonViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(commonViewModel.UniqueSessionId, commonViewModel.ApiUserName, commonViewModel.ApiPassword))
                {
                    commonViewModel.CouponCode.TryValidate("Coupon Code");
                    var data = _promotionsCouponService.GetCouponAmountByCouponCode(commonViewModel.CouponCode);
                 
                    responseViewModel = ResponseViewModel<CouponViewModel>.Succeeded(data, "");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<CouponViewModel>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new CouponViewModel());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<CouponViewModel>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new CouponViewModel());
                return Ok(responseViewModel);
            }
        }




        [HttpPost]
        public IHttpActionResult GetBookingCondition([FromBody]CommonViewModel commonViewModel)
        {
            ResponseViewModel<BookingConditionViewModel> responseViewModel = new ResponseViewModel<BookingConditionViewModel>();
            try
            {
                commonViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(commonViewModel.UniqueSessionId, commonViewModel.ApiUserName, commonViewModel.ApiPassword))
                {
                    commonViewModel.BookingTypeId.TryValidate("BookingType Id");
                    var data = _bookingService.BookingSetting(commonViewModel.BookingTypeId);

                    responseViewModel = ResponseViewModel<BookingConditionViewModel>.Succeeded(data, "");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<BookingConditionViewModel>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new BookingConditionViewModel());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<BookingConditionViewModel>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new BookingConditionViewModel());
                return Ok(responseViewModel);
            }
        }



        [HttpPost]
        public IHttpActionResult IsMemberShipIdExist([FromBody]CommonViewModel commonViewModel)
        {
            ResponseViewModel<Dictionary<string, bool>> responseViewModel = new ResponseViewModel<Dictionary<string, bool>>();
            try
            {
                commonViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(commonViewModel.UniqueSessionId, commonViewModel.ApiUserName, commonViewModel.ApiPassword))
                {
                    commonViewModel.MemberShipId.TryValidate("Membership Id");
                    var status = _bookingService.CheckMemberShipId(commonViewModel);
                    var data = new Dictionary<string, bool> { { "Status", status } };
                    responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Succeeded(data, "");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new Dictionary<string, bool> { { "Status", false } });
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new Dictionary<string, bool> { { "Status", false } });
                return Ok(responseViewModel);
            }
        }

    }
}
