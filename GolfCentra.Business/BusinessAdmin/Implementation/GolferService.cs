using GolfCentra.Business.BusinessAdmin.Interface;
using GolfCentra.Core;
using GolfCentra.Core.DataBase;
using GolfCentra.ViewModel.Admin;
using GolfCentra.ViewModel.Admin.LoginActivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.Business.BusinessAdmin.Implementation
{
    public class GolferService : IGolferService
    {
        private readonly UnitOfWork _unitOfWork;

        public GolferService()
        {
            _unitOfWork = new UnitOfWork();
        }

        /// <summary>
        /// Get All Golfer Detail
        /// </summary>
        /// <returns></returns>
        public List<GolferViewModel> GetAllGolferProfile()
        {
            List<GolferViewModel> golferViewModels = new List<GolferViewModel>();
            List<Golfer> golfers = _unitOfWork.GolferRepository.GetMany(x => x.IsActive == true).ToList();
            foreach (var item in golfers)
            {
                GolferViewModel golferViewModel = new GolferViewModel()
                {
                    Name = item.Name,
                    LastName = item.LastName,
                    Mobile = item.Mobile,
                    Age = item.Age,
                    ClubMemberId = item.ClubMemberId,
                    PhoneCode = item.PhoneCode,
                    MemberSince = item.MemberSince,
                    EmailAddress = item.Email,

                    PlatformName = item.PlatformType.Value,
                    GolferId = item.GolferId,
                    SpecialComments = item.SpecialComments,
                    SpouseName = item.SpouseName,
                    MemberTypeId = item.MemberTypeId,
                    Occpoution = item.Occpoution,
                    MaritalStatusId = item.MaritalStatusId,
                    SelectBoxDisplay = item.Name + " " + (item.ClubMemberId != null ? "(" + item.ClubMemberId + ")" : ""),
                    IsBlocked=item.IsBlocked.GetValueOrDefault()
                };
                golferViewModels.Add(golferViewModel);
            }
            return golferViewModels;
        }

        /// <summary>
        /// Get Golfer  Detail by Id
        /// </summary>
        /// <param name="golferId"></param>
        /// <returns></returns>
        public GolferViewModel GetGolferProfileByGolferId(long golferId)
        {
            Golfer item = _unitOfWork.GolferRepository.Get(x => x.IsActive == true && x.GolferId == golferId);

            GolferViewModel golferViewModel = new GolferViewModel()
            {
                Name = item.Name,
                LastName = item.LastName,
                Age = item.Age,
                ClubMemberId = item.ClubMemberId,
                Mobile = item.Mobile,
                PhoneCode = item.PhoneCode,
                MemberSince = item.MemberSince,
                Occpoution = item.Occpoution,
                MaritalStatusId = item.MaritalStatusId,
                MemberTypeId = item.MemberTypeId,
                EmailAddress = item.Email,
                SpouseName = item.SpouseName,
                SpecialComments = item.SpecialComments,
                MemberTypeName = item.MemberTypeId != null ? item.MemberType.Name : "",
                GenderTypeValue = item.GenderTypeId != null ? item.GenderType.Value : "",
                GolferId = item.GolferId,
                MaritalStatusValue = item.MaritalStatusId != null ? item.MaritalStatu.Value : "",
                GenderTypeId = item.GenderTypeId,
                PlatformName = item.PlatformTypeId != null ? item.PlatformType.Value : "",
                DOB = item.DOB.GetValueOrDefault(),
                SalutationId = item.SalutationId.GetValueOrDefault()
            };

            return golferViewModel;
        }

        /// <summary>
        /// Update Golfer  Detail
        /// </summary>
        /// <param name="golferViewModel"></param>
        /// <returns></returns>
        public bool UpdateGolferProfile(GolferViewModel golferViewModel,long uniqueSessionId)
        {
            Golfer golfer = _unitOfWork.GolferRepository.Get(x => x.IsActive == true && x.GolferId == golferViewModel.GolferId);
            Golfer emailCheck = _unitOfWork.GolferRepository.Get(x => x.Email.Trim().ToLower() == golferViewModel.EmailAddress.Trim().ToLower());

            if (emailCheck != null && emailCheck.Email.Trim().ToLower() != golfer.Email.Trim().ToLower())
                throw new Exception("Email Is Already Registered.");
            if (golferViewModel.UpdateId != 1)
            {
                Golfer mIdCheck = _unitOfWork.GolferRepository.Get(x => x.ClubMemberId == golferViewModel.ClubMemberId && x.GolferId != golferViewModel.GolferId && x.IsActive == true);
                if (mIdCheck != null && mIdCheck.ClubMemberId == golferViewModel.ClubMemberId)
                    throw new Exception("Membership Id Is Already Registered.");
            }

            if (golferViewModel.UpdateId == 1)
            {
                golfer.Name = golferViewModel.Name;
                golfer.Mobile = golferViewModel.Mobile;
                golfer.Email = golferViewModel.EmailAddress;
            }
            else
            {

                golfer.Name = golferViewModel.Name;
                golfer.LastName = golferViewModel.LastName;
                golfer.Age = golferViewModel.Age;
                golfer.ClubMemberId = golferViewModel.ClubMemberId;
                golfer.PhoneCode = golferViewModel.PhoneCode;
                golfer.MemberSince = golferViewModel.MemberSince;
                golfer.Occpoution = golferViewModel.Occpoution;
                golfer.MaritalStatusId = golferViewModel.MaritalStatusId;
                golfer.GenderTypeId = golferViewModel.GenderTypeId;
                golfer.MemberTypeId = golferViewModel.MemberTypeId;
                golfer.SpouseName = golferViewModel.SpouseName;
                golfer.Email = golferViewModel.EmailAddress;
                golfer.SpecialComments = golferViewModel.SpecialComments;
                golfer.Mobile = golferViewModel.Mobile;
                golfer.Email = golferViewModel.EmailAddress;
                golfer.DOB = golferViewModel.DOB;
                if (golfer.SalutationId != 0 && golfer.SalutationId != null)
                {

                    golfer.SalutationId = golferViewModel.SalutationId;
                }
                else
                {
                    golfer.SalutationId = null;
                }
            }

            golfer.UpdatedOn = System.DateTime.UtcNow;
            _unitOfWork.GolferRepository.Update(golfer);

            _unitOfWork.Save();
            LogoutOperation(golferViewModel.GolferId);

            try
            {

                SessionActivityPageViewModel sessionActivityPageViewModel = new SessionActivityPageViewModel()
                {
                    ControllerName = "Golfer Update",
                    ActionName = "Update",
                    PerformOn = golferViewModel.GolferId.ToString(),
                    LoginHistoryId = uniqueSessionId,

                    Info = "Updated a Golfer with id- " + golferViewModel.GolferId.ToString()
                };
                new Common.AddSessionActivity().SaveSessionActivity(sessionActivityPageViewModel);

            }
            catch (Exception)
            {

            }
            return true;
        }

        /// <summary>
        /// Save All Golfer  Detail
        /// </summary>
        /// <param name="golferViewModel"></param>
        /// <returns></returns>
        public bool SaveGolferDetails(GolferViewModel golferViewModel, long uniqueSessionId)
        {


            Golfer emailCheck = _unitOfWork.GolferRepository.Get(x => x.Email.Trim().ToLower() == golferViewModel.EmailAddress.Trim().ToLower() && x.IsActive == true);
            Golfer mIdCheck = _unitOfWork.GolferRepository.Get(x => x.ClubMemberId == golferViewModel.ClubMemberId && x.IsActive == true);
            if (mIdCheck != null)
                throw new Exception("Membership Id Is Already Registered.");
            if (emailCheck != null)
                throw new Exception("Email Is Already Registered.");
            Golfer golfer = new Golfer()
            {
                Name = golferViewModel.Name,

                IsActive = true,
                CreatedOn = System.DateTime.UtcNow,
                LastName = golferViewModel.LastName,
                Age = golferViewModel.Age,
                ClubMemberId = golferViewModel.ClubMemberId,
                Mobile = golferViewModel.Mobile,
                PhoneCode = golferViewModel.PhoneCode,
                Email = golferViewModel.EmailAddress,
                SpecialComments = golferViewModel.SpecialComments,
                MaritalStatusId = golferViewModel.MaritalStatusId,
                SpouseName = golferViewModel.SpouseName,
                Occpoution = golferViewModel.Occpoution,
                MemberSince = golferViewModel.MemberSince,
                MemberTypeId = golferViewModel.MemberTypeId,
                GenderTypeId = golferViewModel.GenderTypeId,
                PlatformTypeId = (int)Core.Helper.Enum.EnumPlatformType.Web,
                Password =  Core.Helper.PasswordConvertor.Password(golferViewModel.Password).ToString(),
                DOB = golferViewModel.DOB,
               
            };
            if (golfer.SalutationId != 0 && golfer.SalutationId != null)
            {

                golfer.SalutationId = golferViewModel.SalutationId;
            }
            else
            {
                golfer.SalutationId = null;
            }
            //List<EquipmentTaxMappingViewModel> equipmentTaxMappingViewModels = new List<EquipmentTaxMappingViewModel>();
            //  List<EquipmentTaxMapping> equipmentTaxMappings = _unitOfWork.EquipmentTaxMappingRepository.GetMany(x => x.IsActive == true & x.EquipmentId == item.EquipmentId).ToList();
            _unitOfWork.GolferRepository.Insert(golfer);
            _unitOfWork.Save();

            try
            {

                SessionActivityPageViewModel sessionActivityPageViewModel = new SessionActivityPageViewModel()
                {
                    ControllerName = "Golfer Save",
                    ActionName = "Save",
                    PerformOn = golferViewModel.GolferId.ToString(),
                    LoginHistoryId = uniqueSessionId,

                    Info = "Created a Golfer with id- " + golferViewModel.GolferId.ToString()
                };
                new Common.AddSessionActivity().SaveSessionActivity(sessionActivityPageViewModel);

            }
            catch (Exception)
            {

            }
            return true;

        }

        /// <summary>
        /// Get All Golfer  By advance Search
        /// </summary>
        /// <param name="golferViewModel"></param>
        /// <returns></returns>
        public List<GolferViewModel> GetGolferByAdvanceSearch(GolferViewModel golferViewModel)
        {

            List<GolferViewModel> golferViewModels = new List<GolferViewModel>();

            List<Golfer> golfers = null;

            if (golferViewModel.Name != null && golferViewModel.Name != "")
            {
                golfers = _unitOfWork.GolferRepository.GetMany(x => x.Name.Trim().ToLower().Contains(golferViewModel.Name.Trim().ToLower()) && x.IsActive == true).ToList();
            }


            if (golferViewModel.LastName != "" && golferViewModel.LastName != null)
            {
                if (golfers != null)
                {
                    golfers = golfers.Where(x => x.LastName == golferViewModel.LastName && x.IsActive == true).ToList();
                }
                else
                {
                    golfers = _unitOfWork.GolferRepository.GetMany(x => x.LastName == golferViewModel.LastName && x.IsActive == true).ToList();
                }
            }

            if (golferViewModel.Mobile != "" && golferViewModel.Mobile != null)
            {
                if (golfers != null)
                {
                    golfers = golfers.Where(x => x.Mobile.Trim().ToLower() == golferViewModel.Mobile.Trim().ToLower() && x.IsActive == true).ToList();
                }
                else
                {
                    golfers = _unitOfWork.GolferRepository.GetMany(x => x.Mobile.Trim().ToLower() == golferViewModel.Mobile.Trim().ToLower() && x.IsActive == true).ToList();

                }
            }
            if (golferViewModel.EmailAddress != "" && golferViewModel.EmailAddress != null)
            {
                if (golfers != null)
                {
                    golfers = golfers.Where(x => x.Email.Trim().ToLower() == golferViewModel.EmailAddress.Trim().ToLower() && x.IsActive == true).ToList();
                }
                else
                {
                    golfers = _unitOfWork.GolferRepository.GetMany(x => x.Email.Trim().ToLower() == golferViewModel.EmailAddress.Trim().ToLower() && x.IsActive == true).ToList();

                }
            }
            if (golferViewModel.MemberTypeId != 0 && golferViewModel.MemberTypeId != null)
            {
                if (golfers != null)
                {
                    golfers = golfers.Where(x => x.MemberTypeId == golferViewModel.MemberTypeId && x.IsActive == true).ToList();
                }
                else
                {
                    golfers = _unitOfWork.GolferRepository.GetMany(x => x.MemberTypeId == golferViewModel.MemberTypeId && x.IsActive == true).ToList();

                }
            }
            if (golferViewModel.ClubMemberId != "" && golferViewModel.ClubMemberId != null)
            {
                if (golfers != null)
                {
                    golfers = golfers.Where(x => x.ClubMemberId == golferViewModel.ClubMemberId && x.IsActive == true).ToList();
                }
                else
                {
                    golfers = _unitOfWork.GolferRepository.GetMany(x => x.ClubMemberId == golferViewModel.ClubMemberId && x.IsActive == true).ToList();

                }
            }




            if (golfers != null)
            {
                foreach (var item in golfers)
                {
                    GolferViewModel golferViewModeldb = new GolferViewModel()
                    {
                        Name = item.Name,
                        LastName = item.LastName,
                        Age = item.Age,
                        ClubMemberId = item.ClubMemberId,
                        Mobile = item.Mobile,
                        PhoneCode = item.PhoneCode,
                        MemberSince = item.MemberSince,
                        Occpoution = item.Occpoution,
                        MaritalStatusId = item.MaritalStatusId,
                        MemberTypeId = item.MemberTypeId,
                        EmailAddress = item.Email,
                        SpouseName = item.SpouseName,
                        SpecialComments = item.SpecialComments,
                        MemberTypeName = item.MemberTypeId != null ? item.MemberType.Name : "",
                        GenderTypeValue = item.GenderTypeId != null ? item.GenderType.Value : "",
                        GolferId = item.GolferId,
                        MaritalStatusValue = item.MaritalStatusId != null ? item.MaritalStatu.Value : "",
                        GenderTypeId = item.GenderTypeId,
                        PlatformName = item.PlatformTypeId != null ? item.PlatformType.Value : "",
                        IsBlocked = item.IsBlocked.GetValueOrDefault()


                    };
                    golferViewModels.Add(golferViewModeldb);
                }
            }
            return golferViewModels;

        }


        public void LogoutOperation(long golferId)
        {
            try
            {
                List<LogInHistory> logInHistory = _unitOfWork.LogInHistoryRepository.GetMany(x => x.UserId == golferId && x.LoggedOutAt == null).ToList();

                foreach (var item in logInHistory)
                {
                    item.LoggedOutAt = System.DateTime.UtcNow;
                    item.UpdatedOn = System.DateTime.UtcNow;
                    _unitOfWork.LogInHistoryRepository.Update(item);
                    _unitOfWork.Save();
                }
            }
            catch (Exception ex)
            {

                //    throw;
            }
        }


        public void BlockUnBlockOperation(long golferId, bool isBlocked, long uniqueSessionId)
        {

            Golfer golfer = _unitOfWork.GolferRepository.Get(x => x.IsActive == true && x.GolferId == golferId);
            if (golfer == null)
                throw new Exception("Golfer Not Found.");
            golfer.IsBlocked = isBlocked;
            golfer.UpdatedOn = System.DateTime.UtcNow;
            _unitOfWork.GolferRepository.Update(golfer);
            _unitOfWork.Save();

            try
            {

                SessionActivityPageViewModel sessionActivityPageViewModel = new SessionActivityPageViewModel()
                {
                    ControllerName = "Golfer Status Update",
                    ActionName = "Update",
                    PerformOn = golfer.GolferId.ToString(),
                    LoginHistoryId = uniqueSessionId,

                    Info = "Updated Golfer Status with id- " + golfer.GolferId.ToString()
                };
                new Common.AddSessionActivity().SaveSessionActivity(sessionActivityPageViewModel);

            }
            catch (Exception)
            {

            }
        }


        public List<GolferViewModel> SearchGolferByMemberShipId(string memberShipId)
        {
            List<GolferViewModel> golferViewModels = new List<GolferViewModel>();
            var golfers = _unitOfWork.GolferRepository.GetMany(x => x.ClubMemberId.Contains(memberShipId) && x.IsActive == true);
            if (golfers != null)
            {
                foreach (var item in golfers)
                {
                    GolferViewModel golferViewModeldb = new GolferViewModel()
                    {
                        Name = item.Name,
                        LastName = item.LastName,
                        Age = item.Age,
                        ClubMemberId = item.ClubMemberId,
                        Mobile = item.Mobile,
                        PhoneCode = item.PhoneCode,
                        MemberSince = item.MemberSince,
                        Occpoution = item.Occpoution,
                        MaritalStatusId = item.MaritalStatusId,
                        MemberTypeId = item.MemberTypeId,
                        EmailAddress = item.Email,
                        SpouseName = item.SpouseName,
                        SpecialComments = item.SpecialComments,
                        MemberTypeName = item.MemberTypeId != null ? item.MemberType.Name : "",
                        GenderTypeValue = item.GenderTypeId != null ? item.GenderType.Value : "",
                        GolferId = item.GolferId,
                        MaritalStatusValue = item.MaritalStatusId != null ? item.MaritalStatu.Value : "",
                        GenderTypeId = item.GenderTypeId,
                        PlatformName = item.PlatformTypeId != null ? item.PlatformType.Value : "",


                    };
                    golferViewModels.Add(golferViewModeldb);
                }
            }
            return golferViewModels;
        }

        public bool ChangePassword(GolferViewModel golferViewModel, long uniqueSessionId)
        {
            Golfer golfer = _unitOfWork.GolferRepository.Get(x => x.GolferId == golferViewModel.GolferId);
            if (golfer == null)
                throw new Exception("No Golfer Found");
            if (golferViewModel.Password == "")
                throw new Exception(" Password Can Not Be Blank");

            if (Core.Helper.PasswordConvertor.ValidatePassword(golferViewModel.Password, golfer.Password))
                throw new Exception("You Can't Use Old Password As New Password");

            golfer.Password =  Core.Helper.PasswordConvertor.Password(golferViewModel.Password).ToString() ;
            golfer.UpdatedOn = System.DateTime.UtcNow;
            _unitOfWork.GolferRepository.Update(golfer);
            _unitOfWork.Save();

            try
            {

                SessionActivityPageViewModel sessionActivityPageViewModel = new SessionActivityPageViewModel()
                {
                    ControllerName = "Golfer Password Update",
                    ActionName = "Update",
                    PerformOn = golferViewModel.GolferId.ToString(),
                    LoginHistoryId = uniqueSessionId,

                    Info = "Updated Password of Golfer with id- " + golferViewModel.GolferId.ToString()
                };
                new Common.AddSessionActivity().SaveSessionActivity(sessionActivityPageViewModel);

            }
            catch (Exception)
            {

            }
            return true;
        }


        public void Logout(string uniqueSessionId)
        {
            try
            {
                long id = Convert.ToInt64(uniqueSessionId);
                List<LogInHistory> logInHistory = _unitOfWork.LogInHistoryRepository.GetMany(x => x.LoginHistoryId == id && x.LoggedOutAt == null).ToList();

                foreach (var item in logInHistory)
                {
                    item.LoggedOutAt = System.DateTime.UtcNow;
                    item.UpdatedOn = System.DateTime.UtcNow;
                    _unitOfWork.LogInHistoryRepository.Update(item);
                    _unitOfWork.Save();
                }
            }
            catch (Exception ex)
            {

                //    throw;
            }
        }


        public List<SalutationViewModel> GetAllSalutation()
        {
            List<SalutationViewModel> salutationViewModels = new List<SalutationViewModel>();
            List<Salutation> salutations = _unitOfWork.SalutationRepository.GetMany(x => x.IsActive == true).OrderBy(x => x.Text).ToList();

            foreach (Salutation salutation in salutations)
            {
                SalutationViewModel salutationViewModel = new SalutationViewModel()
                {
                    SalutationId = salutation.SalutationId,
                    Text = salutation.Text
                };
                salutationViewModels.Add(salutationViewModel);
            }
            return salutationViewModels;
        }



    }
}
