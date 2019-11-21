using GolfCentra.Core.DataBase;
using GolfCentra.Core.GenericRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.Core
{
    public class UnitOfWork : IDisposable
    {

        #region Private member variables...
        private readonly GolfCentraEntities _context;
        private DbContextTransaction Transaction { get; set; }
        private Dictionary<string, object> repositories;
        private GenericRepository<Golfer> _golferRepository;
        private GenericRepository<ParStorke> _parStorkeRepository;
        private GenericRepository<Tee> _teeRepository;
        private GenericRepository<HoleTeeYardage> _holeTeeYardageRepository;
        private GenericRepository<HoleNumber> _holeNumberRepository;
        private GenericRepository<HoleInformation> _holeInformationRepository;
        private GenericRepository<Course> _courseRepository;
        private GenericRepository<Country> _countryRepository;
        private GenericRepository<Score> _scoreRepository;
        private GenericRepository<LogInHistory> _logInHistoryRepository;
        private GenericRepository<APIClient> _apiClientRepository;
        private GenericRepository<AboutU> _aboutUSRepository;
        private GenericRepository<Booking> _bookingRepository;
        private GenericRepository<Coupon> _couponRepository;
        private GenericRepository<Pricing> _pricingRepository;
        private GenericRepository<BucketDetail> _bucketDetailRepository;
        private GenericRepository<SlotSessionWise> _slotSessionWiseRepository;
        private GenericRepository<Session> _sessionRepository;
        private GenericRepository<RuleAndRegulation> _ruleAndRegulationRepository;
        private GenericRepository<Slot> _slotRepository;
        private GenericRepository<HoleType> _holeTypeRepository;
        private GenericRepository<Employee> _employeeRepository;
        private GenericRepository<Page> _pageRepository;
        private GenericRepository<PageRight> _pageRightRepository;
        private GenericRepository<EmployeeType> _employeeTypeRepository;
        private GenericRepository<GenderType> _genderTypeRepository;
        private GenericRepository<DataBase.Action> _actionRepository;
        private GenericRepository<ActionRight> _actionRightRepository;
        private GenericRepository<AppVersion> _appVersionRepository;
        private GenericRepository<NationalHoliday> _nationalHolidayRepository;
        private GenericRepository<MemberType> _memberTypeRepository;
        private GenericRepository<Tax> _taxRepository;
        private GenericRepository<PriceTaxMapping> _priceTaxMappingRepository;
        private GenericRepository<BookingType> _bookingTypeRepository;
        private GenericRepository<DayType> _dayTypeRepository;
        private GenericRepository<Equipment> _equipmentRepository;
        private GenericRepository<EquipmentTaxMapping> _equipmentTaxMappingRepository;

        private GenericRepository<BucketTaxMapping> _bucketTaxMappingRepository;

        private GenericRepository<BookingPlayerMapping> _bookingPlayerMappingRepository;

        private GenericRepository<CourseTaxMapping> _courseTaxMappingRepository;
        private GenericRepository<BookingCourseTaxMapping> _bookingCourseTaxMappingRepository;
        private GenericRepository<MaritalStatu> _maritalStatuRepository;
        private GenericRepository<BookingEquipmentMapping> _bookingEquipmentMapping;

        private GenericRepository<TimeFormat> _timeFormatRepository;
        private GenericRepository<CoursePairing> _coursePairingRepository;
        private GenericRepository<BlockSlotRange> _blockSlotRangeRepository;
        private GenericRepository<BlockSlot> _blockSlotRepository;
        private GenericRepository<SlotBlockReason> _slotBlockReasonRepository;
        private GenericRepository<BookingPlayerDetail> _bookingPlayerDetailRepository;

        private GenericRepository<Promotion> _promotionRepository;
        private GenericRepository<Salutation> _salutationRepository;

        private GenericRepository<SessionActivity> _sessionActivityRepository;
        private GenericRepository<SessionActivityPage> _sessionActivityPageRepository;
        private GenericRepository<PaymentGatewayControl> _paymentGatewayControlRepository;
        private GenericRepository<GolferNotification> _golferNotificationRepository;

        private GenericRepository<BookingStatu> _bookingStatusRepository;

        private GenericRepository<ScoreHole1> _scoreHole1Repository;
        private GenericRepository<ScoreHole10> _scoreHole10Repository;
        private GenericRepository<ScoreHole11> _scoreHole11Repository;
        private GenericRepository<ScoreHole12> _scoreHole12Repository;
        private GenericRepository<ScoreHole13> _scoreHole13Repository;
        private GenericRepository<ScoreHole14> _scoreHole14Repository;
        private GenericRepository<ScoreHole15> _scoreHole15Repository;
        private GenericRepository<ScoreHole16> _scoreHole16Repository;
        private GenericRepository<ScoreHole17> _scoreHole17Repository;
        private GenericRepository<ScoreHole18> _scoreHole18Repository;
        private GenericRepository<ScoreHole2> _scoreHole2Repository;
        private GenericRepository<ScoreHole3> _scoreHole3Repository;
        private GenericRepository<ScoreHole4> _scoreHole4Repository;
        private GenericRepository<ScoreHole5> _scoreHole5Repository;
        private GenericRepository<ScoreHole6> _scoreHole6Repository;
        private GenericRepository<ScoreHole7> _scoreHole7Repository;
        private GenericRepository<ScoreHole8> _scoreHole8Repository;
        private GenericRepository<ScoreHole9> _scoreHole9Repository;
        private GenericRepository<GolferPasswordChange> _golferPasswordChangeRepository;
        #endregion


        public UnitOfWork()
        {
            _context = new GolfCentraEntities();
        }

        public UnitOfWork BeginTransaction()
        {
            Transaction = _context.Database.BeginTransaction();
            return this;
        }

        #region Public Repository Creation properties...
        /// <summary>
        /// Get/Set Property for Golfer repository.
        /// </summary>
        public GenericRepository<Golfer> GolferRepository
        {
            get
            {
                if (this._golferRepository == null)
                    this._golferRepository = new GenericRepository<Golfer>(_context);
                return _golferRepository;
            }
        }

        /// <summary>
        /// Get/Set Property for Tee repository.
        /// </summary>
        public GenericRepository<Tee> TeeRepository
        {
            get
            {
                if (this._teeRepository == null)
                    this._teeRepository = new GenericRepository<Tee>(_context);
                return _teeRepository;
            }
        }

        /// <summary>
        /// Get/Set Property for ParStorke repository.
        /// </summary>
        public GenericRepository<ParStorke> ParStorkeRepository
        {
            get
            {
                if (this._parStorkeRepository == null)
                    this._parStorkeRepository = new GenericRepository<ParStorke>(_context);
                return _parStorkeRepository;
            }
        }

        /// <summary>
        /// Get/Set Property for HoleTeeYardage repository.
        /// </summary>
        public GenericRepository<HoleTeeYardage> HoleTeeYardageRepository
        {
            get
            {
                if (this._holeTeeYardageRepository == null)
                    this._holeTeeYardageRepository = new GenericRepository<HoleTeeYardage>(_context);
                return _holeTeeYardageRepository;
            }
        }

        /// <summary>
        /// Get/Set Property for HoleNumber repository.
        /// </summary>
        public GenericRepository<HoleNumber> HoleNumberRepository
        {
            get
            {
                if (this._holeNumberRepository == null)
                    this._holeNumberRepository = new GenericRepository<HoleNumber>(_context);
                return _holeNumberRepository;
            }
        }

        /// <summary>
        /// Get/Set Property for HoleInformation repository.
        /// </summary>
        public GenericRepository<HoleInformation> HoleInformationRepository
        {
            get
            {
                if (this._holeInformationRepository == null)
                    this._holeInformationRepository = new GenericRepository<HoleInformation>(_context);
                return _holeInformationRepository;
            }
        }

        /// <summary>
        /// Get/Set Property for Course repository.
        /// </summary>
        public GenericRepository<Course> CourseRepository
        {
            get
            {
                if (this._courseRepository == null)
                    this._courseRepository = new GenericRepository<Course>(_context);
                return _courseRepository;
            }
        }


        /// <summary>
        /// Get/Set Property for Country repository.
        /// </summary>
        public GenericRepository<Country> CountryRepository
        {
            get
            {
                if (this._countryRepository == null)
                    this._countryRepository = new GenericRepository<Country>(_context);
                return _countryRepository;
            }
        }

        /// <summary>
        /// Get/Set Property for Score repository.
        /// </summary>
        public GenericRepository<Score> ScoreRepository
        {
            get
            {
                if (this._scoreRepository == null)
                    this._scoreRepository = new GenericRepository<Score>(_context);
                return _scoreRepository;
            }
        }


        /// <summary>
        /// Get/Set Property for LogIn History repository.
        /// </summary>
        public GenericRepository<LogInHistory> LogInHistoryRepository
        {
            get
            {
                if (this._logInHistoryRepository == null)
                    this._logInHistoryRepository = new GenericRepository<LogInHistory>(_context);
                return _logInHistoryRepository;
            }
        }

        /// <summary>
        /// Get/Set Property for APIClient repository.
        /// </summary>
        public GenericRepository<APIClient> APIClientRepository
        {
            get
            {
                if (this._apiClientRepository == null)
                    this._apiClientRepository = new GenericRepository<APIClient>(_context);
                return _apiClientRepository;
            }
        }

        /// <summary>
        /// Get/Set Property for AboutUS repository.
        /// </summary>
        public GenericRepository<AboutU> AboutUSRepository
        {
            get

            {
                if (this._aboutUSRepository == null)
                    this._aboutUSRepository = new GenericRepository<AboutU>(_context);
                return _aboutUSRepository;
            }
        }


        /// <summary>
        /// Get/Set Property for Booking repository.
        /// </summary>
        public GenericRepository<Booking> BookingRepository
        {
            get

            {
                if (this._bookingRepository == null)
                    this._bookingRepository = new GenericRepository<Booking>(_context);
                return _bookingRepository;
            }
        }

        /// <summary>
        /// Get/Set Property for Coupon repository.
        /// </summary>
        public GenericRepository<Coupon> CouponRepository
        {
            get

            {
                if (this._couponRepository == null)
                    this._couponRepository = new GenericRepository<Coupon>(_context);
                return _couponRepository;
            }
        }

        /// <summary>
        /// Get/Set Property for Pricing repository.
        /// </summary>
        public GenericRepository<Pricing> PricingRepository
        {
            get

            {
                if (this._pricingRepository == null)
                    this._pricingRepository = new GenericRepository<Pricing>(_context);
                return _pricingRepository;
            }
        }
        /// <summary>
        ///  Get/Set Property for Bucket repository.
        /// </summary>
        public GenericRepository<BucketDetail> BucketDetailRepository
        {
            get

            {
                if (this._bucketDetailRepository == null)
                    this._bucketDetailRepository = new GenericRepository<BucketDetail>(_context);
                return _bucketDetailRepository;
            }
        }

        /// <summary>
        ///  Get/Set Property for Slot Session Wise repository.
        /// </summary>
        public GenericRepository<SlotSessionWise> SlotSessionWiseRepository
        {
            get

            {
                if (this._slotSessionWiseRepository == null)
                    this._slotSessionWiseRepository = new GenericRepository<SlotSessionWise>(_context);
                return _slotSessionWiseRepository;
            }
        }

        /// <summary>
        ///  Get/Set Property for Session repository.
        /// </summary>
        public GenericRepository<Session> SessionRepository
        {
            get

            {
                if (this._sessionRepository == null)
                    this._sessionRepository = new GenericRepository<Session>(_context);
                return _sessionRepository;
            }
        }

        /// <summary>
        ///  Get/Set Property for Rule And Regulation repository.
        /// </summary>
        public GenericRepository<RuleAndRegulation> RuleAndRegulationRepository
        {
            get

            {
                if (this._ruleAndRegulationRepository == null)
                    this._ruleAndRegulationRepository = new GenericRepository<RuleAndRegulation>(_context);
                return _ruleAndRegulationRepository;
            }
        }

        /// <summary>
        ///  Get/Set Property for Slot repository.
        /// </summary>
        public GenericRepository<Slot> SlotRepository
        {
            get

            {
                if (this._slotRepository == null)
                    this._slotRepository = new GenericRepository<Slot>(_context);
                return _slotRepository;
            }
        }

        /// <summary>
        ///  Get/Set Property for Hole Type repository.
        /// </summary>
        public GenericRepository<HoleType> HoleTypeRepository
        {
            get

            {
                if (this._holeTypeRepository == null)
                    this._holeTypeRepository = new GenericRepository<HoleType>(_context);
                return _holeTypeRepository;
            }
        }

        /// <summary>
        ///  Get/Set Property for Employee repository.
        /// </summary>
        public GenericRepository<Employee> EmployeeRepository
        {
            get

            {
                if (this._employeeRepository == null)
                    this._employeeRepository = new GenericRepository<Employee>(_context);
                return _employeeRepository;
            }
        }

        /// <summary>
        ///  Get/Set Property for Page repository.
        /// </summary>
        public GenericRepository<Page> PageRepository
        {
            get

            {
                if (this._pageRepository == null)
                    this._pageRepository = new GenericRepository<Page>(_context);
                return _pageRepository;
            }
        }

        /// <summary>
        ///  Get/Set Property for Page right repository.
        /// </summary>
        public GenericRepository<PageRight> PageRightRepository
        {
            get

            {
                if (this._pageRightRepository == null)
                    this._pageRightRepository = new GenericRepository<PageRight>(_context);
                return _pageRightRepository;
            }
        }

        /// <summary>
        ///  Get/Set Property for Employee Type repository.
        /// </summary>
        public GenericRepository<EmployeeType> EmployeeTypeRepository
        {
            get
            {
                if (this._employeeTypeRepository == null)
                    this._employeeTypeRepository = new GenericRepository<EmployeeType>(_context);
                return _employeeTypeRepository;
            }
        }

        /// <summary>
        ///  Get/Set Property for Gender Type repository.
        /// </summary>
        public GenericRepository<GenderType> GenderTypeRepository
        {
            get
            {
                if (this._genderTypeRepository == null)
                    this._genderTypeRepository = new GenericRepository<GenderType>(_context);
                return _genderTypeRepository;
            }
        }

        /// <summary>
        ///  Get/Set Property for Action repository.
        /// </summary>
        public GenericRepository<DataBase.Action> ActionRepository
        {
            get
            {
                if (this._actionRepository == null)
                    this._actionRepository = new GenericRepository<DataBase.Action>(_context);
                return _actionRepository;
            }
        }

        /// <summary>
        ///  Get/Set Property for Action Right repository.
        /// </summary>
        public GenericRepository<ActionRight> ActionRightRepository
        {
            get
            {
                if (this._actionRightRepository == null)
                    this._actionRightRepository = new GenericRepository<ActionRight>(_context);
                return _actionRightRepository;
            }
        }

        /// <summary>
        ///  Get/Set Property for App Version repository.
        /// </summary>
        public GenericRepository<AppVersion> AppVersionRepository
        {
            get
            {
                if (this._appVersionRepository == null)
                    this._appVersionRepository = new GenericRepository<AppVersion>(_context);
                return _appVersionRepository;
            }
        }
        /// <summary>
        ///  Get/Set Property for National Holiday repository.
        /// </summary>
        public GenericRepository<NationalHoliday> NationalHolidayRepository
        {
            get
            {
                if (this._nationalHolidayRepository == null)
                    this._nationalHolidayRepository = new GenericRepository<NationalHoliday>(_context);
                return _nationalHolidayRepository;
            }
        }

        /// <summary>
        ///  Get/Set Property for Member Type repository.
        /// </summary>
        public GenericRepository<MemberType> MemberTypeRepository
        {
            get
            {
                if (this._memberTypeRepository == null)
                    this._memberTypeRepository = new GenericRepository<MemberType>(_context);
                return _memberTypeRepository;
            }
        }

        /// <summary>
        ///  Get/Set Property for Tax repository.
        /// </summary>
        public GenericRepository<Tax> TaxRepository
        {
            get
            {
                if (this._taxRepository == null)
                    this._taxRepository = new GenericRepository<Tax>(_context);
                return _taxRepository;
            }
        }

        /// <summary>
        ///  Get/Set Property for Equipment repository.
        /// </summary>
        public GenericRepository<Equipment> EquipmentRepository
        {
            get
            {
                if (this._equipmentRepository == null)
                    this._equipmentRepository = new GenericRepository<Equipment>(_context);
                return _equipmentRepository;
            }
        }

        /// <summary>
        ///  Get/Set Property for EquipmentTaxMapping repository.
        /// </summary>
        public GenericRepository<EquipmentTaxMapping> EquipmentTaxMappingRepository
        {
            get
            {
                if (this._equipmentTaxMappingRepository == null)
                    this._equipmentTaxMappingRepository = new GenericRepository<EquipmentTaxMapping>(_context);
                return _equipmentTaxMappingRepository;
            }
        }

        /// <summary>
        ///  Get/Set Property for bucketTaxMapping repository.
        /// </summary>
        public GenericRepository<BucketTaxMapping> BucketTaxMappingRepository
        {
            get
            {
                if (this._bucketTaxMappingRepository == null)
                    this._bucketTaxMappingRepository = new GenericRepository<BucketTaxMapping>(_context);
                return _bucketTaxMappingRepository;
            }
        }


        /// <summary>
        ///  Get/Set Property for PricetaxMapping repository.
        /// </summary>
        public GenericRepository<PriceTaxMapping> PriceTaxMappingRepository
        {
            get
            {
                if (this._priceTaxMappingRepository == null)
                    this._priceTaxMappingRepository = new GenericRepository<PriceTaxMapping>(_context);
                return _priceTaxMappingRepository;
            }
        }

        /// <summary>
        ///  Get/Set Property for booking Type repository.
        /// </summary>
        public GenericRepository<BookingType> BookingTypeRepository
        {
            get
            {
                if (this._bookingTypeRepository == null)
                    this._bookingTypeRepository = new GenericRepository<BookingType>(_context);
                return _bookingTypeRepository;
            }
        }

        /// <summary>
        ///  Get/Set Property for day Type repository.
        /// </summary>
        public GenericRepository<DayType> DayTypeRepository
        {
            get
            {
                if (this._dayTypeRepository == null)
                    this._dayTypeRepository = new GenericRepository<DayType>(_context);
                return _dayTypeRepository;
            }
        }

        /// <summary>
        ///  Get/Set Property for booking Player repository.
        /// </summary>
        public GenericRepository<BookingPlayerMapping> BookingPlayerMappingRepository
        {
            get
            {
                if (this._bookingPlayerMappingRepository == null)
                    this._bookingPlayerMappingRepository = new GenericRepository<BookingPlayerMapping>(_context);
                return _bookingPlayerMappingRepository;
            }
        }

        /// <summary>
        ///  Get/Set Property for Course TaxMappig repository.
        /// </summary>
        public GenericRepository<CourseTaxMapping> CourseTaxMappingRepository
        {
            get
            {
                if (this._courseTaxMappingRepository == null)
                    this._courseTaxMappingRepository = new GenericRepository<CourseTaxMapping>(_context);
                return _courseTaxMappingRepository;
            }
        }

        /// <summary>
        ///  Get/Set Property for booking CourseTax Mapping repository.
        /// </summary>
        public GenericRepository<BookingCourseTaxMapping> BookingCourseTaxMappingRepository
        {
            get
            {
                if (this._bookingCourseTaxMappingRepository == null)
                    this._bookingCourseTaxMappingRepository = new GenericRepository<BookingCourseTaxMapping>(_context);
                return _bookingCourseTaxMappingRepository;
            }
        }

        /// <summary>
        ///  Get/Set Property for MaritalStatus repository.
        /// </summary>
        public GenericRepository<MaritalStatu> MaritalStatuRepository
        {
            get
            {
                if (this._maritalStatuRepository == null)
                    this._maritalStatuRepository = new GenericRepository<MaritalStatu>(_context);
                return _maritalStatuRepository;
            }
        }

        /// <summary>
        ///  Get/Set Property for BookingEquipmentMapping repository.
        /// </summary>
        public GenericRepository<BookingEquipmentMapping> BookingEquipmentMappingRepository
        {
            get
            {
                if (this._bookingEquipmentMapping == null)
                    this._bookingEquipmentMapping = new GenericRepository<BookingEquipmentMapping>(_context);
                return _bookingEquipmentMapping;
            }
        }

        /// <summary>
        ///  Get/Set Property for TimeFormat repository.
        /// </summary>
        public GenericRepository<TimeFormat> TimeFormatRepository
        {
            get
            {
                if (this._timeFormatRepository == null)
                    this._timeFormatRepository = new GenericRepository<TimeFormat>(_context);
                return _timeFormatRepository;
            }
        }

        public GenericRepository<CoursePairing> CoursePairingRepository
        {
            get
            {
                if (this._coursePairingRepository == null)
                    this._coursePairingRepository = new GenericRepository<CoursePairing>(_context);
                return _coursePairingRepository;
            }
        }

        public GenericRepository<BlockSlotRange> BlockSlotRangeRepository
        {
            get
            {
                if (this._blockSlotRangeRepository == null)
                    this._blockSlotRangeRepository = new GenericRepository<BlockSlotRange>(_context);
                return _blockSlotRangeRepository;
            }
        }

        public GenericRepository<BlockSlot> BlockSlotRepository
        {
            get
            {
                if (this._blockSlotRepository == null)
                    this._blockSlotRepository = new GenericRepository<BlockSlot>(_context);
                return _blockSlotRepository;
            }
        }

        public GenericRepository<SlotBlockReason> SlotBlockReasonRepository
        {
            get
            {
                if (this._slotBlockReasonRepository == null)
                    this._slotBlockReasonRepository = new GenericRepository<SlotBlockReason>(_context);
                return _slotBlockReasonRepository;
            }
        }
        public GenericRepository<BookingPlayerDetail> BookingPlayerDetailRepository
        {
            get
            {
                if (this._bookingPlayerDetailRepository == null)
                    this._bookingPlayerDetailRepository = new GenericRepository<BookingPlayerDetail>(_context);
                return _bookingPlayerDetailRepository;
            }
        }

        public GenericRepository<Promotion> PromotionRepository
        {
            get
            {
                if (this._promotionRepository == null)
                    this._promotionRepository = new GenericRepository<Promotion>(_context);
                return _promotionRepository;
            }
        }

        public GenericRepository<Salutation> SalutationRepository
        {
            get
            {
                if (this._salutationRepository == null)
                    this._salutationRepository = new GenericRepository<Salutation>(_context);
                return _salutationRepository;
            }
        }



        public GenericRepository<SessionActivity> SessionActivityRepository
        {
            get
            {
                if (this._sessionActivityRepository == null)
                    this._sessionActivityRepository = new GenericRepository<SessionActivity>(_context);
                return _sessionActivityRepository;
            }
        }

        public GenericRepository<SessionActivityPage> SessionActivityPageRepository
        {
            get
            {
                if (this._sessionActivityPageRepository == null)
                    this._sessionActivityPageRepository = new GenericRepository<SessionActivityPage>(_context);
                return _sessionActivityPageRepository;
            }
        }

        public GenericRepository<PaymentGatewayControl> PaymentGatewayControlRepository
        {
            get
            {
                if (this._paymentGatewayControlRepository == null)
                    this._paymentGatewayControlRepository = new GenericRepository<PaymentGatewayControl>(_context);
                return _paymentGatewayControlRepository;
            }
        }

        public GenericRepository<GolferNotification> GolferNotificationRepository
        {
            get
            {
                if (this._golferNotificationRepository == null)
                    this._golferNotificationRepository = new GenericRepository<GolferNotification>(_context);
                return _golferNotificationRepository;
            }
        }

        public GenericRepository<BookingStatu> BookingStatusRepository
        {
            get
            {
                if (this._bookingStatusRepository == null)
                    this._bookingStatusRepository = new GenericRepository<BookingStatu>(_context);
                return _bookingStatusRepository;
            }
        }

        public GenericRepository<ScoreHole1> ScoreHole1Repository
        {
            get
            {
                if (this._scoreHole1Repository == null)
                    this._scoreHole1Repository = new GenericRepository<ScoreHole1>(_context);
                return _scoreHole1Repository;
            }
        }

        public GenericRepository<ScoreHole2> ScoreHole2Repository
        {
            get
            {
                if (this._scoreHole2Repository == null)
                    this._scoreHole2Repository = new GenericRepository<ScoreHole2>(_context);
                return _scoreHole2Repository;
            }
        }

        public GenericRepository<ScoreHole3> ScoreHole3Repository
        {
            get
            {
                if (this._scoreHole3Repository == null)
                    this._scoreHole3Repository = new GenericRepository<ScoreHole3>(_context);
                return _scoreHole3Repository;
            }
        }

        public GenericRepository<ScoreHole4> ScoreHole4Repository
        {
            get
            {
                if (this._scoreHole4Repository == null)
                    this._scoreHole4Repository = new GenericRepository<ScoreHole4>(_context);
                return _scoreHole4Repository;
            }
        }

        public GenericRepository<ScoreHole5> ScoreHole5Repository
        {
            get
            {
                if (this._scoreHole5Repository == null)
                    this._scoreHole5Repository = new GenericRepository<ScoreHole5>(_context);
                return _scoreHole5Repository;
            }
        }

        public GenericRepository<ScoreHole6> ScoreHole6Repository
        {
            get
            {
                if (this._scoreHole6Repository == null)
                    this._scoreHole6Repository = new GenericRepository<ScoreHole6>(_context);
                return _scoreHole6Repository;
            }
        }

        public GenericRepository<ScoreHole7> ScoreHole7Repository
        {
            get
            {
                if (this._scoreHole7Repository == null)
                    this._scoreHole7Repository = new GenericRepository<ScoreHole7>(_context);
                return _scoreHole7Repository;
            }
        }

        public GenericRepository<ScoreHole8> ScoreHole8Repository
        {
            get
            {
                if (this._scoreHole8Repository == null)
                    this._scoreHole8Repository = new GenericRepository<ScoreHole8>(_context);
                return _scoreHole8Repository;
            }
        }

        public GenericRepository<ScoreHole9> ScoreHole9Repository
        {
            get
            {
                if (this._scoreHole9Repository == null)
                    this._scoreHole9Repository = new GenericRepository<ScoreHole9>(_context);
                return _scoreHole9Repository;
            }
        }

        public GenericRepository<ScoreHole10> ScoreHole10Repository
        {
            get
            {
                if (this._scoreHole10Repository == null)
                    this._scoreHole10Repository = new GenericRepository<ScoreHole10>(_context);
                return _scoreHole10Repository;
            }
        }

        public GenericRepository<ScoreHole11> ScoreHole11Repository
        {
            get
            {
                if (this._scoreHole11Repository == null)
                    this._scoreHole11Repository = new GenericRepository<ScoreHole11>(_context);
                return _scoreHole11Repository;
            }
        }

        public GenericRepository<ScoreHole12> ScoreHole12Repository
        {
            get
            {
                if (this._scoreHole12Repository == null)
                    this._scoreHole12Repository = new GenericRepository<ScoreHole12>(_context);
                return _scoreHole12Repository;
            }
        }

        public GenericRepository<ScoreHole13> ScoreHole13Repository
        {
            get
            {
                if (this._scoreHole13Repository == null)
                    this._scoreHole13Repository = new GenericRepository<ScoreHole13>(_context);
                return _scoreHole13Repository;
            }
        }

        public GenericRepository<ScoreHole14> ScoreHole14Repository
        {
            get
            {
                if (this._scoreHole14Repository == null)
                    this._scoreHole14Repository = new GenericRepository<ScoreHole14>(_context);
                return _scoreHole14Repository;
            }
        }
        public GenericRepository<ScoreHole15> ScoreHole15Repository
        {
            get
            {
                if (this._scoreHole15Repository == null)
                    this._scoreHole15Repository = new GenericRepository<ScoreHole15>(_context);
                return _scoreHole15Repository;
            }
        }
        public GenericRepository<ScoreHole16> ScoreHole16Repository
        {
            get
            {
                if (this._scoreHole16Repository == null)
                    this._scoreHole16Repository = new GenericRepository<ScoreHole16>(_context);
                return _scoreHole16Repository;
            }
        }
        public GenericRepository<ScoreHole17> ScoreHole17Repository
        {
            get
            {
                if (this._scoreHole17Repository == null)
                    this._scoreHole17Repository = new GenericRepository<ScoreHole17>(_context);
                return _scoreHole17Repository;
            }
        }

        public GenericRepository<ScoreHole18> ScoreHole18Repository
        {
            get
            {
                if (this._scoreHole18Repository == null)
                    this._scoreHole18Repository = new GenericRepository<ScoreHole18>(_context);
                return _scoreHole18Repository;
            }
        }

        public GenericRepository<GolferPasswordChange> GolferPasswordChangeRepository
        {
            get
            {
                if (this._golferPasswordChangeRepository == null)
                    this._golferPasswordChangeRepository = new GenericRepository<GolferPasswordChange>(_context);
                return _golferPasswordChangeRepository;
            }
        }
        
        #endregion


        #region Public member methods...
        /// <summary>
        /// Save method.
        /// </summary>
        public void Save()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {

                var outputLines = new List<string>();
                foreach (var eve in e.EntityValidationErrors)
                {
                    outputLines.Add(string.Format("{0}: Entity of type \"{1}\" in state \"{2}\" has the following validation errors:", DateTime.Now, eve.Entry.Entity.GetType().Name, eve.Entry.State));
                    foreach (var ve in eve.ValidationErrors)
                    {
                        outputLines.Add(string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                    }
                }
                //   System.IO.File.AppendAllLines(@"C:\errors.txt", outputLines);

                throw e;
            }

        }
        public bool EndTransaction()
        {
            try
            {
                _context.SaveChanges();
                Transaction.Commit();
            }
            catch (DbEntityValidationException dbEx)
            {
                Transaction.Rollback();
                Console.WriteLine(dbEx.Message);
                throw new Exception() { };
                // add your exception handling code here
            }
            return true;
        }

        public void RollBack()
        {
            Transaction.Rollback();
            Dispose();
        }
        #endregion

        #region Implementing IDiosposable...

        #region private dispose variable declaration...
        private bool disposed = false;
        #endregion

        /// <summary>
        /// Protected Virtual Dispose method
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    Debug.WriteLine("UnitOfWork is being disposed");
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        /// <summary>
        /// Dispose method
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion


        //when we need to create separte repository for every table
        public GenericRepository<T> Repository<T>() where T : class
        {
            if (repositories == null)
            {
                repositories = new Dictionary<string, object>();
            }

            var type = typeof(T).Name;

            if (!repositories.ContainsKey(type))
            {
                var repositoryType = typeof(GenericRepository<>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), _context);
                repositories.Add(type, repositoryInstance);
            }
            return (GenericRepository<T>)repositories[type];
        }



    }
}
