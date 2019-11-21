using GolfCentra.Business.Business.Implementation;
using GolfCentra.Business.Business.Interface;
using GolfCentra.Business.BusinessAdmin.Implementation;
using GolfCentra.Business.BusinessAdmin.Interface;
using GolfCentra.Core.Helper;
using GolfCentra.Core.ResponseModel;
using GolfCentra.ViewModel.Admin;
using GolfCentra.ViewModel.Admin.FireBase;
using GolfCentra.ViewModel.Admin.LoginActivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using static GolfCentra.Filter.MyAttribute;

namespace GolfCentra.Controllers
{
  
    public class AdminPanelController : ApiController
    {
        #region Private member variables...
        private readonly IEmployeeService _employeeService;
        private readonly Business.BusinessAdmin.Interface.IBookingService _bookingService;
        private readonly IAPIClientVaildationService _apiClientVaildationService;
        private readonly IHoleInformationService _holeInformationService;
        private readonly IGolferService _golferService;
        private readonly ICouponService _couponService;
        private readonly ITimeSlotService _timeSlotService;
        private readonly IEmployeeTypeService _employeeTypeService;
        private readonly IGenderTypeService _genderTypeService;
        private readonly IPageService _pageService;
        private readonly IPermissionService _permissionService;
        private readonly Business.BusinessAdmin.Interface.IScoreService _scoreService;
        private readonly IPricingService _pricingService;
        private readonly ISessionService _sessionService;
        private readonly IMemberTypeService _memberTypeService;
        private readonly ITaxManagementService _taxManagementService;
        private readonly IEquipmentService _equipmentService;
        private readonly IBucketService _bucketService;
        private readonly IMaritalStatusService _maritalStatusService;

        private readonly ITimeFormatService _timeFormatService;
        private readonly ISlotBlockReasonService _slotBlockReasonService;
        private readonly ICoursePairingService _coursePairingService;
        private readonly ISlotReservationService _slotReservationService;
        private readonly ISendPushNotificationService _sendPushNotificationService;
        private readonly ISessionActivityService _sessionActivityService;
        private readonly IReportingService _reportingService;
        private readonly IPromotionService _promotionService;
        private readonly ITeeSheetService _teeSheetService;
        private readonly IPaymentGatewayService _paymentGatewayService;
        #endregion

        #region Public Constructor...
        public AdminPanelController()
        {
            _employeeService = new EmployeeService();
            _bookingService = new Business.BusinessAdmin.Implementation.BookingService();
            this._apiClientVaildationService = new APIClientVaildationService();
            _holeInformationService = new HoleInformationService();
            _golferService = new GolferService();
            _couponService = new CouponService();
            _timeSlotService = new TimeSlotService();
            _employeeTypeService = new EmployeeTypeService();
            _genderTypeService = new GenderTypeService();
            _pageService = new PageService();
            _permissionService = new PermissionService();
            _scoreService = new Business.BusinessAdmin.Implementation.ScoreService();
            _pricingService = new PricingService();
            _sessionService = new SessionService();
            _memberTypeService = new MemberTypeService();
            _taxManagementService = new TaxManagementService();
            _equipmentService = new EquipmentService();
            _bucketService = new BucketService();
            _maritalStatusService = new MaritalStatusService();
            _timeFormatService = new TimeFormatService();
            _slotBlockReasonService = new SlotBlockReasonService();
            _coursePairingService = new CoursePairingService();
            _slotReservationService = new SlotReservationService();
            _sendPushNotificationService = new SendPushNotificationService();
            _sessionActivityService = new SessionActivityService();
            _reportingService = new ReportingService();
            _promotionService = new PromotionService();
            _teeSheetService = new TeeSheetService();
            _paymentGatewayService = new PaymentGatewayService();

        }
        #endregion


        [HttpPost]
        public IHttpActionResult Login([FromBody]EmployeeViewModel employeeViewModel)
        {
            ResponseViewModel<EmployeeViewModel> responseViewModel = new ResponseViewModel<EmployeeViewModel>();

            try
            {
                if (_apiClientVaildationService.IsClientVaild(employeeViewModel.ApiClientViewModel.UserName, employeeViewModel.ApiClientViewModel.Password))
                {
                    employeeViewModel.EmailId.TryValidateEmail("Email Address");
                    employeeViewModel.Password.TryValidate("Password");
                    var data = _employeeService.LoginValidation(employeeViewModel.EmailId, employeeViewModel.Password);

                    responseViewModel = ResponseViewModel<EmployeeViewModel>.Succeeded(data, "Logged In Successfully");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<EmployeeViewModel>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new EmployeeViewModel());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<EmployeeViewModel>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new EmployeeViewModel());
                return Ok(responseViewModel);
            }
        }


