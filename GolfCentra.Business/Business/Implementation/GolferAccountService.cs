using GolfCentra.Business.Business.Interface;
using GolfCentra.Core;
using GolfCentra.Core.DataBase;
using GolfCentra.Core.Helper;
using GolfCentra.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.Business.Business.Implementation
{
    public class GolferAccountService : IGolferAccountService
    {
        private readonly UnitOfWork _unitOfWork;

        public GolferAccountService()
        {
            _unitOfWork = new UnitOfWork();
        }

        /// <summary>
        /// Vaildate Email And Password Of Golfer
        /// </summary>
        /// <param name="email">email of golfer</param>
        /// <param name="password">password of golfer</param>
        /// <returns>true if succeed/ false if failed</returns>
        private Golfer LogInValidate(string clubMemberId, string password)
        {

            var result = _unitOfWork.GolferRepository.Get(x => x.ClubMemberId == clubMemberId && x.IsActive == true);
            if (result != null)
            {
                if (!Core.Helper.PasswordConvertor.ValidatePassword(password, result.Password))
                {
                    return null;
                }
            }
            return result;
        }

        /// <summary>
        /// Get All Phone Code List
        /// </summary>
        /// <returns></returns>
        public List<PhoneCodeViewModel> GetPhoneCodeList()
        {
            List<PhoneCodeViewModel> phoneCodeViewModels = new List<PhoneCodeViewModel>();
            List<Country> countries = _unitOfWork.CountryRepository.GetMany(x => x.IsActive == true).OrderBy(x => x.Name).ToList();

            foreach (Country country in countries)
            {
                PhoneCodeViewModel phoneCodeViewModel = new PhoneCodeViewModel()
                {
                    CountryName = country.Name,
                    PhoneCode = country.PhoneCode
                };
                phoneCodeViewModels.Add(phoneCodeViewModel);
            }
            return phoneCodeViewModels;
        }

        /// <summary>
        /// Save Golfer Details Only
        /// </summary>
        /// <param name="golferViewModel"></param>
        /// <returns></returns>
        public Golfer AddGolfer(CommonViewModel golferViewModel)
        {

            Golfer golferCheck = _unitOfWork.GolferRepository.Get(x => x.Email.ToLower() == golferViewModel.EmailAddress.ToLower() && x.IsActive == true);

            if (golferCheck != null)
                throw new Exception("Already Registered");

            string code = "123456";
            code = RamdomNumber.RandomNumber();
            Golfer golfer = new Golfer()
            {
                Name = golferViewModel.Name,
                Email = golferViewModel.EmailAddress,
                ClubMemberId = golferViewModel.MemberShipId != null ? golferViewModel.MemberShipId : string.Empty,
                Mobile = golferViewModel.MobileNumber,
                Password = Core.Helper.PasswordConvertor.Password(golferViewModel.Password).ToString(),
                IsActive = false,
                CreatedOn = System.DateTime.UtcNow,
                PlatformTypeId = golferViewModel.PlatformTypeId,
                VerficationCode = code,
                PhoneCode = golferViewModel.PhoneCode,
                MemberTypeId = (int)Core.Helper.Enum.EnumMemberType.NonMember

            };
            _unitOfWork.GolferRepository.Insert(golfer);
            _unitOfWork.Save();

            MailerViewModel mailerViewModel = new MailerViewModel()
            {
                ToEmails = golferViewModel.EmailAddress,
                From = Constants.MailId.FromMails,
                Subject = "One Time Password for " + Constants.Common.AppName + " App",
                Body = "Your OTP for " + Constants.Common.AppName + " Registration is " + code + ".Please use this code for Registration"
            };
            EmailNotification emailNotification = new EmailNotification();
            bool status = emailNotification.SendMail(mailerViewModel);
            if (status == false) { throw new Exception("Email Not Sent."); }
            return golfer;
        }

        /// <summary>
        /// Resend Verfication Code By Email
        /// </summary>
        /// <param name="golferId"></param>
        /// <returns></returns>
        public bool ResendVerficationCode(long golferId)
        {
            Golfer golfer = _unitOfWork.GolferRepository.Get(x => x.GolferId == golferId);
            if (golfer == null)
                throw new Exception("Invalid Golfer Detail.");
            if (golfer.IsActive == true)
                throw new Exception("Golfer Already Registered.");

            string code = "123456";
            code = RamdomNumber.RandomNumber();
            MailerViewModel mailerViewModel = new MailerViewModel()
            {
                ToEmails = golfer.Email,
                From = Constants.MailId.FromMails,
                Subject = "One Time Password for " + Constants.Common.AppName + " App",
                Body = "Your OTP for " + Constants.Common.AppName + " Registration is " + code + ".Please use this code for Registration"
            };
            EmailNotification emailNotification = new EmailNotification();
            bool status = emailNotification.SendMail(mailerViewModel);
            if (status == false) { throw new Exception("Email Not Sent."); }
            golfer.VerficationCode = code;
            _unitOfWork.GolferRepository.Update(golfer);
            _unitOfWork.Save();
            return true;
        }

        /// <summary>
        /// Verify Golfer Account 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="golferId"></param>
        /// <param name="platformId"></param>
        /// <returns></returns>
        public GolferViewModel VerifyGolferAccount(string code, long golferId, int platformId)
        {
            Golfer golfer = _unitOfWork.GolferRepository.Get(x => x.GolferId == golferId);
            if (golfer == null)
                throw new Exception("Invalid Golfer Detail.");
            if (golfer.IsActive == true)
                throw new Exception("Golfer Already Registered.");
            if (golfer.VerficationCode != code)
                throw new Exception("Invalid Verification Code.");

            golfer.IsActive = true;
            _unitOfWork.GolferRepository.Update(golfer);
            _unitOfWork.Save();

            GolferViewModel golferViewModel1 = new GolferViewModel()
            {
                Name = golfer.Name,
                PlatformTypeId = platformId,
                Password = golfer.Password,
                EmailAddress = golfer.Email,
                GolferId = golfer.GolferId

            };
            LogInHistory logInHistory = SaveLogInHistory(golferViewModel1);

            GolferViewModel golferViewModel = new GolferViewModel()
            {
                Name = golfer.Name,
                EmailAddress = golfer.Email,
                MobileNumber = golfer.Mobile,
                MemberShipId = golfer.ClubMemberId != null ? golfer.ClubMemberId : string.Empty,
                GolferId = golfer.GolferId,
                UniqueSessionId = Core.Helper.Crypto.EncryptStringAES(logInHistory.LoginHistoryId.ToString()),

            };
            if (golfer.MemberTypeId == (int)Core.Helper.Enum.EnumMemberType.NonMember)
            {
                golferViewModel.IsMember = false;
            }
            else
            {
                golferViewModel.IsMember = true;
            }
            return golferViewModel;
        }

        /// <summary>
        ///  Send Current Password To Golfer By Email
        /// </summary>
        /// <param name="clubMemberId"></param>
        /// <returns></returns>
        public bool ForgetPassword(string clubMemberId)
        {
            Golfer golfer = _unitOfWork.GolferRepository.Get(x => x.ClubMemberId == clubMemberId && x.IsActive == true);
            if (golfer == null)
                throw new Exception("Reset password link has been shared on your email id.");
            if (golfer != null)
            {
                golfer.LoginAttempt = 0;
                // golfer.IsBlocked = false;
                _unitOfWork.GolferRepository.Update(golfer);
                _unitOfWork.Save();
            }

            GolferPasswordChange golferPasswordChange = new GolferPasswordChange()
            {
                GolferId = golfer.GolferId,
                Createdon = System.DateTime.UtcNow,
                IsActive = true,
                IsUsed = false
            };
            _unitOfWork.GolferPasswordChangeRepository.Insert(golferPasswordChange);
            _unitOfWork.Save();

            MailerViewModel mailerViewModel = new MailerViewModel()
            {
                ToEmails = golfer.Email,
                From = Constants.MailId.FromMails,
                Subject = "Forget Password for " + Constants.Common.AppName + " App",
                Body = "Please reset your password at following link  " + Constants.Url.UserPanelUrlWithoutSlash + "/ResetPassword/Index?r=" + Core.Helper.Crypto.EncryptStringAES(golferPasswordChange.GolferPasswordChangeId.ToString()),
            };
            EmailNotification emailNotification = new EmailNotification();
            bool status = emailNotification.SendMail(mailerViewModel);
            if (status == false) { throw new Exception("Reset password link has been shared on your email id."); }
            return true;
        }

        /// <summary>
        /// Save Login History From Golfer
        /// </summary>
        /// <param name="golferViewModel"></param>
        /// <returns></returns>
        private LogInHistory SaveLogInHistory(GolferViewModel golferViewModel)
        {
            LogInHistory logInHistory = new LogInHistory()
            {
                LoginStatusId = 1,
                CreatedOn = System.DateTime.UtcNow,
                IsActive = true,
                UserId = golferViewModel.GolferId,
                IPAddress = golferViewModel.IpAddress,
                PlatformTypeId = golferViewModel.PlatformTypeId,
                MACAddress = golferViewModel.MacAddress,
                UserName = golferViewModel.EmailAddress,
                DeviceTokenId = golferViewModel.DeviceTokenId
            };
            _unitOfWork.LogInHistoryRepository.Insert(logInHistory);
            _unitOfWork.Save();
            return logInHistory;
        }

        ///// <summary>
        ///// Login Opertaion For Golfer
        ///// </summary>
        ///// <param name="golfer"></param>
        ///// <returns></returns>
        //public GolferViewModel LogInUser(GolferViewModel golfer)
        //{
        //    Golfer golferDB = LogInValidate(golfer.EmailAddress, golfer.Password);
        //    if (golferDB == null)
        //        throw new Exception("No User Found For User Name And Password.");
        //    golfer.Name = golferDB.Name;
        //    golfer.GolferId = golferDB.GolferId;
        //    //Entry To Login History
        //    LogInHistory logInHistory = SaveLogInHistory(golfer);

        //    GolferViewModel golferViewModel = new GolferViewModel()
        //    {
        //        Name = golferDB.Name,
        //        EmailAddress = golferDB.Email,
        //        MobileNumber = golferDB.Mobile,
        //        MemberShipId = golferDB.ClubMemberId != null ? golferDB.ClubMemberId : string.Empty,
        //        GolferId = golferDB.GolferId,
        //        UniqueSessionId = logInHistory.LoginHistoryId.ToString(),
        //        //IpAddress = string.Empty,
        //        //OTP = string.Empty,
        //        //PlatformTypeId = 0,
        //        //Password = string.Empty,
        //        //MacAddress = string.Empty,
        //    };

        //    return golferViewModel;
        //}p

        /// <summary>
        /// Get Login hisory By Unique Session Id
        /// </summary>
        /// <param name="loginHistoryId"></param>
        /// <returns></returns>
        public LogInHistory GetLoginHistoryById(string loginHistoryId)
        {
            long id = Convert.ToInt64(loginHistoryId);
            LogInHistory logInHistory = _unitOfWork.LogInHistoryRepository.Get(x => x.LoginHistoryId == id);
            return logInHistory;
        }

        /// <summary>
        /// Get Golfer Id From Login History Using Unique Session Id
        /// </summary>
        /// <param name="loginHistoryId"></param>
        /// <returns></returns>
        public long GetGolferIdFromLoginHistoryById(string loginHistoryId)
        {
            long id = Convert.ToInt64(Core.Helper.Crypto.DecryptStringAES(loginHistoryId));
            LogInHistory logInHistory = _unitOfWork.LogInHistoryRepository.Get(x => x.LoginHistoryId == id);
            return logInHistory.UserId;
        }

        /// <summary>
        /// Get Golfer Detail By Golfer Id
        /// </summary>
        /// <param name="golferId"></param>
        /// <returns></returns>
        public GolferViewModel GetGolferDetailByGolferId(long golferId)
        {
            Golfer golferDB = _unitOfWork.GolferRepository.Get(x => x.GolferId == golferId && x.IsActive == true);

            GolferViewModel golferViewModel = new GolferViewModel()
            {
                Name = golferDB.Name != null ? golferDB.Name : "",
                EmailAddress = golferDB.Email,
                MobileNumber = golferDB.Mobile,
                MemberShipId = golferDB.ClubMemberId,
                GolferId = golferDB.GolferId,

                SalutationId = golferDB.SalutationId != null ? golferDB.SalutationId.GetValueOrDefault() : 0,
                SalutationText = golferDB.SalutationId != null ? golferDB.Salutation.Text : "",
                DOB = golferDB.DOB != null ? golferDB.DOB.GetValueOrDefault().ToShortDateString() : "",
                GenderId = golferDB.GenderTypeId != null ? golferDB.GenderTypeId.GetValueOrDefault() : 0,
                Occuption = golferDB.Occpoution != null ? golferDB.Occpoution : "",
                Address = golferDB.Address != null ? golferDB.Address : "",
                LastName = golferDB.LastName != null ? golferDB.LastName : "",
                MemberTypeId = golferDB.MemberTypeId.GetValueOrDefault()
            };
            return golferViewModel;
        }

        /// <summary>
        /// Change Password Of Golfer
        /// </summary>
        /// <param name="golferId"></param>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        public bool ChangePassword(long golferId, string oldPassword, string newPassword, string uid)
        {
            long id = Convert.ToInt64(Core.Helper.Crypto.DecryptStringAES(uid));
            LogInHistory logInHistory = _unitOfWork.LogInHistoryRepository.Get(x => x.LoginHistoryId == id && x.LoggedOutAt == null);
            if (logInHistory.ChangePasswordAttempt.GetValueOrDefault() > 5 && logInHistory.UpdatedOn > System.DateTime.UtcNow.AddMinutes(-30))
            {
                throw new Exception("Tried Too Many Attempt. Please Try After Sometime.");
            }
            else if (logInHistory.ChangePasswordAttempt.GetValueOrDefault() > 5 && logInHistory.UpdatedOn < System.DateTime.UtcNow.AddMinutes(-30))
            {
                logInHistory.ChangePasswordAttempt = 0;
                logInHistory.UpdatedOn = System.DateTime.UtcNow;
                _unitOfWork.LogInHistoryRepository.Update(logInHistory);
                _unitOfWork.Save();
            }
            if (logInHistory.ChangePasswordAttempt.GetValueOrDefault() < 6)
            {

                Core.Helper.Vaildation.CheckPasswordStrength(newPassword);
                Golfer golfer = _unitOfWork.GolferRepository.Get(x => x.GolferId == golferId);
                if (golfer == null)
                {
                    logInHistory.ChangePasswordAttempt = logInHistory.ChangePasswordAttempt.GetValueOrDefault() + 1;
                    logInHistory.UpdatedOn = System.DateTime.UtcNow;
                    _unitOfWork.LogInHistoryRepository.Update(logInHistory);
                    _unitOfWork.Save();
                    throw new Exception("No Golfer Found");
                }
                if (!Core.Helper.PasswordConvertor.ValidatePassword(oldPassword, golfer.Password))
                {
                    logInHistory.ChangePasswordAttempt = logInHistory.ChangePasswordAttempt.GetValueOrDefault() + 1;
                    logInHistory.UpdatedOn = System.DateTime.UtcNow;
                    _unitOfWork.LogInHistoryRepository.Update(logInHistory);
                    _unitOfWork.Save();
                    throw new Exception("Old Password Is Not Correct");
                }

                if (Core.Helper.PasswordConvertor.ValidatePassword(newPassword, golfer.Password))
                {
                    logInHistory.ChangePasswordAttempt = logInHistory.ChangePasswordAttempt.GetValueOrDefault() + 1;
                    logInHistory.UpdatedOn = System.DateTime.UtcNow;
                    _unitOfWork.LogInHistoryRepository.Update(logInHistory);
                    _unitOfWork.Save();
                    throw new Exception("You Can't Use Old Password As New Password");
                }


                golfer.Password = Core.Helper.PasswordConvertor.Password(newPassword).ToString();
                golfer.UpdatedOn = System.DateTime.UtcNow;
                golfer.IsPrimaryPasswordChanged = true;
                _unitOfWork.GolferRepository.Update(golfer);

                logInHistory.ChangePasswordAttempt = 0;
                logInHistory.UpdatedOn = System.DateTime.UtcNow;
                _unitOfWork.LogInHistoryRepository.Update(logInHistory);
                _unitOfWork.Save();
                return true;
            }
            else
            {
                throw new Exception("Tried Too Many Attempt. Please Try After Sometime.");
            }
        }

        /// <summary>
        /// Logout Golfer From APP
        /// </summary>
        /// <param name="uniqueSessionId"></param>
        /// <returns></returns>
        public bool Logout(string uniqueSessionId)
        {
            try
            {
                if (uniqueSessionId != null)
                {
                    if (!uniqueSessionId.Trim().Equals(""))
                    {
                        long id = Convert.ToInt64(Core.Helper.Crypto.DecryptStringAES(uniqueSessionId));
                        LogInHistory logInHistory = _unitOfWork.LogInHistoryRepository.Get(x => x.LoginHistoryId == id && x.LoggedOutAt == null);
                        if (logInHistory == null)
                        {
                            return true;
                        }
                        logInHistory.LoggedOutAt = System.DateTime.UtcNow;
                        logInHistory.UpdatedOn = System.DateTime.UtcNow;
                        _unitOfWork.LogInHistoryRepository.Update(logInHistory);
                        _unitOfWork.Save();
                    }
                }
                return true;
            }
            catch (Exception)
            {

                return true;
            }

        }

        public GolferViewModel LogInUser(GolferViewModel golfer)
        {
            Golfer golfers = _unitOfWork.GolferRepository.Get(x => x.ClubMemberId == golfer.MemberShipId && x.MemberTypeId == 1 && x.IsActive == true);
            if (golfers == null)
                throw new Exception("Please Check The Membership Id And Password");
            if (golfers.LoginAttempt >= 3)
            {
                throw new Exception("Your Account Is Block");
            }

            Golfer golferDB = LogInValidate(golfer.MemberShipId, golfer.Password);
            if (golferDB == null)
            {
                golfers.LoginAttempt = golfers.LoginAttempt + 1;


                if (golfers.LoginAttempt >= 3)
                {
                    //  golfers.IsBlocked = true;
                    _unitOfWork.GolferRepository.Update(golfers);
                    _unitOfWork.Save();
                    LogoutOperation(golfers.GolferId);
                    throw new Exception("Your Account Is Block");
                }
                else
                {
                    _unitOfWork.GolferRepository.Update(golfers);
                    _unitOfWork.Save();
                    throw new Exception("Please Check The Membership Id And Password");
                    throw new Exception("Please Check The Password, Login Attempt Left : " + (3 - golfers.LoginAttempt));
                }

            }
            else
            {
                golferDB.LoginAttempt = 0;
                _unitOfWork.GolferRepository.Update(golferDB);
                _unitOfWork.Save();

            }


            golfer.Name = golferDB.Name;
            golfer.GolferId = golferDB.GolferId;
            golfer.EmailAddress = golferDB.Email;
            //Entry To Login History
            LogoutOperation(golfers.GolferId);
            LogInHistory logInHistory = SaveLogInHistory(golfer);

            GolferViewModel golferViewModel = new GolferViewModel()
            {
                Name = golferDB.Name != null ? golferDB.Name : "",
                EmailAddress = golferDB.Email,
                MobileNumber = golferDB.Mobile,
                MemberShipId = golferDB.ClubMemberId != null ? golferDB.ClubMemberId : string.Empty,
                GolferId = golferDB.GolferId,
                UniqueSessionId = Core.Helper.Crypto.EncryptStringAES(logInHistory.LoginHistoryId.ToString()),
                SalutationId = golferDB.SalutationId != null ? golferDB.SalutationId.GetValueOrDefault() : 0,
                SalutationText = golferDB.SalutationId != null ? golferDB.Salutation.Text : "",
                DOB = golferDB.DOB != null ? golferDB.DOB.GetValueOrDefault().ToShortDateString() : "",
                GenderId = golferDB.GenderTypeId != null ? golferDB.GenderTypeId.GetValueOrDefault() : 0,
                Occuption = golferDB.Occpoution != null ? golferDB.Occpoution : "",
                Address = golferDB.Address != null ? golferDB.Address : "",
                LastName = golferDB.LastName != null ? golferDB.LastName : "",
                MemberTypeId = golferDB.MemberTypeId.GetValueOrDefault()

            };
            if (golferDB.MemberTypeId == (int)Core.Helper.Enum.EnumMemberType.NonMember)
            {

                golferViewModel.IsMember = false;
            }
            else
            {
                golferViewModel.IsMember = true;
            }
            if (golferDB.IsPrimaryPasswordChanged == null || golferDB.IsPrimaryPasswordChanged == false)
            {

                golferViewModel.IsFirstLogIn = true;
            }
            else
            {
                golferViewModel.IsFirstLogIn = false;
            }


            return golferViewModel;
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


        public GolferViewModel UpdateGolferProfile(GolferViewModel golferViewModel)
        {
            Golfer golfer = _unitOfWork.GolferRepository.Get(x => x.IsActive == true && x.GolferId == golferViewModel.GolferId);

            golfer.Name = golferViewModel.Name;
            golfer.LastName = golferViewModel.LastName;
            golfer.Occpoution = golferViewModel.Occuption;
            if (golferViewModel.GenderId != 0)
                golfer.GenderTypeId = golferViewModel.GenderId;
            if (golferViewModel.SalutationId != 0)
                golfer.SalutationId = golferViewModel.SalutationId;
            if (golferViewModel.DOB != null && golferViewModel.DOB != "")
                golfer.DOB = Convert.ToDateTime(golferViewModel.DOB);
            golfer.Address = golferViewModel.Address;
            golfer.LastName = golferViewModel.LastName;

            golfer.UpdatedOn = System.DateTime.UtcNow;
            _unitOfWork.GolferRepository.Update(golfer);

            _unitOfWork.Save();
            //LogoutOperation(golferViewModel.GolferId);

            return GetGolferDetailByGolferId(golferViewModel.GolferId);
        }

        public List<GenderTypeViewModel> GetAllGenderType()
        {
            List<GenderTypeViewModel> genderTypeViewModels = new List<GenderTypeViewModel>();
            List<GenderType> genderTypes = _unitOfWork.GenderTypeRepository.GetMany(x => x.IsActive == true).OrderBy(x => x.Value).ToList();

            foreach (GenderType genderType in genderTypes)
            {
                GenderTypeViewModel genderTypeViewModel = new GenderTypeViewModel()
                {
                    GenderId = genderType.GenderTypeId,
                    Text = genderType.Value
                };
                genderTypeViewModels.Add(genderTypeViewModel);
            }
            return genderTypeViewModels;
        }

        public bool ChangePassword(string ss, string newPassword)
        {
            long id = Convert.ToInt64(Core.Helper.Crypto.DecryptStringAES(ss));
            GolferPasswordChange golferPasswordChange = _unitOfWork.GolferPasswordChangeRepository.Get(x => x.GolferPasswordChangeId == id);
            if (golferPasswordChange == null)
                throw new Exception("Link Expired");
            if (golferPasswordChange.IsUsed == true || golferPasswordChange.Createdon.AddMinutes(15) < System.DateTime.UtcNow)
                throw new Exception("Link Expired");
            GolferPasswordChange golferPasswordChange1 = _unitOfWork.GolferPasswordChangeRepository.Get(x => x.GolferPasswordChangeId > id && x.GolferId == golferPasswordChange.GolferId);
            if (golferPasswordChange1 != null)
                throw new Exception("Link Expired");

            Golfer golfer = _unitOfWork.GolferRepository.Get(x => x.GolferId == golferPasswordChange.GolferId);
            if (golfer == null)
                throw new Exception("No Golfer Found");

            golfer.Password = Core.Helper.PasswordConvertor.Password(newPassword).ToString();
            golfer.UpdatedOn = System.DateTime.UtcNow;
            golfer.IsPrimaryPasswordChanged = true;
            _unitOfWork.GolferRepository.Update(golfer);
            _unitOfWork.Save();


            golferPasswordChange.IsUsed = true;
            golferPasswordChange.UpdatedOn = System.DateTime.UtcNow;
            _unitOfWork.GolferPasswordChangeRepository.Update(golferPasswordChange);
            _unitOfWork.Save();

            return true;
        }

        public bool CheckPasswordLinkStatus(string ss)
        {
            long id = Convert.ToInt64(Core.Helper.Crypto.DecryptStringAES(ss));
            GolferPasswordChange golferPasswordChange = _unitOfWork.GolferPasswordChangeRepository.Get(x => x.GolferPasswordChangeId == id);
            if (golferPasswordChange == null)
                throw new Exception("Link Expired");
            if (golferPasswordChange.IsUsed == true || golferPasswordChange.Createdon.AddMinutes(15) < System.DateTime.UtcNow)
                throw new Exception("Link Expired");

            GolferPasswordChange golferPasswordChange1 = _unitOfWork.GolferPasswordChangeRepository.Get(x => x.GolferPasswordChangeId > id && x.GolferId == golferPasswordChange.GolferId);
            if (golferPasswordChange1 != null)
                throw new Exception("Link Expired");
            return true;

        }



    }
}
