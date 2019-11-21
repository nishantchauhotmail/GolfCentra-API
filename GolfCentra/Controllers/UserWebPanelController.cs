using GolfCentra.Business.Business.Implementation;
using GolfCentra.Business.Business.Interface;
using GolfCentra.Core.Helper;
using GolfCentra.Core.ResponseModel;
using GolfCentra.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using static GolfCentra.Filter.MyAttribute;

namespace GolfCentra.Controllers
{
    public class UserWebPanelController : ApiController
    {
        #region Private member variables...
        private readonly ICourseDetailsService _courseDetailsService;
        private readonly IGolferAccountService _golferAccountService;
        private readonly IAPIClientVaildationService _apiClientVaildationService;
        private readonly IScoreService _scoreService;
        private readonly IBookingService _bookingService;
        private readonly IStaticPageService _staticPageService;
        private readonly IAppVersionService _appVersionService;
        private readonly IPaytmPaymentService _paytmPaymentService;
        private readonly IPromotionsCouponService _promotionsCouponService;
        private readonly IBookingDetailService _bookingDetailService;
        #endregion

        #region Public Constructor...
        public UserWebPanelController()
        {
            this._courseDetailsService = new CourseDetailsService();
            this._golferAccountService = new GolferAccountService();
            this._apiClientVaildationService = new APIClientVaildationService();
            this._scoreService = new ScoreService();
            this._bookingService = new BookingService();
            this._staticPageService = new StaticPageService();
            _appVersionService = new AppVersionService();
            this._paytmPaymentService = new PaytmPaymentService();
            _promotionsCouponService = new PromotionsCouponService();
            _bookingDetailService = new BookingDetailService();
        }
        #endregion

        #region Login Register
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
                    golferViewModel.PlatformTypeId = (int)Core.Helper.Enum.EnumPlatformType.UserPanel;
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
        public IHttpActionResult GetBookingList([FromBody]CommonViewModel commonViewModel)
        {
            ResponseViewModel<List<BookingViewModel>> responseViewModel = new ResponseViewModel<List<BookingViewModel>>();
            try
            {
                commonViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(commonViewModel.UniqueSessionId, commonViewModel.ApiUserName, commonViewModel.ApiPassword))
                {
                    long id = _golferAccountService.GetGolferIdFromLoginHistoryById(commonViewModel.UniqueSessionId);
                    var model = _bookingDetailService.GetBookingsByBookingTypeId(id, commonViewModel.BookingTypeId);

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
                    responseViewModel = data.Any() ? ResponseViewModel<List<ScoreViewModel>>.Succeeded(data, "") : ResponseViewModel<List<ScoreViewModel>>.Succeeded(data, "No ScoreCard Found");
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
        /// Get Score Card Detail By Score ID And Unique Session Id
        /// </summary>
        /// <param name="commonViewModel"></param>
        /// <returns></returns>
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



        [HttpPost]
        public IHttpActionResult CourseDetailForScorPost([FromBody]CommonViewModel commonViewModel)
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


        #region ProfileDetails
        /// <summary>
        /// Get Golfer Detail By  Unique Session Id
        /// </summary>
        /// <param name="commonViewModel"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Change Password Of Golfer
        /// </summary>
        /// <param name="commonViewModel"></param>
        /// <returns></returns>
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
                    var status = _golferAccountService.ChangePassword(id, commonViewModel.OldPassword, commonViewModel.NewPassword, commonViewModel.UniqueSessionId);
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


        [HttpPost]
        public IHttpActionResult GetGenderList([FromBody]GolferViewModel golferViewModel)
        {
            ResponseViewModel<List<GenderTypeViewModel>> responseViewModel = new ResponseViewModel<List<GenderTypeViewModel>>();
            try
            {
                if (_apiClientVaildationService.IsClientVaild(golferViewModel.ApiUserName, golferViewModel.ApiPassword))
                {
                    var data = _golferAccountService.GetAllGenderType();
                    responseViewModel = data.Any() ? ResponseViewModel<List<GenderTypeViewModel>>.Succeeded(data, "") : ResponseViewModel<List<GenderTypeViewModel>>.Succeeded(data, "No Record Found");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<List<GenderTypeViewModel>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new List<GenderTypeViewModel>());
                    return Ok(responseViewModel);
                }

            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<List<GenderTypeViewModel>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new List<GenderTypeViewModel>());
                return Ok(responseViewModel);
            }

        }
        #endregion