        [HttpPost]
        public IHttpActionResult GetRecentBookingByTake([FromBody]BookingViewModel bookingViewModel)
        {
            ResponseViewModel<List<BookingViewModel>> responseViewModel = new ResponseViewModel<List<BookingViewModel>>();
            try
            {
                bookingViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(bookingViewModel.ApiClientViewModel.UniqueSessionId, bookingViewModel.ApiClientViewModel.UserName, bookingViewModel.ApiClientViewModel.Password))
                {

                    var model = _bookingService.GetBookingDetailsByTake(bookingViewModel.NoOfRecord);

                    responseViewModel = model == null ? ResponseViewModel<List<BookingViewModel>>.Succeeded(model, "") : ResponseViewModel<List<BookingViewModel>>.Succeeded(model, "No Booking Found");
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
        public IHttpActionResult GetBookingDetailByBookingId([FromBody]BookingViewModel bookingViewModel)
        {
            ResponseViewModel<BookingViewModel> responseViewModel = new ResponseViewModel<BookingViewModel>();
            try
            {
                bookingViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(bookingViewModel.ApiClientViewModel.UniqueSessionId, bookingViewModel.ApiClientViewModel.UserName, bookingViewModel.ApiClientViewModel.Password))
                {
                    var model = _bookingService.GetBookingDetailsByBookingId(bookingViewModel.BookingId);

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
        public IHttpActionResult GetDataForDashboardTopBar([FromBody]BookingViewModel bookingViewModel)
        {
            ResponseViewModel<DashBoardTopBarViewModel> responseViewModel = new ResponseViewModel<DashBoardTopBarViewModel>();
            try
            {
                bookingViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(bookingViewModel.ApiClientViewModel.UniqueSessionId, bookingViewModel.ApiClientViewModel.UserName, bookingViewModel.ApiClientViewModel.Password))
                {
                    var model = _bookingService.GetDataForTopBar();

                    responseViewModel = model == null ? ResponseViewModel<DashBoardTopBarViewModel>.Succeeded(model, "") : ResponseViewModel<DashBoardTopBarViewModel>.Succeeded(model, "");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<DashBoardTopBarViewModel>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new DashBoardTopBarViewModel());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<DashBoardTopBarViewModel>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new DashBoardTopBarViewModel());
                return Ok(responseViewModel);
            }
        }


        [HttpPost]
        public IHttpActionResult GetHoleNumberList([FromBody]HoleViewModel holeViewModel)
        {
            ResponseViewModel<List<HoleViewModel>> responseViewModel = new ResponseViewModel<List<HoleViewModel>>();
            try
            {
                holeViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(holeViewModel.ApiClientViewModel.UniqueSessionId, holeViewModel.ApiClientViewModel.UserName, holeViewModel.ApiClientViewModel.Password))
                {

                    var model = _holeInformationService.GetHoleNumberList();

                    responseViewModel = model == null ? ResponseViewModel<List<HoleViewModel>>.Succeeded(model, "") : ResponseViewModel<List<HoleViewModel>>.Succeeded(model, "No Booking Found");
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


        [HttpPost]
        public IHttpActionResult GetHoleDetailsByHoleNumberId([FromBody]HoleViewModel holeViewModel)
        {
            ResponseViewModel<HoleViewModel> responseViewModel = new ResponseViewModel<HoleViewModel>();
            try
            {
                holeViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(holeViewModel.ApiClientViewModel.UniqueSessionId, holeViewModel.ApiClientViewModel.UserName, holeViewModel.ApiClientViewModel.Password))
                {
                    var model = _holeInformationService.GetHoleDetailsByHoleNumberId(holeViewModel.HoleNumberId);

                    responseViewModel = model == null ? ResponseViewModel<HoleViewModel>.Succeeded(model, "") : ResponseViewModel<HoleViewModel>.Succeeded(model, "");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<HoleViewModel>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new HoleViewModel());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<HoleViewModel>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new HoleViewModel());
                return Ok(responseViewModel);
            }
        }

        [HttpPost]
        public IHttpActionResult SaveUpdateHoleInformation([FromBody]HoleViewModel holeViewModel)
        {
            ResponseViewModel<Dictionary<string, bool>> responseViewModel = new ResponseViewModel<Dictionary<string, bool>>();
            try
            {
                holeViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(holeViewModel.ApiClientViewModel.UniqueSessionId, holeViewModel.ApiClientViewModel.UserName, holeViewModel.ApiClientViewModel.Password))
                {
                    long id = Convert.ToInt64(Core.Helper.Crypto.DecryptStringAES(holeViewModel.ApiClientViewModel.UniqueSessionId.ToString()));

                    _holeInformationService.SaveUpdateHoleInformation(holeViewModel,id);
                    var model = new Dictionary<string, bool>() { { "Status", true } };
                    responseViewModel = model == null ? ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "") : ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new Dictionary<string, bool>());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new Dictionary<string, bool>());
                return Ok(responseViewModel);
            }
        }

        [HttpPost]
        public IHttpActionResult GetAllGolferProfile([FromBody]GolferViewModel golferViewModel)
        {
            ResponseViewModel<List<GolferViewModel>> responseViewModel = new ResponseViewModel<List<GolferViewModel>>();
            try
            {
                golferViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(golferViewModel.ApiClientViewModel.UniqueSessionId, golferViewModel.ApiClientViewModel.UserName, golferViewModel.ApiClientViewModel.Password))
                {

                    var model = _golferService.GetAllGolferProfile();

                    responseViewModel = model.Any() ? ResponseViewModel<List<GolferViewModel>>.Succeeded(model, "") : ResponseViewModel<List<GolferViewModel>>.Succeeded(model, "No Golfer Found");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<List<GolferViewModel>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new List<GolferViewModel>());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<List<GolferViewModel>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new List<GolferViewModel>());
                return Ok(responseViewModel);
            }
        }

        [HttpPost]
        public IHttpActionResult GetGolferProfileByGolferId([FromBody]GolferViewModel golferViewModel)
        {
            ResponseViewModel<GolferViewModel> responseViewModel = new ResponseViewModel<GolferViewModel>();
            try
            {
                golferViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(golferViewModel.ApiClientViewModel.UniqueSessionId, golferViewModel.ApiClientViewModel.UserName, golferViewModel.ApiClientViewModel.Password))
                {

                    var model = _golferService.GetGolferProfileByGolferId(golferViewModel.GolferId);

                    responseViewModel = model == null ? ResponseViewModel<GolferViewModel>.Succeeded(model, "") : ResponseViewModel<GolferViewModel>.Succeeded(model, "No Golfer Found");
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
        public IHttpActionResult UpdateGolferProfile([FromBody]GolferViewModel golferViewModel)
        {
            ResponseViewModel<Dictionary<string, bool>> responseViewModel = new ResponseViewModel<Dictionary<string, bool>>();
            try
            {
                golferViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(golferViewModel.ApiClientViewModel.UniqueSessionId, golferViewModel.ApiClientViewModel.UserName, golferViewModel.ApiClientViewModel.Password))
                {
                    long id = Convert.ToInt64(Core.Helper.Crypto.DecryptStringAES(golferViewModel.ApiClientViewModel.UniqueSessionId.ToString()));

                    _golferService.UpdateGolferProfile(golferViewModel,id);
                    var model = new Dictionary<string, bool>() { { "Status", true } };
                    responseViewModel = model == null ? ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "") : ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new Dictionary<string, bool>());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new Dictionary<string, bool>());
                return Ok(responseViewModel);
            }
        }


        [HttpPost]
        public IHttpActionResult SaveCouponDetail([FromBody]CouponViewModel couponViewModel)
        {
            ResponseViewModel<Dictionary<string, bool>> responseViewModel = new ResponseViewModel<Dictionary<string, bool>>();
            try
            {
                couponViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(couponViewModel.ApiClientViewModel.UniqueSessionId, couponViewModel.ApiClientViewModel.UserName, couponViewModel.ApiClientViewModel.Password))
                {
                    long id = Convert.ToInt64(Core.Helper.Crypto.DecryptStringAES(couponViewModel.ApiClientViewModel.UniqueSessionId.ToString()));
                    _couponService.SaveCouponDetail(couponViewModel,id);
                    var model = new Dictionary<string, bool>() { { "Status", true } };
                    responseViewModel = model == null ? ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "") : ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new Dictionary<string, bool>());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new Dictionary<string, bool>());
                return Ok(responseViewModel);
            }
        }


        [HttpPost]
        public IHttpActionResult GetAllTimeSlot([FromBody]SlotViewModel slotViewModel)
        {
            ResponseViewModel<List<SlotViewModel>> responseViewModel = new ResponseViewModel<List<SlotViewModel>>();
            try
            {
                slotViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(slotViewModel.ApiClientViewModel.UniqueSessionId, slotViewModel.ApiClientViewModel.UserName, slotViewModel.ApiClientViewModel.Password))
                {

                    var model = _timeSlotService.GetAllTimeSlot();

                    responseViewModel = model.Any() ? ResponseViewModel<List<SlotViewModel>>.Succeeded(model, "") : ResponseViewModel<List<SlotViewModel>>.Succeeded(model, "No Golfer Found");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<List<SlotViewModel>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new List<SlotViewModel>());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<List<SlotViewModel>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new List<SlotViewModel>());
                return Ok(responseViewModel);
            }
        }


        [HttpPost]
        public IHttpActionResult SaveSlotTimeDetail([FromBody]SlotViewModel slotViewModel)
        {
            ResponseViewModel<Dictionary<string, bool>> responseViewModel = new ResponseViewModel<Dictionary<string, bool>>();
            try
            {
                slotViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(slotViewModel.ApiClientViewModel.UniqueSessionId, slotViewModel.ApiClientViewModel.UserName, slotViewModel.ApiClientViewModel.Password))
                {
                    long id = Convert.ToInt64(Core.Helper.Crypto.DecryptStringAES(slotViewModel.ApiClientViewModel.UniqueSessionId.ToString()));

                    _timeSlotService.SaveTimeSlotDetails(slotViewModel.Time,id);
                    var model = new Dictionary<string, bool>() { { "Status", true } };
                    responseViewModel = model == null ? ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "") : ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new Dictionary<string, bool>());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new Dictionary<string, bool>());
                return Ok(responseViewModel);
            }
        }

        [HttpPost]
        public IHttpActionResult UpdateTimeSlot([FromBody]SlotViewModel slotViewModel)
        {
            ResponseViewModel<Dictionary<string, bool>> responseViewModel = new ResponseViewModel<Dictionary<string, bool>>();
            try
            {
                slotViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(slotViewModel.ApiClientViewModel.UniqueSessionId, slotViewModel.ApiClientViewModel.UserName, slotViewModel.ApiClientViewModel.Password))
                {
                    long id = Convert.ToInt64(Core.Helper.Crypto.DecryptStringAES(slotViewModel.ApiClientViewModel.UniqueSessionId.ToString()));

                    _timeSlotService.UpdateTimeSlotDetails(slotViewModel.SlotId, slotViewModel.Time,id);
                    var model = new Dictionary<string, bool>() { { "Status", true } };
                    responseViewModel = model == null ? ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "") : ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new Dictionary<string, bool>());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new Dictionary<string, bool>());
                return Ok(responseViewModel);
            }
        }


        [HttpPost]
        public IHttpActionResult DeleteTimeSlotDetails([FromBody]SlotViewModel slotViewModel)
        {
            ResponseViewModel<Dictionary<string, bool>> responseViewModel = new ResponseViewModel<Dictionary<string, bool>>();
            try
            {
                slotViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(slotViewModel.ApiClientViewModel.UniqueSessionId, slotViewModel.ApiClientViewModel.UserName, slotViewModel.ApiClientViewModel.Password))
                {
                    long id = Convert.ToInt64(Core.Helper.Crypto.DecryptStringAES(slotViewModel.ApiClientViewModel.UniqueSessionId.ToString()));

                    _timeSlotService.DeleteTimeSlotDetails(slotViewModel.SlotId,id);
                    var model = new Dictionary<string, bool>() { { "Status", true } };
                    responseViewModel = model == null ? ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "") : ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new Dictionary<string, bool>());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new Dictionary<string, bool>());
                return Ok(responseViewModel);
            }
        }



        [HttpPost]
        public IHttpActionResult GetALLGenderType([FromBody]GenderTypeViewModel genderTypeViewModel)
        {
            ResponseViewModel<List<GenderTypeViewModel>> responseViewModel = new ResponseViewModel<List<GenderTypeViewModel>>();
            try
            {
                genderTypeViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(genderTypeViewModel.ApiClientViewModel.UniqueSessionId, genderTypeViewModel.ApiClientViewModel.UserName, genderTypeViewModel.ApiClientViewModel.Password))
                {

                    var model = _genderTypeService.GetALLGenderType();

                    responseViewModel = model.Any() ? ResponseViewModel<List<GenderTypeViewModel>>.Succeeded(model, "") : ResponseViewModel<List<GenderTypeViewModel>>.Succeeded(model, "No Golfer Found");
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

        [HttpPost]
        public IHttpActionResult GetAllEmployeeType([FromBody]EmployeeTypeViewModel slotViewModel)
        {
            ResponseViewModel<List<EmployeeTypeViewModel>> responseViewModel = new ResponseViewModel<List<EmployeeTypeViewModel>>();
            try
            {
                slotViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(slotViewModel.ApiClientViewModel.UniqueSessionId, slotViewModel.ApiClientViewModel.UserName, slotViewModel.ApiClientViewModel.Password))
                {

                    var model = _employeeTypeService.GetAllEmployeeType();

                    responseViewModel = model.Any() ? ResponseViewModel<List<EmployeeTypeViewModel>>.Succeeded(model, "") : ResponseViewModel<List<EmployeeTypeViewModel>>.Succeeded(model, "No Employee Type Found");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<List<EmployeeTypeViewModel>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new List<EmployeeTypeViewModel>());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<List<EmployeeTypeViewModel>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new List<EmployeeTypeViewModel>());
                return Ok(responseViewModel);
            }
        }


        [HttpPost]
        public IHttpActionResult SaveEmployeeType([FromBody]EmployeeTypeViewModel employeeTypeViewModel)
        {
            ResponseViewModel<Dictionary<string, bool>> responseViewModel = new ResponseViewModel<Dictionary<string, bool>>();
            try
            {
                employeeTypeViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(employeeTypeViewModel.ApiClientViewModel.UniqueSessionId, employeeTypeViewModel.ApiClientViewModel.UserName, employeeTypeViewModel.ApiClientViewModel.Password))
                {
                    long id = Convert.ToInt64(Core.Helper.Crypto.DecryptStringAES(employeeTypeViewModel.ApiClientViewModel.UniqueSessionId.ToString()));

                    _employeeTypeService.SaveEmployeeType(employeeTypeViewModel,id);
                    var model = new Dictionary<string, bool>() { { "Status", true } };
                    responseViewModel = model == null ? ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "") : ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new Dictionary<string, bool>());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new Dictionary<string, bool>());
                return Ok(responseViewModel);
            }
        }


        [HttpPost]
        public IHttpActionResult GetAllPageDetails([FromBody]PageViewModel slotViewModel)
        {
            ResponseViewModel<List<PageViewModel>> responseViewModel = new ResponseViewModel<List<PageViewModel>>();
            try
            {
                slotViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(slotViewModel.ApiClientViewModel.UniqueSessionId, slotViewModel.ApiClientViewModel.UserName, slotViewModel.ApiClientViewModel.Password))
                {

                    var model = _pageService.GetAllPageDetails();

                    responseViewModel = model.Any() ? ResponseViewModel<List<PageViewModel>>.Succeeded(model, "") : ResponseViewModel<List<PageViewModel>>.Succeeded(model, "No Employee Type Found");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<List<PageViewModel>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new List<PageViewModel>());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<List<PageViewModel>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new List<PageViewModel>());
                return Ok(responseViewModel);
            }
        }

        [HttpPost]
        public IHttpActionResult UpdatePageOrdering([FromBody]PageViewModel pageViewModel)
        {
            ResponseViewModel<Dictionary<string, bool>> responseViewModel = new ResponseViewModel<Dictionary<string, bool>>();
            try
            {
                pageViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(pageViewModel.ApiClientViewModel.UniqueSessionId, pageViewModel.ApiClientViewModel.UserName, pageViewModel.ApiClientViewModel.Password))
                {
                    _pageService.UpdatePageOrdering(pageViewModel);
                    var model = new Dictionary<string, bool>() { { "Status", true } };
                    responseViewModel = model == null ? ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "") : ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new Dictionary<string, bool>());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new Dictionary<string, bool>());
                return Ok(responseViewModel);
            }
        }


        [HttpPost]
        public IHttpActionResult GetAllPermissionDetailsForEmployee([FromBody]PermissionMainViewModel permissionMainViewModel)
        {
            ResponseViewModel<PermissionMainViewModel> responseViewModel = new ResponseViewModel<PermissionMainViewModel>();
            try
            {
                permissionMainViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(permissionMainViewModel.ApiClientViewModel.UniqueSessionId, permissionMainViewModel.ApiClientViewModel.UserName, permissionMainViewModel.ApiClientViewModel.Password))
                {
                    var model = _permissionService.GetAllPermissionDetails(permissionMainViewModel.EmployeeId);
                    responseViewModel = model == null ? ResponseViewModel<PermissionMainViewModel>.Succeeded(model, "") : ResponseViewModel<PermissionMainViewModel>.Succeeded(model, "Not Found");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<PermissionMainViewModel>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new PermissionMainViewModel());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<PermissionMainViewModel>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new PermissionMainViewModel());
                return Ok(responseViewModel);
            }
        }

        [HttpPost]
        public IHttpActionResult GetAllActiveEmployeeForList([FromBody]EmployeeViewModel employeeViewModel)
        {
            ResponseViewModel<List<EmployeeViewModel>> responseViewModel = new ResponseViewModel<List<EmployeeViewModel>>();
            try
            {
                employeeViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(employeeViewModel.ApiClientViewModel.UniqueSessionId, employeeViewModel.ApiClientViewModel.UserName, employeeViewModel.ApiClientViewModel.Password))
                {

                    var model = _employeeService.GetAllActiveEmployeeForList();

                    responseViewModel = model.Any() ? ResponseViewModel<List<EmployeeViewModel>>.Succeeded(model, "") : ResponseViewModel<List<EmployeeViewModel>>.Succeeded(model, "No Employee Type Found");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<List<EmployeeViewModel>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new List<EmployeeViewModel>());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<List<EmployeeViewModel>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new List<EmployeeViewModel>());
                return Ok(responseViewModel);
            }
        }

        [HttpPost]
        public IHttpActionResult UpdatePermission([FromBody]PermissionMainViewModel permissionMainViewModel)
        {
            ResponseViewModel<Dictionary<string, bool>> responseViewModel = new ResponseViewModel<Dictionary<string, bool>>();
            try
            {
                permissionMainViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(permissionMainViewModel.ApiClientViewModel.UniqueSessionId, permissionMainViewModel.ApiClientViewModel.UserName, permissionMainViewModel.ApiClientViewModel.Password))
                {
                    _permissionService.UpdatePermission(permissionMainViewModel.PermissionViewModels, permissionMainViewModel.EmployeeId);
                    var model = new Dictionary<string, bool>() { { "Status", true } };
                    responseViewModel = model == null ? ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "") : ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new Dictionary<string, bool>());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new Dictionary<string, bool>());
                return Ok(responseViewModel);
            }
        }


        [HttpPost]
        public IHttpActionResult GetScoreDetailByAdvanceSearch([FromBody]ScoreSearchViewModel scoreSearchViewModel)
        {
            ResponseViewModel<List<ScoreDetailsViewModel>> responseViewModel = new ResponseViewModel<List<ScoreDetailsViewModel>>();
            try
            {
                scoreSearchViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(scoreSearchViewModel.ApiClientViewModel.UniqueSessionId, scoreSearchViewModel.ApiClientViewModel.UserName, scoreSearchViewModel.ApiClientViewModel.Password))
                {

                    var model = _scoreService.GetScoreDetailByAdvanceSearch(scoreSearchViewModel);

                    responseViewModel = model.Any() ? ResponseViewModel<List<ScoreDetailsViewModel>>.Succeeded(model, "") : ResponseViewModel<List<ScoreDetailsViewModel>>.Succeeded(model, "No Score Found");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<List<ScoreDetailsViewModel>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new List<ScoreDetailsViewModel>());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<List<ScoreDetailsViewModel>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new List<ScoreDetailsViewModel>());
                return Ok(responseViewModel);
            }
        }


        [HttpPost]
        public IHttpActionResult GetBookingDetails([FromBody]BookingViewModel bookingViewModel)
        {
            ResponseViewModel<List<BookingViewModel>> responseViewModel = new ResponseViewModel<List<BookingViewModel>>();
            try
            {
                bookingViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(bookingViewModel.ApiClientViewModel.UniqueSessionId, bookingViewModel.ApiClientViewModel.UserName, bookingViewModel.ApiClientViewModel.Password))
                {

                    var model = _bookingService.GetBookingDetails();

                    responseViewModel = model == null ? ResponseViewModel<List<BookingViewModel>>.Succeeded(model, "") : ResponseViewModel<List<BookingViewModel>>.Succeeded(model, "No Booking Found");
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
        public IHttpActionResult GetBookingDetailsBySearch([FromBody]BookingViewModel bookingViewModel)
        {
            ResponseViewModel<List<BookingViewModel>> responseViewModel = new ResponseViewModel<List<BookingViewModel>>();
            try
            {
                bookingViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(bookingViewModel.ApiClientViewModel.UniqueSessionId, bookingViewModel.ApiClientViewModel.UserName, bookingViewModel.ApiClientViewModel.Password))
                {

                    var model = _bookingService.GetBookingDetailsBySearch(bookingViewModel);

                    responseViewModel = model == null ? ResponseViewModel<List<BookingViewModel>>.Succeeded(model, "") : ResponseViewModel<List<BookingViewModel>>.Succeeded(model, "No Booking Found");
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
        public IHttpActionResult GetBookingDetailsByGolferId([FromBody]BookingViewModel bookingViewModel)
        {
            ResponseViewModel<List<BookingViewModel>> responseViewModel = new ResponseViewModel<List<BookingViewModel>>();
            try
            {
                bookingViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(bookingViewModel.ApiClientViewModel.UniqueSessionId, bookingViewModel.ApiClientViewModel.UserName, bookingViewModel.ApiClientViewModel.Password))
                {

                    var model = _bookingService.GetBookingDetailsByGolferId(bookingViewModel.GolferId);

                    responseViewModel = model == null ? ResponseViewModel<List<BookingViewModel>>.Succeeded(model, "") : ResponseViewModel<List<BookingViewModel>>.Succeeded(model, "No Booking Found");
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
        public IHttpActionResult GetAllActiveEmployee([FromBody]EmployeeViewModel employeeViewModel)
        {
            ResponseViewModel<List<EmployeeViewModel>> responseViewModel = new ResponseViewModel<List<EmployeeViewModel>>();
            try
            {
                employeeViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(employeeViewModel.ApiClientViewModel.UniqueSessionId, employeeViewModel.ApiClientViewModel.UserName, employeeViewModel.ApiClientViewModel.Password))
                {

                    var model = _employeeService.GetAllActiveEmployee();

                    responseViewModel = model.Any() ? ResponseViewModel<List<EmployeeViewModel>>.Succeeded(model, "") : ResponseViewModel<List<EmployeeViewModel>>.Succeeded(model, "No Employee  Found");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<List<EmployeeViewModel>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new List<EmployeeViewModel>());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<List<EmployeeViewModel>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new List<EmployeeViewModel>());
                return Ok(responseViewModel);
            }
        }

        //[HttpPost]
        //public IHttpActionResult GetAllMemberType([FromBody]MemberTypeViewModel employeeViewModel)
        //{
        //    ResponseViewModel<List<MemberTypeViewModel>> responseViewModel = new ResponseViewModel<List<MemberTypeViewModel>>();
        //    try
        //    {
        //        employeeViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
        //        if (_apiClientVaildationService.IsLoggedInClientVaild(employeeViewModel.ApiClientViewModel.UniqueSessionId, employeeViewModel.ApiClientViewModel.UserName, employeeViewModel.ApiClientViewModel.Password))
        //        {

        //            var model = _bookingService.GetAllMemberType();

        //            responseViewModel = model.Any() ? ResponseViewModel<List<MemberTypeViewModel>>.Succeeded(model, "") : ResponseViewModel<List<MemberTypeViewModel>>.Succeeded(model, "No Member Type  Found");
        //            return Ok(responseViewModel);
        //        }
        //        else
        //        {
        //            responseViewModel = ResponseViewModel<List<MemberTypeViewModel>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InvalidAccess, Core.Helper.Constants.StrMessage.InvalidAccess, null, "", new List<MemberTypeViewModel>());
        //            return Ok(responseViewModel);
        //        }
        //    }
        //    catch (Exception err)
        //    {
        //        responseViewModel = ResponseViewModel<List<MemberTypeViewModel>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new List<MemberTypeViewModel>());
        //        return Ok(responseViewModel);
        //    }
        //}

        [HttpPost]
        public IHttpActionResult SaveEmployee([FromBody]EmployeeViewModel employeeViewModel)
        {
            ResponseViewModel<Dictionary<string, bool>> responseViewModel = new ResponseViewModel<Dictionary<string, bool>>();
            try
            {
                employeeViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(employeeViewModel.ApiClientViewModel.UniqueSessionId, employeeViewModel.ApiClientViewModel.UserName, employeeViewModel.ApiClientViewModel.Password))
                {
                    long id = Convert.ToInt64(Core.Helper.Crypto.DecryptStringAES(employeeViewModel.ApiClientViewModel.UniqueSessionId.ToString()));

                    _employeeService.SaveEmployee(employeeViewModel,id);
                    var model = new Dictionary<string, bool>() { { "Status", true } };
                    responseViewModel = model == null ? ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "") : ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new Dictionary<string, bool>());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new Dictionary<string, bool>());
                return Ok(responseViewModel);
            }
        }


        [HttpPost]
        public IHttpActionResult UpdateEmployee([FromBody]EmployeeViewModel employeeViewModel)
        {
            ResponseViewModel<Dictionary<string, bool>> responseViewModel = new ResponseViewModel<Dictionary<string, bool>>();
            try
            {
                employeeViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(employeeViewModel.ApiClientViewModel.UniqueSessionId, employeeViewModel.ApiClientViewModel.UserName, employeeViewModel.ApiClientViewModel.Password))
                {
                    long id = Convert.ToInt64(Core.Helper.Crypto.DecryptStringAES(employeeViewModel.ApiClientViewModel.UniqueSessionId.ToString()));

                    _employeeService.UpdateEmployee(employeeViewModel,id);
                    var model = new Dictionary<string, bool>() { { "Status", true } };
                    responseViewModel = model == null ? ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "") : ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new Dictionary<string, bool>());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new Dictionary<string, bool>());
                return Ok(responseViewModel);
            }
        }

        [HttpPost]
        public IHttpActionResult UpdateEmployeeStatus([FromBody]EmployeeViewModel employeeViewModel)
        {
            ResponseViewModel<Dictionary<string, bool>> responseViewModel = new ResponseViewModel<Dictionary<string, bool>>();
            try
            {
                employeeViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(employeeViewModel.ApiClientViewModel.UniqueSessionId, employeeViewModel.ApiClientViewModel.UserName, employeeViewModel.ApiClientViewModel.Password))
                {
                    long id = Convert.ToInt64(Core.Helper.Crypto.DecryptStringAES(employeeViewModel.ApiClientViewModel.UniqueSessionId.ToString()));

                    _employeeService.EmployeeStatus(employeeViewModel.EmployeeId, employeeViewModel.IsActive,id);
                    var model = new Dictionary<string, bool>() { { "Status", true } };
                    responseViewModel = model == null ? ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "") : ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new Dictionary<string, bool>());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new Dictionary<string, bool>());
                return Ok(responseViewModel);
            }
        }




        [HttpPost]
        public IHttpActionResult SavePricing([FromBody]UpdatePricingViewModel updatePricingViewModel)
        {
            ResponseViewModel<Dictionary<string, bool>> responseViewModel = new ResponseViewModel<Dictionary<string, bool>>();
            try
            {
                updatePricingViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(updatePricingViewModel.ApiClientViewModel.UniqueSessionId, updatePricingViewModel.ApiClientViewModel.UserName, updatePricingViewModel.ApiClientViewModel.Password))
                {
                    _pricingService.SavePricing(updatePricingViewModel);
                    var model = new Dictionary<string, bool>() { { "Status", true } };
                    responseViewModel = model == null ? ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "") : ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new Dictionary<string, bool>() { { "Status", false } });
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new Dictionary<string, bool>() { { "Status", false } });
                return Ok(responseViewModel);
            }
        }



        [HttpPost]
        public IHttpActionResult GetAllActiveSessionMaster([FromBody]SessionMasterViewModel sessionMasterViewModel)
        {
            ResponseViewModel<List<SessionMasterViewModel>> responseViewModel = new ResponseViewModel<List<SessionMasterViewModel>>();
            try
            {
                sessionMasterViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(sessionMasterViewModel.ApiClientViewModel.UniqueSessionId, sessionMasterViewModel.ApiClientViewModel.UserName, sessionMasterViewModel.ApiClientViewModel.Password))
                {

                    var model = _sessionService.GetSessionDetailsWithTime(sessionMasterViewModel.BookingTypeId);

                    responseViewModel = model.Any() ? ResponseViewModel<List<SessionMasterViewModel>>.Succeeded(model, "") : ResponseViewModel<List<SessionMasterViewModel>>.Succeeded(model, "No Employee  Found");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<List<SessionMasterViewModel>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new List<SessionMasterViewModel>());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<List<SessionMasterViewModel>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new List<SessionMasterViewModel>());
                return Ok(responseViewModel);
            }
        }



        [HttpPost]
        public IHttpActionResult GetEmployeeProfileByEmployeeId([FromBody]EmployeeViewModel employeeViewModel)
        {
            ResponseViewModel<EmployeeViewModel> responseViewModel = new ResponseViewModel<EmployeeViewModel>();
            try
            {
                employeeViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(employeeViewModel.ApiClientViewModel.UniqueSessionId, employeeViewModel.ApiClientViewModel.UserName, employeeViewModel.ApiClientViewModel.Password))
                {

                    var model = _employeeService.GetEmployeeById(employeeViewModel.EmployeeId);

                    responseViewModel = model == null ? ResponseViewModel<EmployeeViewModel>.Succeeded(model, "") : ResponseViewModel<EmployeeViewModel>.Succeeded(model, "No Employee Found");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<EmployeeViewModel>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new EmployeeViewModel());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<EmployeeViewModel>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new EmployeeViewModel());
                return Ok(responseViewModel);
            }
        }


        [HttpPost]
        public IHttpActionResult SaveSessionDetail([FromBody]SessionMasterViewModel sessionMasterViewModel)
        {
            ResponseViewModel<Dictionary<string, bool>> responseViewModel = new ResponseViewModel<Dictionary<string, bool>>();
            try
            {
                sessionMasterViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(sessionMasterViewModel.ApiClientViewModel.UniqueSessionId, sessionMasterViewModel.ApiClientViewModel.UserName, sessionMasterViewModel.ApiClientViewModel.Password))
                {
                    long id = Convert.ToInt64(Core.Helper.Crypto.DecryptStringAES(sessionMasterViewModel.ApiClientViewModel.UniqueSessionId.ToString()));

                    _sessionService.SaveSession(sessionMasterViewModel,id);
                    var model = new Dictionary<string, bool>() { { "Status", true } };
                    responseViewModel = model == null ? ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "") : ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new Dictionary<string, bool>());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new Dictionary<string, bool>());
                return Ok(responseViewModel);
            }
        }


        [HttpPost]
        public IHttpActionResult UpdateSessionDetail([FromBody]SessionMasterViewModel sessionMasterViewModel)
        {
            ResponseViewModel<Dictionary<string, bool>> responseViewModel = new ResponseViewModel<Dictionary<string, bool>>();
            try
            {
                sessionMasterViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(sessionMasterViewModel.ApiClientViewModel.UniqueSessionId, sessionMasterViewModel.ApiClientViewModel.UserName, sessionMasterViewModel.ApiClientViewModel.Password))
                {
                    long id = Convert.ToInt64(Core.Helper.Crypto.DecryptStringAES(sessionMasterViewModel.ApiClientViewModel.UniqueSessionId.ToString()));

                    _sessionService.UpdateSession(sessionMasterViewModel,id);
                    var model = new Dictionary<string, bool>() { { "Status", true } };
                    responseViewModel = model == null ? ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "") : ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new Dictionary<string, bool>());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new Dictionary<string, bool>());
                return Ok(responseViewModel);
            }
        }

        [HttpPost]
        public IHttpActionResult UpdateSlotSessionDetailTeeTime([FromBody]List<SessionMasterViewModel> sessionMasterViewModels)
        {
            ResponseViewModel<Dictionary<string, bool>> responseViewModel = new ResponseViewModel<Dictionary<string, bool>>();
            try
            {
                sessionMasterViewModels[0].ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(sessionMasterViewModels[0].ApiClientViewModel.UniqueSessionId, sessionMasterViewModels[0].ApiClientViewModel.UserName, sessionMasterViewModels[0].ApiClientViewModel.Password))
                {
                    long id = Convert.ToInt64(Core.Helper.Crypto.DecryptStringAES(sessionMasterViewModels[0].ApiClientViewModel.UniqueSessionId.ToString()));

                    _sessionService.UpdateSlotSessionDetailTeeTime(sessionMasterViewModels,id);
                    var model = new Dictionary<string, bool>() { { "Status", true } };
                    responseViewModel = model == null ? ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "") : ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new Dictionary<string, bool>());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new Dictionary<string, bool>());
                return Ok(responseViewModel);
            }
        }


        [HttpPost]
        public IHttpActionResult UpdateSlotSessionDetailDrivingRange([FromBody]List<SessionMasterViewModel> sessionMasterViewModels)
        {
            ResponseViewModel<Dictionary<string, bool>> responseViewModel = new ResponseViewModel<Dictionary<string, bool>>();
            try
            {
                sessionMasterViewModels[0].ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(sessionMasterViewModels[0].ApiClientViewModel.UniqueSessionId, sessionMasterViewModels[0].ApiClientViewModel.UserName, sessionMasterViewModels[0].ApiClientViewModel.Password))
                {
                    long id = Convert.ToInt64(Core.Helper.Crypto.DecryptStringAES(sessionMasterViewModels[0].ApiClientViewModel.UniqueSessionId.ToString()));

                    _sessionService.UpdateSlotSessionDetailDrivingRange(sessionMasterViewModels,id);
                    var model = new Dictionary<string, bool>() { { "Status", true } };
                    responseViewModel = model == null ? ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "") : ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new Dictionary<string, bool>());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new Dictionary<string, bool>());
                return Ok(responseViewModel);
            }
        }


        [HttpPost]
        public IHttpActionResult GetAllActiveSlotBySessionId([FromBody]SlotViewModel slotViewModel)
        {
            ResponseViewModel<List<SlotViewModel>> responseViewModel = new ResponseViewModel<List<SlotViewModel>>();
            try
            {
                slotViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(slotViewModel.ApiClientViewModel.UniqueSessionId, slotViewModel.ApiClientViewModel.UserName, slotViewModel.ApiClientViewModel.Password))
                {

                    var model = _sessionService.GetAllActiveSlotBySessionId(slotViewModel.SessionId);

                    responseViewModel = model.Any() ? ResponseViewModel<List<SlotViewModel>>.Succeeded(model, "") : ResponseViewModel<List<SlotViewModel>>.Succeeded(model, "No Employee  Found");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<List<SlotViewModel>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new List<SlotViewModel>());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<List<SlotViewModel>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new List<SlotViewModel>());
                return Ok(responseViewModel);
            }
        }


        [HttpPost]
        public IHttpActionResult GetAllPricingDetail([FromBody]UpdatePricingViewModel updatePricingViewModel)
        {
            ResponseViewModel<List<UpdatePricingViewModel>> responseViewModel = new ResponseViewModel<List<UpdatePricingViewModel>>();
            try
            {
                updatePricingViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(updatePricingViewModel.ApiClientViewModel.UniqueSessionId, updatePricingViewModel.ApiClientViewModel.UserName, updatePricingViewModel.ApiClientViewModel.Password))
                {

                    var model = _pricingService.SearchAllPricing();

                    responseViewModel = model.Any() ? ResponseViewModel<List<UpdatePricingViewModel>>.Succeeded(model, "") : ResponseViewModel<List<UpdatePricingViewModel>>.Succeeded(model, "No Employee  Found");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<List<UpdatePricingViewModel>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new List<UpdatePricingViewModel>());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<List<UpdatePricingViewModel>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new List<UpdatePricingViewModel>());
                return Ok(responseViewModel);
            }
        }



        [HttpPost]
        public IHttpActionResult GetAllSessionDetail([FromBody]SessionMasterViewModel sessionMasterViewModel)
        {
            ResponseViewModel<List<SessionMasterViewModel>> responseViewModel = new ResponseViewModel<List<SessionMasterViewModel>>();
            try
            {
                sessionMasterViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(sessionMasterViewModel.ApiClientViewModel.UniqueSessionId, sessionMasterViewModel.ApiClientViewModel.UserName, sessionMasterViewModel.ApiClientViewModel.Password))
                {

                    var model = _sessionService.GetAllSession();

                    responseViewModel = model.Any() ? ResponseViewModel<List<SessionMasterViewModel>>.Succeeded(model, "") : ResponseViewModel<List<SessionMasterViewModel>>.Succeeded(model, "No Employee  Found");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<List<SessionMasterViewModel>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new List<SessionMasterViewModel>());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<List<SessionMasterViewModel>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new List<SessionMasterViewModel>());
                return Ok(responseViewModel);
            }
        }

        [HttpPost]
        public IHttpActionResult GetAllSlotDetailForSpotBooking([FromBody]SlotViewModel slotViewModel)
        {
            ResponseViewModel<List<SlotViewModel>> responseViewModel = new ResponseViewModel<List<SlotViewModel>>();
            try
            {
                slotViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(slotViewModel.ApiClientViewModel.UniqueSessionId, slotViewModel.ApiClientViewModel.UserName, slotViewModel.ApiClientViewModel.Password))
                {

                    var model = _sessionService.GetSlotDetailsByDateAndBookingTypeAndSessionId(slotViewModel.Date, slotViewModel.BookingTypeId, slotViewModel.SessionId, slotViewModel.CoursePairingId);

                    responseViewModel = model.Any() ? ResponseViewModel<List<SlotViewModel>>.Succeeded(model, "") : ResponseViewModel<List<SlotViewModel>>.Succeeded(model, "No Slot  Found");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<List<SlotViewModel>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new List<SlotViewModel>());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<List<SlotViewModel>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new List<SlotViewModel>());
                return Ok(responseViewModel);
            }
        }

        [HttpPost]
        public IHttpActionResult GetBucketDetailList([FromBody]BucketViewModel bucketViewModel)
        {
            ResponseViewModel<List<BucketViewModel>> responseViewModel = new ResponseViewModel<List<BucketViewModel>>();
            try
            {
                bucketViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(bucketViewModel.ApiClientViewModel.UniqueSessionId, bucketViewModel.ApiClientViewModel.UserName, bucketViewModel.ApiClientViewModel.Password))
                {

                    var model = _bookingService.GetBucketDetailList(bucketViewModel.Date, bucketViewModel.MemberTypeId.GetValueOrDefault());
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
        public IHttpActionResult GetBookingPricingCalcalution([FromBody]BookingPricingViewModel bookingPricingViewModel)
        {
            ResponseViewModel<BookingPricingViewModel> responseViewModel = new ResponseViewModel<BookingPricingViewModel>();
            try
            {
                bookingPricingViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(bookingPricingViewModel.ApiClientViewModel.UniqueSessionId, bookingPricingViewModel.ApiClientViewModel.UserName, bookingPricingViewModel.ApiClientViewModel.Password))
                {

                    var model = _bookingService.GetPricingCalculation(bookingPricingViewModel);

                    responseViewModel = model == null ? ResponseViewModel<BookingPricingViewModel>.Succeeded(model, "") : ResponseViewModel<BookingPricingViewModel>.Succeeded(model, "No Employee Found");
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
        public IHttpActionResult SaveBooking([FromBody]SaveBookingViewModel saveBookingViewModel)
        {
            ResponseViewModel<Dictionary<string, bool>> responseViewModel = new ResponseViewModel<Dictionary<string, bool>>();
            try
            {
                saveBookingViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(saveBookingViewModel.ApiClientViewModel.UniqueSessionId, saveBookingViewModel.ApiClientViewModel.UserName, saveBookingViewModel.ApiClientViewModel.Password))
                {
                    _bookingService.SaveBooking(saveBookingViewModel);
                    var model = new Dictionary<string, bool>() { { "Status", true } };
                    responseViewModel = model == null ? ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "") : ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new Dictionary<string, bool>());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new Dictionary<string, bool>());
                return Ok(responseViewModel);
            }
        }


        [HttpPost]
        public IHttpActionResult SaveNationalHolidayPricing([FromBody]UpdatePricingViewModel updatePricingViewModel)
        {
            ResponseViewModel<Dictionary<string, bool>> responseViewModel = new ResponseViewModel<Dictionary<string, bool>>();
            try
            {
                updatePricingViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(updatePricingViewModel.ApiClientViewModel.UniqueSessionId, updatePricingViewModel.ApiClientViewModel.UserName, updatePricingViewModel.ApiClientViewModel.Password))
                {
                    _pricingService.SaveNationalHoildayPricing(updatePricingViewModel);
                    var model = new Dictionary<string, bool>() { { "Status", true } };
                    responseViewModel = model == null ? ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "") : ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new Dictionary<string, bool>() { { "Status", false } });
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new Dictionary<string, bool>() { { "Status", false } });
                return Ok(responseViewModel);
            }
        }
        [HttpPost]
        public IHttpActionResult GetAllMemberType([FromBody]MemberTypeViewModel memberTypeViewModel)
        {
            ResponseViewModel<List<MemberTypeViewModel>> responseViewModel = new ResponseViewModel<List<MemberTypeViewModel>>();
            try
            {
                memberTypeViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(memberTypeViewModel.ApiClientViewModel.UniqueSessionId, memberTypeViewModel.ApiClientViewModel.UserName, memberTypeViewModel.ApiClientViewModel.Password))
                {

                    var model = _memberTypeService.GetAllMemberType();

                    responseViewModel = model.Any() ? ResponseViewModel<List<MemberTypeViewModel>>.Succeeded(model, "") : ResponseViewModel<List<MemberTypeViewModel>>.Succeeded(model, "No Member Type found");
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

        [HttpPost]
        public IHttpActionResult SaveMemberTypeDetails([FromBody]MemberTypeViewModel memberTypeViewModel)
        {
            ResponseViewModel<Dictionary<string, bool>> responseViewModel = new ResponseViewModel<Dictionary<string, bool>>();
            try
            {
                memberTypeViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(memberTypeViewModel.ApiClientViewModel.UniqueSessionId, memberTypeViewModel.ApiClientViewModel.UserName, memberTypeViewModel.ApiClientViewModel.Password))
                {
                    long id = Convert.ToInt64(Core.Helper.Crypto.DecryptStringAES(memberTypeViewModel.ApiClientViewModel.UniqueSessionId.ToString()));

                    _memberTypeService.SaveMemberTypeDetails(memberTypeViewModel.Name, memberTypeViewModel.ValueToShow,id);
                    var model = new Dictionary<string, bool>() { { "Status", true } };
                    responseViewModel = model == null ? ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "") : ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new Dictionary<string, bool>());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new Dictionary<string, bool>());
                return Ok(responseViewModel);
            }
        }
        [HttpPost]
        public IHttpActionResult UpdateMemberTypeDetails([FromBody]MemberTypeViewModel memberTypeViewModel)
        {
            ResponseViewModel<Dictionary<string, bool>> responseViewModel = new ResponseViewModel<Dictionary<string, bool>>();
            try
            {
                memberTypeViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(memberTypeViewModel.ApiClientViewModel.UniqueSessionId, memberTypeViewModel.ApiClientViewModel.UserName, memberTypeViewModel.ApiClientViewModel.Password))
                {
                    long id = Convert.ToInt64(Core.Helper.Crypto.DecryptStringAES(memberTypeViewModel.ApiClientViewModel.UniqueSessionId.ToString()));

                    _memberTypeService.UpdateMemberTypeDetails(memberTypeViewModel.MemberTypeId, memberTypeViewModel.Name, memberTypeViewModel.ValueToShow,id);
                    var model = new Dictionary<string, bool>() { { "Status", true } };
                    responseViewModel = model == null ? ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "") : ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new Dictionary<string, bool>());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new Dictionary<string, bool>());
                return Ok(responseViewModel);
            }
        }
        [HttpPost]
        public IHttpActionResult DeleteMemberTypeDetails([FromBody]MemberTypeViewModel memberTypeViewModel)
        {
            ResponseViewModel<Dictionary<string, bool>> responseViewModel = new ResponseViewModel<Dictionary<string, bool>>();
            try
            {
                memberTypeViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(memberTypeViewModel.ApiClientViewModel.UniqueSessionId, memberTypeViewModel.ApiClientViewModel.UserName, memberTypeViewModel.ApiClientViewModel.Password))
                {
                    long id = Convert.ToInt64(Core.Helper.Crypto.DecryptStringAES(memberTypeViewModel.ApiClientViewModel.UniqueSessionId.ToString()));

                    _memberTypeService.DeleteMemberTypeDetails(memberTypeViewModel.MemberTypeId,id);
                    var model = new Dictionary<string, bool>() { { "Status", true } };
                    responseViewModel = model == null ? ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "") : ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new Dictionary<string, bool>());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new Dictionary<string, bool>());
                return Ok(responseViewModel);
            }
        }
        [HttpPost]
        public IHttpActionResult GetAllTaxType([FromBody]TaxManagementViewModel taxManagementViewModel)
        {
            ResponseViewModel<List<TaxManagementViewModel>> responseViewModel = new ResponseViewModel<List<TaxManagementViewModel>>();
            try
            {
                taxManagementViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(taxManagementViewModel.ApiClientViewModel.UniqueSessionId, taxManagementViewModel.ApiClientViewModel.UserName, taxManagementViewModel.ApiClientViewModel.Password))
                {

                    var model = _taxManagementService.GetAllTaxType();

                    responseViewModel = model.Any() ? ResponseViewModel<List<TaxManagementViewModel>>.Succeeded(model, "") : ResponseViewModel<List<TaxManagementViewModel>>.Succeeded(model, "No Member Type found");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<List<TaxManagementViewModel>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new List<TaxManagementViewModel>());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<List<TaxManagementViewModel>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new List<TaxManagementViewModel>());
                return Ok(responseViewModel);
            }
        }
        [HttpPost]
        public IHttpActionResult SaveTaxTypeDetails([FromBody]TaxManagementViewModel taxManagementViewModel)
        {
            ResponseViewModel<Dictionary<string, bool>> responseViewModel = new ResponseViewModel<Dictionary<string, bool>>();
            try
            {
                taxManagementViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(taxManagementViewModel.ApiClientViewModel.UniqueSessionId, taxManagementViewModel.ApiClientViewModel.UserName, taxManagementViewModel.ApiClientViewModel.Password))
                {
                    long id = Convert.ToInt64(Core.Helper.Crypto.DecryptStringAES(taxManagementViewModel.ApiClientViewModel.UniqueSessionId.ToString()));

                    _taxManagementService.SaveTaxTypeDetails(taxManagementViewModel.Name, taxManagementViewModel.Percentage,id);
                    var model = new Dictionary<string, bool>() { { "Status", true } };
                    responseViewModel = model == null ? ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "") : ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new Dictionary<string, bool>());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new Dictionary<string, bool>());
                return Ok(responseViewModel);
            }
        }
        [HttpPost]
        public IHttpActionResult UpdateTaxTypeDetails([FromBody]TaxManagementViewModel taxManagementViewModel)
        {
            ResponseViewModel<Dictionary<string, bool>> responseViewModel = new ResponseViewModel<Dictionary<string, bool>>();
            try
            {
                taxManagementViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(taxManagementViewModel.ApiClientViewModel.UniqueSessionId, taxManagementViewModel.ApiClientViewModel.UserName, taxManagementViewModel.ApiClientViewModel.Password))
                {
                    long id = Convert.ToInt64(Core.Helper.Crypto.DecryptStringAES(taxManagementViewModel.ApiClientViewModel.UniqueSessionId.ToString()));

                    _taxManagementService.UpdateTaxTypeDetails(taxManagementViewModel.TaxId, taxManagementViewModel.Name, taxManagementViewModel.Percentage,id);
                    var model = new Dictionary<string, bool>() { { "Status", true } };
                    responseViewModel = model == null ? ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "") : ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new Dictionary<string, bool>());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new Dictionary<string, bool>());
                return Ok(responseViewModel);
            }
        }
        [HttpPost]
        public IHttpActionResult DeleteTaxTypeDetails([FromBody]TaxManagementViewModel taxManagementViewModel)
        {
            ResponseViewModel<Dictionary<string, bool>> responseViewModel = new ResponseViewModel<Dictionary<string, bool>>();
            try
            {
                taxManagementViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(taxManagementViewModel.ApiClientViewModel.UniqueSessionId, taxManagementViewModel.ApiClientViewModel.UserName, taxManagementViewModel.ApiClientViewModel.Password))
                {
                    long id = Convert.ToInt64(Core.Helper.Crypto.DecryptStringAES(taxManagementViewModel.ApiClientViewModel.UniqueSessionId.ToString()));

                    _taxManagementService.DeleteTaxTypeDetails(taxManagementViewModel.TaxId,id);
                    var model = new Dictionary<string, bool>() { { "Status", true } };
                    responseViewModel = model == null ? ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "") : ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new Dictionary<string, bool>());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new Dictionary<string, bool>());
                return Ok(responseViewModel);
            }
        }
        [HttpPost]
        public IHttpActionResult GetAllEquipmentType([FromBody]EquipmentViewModel equipmentViewModel)
        {
            ResponseViewModel<List<EquipmentViewModel>> responseViewModel = new ResponseViewModel<List<EquipmentViewModel>>();
            try
            {
                equipmentViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(equipmentViewModel.ApiClientViewModel.UniqueSessionId, equipmentViewModel.ApiClientViewModel.UserName, equipmentViewModel.ApiClientViewModel.Password))
                {

                    var model = _equipmentService.GetAllEquipmentType();

                    responseViewModel = model.Any() ? ResponseViewModel<List<EquipmentViewModel>>.Succeeded(model, "") : ResponseViewModel<List<EquipmentViewModel>>.Succeeded(model, "No Equipment found");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<List<EquipmentViewModel>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new List<EquipmentViewModel>());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<List<EquipmentViewModel>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new List<EquipmentViewModel>());
                return Ok(responseViewModel);
            }
        }
        [HttpPost]
        public IHttpActionResult SaveEquipmentDetails([FromBody] EquipmentViewModel equipmentViewModel)
        {
            ResponseViewModel<Dictionary<string, bool>> responseViewModel = new ResponseViewModel<Dictionary<string, bool>>();
            try
            {
                equipmentViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(equipmentViewModel.ApiClientViewModel.UniqueSessionId, equipmentViewModel.ApiClientViewModel.UserName, equipmentViewModel.ApiClientViewModel.Password))
                {
                    long id = Convert.ToInt64(Core.Helper.Crypto.DecryptStringAES(equipmentViewModel.ApiClientViewModel.UniqueSessionId.ToString()));

                    _equipmentService.SaveEquipmentDetails(equipmentViewModel,id);
                    var model = new Dictionary<string, bool>() { { "Status", true } };
                    responseViewModel = model == null ? ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "") : ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new Dictionary<string, bool>());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new Dictionary<string, bool>());
                return Ok(responseViewModel);
            }
        }
        [HttpPost]
        public IHttpActionResult UpdateEquipmentDetails([FromBody]EquipmentViewModel equipmentViewModel)
        {
            ResponseViewModel<Dictionary<string, bool>> responseViewModel = new ResponseViewModel<Dictionary<string, bool>>();
            try
            {
                equipmentViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(equipmentViewModel.ApiClientViewModel.UniqueSessionId, equipmentViewModel.ApiClientViewModel.UserName, equipmentViewModel.ApiClientViewModel.Password))
                {
                    long id = Convert.ToInt64(Core.Helper.Crypto.DecryptStringAES(equipmentViewModel.ApiClientViewModel.UniqueSessionId.ToString()));

                    _equipmentService.UpdateEquipmentDetails(equipmentViewModel,id);
                    var model = new Dictionary<string, bool>() { { "Status", true } };
                    responseViewModel = model == null ? ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "") : ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new Dictionary<string, bool>());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new Dictionary<string, bool>());
                return Ok(responseViewModel);
            }
        }
        [HttpPost]
        public IHttpActionResult DeleteEquipmentDetails([FromBody]EquipmentViewModel equipmentViewModel)
        {
            ResponseViewModel<Dictionary<string, bool>> responseViewModel = new ResponseViewModel<Dictionary<string, bool>>();
            try
            {
                equipmentViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(equipmentViewModel.ApiClientViewModel.UniqueSessionId, equipmentViewModel.ApiClientViewModel.UserName, equipmentViewModel.ApiClientViewModel.Password))
                {
                    long id = Convert.ToInt64(Core.Helper.Crypto.DecryptStringAES(equipmentViewModel.ApiClientViewModel.UniqueSessionId.ToString()));

                    _equipmentService.DeleteEquipmentDetails(equipmentViewModel.EquipmentId,id);
                    var model = new Dictionary<string, bool>() { { "Status", true } };
                    responseViewModel = model == null ? ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "") : ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new Dictionary<string, bool>());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new Dictionary<string, bool>());
                return Ok(responseViewModel);
            }
        }


        [HttpPost]
        public IHttpActionResult SavePricingNew([FromBody]PricingViewModel pricingViewModel)
        {
            ResponseViewModel<Dictionary<string, bool>> responseViewModel = new ResponseViewModel<Dictionary<string, bool>>();
            try
            {
                pricingViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(pricingViewModel.ApiClientViewModel.UniqueSessionId, pricingViewModel.ApiClientViewModel.UserName, pricingViewModel.ApiClientViewModel.Password))
                {
                    long id = Convert.ToInt64(Core.Helper.Crypto.DecryptStringAES(pricingViewModel.ApiClientViewModel.UniqueSessionId.ToString()));

                    _pricingService.SavePricing(pricingViewModel,id);
                    var model = new Dictionary<string, bool>() { { "Status", true } };
                    responseViewModel = model == null ? ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "") : ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new Dictionary<string, bool>() { { "Status", false } });
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new Dictionary<string, bool>() { { "Status", false } });
                return Ok(responseViewModel);
            }
        }



        [HttpPost]
        public IHttpActionResult GetPriceDetail([FromBody]PricingViewModel pricingViewModel)
        {
            ResponseViewModel<PricingViewModel> responseViewModel = new ResponseViewModel<PricingViewModel>();
            try
            {
                pricingViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(pricingViewModel.ApiClientViewModel.UniqueSessionId, pricingViewModel.ApiClientViewModel.UserName, pricingViewModel.ApiClientViewModel.Password))
                {

                    var model = _pricingService.SearchPricing(pricingViewModel);

                    responseViewModel = model != null ? ResponseViewModel<PricingViewModel>.Succeeded(model, "") : ResponseViewModel<PricingViewModel>.Succeeded(model, "No Price  Found");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<PricingViewModel>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new PricingViewModel());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<PricingViewModel>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new PricingViewModel());
                return Ok(responseViewModel);
            }
        }


        [HttpPost]
        public IHttpActionResult GetAllBookingType([FromBody]BookingTypeViewModel bookingTypeViewModel)
        {
            ResponseViewModel<List<BookingTypeViewModel>> responseViewModel = new ResponseViewModel<List<BookingTypeViewModel>>();
            try
            {
                bookingTypeViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(bookingTypeViewModel.ApiClientViewModel.UniqueSessionId, bookingTypeViewModel.ApiClientViewModel.UserName, bookingTypeViewModel.ApiClientViewModel.Password))
                {

                    var model = _bookingService.GetAllBookingType();

                    responseViewModel = model.Any() ? ResponseViewModel<List<BookingTypeViewModel>>.Succeeded(model, "") : ResponseViewModel<List<BookingTypeViewModel>>.Succeeded(model, "No Booking Type found");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<List<BookingTypeViewModel>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new List<BookingTypeViewModel>());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<List<BookingTypeViewModel>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new List<BookingTypeViewModel>());
                return Ok(responseViewModel);
            }
        }

        [HttpPost]
        public IHttpActionResult GetAllHoleType([FromBody]HoleTypeViewModel bookingTypeViewModel)
        {
            ResponseViewModel<List<HoleTypeViewModel>> responseViewModel = new ResponseViewModel<List<HoleTypeViewModel>>();
            try
            {
                bookingTypeViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(bookingTypeViewModel.ApiClientViewModel.UniqueSessionId, bookingTypeViewModel.ApiClientViewModel.UserName, bookingTypeViewModel.ApiClientViewModel.Password))
                {

                    var model = _holeInformationService.GetAllHoleType();

                    responseViewModel = model.Any() ? ResponseViewModel<List<HoleTypeViewModel>>.Succeeded(model, "") : ResponseViewModel<List<HoleTypeViewModel>>.Succeeded(model, "No Hole Type found");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<List<HoleTypeViewModel>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new List<HoleTypeViewModel>());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<List<HoleTypeViewModel>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new List<HoleTypeViewModel>());
                return Ok(responseViewModel);
            }
        }



        [HttpPost]
        public IHttpActionResult GetAllDayType([FromBody]DayTypeViewModel bookingTypeViewModel)
        {
            ResponseViewModel<List<DayTypeViewModel>> responseViewModel = new ResponseViewModel<List<DayTypeViewModel>>();
            try
            {
                bookingTypeViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(bookingTypeViewModel.ApiClientViewModel.UniqueSessionId, bookingTypeViewModel.ApiClientViewModel.UserName, bookingTypeViewModel.ApiClientViewModel.Password))
                {

                    var model = _bookingService.GetAllDayType();

                    responseViewModel = model.Any() ? ResponseViewModel<List<DayTypeViewModel>>.Succeeded(model, "") : ResponseViewModel<List<DayTypeViewModel>>.Succeeded(model, "No Day Type found");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<List<DayTypeViewModel>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new List<DayTypeViewModel>());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<List<DayTypeViewModel>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new List<DayTypeViewModel>());
                return Ok(responseViewModel);
            }
        }
        [HttpPost]
        public IHttpActionResult GetAllBucketType([FromBody]BucketViewModel bucketViewModel)
        {
            ResponseViewModel<List<BucketViewModel>> responseViewModel = new ResponseViewModel<List<BucketViewModel>>();
            try
            {
                bucketViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(bucketViewModel.ApiClientViewModel.UniqueSessionId, bucketViewModel.ApiClientViewModel.UserName, bucketViewModel.ApiClientViewModel.Password))
                {

                    var model = _bucketService.GetAllBucketType();

                    responseViewModel = model.Any() ? ResponseViewModel<List<BucketViewModel>>.Succeeded(model, "") : ResponseViewModel<List<BucketViewModel>>.Succeeded(model, "No Bucket found");
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
        public IHttpActionResult SaveBucketDetails([FromBody] BucketViewModel bucketViewModel)
        {
            ResponseViewModel<Dictionary<string, bool>> responseViewModel = new ResponseViewModel<Dictionary<string, bool>>();
            try
            {
                bucketViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(bucketViewModel.ApiClientViewModel.UniqueSessionId, bucketViewModel.ApiClientViewModel.UserName, bucketViewModel.ApiClientViewModel.Password))
                {
                    long id = Convert.ToInt64(Core.Helper.Crypto.DecryptStringAES(bucketViewModel.ApiClientViewModel.UniqueSessionId.ToString()));

                    _bucketService.SaveBucketDetails(bucketViewModel,id);
                    var model = new Dictionary<string, bool>() { { "Status", true } };
                    responseViewModel = model == null ? ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "") : ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new Dictionary<string, bool>());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new Dictionary<string, bool>());
                return Ok(responseViewModel);
            }
        }
        [HttpPost]
        public IHttpActionResult DeleteBucketDetails([FromBody]BucketViewModel bucketViewModel)
        {
            ResponseViewModel<Dictionary<string, bool>> responseViewModel = new ResponseViewModel<Dictionary<string, bool>>();
            try
            {
                bucketViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(bucketViewModel.ApiClientViewModel.UniqueSessionId, bucketViewModel.ApiClientViewModel.UserName, bucketViewModel.ApiClientViewModel.Password))
                {
                    long id = Convert.ToInt64(Core.Helper.Crypto.DecryptStringAES(bucketViewModel.ApiClientViewModel.UniqueSessionId.ToString()));

                    _bucketService.DeleteBucketDetails(bucketViewModel.BucketDetailId,id);
                    var model = new Dictionary<string, bool>() { { "Status", true } };
                    responseViewModel = model == null ? ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "") : ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new Dictionary<string, bool>());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new Dictionary<string, bool>());
                return Ok(responseViewModel);
            }
        }
        [HttpPost]
        public IHttpActionResult UpdateBucketDetails([FromBody]BucketViewModel bucketViewModel)
        {
            ResponseViewModel<Dictionary<string, bool>> responseViewModel = new ResponseViewModel<Dictionary<string, bool>>();
            try
            {
                bucketViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(bucketViewModel.ApiClientViewModel.UniqueSessionId, bucketViewModel.ApiClientViewModel.UserName, bucketViewModel.ApiClientViewModel.Password))
                {
                    long id = Convert.ToInt64(Core.Helper.Crypto.DecryptStringAES(bucketViewModel.ApiClientViewModel.UniqueSessionId.ToString()));

                    _bucketService.UpdateBucketDetails(bucketViewModel,id);
                    var model = new Dictionary<string, bool>() { { "Status", true } };
                    responseViewModel = model == null ? ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "") : ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new Dictionary<string, bool>());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new Dictionary<string, bool>());
                return Ok(responseViewModel);
            }
        }

        [HttpPost]
        public IHttpActionResult GetAllPricing([FromBody]PricingViewModel pricingViewModel)
        {
            ResponseViewModel<List<PricingViewModel>> responseViewModel = new ResponseViewModel<List<PricingViewModel>>();
            try
            {
                pricingViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(pricingViewModel.ApiClientViewModel.UniqueSessionId, pricingViewModel.ApiClientViewModel.UserName, pricingViewModel.ApiClientViewModel.Password))
                {

                    var model = _pricingService.GetAllPricing();

                    responseViewModel = model.Any() ? ResponseViewModel<List<PricingViewModel>>.Succeeded(model, "") : ResponseViewModel<List<PricingViewModel>>.Succeeded(model, "No Pricing found");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<List<PricingViewModel>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new List<PricingViewModel>());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<List<PricingViewModel>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new List<PricingViewModel>());
                return Ok(responseViewModel);
            }
        }
        [HttpPost]
        public IHttpActionResult GetAllMaritalStatusType([FromBody]MaritalStatusViewModel maritalStatusViewModel)
        {
            ResponseViewModel<List<MaritalStatusViewModel>> responseViewModel = new ResponseViewModel<List<MaritalStatusViewModel>>();
            try
            {
                maritalStatusViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(maritalStatusViewModel.ApiClientViewModel.UniqueSessionId, maritalStatusViewModel.ApiClientViewModel.UserName, maritalStatusViewModel.ApiClientViewModel.Password))
                {

                    var model = _maritalStatusService.GetAllMaritalStatusType();

                    responseViewModel = model.Any() ? ResponseViewModel<List<MaritalStatusViewModel>>.Succeeded(model, "") : ResponseViewModel<List<MaritalStatusViewModel>>.Succeeded(model, "No Golfer Found");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<List<MaritalStatusViewModel>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new List<MaritalStatusViewModel>());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<List<MaritalStatusViewModel>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new List<MaritalStatusViewModel>());
                return Ok(responseViewModel);
            }
        }
        [HttpPost]
        public IHttpActionResult SaveGolferDetails([FromBody]GolferViewModel golferViewModel)
        {
            ResponseViewModel<Dictionary<string, bool>> responseViewModel = new ResponseViewModel<Dictionary<string, bool>>();
            try
            {
                golferViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(golferViewModel.ApiClientViewModel.UniqueSessionId, golferViewModel.ApiClientViewModel.UserName, golferViewModel.ApiClientViewModel.Password))
                {
                    long id = Convert.ToInt64(Core.Helper.Crypto.DecryptStringAES(golferViewModel.ApiClientViewModel.UniqueSessionId.ToString()));

                    _golferService.SaveGolferDetails(golferViewModel,id);
                    var model = new Dictionary<string, bool>() { { "Status", true } };
                    responseViewModel = model == null ? ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "") : ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new Dictionary<string, bool>());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new Dictionary<string, bool>());
                return Ok(responseViewModel);
            }
        }
        [HttpPost]
        public IHttpActionResult GetGolferByAdvanceSearch([FromBody]GolferViewModel golferViewModel)
        {
            ResponseViewModel<List<GolferViewModel>> responseViewModel = new ResponseViewModel<List<GolferViewModel>>();
            try
            {
                golferViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(golferViewModel.ApiClientViewModel.UniqueSessionId, golferViewModel.ApiClientViewModel.UserName, golferViewModel.ApiClientViewModel.Password))
                {

                    var model = _golferService.GetGolferByAdvanceSearch(golferViewModel);

                    responseViewModel = model.Any() ? ResponseViewModel<List<GolferViewModel>>.Succeeded(model, "") : ResponseViewModel<List<GolferViewModel>>.Succeeded(model, "No Golfer found");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<List<GolferViewModel>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new List<GolferViewModel>());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<List<GolferViewModel>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new List<GolferViewModel>());
                return Ok(responseViewModel);
            }
        }


        [HttpPost]
        public IHttpActionResult GetAllTimeFormat([FromBody]TimeFormatViewModel maritalStatusViewModel)
        {
            ResponseViewModel<List<TimeFormatViewModel>> responseViewModel = new ResponseViewModel<List<TimeFormatViewModel>>();
            try
            {
                maritalStatusViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(maritalStatusViewModel.ApiClientViewModel.UniqueSessionId, maritalStatusViewModel.ApiClientViewModel.UserName, maritalStatusViewModel.ApiClientViewModel.Password))
                {

                    var model = _timeFormatService.GetAllTimeFormat();

                    responseViewModel = model.Any() ? ResponseViewModel<List<TimeFormatViewModel>>.Succeeded(model, "") : ResponseViewModel<List<TimeFormatViewModel>>.Succeeded(model, "No Golfer Found");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<List<TimeFormatViewModel>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new List<TimeFormatViewModel>());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<List<TimeFormatViewModel>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new List<TimeFormatViewModel>());
                return Ok(responseViewModel);
            }
        }
        [HttpPost]
        public IHttpActionResult GetALLSlotBlockReason([FromBody]SlotBlockReasonViewModel slotBlockReasonViewModel)
        {
            ResponseViewModel<List<SlotBlockReasonViewModel>> responseViewModel = new ResponseViewModel<List<SlotBlockReasonViewModel>>();
            try
            {
                slotBlockReasonViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(slotBlockReasonViewModel.ApiClientViewModel.UniqueSessionId, slotBlockReasonViewModel.ApiClientViewModel.UserName, slotBlockReasonViewModel.ApiClientViewModel.Password))
                {

                    var model = _slotBlockReasonService.GetALLSlotBlockReason();

                    responseViewModel = model.Any() ? ResponseViewModel<List<SlotBlockReasonViewModel>>.Succeeded(model, "") : ResponseViewModel<List<SlotBlockReasonViewModel>>.Succeeded(model, "No Slot Block Reason Found");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<List<SlotBlockReasonViewModel>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new List<SlotBlockReasonViewModel>());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<List<SlotBlockReasonViewModel>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new List<SlotBlockReasonViewModel>());
                return Ok(responseViewModel);
            }
        }
        [HttpPost]
        public IHttpActionResult GetALLCoursePairing([FromBody]CoursePairingViewModel coursePairingViewModel)
        {
            ResponseViewModel<List<CoursePairingViewModel>> responseViewModel = new ResponseViewModel<List<CoursePairingViewModel>>();
            try
            {
                coursePairingViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(coursePairingViewModel.ApiClientViewModel.UniqueSessionId, coursePairingViewModel.ApiClientViewModel.UserName, coursePairingViewModel.ApiClientViewModel.Password))
                {

                    var model = _coursePairingService.GetALLCoursePairing();

                    responseViewModel = model.Any() ? ResponseViewModel<List<CoursePairingViewModel>>.Succeeded(model, "") : ResponseViewModel<List<CoursePairingViewModel>>.Succeeded(model, "No Course Pairing Found");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<List<CoursePairingViewModel>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new List<CoursePairingViewModel>());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<List<CoursePairingViewModel>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new List<CoursePairingViewModel>());
                return Ok(responseViewModel);
            }
        }

        [HttpPost]
        public IHttpActionResult GetSlotBlockRangeDetails([FromBody]BlockSlotRangeViewModel blockSlotRangeViewModel)
        {
            ResponseViewModel<List<BlockSlotRangeViewModel>> responseViewModel = new ResponseViewModel<List<BlockSlotRangeViewModel>>();
            try
            {
                blockSlotRangeViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(blockSlotRangeViewModel.ApiClientViewModel.UniqueSessionId, blockSlotRangeViewModel.ApiClientViewModel.UserName, blockSlotRangeViewModel.ApiClientViewModel.Password))
                {

                    var model = _slotReservationService.GetSlotBlockRangeDetails();

                    responseViewModel = model.Any() ? ResponseViewModel<List<BlockSlotRangeViewModel>>.Succeeded(model, "") : ResponseViewModel<List<BlockSlotRangeViewModel>>.Succeeded(model, "No Slot Block Detail Found");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<List<BlockSlotRangeViewModel>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new List<BlockSlotRangeViewModel>());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<List<BlockSlotRangeViewModel>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new List<BlockSlotRangeViewModel>());
                return Ok(responseViewModel);
            }
        }

        [HttpPost]
        public IHttpActionResult GetALLSlotForBlockRange([FromBody]BlockSlotRangeViewModel blockSlotRangeViewModel)
        {
            ResponseViewModel<List<BlockSlotViewModel>> responseViewModel = new ResponseViewModel<List<BlockSlotViewModel>>();
            try
            {
                blockSlotRangeViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(blockSlotRangeViewModel.ApiClientViewModel.UniqueSessionId, blockSlotRangeViewModel.ApiClientViewModel.UserName, blockSlotRangeViewModel.ApiClientViewModel.Password))
                {

                    var model = _slotReservationService.GetAllSlotDetails(blockSlotRangeViewModel.StartDate, blockSlotRangeViewModel.EndDate);

                    responseViewModel = model.Any() ? ResponseViewModel<List<BlockSlotViewModel>>.Succeeded(model, "") : ResponseViewModel<List<BlockSlotViewModel>>.Succeeded(model, "No Slot Block Detail Found");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<List<BlockSlotViewModel>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new List<BlockSlotViewModel>());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<List<BlockSlotViewModel>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new List<BlockSlotViewModel>());
                return Ok(responseViewModel);
            }
        }


        [HttpPost]
        public IHttpActionResult SaveBlockSlotRangeDetail([FromBody]BlockSlotRangeViewModel blockSlotRangeViewModel)
        {
            ResponseViewModel<Dictionary<string, bool>> responseViewModel = new ResponseViewModel<Dictionary<string, bool>>();
            try
            {
                blockSlotRangeViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(blockSlotRangeViewModel.ApiClientViewModel.UniqueSessionId, blockSlotRangeViewModel.ApiClientViewModel.UserName, blockSlotRangeViewModel.ApiClientViewModel.Password))
                {
                    long id = Convert.ToInt64(Core.Helper.Crypto.DecryptStringAES(blockSlotRangeViewModel.ApiClientViewModel.UniqueSessionId.ToString()));

                    _slotReservationService.SaveBlockSlotRangeDetails(blockSlotRangeViewModel,id);
                    var model = new Dictionary<string, bool>() { { "Status", true } };
                    responseViewModel = model == null ? ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "") : ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new Dictionary<string, bool>());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new Dictionary<string, bool>());
                return Ok(responseViewModel);
            }
        }


        [HttpPost]
        public IHttpActionResult DeleteBlockSlotRangeDetails([FromBody]BlockSlotRangeViewModel blockSlotRangeViewModel)
        {
            ResponseViewModel<Dictionary<string, bool>> responseViewModel = new ResponseViewModel<Dictionary<string, bool>>();
            try
            {
                blockSlotRangeViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(blockSlotRangeViewModel.ApiClientViewModel.UniqueSessionId, blockSlotRangeViewModel.ApiClientViewModel.UserName, blockSlotRangeViewModel.ApiClientViewModel.Password))
                {
                    long id = Convert.ToInt64(Core.Helper.Crypto.DecryptStringAES(blockSlotRangeViewModel.ApiClientViewModel.UniqueSessionId.ToString()));

                    _slotReservationService.DeleteBlockSlotRangeDetails(blockSlotRangeViewModel.BlockSlotRangeId,id);
                    var model = new Dictionary<string, bool>() { { "Status", true } };
                    responseViewModel = model == null ? ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "") : ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new Dictionary<string, bool>());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new Dictionary<string, bool>());
                return Ok(responseViewModel);
            }
        }



        [HttpPost]
        public IHttpActionResult BlockUnBlockOperation([FromBody]GolferViewModel golferViewModel)
        {
            ResponseViewModel<Dictionary<string, bool>> responseViewModel = new ResponseViewModel<Dictionary<string, bool>>();
            try
            {
                golferViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(golferViewModel.ApiClientViewModel.UniqueSessionId, golferViewModel.ApiClientViewModel.UserName, golferViewModel.ApiClientViewModel.Password))
                {
                    long id = Convert.ToInt64(Core.Helper.Crypto.DecryptStringAES(golferViewModel.ApiClientViewModel.UniqueSessionId.ToString()));

                    _golferService.BlockUnBlockOperation(golferViewModel.GolferId, golferViewModel.IsBlocked,id);
                    var model = new Dictionary<string, bool>() { { "Status", true } };
                    responseViewModel = model == null ? ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "") : ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new Dictionary<string, bool>());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new Dictionary<string, bool>());
                return Ok(responseViewModel);
            }
        }


        [HttpPost]
        public IHttpActionResult CancelBookingByBookingId([FromBody]BookingViewModel bookingViewModel)
        {
            ResponseViewModel<Dictionary<string, bool>> responseViewModel = new ResponseViewModel<Dictionary<string, bool>>();
            try
            {
                bookingViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(bookingViewModel.ApiClientViewModel.UniqueSessionId, bookingViewModel.ApiClientViewModel.UserName, bookingViewModel.ApiClientViewModel.Password))
                {
                    _bookingService.CancelBooking(bookingViewModel.BookingId, Convert.ToInt64(Core.Helper.Crypto.DecryptStringAES(bookingViewModel.ApiClientViewModel.UniqueSessionId)));
                    var model = new Dictionary<string, bool>() { { "Status", true } };
                    responseViewModel = model == null ? ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "") : ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new Dictionary<string, bool>());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new Dictionary<string, bool>());
                return Ok(responseViewModel);
            }
        }


        [HttpPost]
        public IHttpActionResult SearchGolferByMemberShipId([FromBody]GolferViewModel golferViewModel)
        {
            ResponseViewModel<List<GolferViewModel>> responseViewModel = new ResponseViewModel<List<GolferViewModel>>();
            try
            {
                golferViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(golferViewModel.ApiClientViewModel.UniqueSessionId, golferViewModel.ApiClientViewModel.UserName, golferViewModel.ApiClientViewModel.Password))
                {

                    var model = _golferService.SearchGolferByMemberShipId(golferViewModel.ClubMemberId);

                    responseViewModel = model.Any() ? ResponseViewModel<List<GolferViewModel>>.Succeeded(model, "") : ResponseViewModel<List<GolferViewModel>>.Succeeded(model, "No Golfer Found");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<List<GolferViewModel>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new List<GolferViewModel>());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<List<GolferViewModel>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new List<GolferViewModel>());
                return Ok(responseViewModel);
            }
        }


        [HttpPost]
        public IHttpActionResult SendNotification([FromBody]NotificationGolferViewModel fireBaseViewModel)
        {
            ResponseViewModel<Dictionary<string, bool>> responseViewModel = new ResponseViewModel<Dictionary<string, bool>>();
            try
            {
                fireBaseViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(fireBaseViewModel.ApiClientViewModel.UniqueSessionId, fireBaseViewModel.ApiClientViewModel.UserName, fireBaseViewModel.ApiClientViewModel.Password))
                {
                    long id = Convert.ToInt64(Core.Helper.Crypto.DecryptStringAES(fireBaseViewModel.ApiClientViewModel.UniqueSessionId.ToString()));

                    var model1 = _sendPushNotificationService.SendNotification(fireBaseViewModel,id);
                    var model = new Dictionary<string, bool>() { { "Status", model1 } };
                    responseViewModel = model == null ? ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "") : ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new Dictionary<string, bool>());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new Dictionary<string, bool>());
                return Ok(responseViewModel);
            }
        }




        [HttpPost]
        public IHttpActionResult AddSessionActivityPage([FromBody]List<SessionActivityPageViewModel> sessionActivityPageViewModels)
        {
            ResponseViewModel<Dictionary<string, bool>> responseViewModel = new ResponseViewModel<Dictionary<string, bool>>();
            try
            {
                sessionActivityPageViewModels[0].ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(sessionActivityPageViewModels[0].ApiClientViewModel.UniqueSessionId, sessionActivityPageViewModels[0].ApiClientViewModel.UserName, sessionActivityPageViewModels[0].ApiClientViewModel.Password))
                {
                    _sessionActivityService.AddSessionActivityPage(sessionActivityPageViewModels);
                    var model = new Dictionary<string, bool>() { { "Status", true } };
                    responseViewModel = model == null ? ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "") : ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new Dictionary<string, bool>());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new Dictionary<string, bool>());
                return Ok(responseViewModel);
            }
        }

        [HttpPost]
        public IHttpActionResult SaveSessionActivity([FromBody]SessionActivityPageViewModel sessionActivityPageViewModels)
        {
            ResponseViewModel<Dictionary<string, bool>> responseViewModel = new ResponseViewModel<Dictionary<string, bool>>();
            try
            {
                sessionActivityPageViewModels.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(sessionActivityPageViewModels.ApiClientViewModel.UniqueSessionId, sessionActivityPageViewModels.ApiClientViewModel.UserName, sessionActivityPageViewModels.ApiClientViewModel.Password))
                {
                    _sessionActivityService.AddSessionActivity(sessionActivityPageViewModels, sessionActivityPageViewModels.ApiClientViewModel.UniqueSessionId);
                    var model = new Dictionary<string, bool>() { { "Status", true } };
                    responseViewModel = model == null ? ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "") : ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new Dictionary<string, bool>());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new Dictionary<string, bool>());
                return Ok(responseViewModel);
            }
        }


        [HttpPost]
        public IHttpActionResult GetScoreDetailReportByAdvanceSearch([FromBody]ScoreSearchViewModel scoreSearchViewModel)
        {
            ResponseViewModel<List<ScoreDetailsViewModel>> responseViewModel = new ResponseViewModel<List<ScoreDetailsViewModel>>();
            try
            {
                scoreSearchViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(scoreSearchViewModel.ApiClientViewModel.UniqueSessionId, scoreSearchViewModel.ApiClientViewModel.UserName, scoreSearchViewModel.ApiClientViewModel.Password))
                {

                    var model = _scoreService.GetScoreDetailReportByAdvanceSearch(scoreSearchViewModel);

                    responseViewModel = model.Any() ? ResponseViewModel<List<ScoreDetailsViewModel>>.Succeeded(model, "") : ResponseViewModel<List<ScoreDetailsViewModel>>.Succeeded(model, "No Score Found");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<List<ScoreDetailsViewModel>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new List<ScoreDetailsViewModel>());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<List<ScoreDetailsViewModel>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new List<ScoreDetailsViewModel>());
                return Ok(responseViewModel);
            }
        }


        [HttpPost]
        public IHttpActionResult GetNotificationGolferDetails([FromBody]NotificationGolferViewModel scoreSearchViewModel)
        {
            ResponseViewModel<List<NotificationGolferViewModel>> responseViewModel = new ResponseViewModel<List<NotificationGolferViewModel>>();
            try
            {
                scoreSearchViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(scoreSearchViewModel.ApiClientViewModel.UniqueSessionId, scoreSearchViewModel.ApiClientViewModel.UserName, scoreSearchViewModel.ApiClientViewModel.Password))
                {

                    var model = _sendPushNotificationService.FindAllNotification();

                    responseViewModel = model.Any() ? ResponseViewModel<List<NotificationGolferViewModel>>.Succeeded(model, "") : ResponseViewModel<List<NotificationGolferViewModel>>.Succeeded(model, "No Record Found");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<List<NotificationGolferViewModel>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new List<NotificationGolferViewModel>());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<List<NotificationGolferViewModel>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new List<NotificationGolferViewModel>());
                return Ok(responseViewModel);
            }
        }

        [HttpPost]
        public IHttpActionResult DeleteCoupon([FromBody]CouponViewModel couponViewModel)
        {
            ResponseViewModel<Dictionary<string, bool>> responseViewModel = new ResponseViewModel<Dictionary<string, bool>>();
            try
            {
                couponViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(couponViewModel.ApiClientViewModel.UniqueSessionId, couponViewModel.ApiClientViewModel.UserName, couponViewModel.ApiClientViewModel.Password))
                {
                    long id = Convert.ToInt64(Core.Helper.Crypto.DecryptStringAES(couponViewModel.ApiClientViewModel.UniqueSessionId.ToString()));
                    var model1 = _couponService.DeleteCoupon(couponViewModel.CouponId,id);
                    var model = new Dictionary<string, bool>() { { "Status", model1 } };
                    responseViewModel = model == null ? ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "") : ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new Dictionary<string, bool>());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new Dictionary<string, bool>());
                return Ok(responseViewModel);
            }
        }

        [HttpPost]
        public IHttpActionResult GetAllActiveCoupon([FromBody]CouponViewModel couponViewModel)
        {
            ResponseViewModel<List<CouponViewModel>> responseViewModel = new ResponseViewModel<List<CouponViewModel>>();
            try
            {
                couponViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(couponViewModel.ApiClientViewModel.UniqueSessionId, couponViewModel.ApiClientViewModel.UserName, couponViewModel.ApiClientViewModel.Password))
                {

                    var model = _couponService.GetAllActiveCoupon();

                    responseViewModel = model.Any() ? ResponseViewModel<List<CouponViewModel>>.Succeeded(model, "") : ResponseViewModel<List<CouponViewModel>>.Succeeded(model, "No Record Found");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<List<CouponViewModel>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new List<CouponViewModel>());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<List<CouponViewModel>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new List<CouponViewModel>());
                return Ok(responseViewModel);
            }
        }


        [HttpPost]
        public IHttpActionResult GetReportBookingDetailsBySearch([FromBody]ReportingViewModel reportingViewModel)
        {
            ResponseViewModel<List<ReportingViewModel>> responseViewModel = new ResponseViewModel<List<ReportingViewModel>>();
            try
            {
                reportingViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(reportingViewModel.ApiClientViewModel.UniqueSessionId, reportingViewModel.ApiClientViewModel.UserName, reportingViewModel.ApiClientViewModel.Password))
                {

                    var model = _reportingService.GetBookingDetailsBySearch(reportingViewModel);

                    responseViewModel = model.Any() ? ResponseViewModel<List<ReportingViewModel>>.Succeeded(model, "") : ResponseViewModel<List<ReportingViewModel>>.Succeeded(model, "No Record Found");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<List<ReportingViewModel>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new List<ReportingViewModel>());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<List<ReportingViewModel>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new List<ReportingViewModel>());
                return Ok(responseViewModel);
            }
        }

        [HttpPost]
        public IHttpActionResult GetMoneyDetailsBySearch([FromBody]ReportingViewModel reportingViewModel)
        {
            ResponseViewModel<List<ReportingViewModel>> responseViewModel = new ResponseViewModel<List<ReportingViewModel>>();
            try
            {
                reportingViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(reportingViewModel.ApiClientViewModel.UniqueSessionId, reportingViewModel.ApiClientViewModel.UserName, reportingViewModel.ApiClientViewModel.Password))
                {

                    var model = _reportingService.GetMoneyDetailsBySearch(reportingViewModel);

                    responseViewModel = model.Any() ? ResponseViewModel<List<ReportingViewModel>>.Succeeded(model, "") : ResponseViewModel<List<ReportingViewModel>>.Succeeded(model, "No Record Found");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<List<ReportingViewModel>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new List<ReportingViewModel>());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<List<ReportingViewModel>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new List<ReportingViewModel>());
                return Ok(responseViewModel);
            }
        }


        [HttpPost]
        public IHttpActionResult GetAllBookingStatus([FromBody]BookingStatusViewModel bookingStatusViewModel)
        {
            ResponseViewModel<List<BookingStatusViewModel>> responseViewModel = new ResponseViewModel<List<BookingStatusViewModel>>();
            try
            {
                bookingStatusViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(bookingStatusViewModel.ApiClientViewModel.UniqueSessionId, bookingStatusViewModel.ApiClientViewModel.UserName, bookingStatusViewModel.ApiClientViewModel.Password))
                {

                    var model = _bookingService.GetAllBookingStatus();

                    responseViewModel = model.Any() ? ResponseViewModel<List<BookingStatusViewModel>>.Succeeded(model, "") : ResponseViewModel<List<BookingStatusViewModel>>.Succeeded(model, "No Record Found");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<List<BookingStatusViewModel>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new List<BookingStatusViewModel>());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<List<BookingStatusViewModel>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new List<BookingStatusViewModel>());
                return Ok(responseViewModel);
            }
        }


        [HttpPost]
        public IHttpActionResult GetAllPromotion([FromBody]BookingStatusViewModel bookingStatusViewModel)
        {
            ResponseViewModel<List<PromotionViewModel>> responseViewModel = new ResponseViewModel<List<PromotionViewModel>>();
            try
            {
                bookingStatusViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(bookingStatusViewModel.ApiClientViewModel.UniqueSessionId, bookingStatusViewModel.ApiClientViewModel.UserName, bookingStatusViewModel.ApiClientViewModel.Password))
                {

                    var model = _promotionService.GetAllPromotion();

                    responseViewModel = model.Any() ? ResponseViewModel<List<PromotionViewModel>>.Succeeded(model, "") : ResponseViewModel<List<PromotionViewModel>>.Succeeded(model, "No Record Found");
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
        public IHttpActionResult UpdateEmployeePassword([FromBody]EmployeeViewModel employeeViewModel)
        {
            ResponseViewModel<Dictionary<string, bool>> responseViewModel = new ResponseViewModel<Dictionary<string, bool>>();
            try
            {
                employeeViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(employeeViewModel.ApiClientViewModel.UniqueSessionId, employeeViewModel.ApiClientViewModel.UserName, employeeViewModel.ApiClientViewModel.Password))
                {
                    long id = Convert.ToInt64(Core.Helper.Crypto.DecryptStringAES(employeeViewModel.ApiClientViewModel.UniqueSessionId.ToString()));

                    var model1 = _employeeService.ChangePassword(employeeViewModel,id);
                    var model = new Dictionary<string, bool>() { { "Status", model1 } };
                    responseViewModel = model == null ? ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "") : ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new Dictionary<string, bool>());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new Dictionary<string, bool>());
                return Ok(responseViewModel);
            }
        }

        [HttpPost]
        public IHttpActionResult UpdateGolferPassword([FromBody]GolferViewModel golferViewModel)
        {
            ResponseViewModel<Dictionary<string, bool>> responseViewModel = new ResponseViewModel<Dictionary<string, bool>>();
            try
            {
                golferViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(golferViewModel.ApiClientViewModel.UniqueSessionId, golferViewModel.ApiClientViewModel.UserName, golferViewModel.ApiClientViewModel.Password))
                {
                    long id = Convert.ToInt64(Core.Helper.Crypto.DecryptStringAES(golferViewModel.ApiClientViewModel.UniqueSessionId.ToString()));

                    var model1 = _golferService.ChangePassword(golferViewModel,id);
                    var model = new Dictionary<string, bool>() { { "Status", model1 } };
                    responseViewModel = model == null ? ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "") : ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new Dictionary<string, bool>());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new Dictionary<string, bool>());
                return Ok(responseViewModel);
            }
        }


        [HttpPost]
        public IHttpActionResult GetSessionActivityBySearch([FromBody]SessionDetailViewModel sessionDetailViewModel)
        {
            ResponseViewModel<List<SessionDetailViewModel>> responseViewModel = new ResponseViewModel<List<SessionDetailViewModel>>();
            try
            {
                sessionDetailViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(sessionDetailViewModel.ApiClientViewModel.UniqueSessionId, sessionDetailViewModel.ApiClientViewModel.UserName, sessionDetailViewModel.ApiClientViewModel.Password))
                {

                    var model = _sessionActivityService.GetSessionActivityBySearch(sessionDetailViewModel);

                    responseViewModel = model.Any() ? ResponseViewModel<List<SessionDetailViewModel>>.Succeeded(model, "") : ResponseViewModel<List<SessionDetailViewModel>>.Succeeded(model, "No Record Found");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<List<SessionDetailViewModel>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new List<SessionDetailViewModel>());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<List<SessionDetailViewModel>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new List<SessionDetailViewModel>());
                return Ok(responseViewModel);
            }
        }


        [HttpPost]
        public IHttpActionResult GetTeeTimeSheet([FromBody]TeeSheetViewModel teeSheetViewModel)
        {
            ResponseViewModel<List<TeeSheetViewModel>> responseViewModel = new ResponseViewModel<List<TeeSheetViewModel>>();
            try
            {
                teeSheetViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(teeSheetViewModel.ApiClientViewModel.UniqueSessionId, teeSheetViewModel.ApiClientViewModel.UserName, teeSheetViewModel.ApiClientViewModel.Password))
                {

                    var model = _teeSheetService.GetTeeTimeSheet(teeSheetViewModel);

                    responseViewModel = model.Any() ? ResponseViewModel<List<TeeSheetViewModel>>.Succeeded(model, "") : ResponseViewModel<List<TeeSheetViewModel>>.Succeeded(model, "No Record Found");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<List<TeeSheetViewModel>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new List<TeeSheetViewModel>());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<List<TeeSheetViewModel>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new List<TeeSheetViewModel>());
                return Ok(responseViewModel);
            }
        }



        [HttpPost]
        public IHttpActionResult SearchAllPaymentGatewayControl([FromBody]PaymentGatewayControlViewModel controlViewModel)
        {
            ResponseViewModel<List<PaymentGatewayControlViewModel>> responseViewModel = new ResponseViewModel<List<PaymentGatewayControlViewModel>>();
            try
            {
                controlViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(controlViewModel.ApiClientViewModel.UniqueSessionId, controlViewModel.ApiClientViewModel.UserName, controlViewModel.ApiClientViewModel.Password))
                {

                    var model = _paymentGatewayService.SearchAllPaymentGatewayControl();

                    responseViewModel = model.Any() ? ResponseViewModel<List<PaymentGatewayControlViewModel>>.Succeeded(model, "") : ResponseViewModel<List<PaymentGatewayControlViewModel>>.Succeeded(model, "No Record Found");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<List<PaymentGatewayControlViewModel>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new List<PaymentGatewayControlViewModel>());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<List<PaymentGatewayControlViewModel>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new List<PaymentGatewayControlViewModel>());
                return Ok(responseViewModel);
            }
        }


        [HttpPost]
        public IHttpActionResult SavePaymentGatewayControl([FromBody]PaymentGatewayControlViewModel controlViewModel)
        {
            ResponseViewModel<Dictionary<string, bool>> responseViewModel = new ResponseViewModel<Dictionary<string, bool>>();
            try
            {
                controlViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(controlViewModel.ApiClientViewModel.UniqueSessionId, controlViewModel.ApiClientViewModel.UserName, controlViewModel.ApiClientViewModel.Password))
                {
                    long id = Convert.ToInt64(Core.Helper.Crypto.DecryptStringAES(controlViewModel.ApiClientViewModel.UniqueSessionId.ToString()));

                    _paymentGatewayService.SavePaymentGatewayControl(controlViewModel,id);
                    var model = new Dictionary<string, bool>() { { "Status", true } };
                    responseViewModel = model == null ? ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "") : ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new Dictionary<string, bool>());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new Dictionary<string, bool>());
                return Ok(responseViewModel);
            }
        }


        public IHttpActionResult SavePromotion([FromBody]PromotionViewModel promotionViewModel)
        {
            ResponseViewModel<Dictionary<string, bool>> responseViewModel = new ResponseViewModel<Dictionary<string, bool>>();
            try
            {
                promotionViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(promotionViewModel.ApiClientViewModel.UniqueSessionId, promotionViewModel.ApiClientViewModel.UserName, promotionViewModel.ApiClientViewModel.Password))
                {
                    long id = Convert.ToInt64(Core.Helper.Crypto.DecryptStringAES(promotionViewModel.ApiClientViewModel.UniqueSessionId.ToString()));

                    _promotionService.SavePromotion(promotionViewModel,id);
                    var model = new Dictionary<string, bool>() { { "Status", true } };
                    responseViewModel = model == null ? ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "") : ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new Dictionary<string, bool>());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new Dictionary<string, bool>());
                return Ok(responseViewModel);
            }
        }


        [HttpPost]
        public IHttpActionResult CourseDetailForScorPost([FromBody]ScoreDetailsViewModel commonViewModel)
        {
            ResponseViewModel<ScoreDetailsViewModel> responseViewModel = new ResponseViewModel<ScoreDetailsViewModel>();
            try
            {
                commonViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(commonViewModel.ApiClientViewModel.UniqueSessionId, commonViewModel.ApiClientViewModel.UserName, commonViewModel.ApiClientViewModel.Password))
                {
                    var data = _scoreService.GetScoreCardDataForScorePost(commonViewModel.ScoreId);
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


        [HttpPost]
        public IHttpActionResult DeletePaymentGatewayControl([FromBody]PaymentGatewayControlViewModel controlViewModel)
        {
            ResponseViewModel<Dictionary<string, bool>> responseViewModel = new ResponseViewModel<Dictionary<string, bool>>();
            try
            {
                controlViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(controlViewModel.ApiClientViewModel.UniqueSessionId, controlViewModel.ApiClientViewModel.UserName, controlViewModel.ApiClientViewModel.Password))
                {
                    long id = Convert.ToInt64(Core.Helper.Crypto.DecryptStringAES(controlViewModel.ApiClientViewModel.UniqueSessionId.ToString()));

                    _paymentGatewayService.DeletePaymentGatewayControl(controlViewModel.PaymentGatewayControlId,id);
                    var model = new Dictionary<string, bool>() { { "Status", true } };
                    responseViewModel = model == null ? ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "") : ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new Dictionary<string, bool>());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new Dictionary<string, bool>());
                return Ok(responseViewModel);
            }
        }

        [HttpPost]
        public IHttpActionResult DeletePromotion([FromBody]PromotionViewModel promotionViewModel)
        {
            ResponseViewModel<Dictionary<string, bool>> responseViewModel = new ResponseViewModel<Dictionary<string, bool>>();
            try
            {
                promotionViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(promotionViewModel.ApiClientViewModel.UniqueSessionId, promotionViewModel.ApiClientViewModel.UserName, promotionViewModel.ApiClientViewModel.Password))
                {
                    long id = Convert.ToInt64(Core.Helper.Crypto.DecryptStringAES(promotionViewModel.ApiClientViewModel.UniqueSessionId.ToString()));

                    _promotionService.DeletePromotion(promotionViewModel.PromotionsId,id);
                    var model = new Dictionary<string, bool>() { { "Status", true } };
                    responseViewModel = model == null ? ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "") : ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new Dictionary<string, bool>());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new Dictionary<string, bool>());
                return Ok(responseViewModel);
            }
        }


        [HttpPost]
        public IHttpActionResult UpdateScore([FromBody]ScoreDetailsViewModel scoreDetailsViewModel)
        {
            ResponseViewModel<Dictionary<string, bool>> responseViewModel = new ResponseViewModel<Dictionary<string, bool>>();
            try
            {
                scoreDetailsViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(scoreDetailsViewModel.ApiClientViewModel.UniqueSessionId, scoreDetailsViewModel.ApiClientViewModel.UserName, scoreDetailsViewModel.ApiClientViewModel.Password))
                {
                    long id = Convert.ToInt64(Core.Helper.Crypto.DecryptStringAES(scoreDetailsViewModel.ApiClientViewModel.UniqueSessionId.ToString()));

                    _scoreService.UpdateScoreDetails(scoreDetailsViewModel,id);
                    var model = new Dictionary<string, bool>() { { "Status", true } };
                    responseViewModel = model == null ? ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "") : ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new Dictionary<string, bool>());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new Dictionary<string, bool>());
                return Ok(responseViewModel);
            }
        }

        [HttpPost]
        public IHttpActionResult UpdateBooking([FromBody]BookingViewModel bookingViewModel)
        {
            ResponseViewModel<Dictionary<string, bool>> responseViewModel = new ResponseViewModel<Dictionary<string, bool>>();
            try
            {
                bookingViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(bookingViewModel.ApiClientViewModel.UniqueSessionId, bookingViewModel.ApiClientViewModel.UserName, bookingViewModel.ApiClientViewModel.Password))
                {
                    long id = Convert.ToInt64(Core.Helper.Crypto.DecryptStringAES(bookingViewModel.ApiClientViewModel.UniqueSessionId.ToString()));

                    _teeSheetService.UpdateBooking(bookingViewModel,id);
                    var model = new Dictionary<string, bool>() { { "Status", true } };
                    responseViewModel = model == null ? ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "") : ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new Dictionary<string, bool>());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new Dictionary<string, bool>());
                return Ok(responseViewModel);
            }
        }



        [HttpPost]
        public IHttpActionResult Logout([FromBody]HoleViewModel holeViewModel)
        {
            ResponseViewModel<Dictionary<string, bool>> responseViewModel = new ResponseViewModel<Dictionary<string, bool>>();
            try
            {
                holeViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(holeViewModel.ApiClientViewModel.UniqueSessionId, holeViewModel.ApiClientViewModel.UserName, holeViewModel.ApiClientViewModel.Password))
                {
                    _golferService.Logout(holeViewModel.ApiClientViewModel.UniqueSessionId);
                    var model = new Dictionary<string, bool>() { { "Status", true } };
                    responseViewModel = model == null ? ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "") : ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new Dictionary<string, bool>());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new Dictionary<string, bool>());
                return Ok(responseViewModel);
            }
        }


        [HttpPost]
        public IHttpActionResult GetSalutationList([FromBody]GolferViewModel golferViewModel)
        {
            ResponseViewModel<List<SalutationViewModel>> responseViewModel = new ResponseViewModel<List<SalutationViewModel>>();
            try
            {
                if (_apiClientVaildationService.IsLoggedInClientVaild(golferViewModel.ApiClientViewModel.UniqueSessionId, golferViewModel.ApiClientViewModel.UserName, golferViewModel.ApiClientViewModel.Password))
                {
                    var data = _golferService.GetAllSalutation();
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
        public IHttpActionResult DeleteSession([FromBody]SessionMasterViewModel sessionMasterViewModel)
        {
            ResponseViewModel<Dictionary<string, bool>> responseViewModel = new ResponseViewModel<Dictionary<string, bool>>();
            try
            {
                sessionMasterViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(sessionMasterViewModel.ApiClientViewModel.UniqueSessionId, sessionMasterViewModel.ApiClientViewModel.UserName, sessionMasterViewModel.ApiClientViewModel.Password))
                {
                    long id = Convert.ToInt64(Core.Helper.Crypto.DecryptStringAES(sessionMasterViewModel.ApiClientViewModel.UniqueSessionId.ToString()));

                    _sessionService.DeleteSession(sessionMasterViewModel,id);
                    var model = new Dictionary<string, bool>() { { "Status", true } };
                    responseViewModel = model == null ? ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "") : ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new Dictionary<string, bool>());
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new Dictionary<string, bool>());
                return Ok(responseViewModel);
            }
        }


        [HttpPost]
        public IHttpActionResult GetCouponByCode([FromBody]CouponViewModel couponViewModel)
        {
            ResponseViewModel<CouponViewModel> responseViewModel = new ResponseViewModel<CouponViewModel>();
            try
            {
                couponViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(couponViewModel.ApiClientViewModel.UniqueSessionId, couponViewModel.ApiClientViewModel.UserName, couponViewModel.ApiClientViewModel.Password))
                {

                    var model = _bookingService.GetCouponAmountByCouponCode(couponViewModel.Code);

                    responseViewModel = model != null ? ResponseViewModel<CouponViewModel>.Succeeded(model, "") : ResponseViewModel<CouponViewModel>.Succeeded(model, "No Record Found");
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
        public IHttpActionResult SavePricingNewMultiple([FromBody]PricingViewModel pricingViewModel)
        {
            ResponseViewModel<Dictionary<string, bool>> responseViewModel = new ResponseViewModel<Dictionary<string, bool>>();
            try
            {
                pricingViewModel.ApiClientViewModel.UniqueSessionId.TryValidate("Token");
                if (_apiClientVaildationService.IsLoggedInClientVaild(pricingViewModel.ApiClientViewModel.UniqueSessionId, pricingViewModel.ApiClientViewModel.UserName, pricingViewModel.ApiClientViewModel.Password))
                {
                    long id = Convert.ToInt64(Core.Helper.Crypto.DecryptStringAES(pricingViewModel.ApiClientViewModel.UniqueSessionId.ToString()));

                    _pricingService.SavePricingMuliple(pricingViewModel,id);
                    var model = new Dictionary<string, bool>() { { "Status", true } };
                    responseViewModel = model == null ? ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "") : ResponseViewModel<Dictionary<string, bool>>.Succeeded(model, "");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new Dictionary<string, bool>() { { "Status", false } });
                    return Ok(responseViewModel);
                }
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<Dictionary<string, bool>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new Dictionary<string, bool>() { { "Status", false } });
                return Ok(responseViewModel);
            }
        }


    }
}