        #region Paytm
        /// <summary>
        /// Generate CheckSum For Paytm
        /// </summary>
        /// <param name="paytmCheckSumViewModel"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Verify CheckSum For Paytm
        /// </summary>
        /// <param name="paytmCheckSumViewModel"></param>
        /// <returns></returns>
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
        public IHttpActionResult GetCouponDetails([FromBody]CommonViewModel commonViewModel)
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
        public IHttpActionResult GetSlotDetails([FromBody]CommonViewModel commonViewModel)
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

                    var model = _bookingService.GetSlotDetailsByDateAndBookingType(commonViewModel.Date, commonViewModel.BookingTypeId, commonViewModel.CoursePairingId, id,commonViewModel.PromotionsId);

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
        public IHttpActionResult UpdateBookingStatus([FromBody]CommonViewModel commonViewModel)
        {
            ResponseViewModel<PaymentViewModel> responseViewModel = new ResponseViewModel<PaymentViewModel>();
            try
            {
                commonViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(commonViewModel.UniqueSessionId, commonViewModel.ApiUserName, commonViewModel.ApiPassword))
                {
                    commonViewModel.PaymentGatewayBookingId.TryValidate("Booking Id");
                    commonViewModel.PaymentStatusId.GetValueOrDefault().TryValidate("Payment Status Id");
                    long id = _golferAccountService.GetGolferIdFromLoginHistoryById(commonViewModel.UniqueSessionId);
                    Tuple<bool, string> status = _bookingService.UpdateBookingStatus(commonViewModel);
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


        #region StartBooking
        /// <summary>
        /// Get RateCard For BTT and BDR
        /// </summary>
        /// <param name="commonViewModel"></param>
        /// <returns></returns>
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
     
        /// <summary>
        /// Get Pricing Details For BTT
        /// </summary>
        /// <param name="commonViewModel"></param>
        /// <returns></returns>
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
                    var model = _bookingService.GetPricingCalculation(commonViewModel.HoleTypeId, commonViewModel.Date, commonViewModel.SlotId, commonViewModel.NoOfPlayer, commonViewModel.NoOfMemberPlayer, commonViewModel.NoOfNonMemberPlayer, commonViewModel.MemberTypeViewModels, id, commonViewModel.CoursePairingId, commonViewModel.BookingPlayerDetailViewModels, commonViewModel.PromotionsId);

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

        /// <summary>
        /// Save Tee Time Booking 
        /// </summary>
        /// <param name="commonViewModel"></param>
        /// <returns></returns>
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
                    //  if (commonViewModel.NoOfPlayer != (commonViewModel.NoOfMemberPlayer + commonViewModel.NoOfNonMemberPlayer)) { throw new Exception("Invalid Player Details."); }
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
        public IHttpActionResult CancelBookingByGolfer([FromBody]CommonViewModel commonViewModel)
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
                    responseViewModel = status != null ? ResponseViewModel<Dictionary<string, bool>>.Succeeded(data, "Booking Is Cancelled") : ResponseViewModel<Dictionary<string, bool>>.Succeeded(data, "Booking Is Cancelled");
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
        public IHttpActionResult ResetPassword([FromBody]CommonViewModel commonViewModel)
        {
            ResponseViewModel<Dictionary<string, bool>> responseViewModel = new ResponseViewModel<Dictionary<string, bool>>();
            try
            {
              
                if (_apiClientVaildationService.IsClientVaild( commonViewModel.ApiUserName, commonViewModel.ApiPassword))
                {
                    commonViewModel.SS.TryValidate("Invaild Request");
                    commonViewModel.NewPassword.TryValidate("New Password");
                  
                    var status = _golferAccountService.ChangePassword(commonViewModel.SS, commonViewModel.NewPassword);
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
        public IHttpActionResult CheckPasswordLinkStatus([FromBody]CommonViewModel commonViewModel)
        {
            ResponseViewModel<Dictionary<string, bool>> responseViewModel = new ResponseViewModel<Dictionary<string, bool>>();
            try
            {
               
                if (_apiClientVaildationService.IsClientVaild(commonViewModel.ApiUserName, commonViewModel.ApiPassword))
                {
                    commonViewModel.SS.TryValidate("Invaild Request");

                    var status = _golferAccountService.CheckPasswordLinkStatus(commonViewModel.SS);
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
