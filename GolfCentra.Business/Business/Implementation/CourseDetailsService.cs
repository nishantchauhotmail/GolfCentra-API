using GolfCentra.Business.Business.Interface;
using GolfCentra.Core;
using GolfCentra.Core.DataBase;
using GolfCentra.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.Business.Business.Implementation
{
    public class CourseDetailsService : ICourseDetailsService
    {
        private readonly UnitOfWork _unitOfWork;

        public CourseDetailsService()
        {
            _unitOfWork = new UnitOfWork();
        }

        /// <summary>
        /// Get Course GPS Page Details
        /// </summary>
        /// <returns></returns>
        public List<HoleViewModel> CourseGPSDetails(long courseNameTypeId)
        {
            List<HoleViewModel> holeViewList = new List<HoleViewModel>();
            Course course = _unitOfWork.CourseRepository.Get().FirstOrDefault();
            var startHole=0;
          

            var maxHole = 0;
            if(courseNameTypeId ==(int)Core.Helper.Enum.EnumCourseNameTypeId.Ridge)
            {
                startHole = 1;
                maxHole = 9;

            }
            else if(courseNameTypeId == (int)Core.Helper.Enum.EnumCourseNameTypeId.Valley)
            {
                startHole = 10;
                maxHole = 18;
            }
            else if (courseNameTypeId == (int)Core.Helper.Enum.EnumCourseNameTypeId.Canyon)
            {
                startHole = 19;
                maxHole = 27;
            }
            else
            {
                throw new Exception("Invalid Course Name");
            }
            int holeNumber = 1;
            for (int i =startHole ; i <= maxHole; i++)
            {
               
                HoleViewModel holeViewModel = new HoleViewModel();
                holeViewModel.HoleNumber = holeNumber;
                ParStorke parStorke = _unitOfWork.ParStorkeRepository.Get(x => x.IsActive == true && x.HoleNumber.Value == i);
                HoleInformation holeInformation = _unitOfWork.HoleInformationRepository.Get(x => x.IsActive == true && x.HoleNumber.Value == i);
                if (parStorke != null)
                {
                    holeViewModel.HoleNumber = holeNumber;
                    holeViewModel.Par = parStorke.Par;
                    holeViewModel.Storke = parStorke.Storke;
                }
                if (holeInformation != null)
                {
                    holeViewModel.Latitude = holeInformation.Latitude;
                    holeViewModel.Longitude = holeInformation.Longitude;
                    holeViewModel.ImgURL = Core.Helper.Constants.Url.WebApiUrlWithoutSlash + "/Upload/HoleImage/" + holeInformation.ImgUrl;
                }


                holeViewModel.CourseTeeViewList = new List<CourseTeeViewModel>();
                //
                List<HoleTeeYardage> holeTeeYardages = _unitOfWork.HoleTeeYardageRepository.GetMany(x => x.IsActive == true && x.HoleNumber.Value == i).ToList();
                foreach (HoleTeeYardage hole in holeTeeYardages)
                {
                    CourseTeeViewModel courseTeeViewModel = new CourseTeeViewModel
                    {
                        TeeName = hole.Tee.Name,
                        Yardage = hole.Yardage,
                    };
                    if (hole.Tee.Name == "Gold") {
                        holeViewModel.CourseTeeViewList.Insert(1,courseTeeViewModel);
                    }
                    else
                    {
                        holeViewModel.CourseTeeViewList.Add(courseTeeViewModel);
                    }
                }
                holeViewList.Add(holeViewModel);
                holeNumber++;
            }
            return holeViewList;
        }

        /// <summary>
        /// Get Course Details For Score Post
        /// </summary>
        /// <returns></returns>
        public List<ScoreViewModel> GetCourseDetailForScorePost(long courseHoleTypeId)
        {
            List<ScoreViewModel> holeViewList = new List<ScoreViewModel>();

            Course course = _unitOfWork.CourseRepository.Get().FirstOrDefault();
            var maxHole = course.HoleType.Value;


            int count = 1;
            for (int i = 1; i <= maxHole; i++)
            {
      
                 if (courseHoleTypeId == (int)Core.Helper.Enum.EnumCourseHoleTypeId.RidgeValley)
                {
                    if (i > 18)
                        break;
                    ScoreViewModel scoreViewModel = new ScoreViewModel
                    {
                        HoleNumber = count
                    };
                    ParStorke parStorke = _unitOfWork.ParStorkeRepository.Get(x => x.IsActive == true && x.HoleNumber.Value == i);
                    HoleInformation holeInformation = _unitOfWork.HoleInformationRepository.Get(x => x.IsActive == true && x.HoleNumber.Value == i);
                    if (parStorke != null)
                    {
                        scoreViewModel.HoleNumber = count;
                        scoreViewModel.Par = parStorke.Par;
                        scoreViewModel.StorkeIndex = parStorke.Storke;
                    }
                    if (holeInformation != null)
                    {
                        scoreViewModel.HoleName = holeInformation.HoleName;
                    }
                    holeViewList.Add(scoreViewModel);
                    count++;
                }
                else if (courseHoleTypeId == (int)Core.Helper.Enum.EnumCourseHoleTypeId.ValleyCanyon)
                {
                    if (i < 10)
                        continue;
                    ScoreViewModel scoreViewModel = new ScoreViewModel
                    {
                        HoleNumber = count
                    };
                    ParStorke parStorke = _unitOfWork.ParStorkeRepository.Get(x => x.IsActive == true && x.HoleNumber.Value == i);
                    HoleInformation holeInformation = _unitOfWork.HoleInformationRepository.Get(x => x.IsActive == true && x.HoleNumber.Value == i);
                    if (parStorke != null)
                    {
                        scoreViewModel.HoleNumber = count;
                        scoreViewModel.Par = parStorke.Par;
                        scoreViewModel.StorkeIndex = parStorke.Storke;
                    }
                    if (holeInformation != null)
                    {
                        scoreViewModel.HoleName = holeInformation.HoleName;
                    }
                    holeViewList.Add(scoreViewModel);
                    count++;
                }
                else
                {
                    throw new Exception("Invalid Course Hole Type Id");
                }
            }
            return holeViewList.OrderBy(x=>x.HoleNumber).ToList();
        }

        /// <summary>
        /// Get About Us Page Details
        /// </summary>
        /// <returns></returns>
        public List<AboutUsViewModel> CourseAboutUsDetails()
        {
            List<AboutU> aboutU = _unitOfWork.AboutUSRepository.GetMany(x => x.IsActive == true).ToList();
            List<AboutUsViewModel> aboutUsViewModels = new List<AboutUsViewModel>();
            foreach (AboutU aboutUS in aboutU)
            {
                AboutUsViewModel aboutUsViewModel = new AboutUsViewModel()
                {
                    Name = aboutUS.Name,
                    ImageURL = Core.Helper.Constants.Url.WebApiUrlWithoutSlash + "/Upload/AboutUs/" + aboutUS.ImageURL,
                    URL = aboutUS.URL
                };
                aboutUsViewModels.Add(aboutUsViewModel);

            }
            return aboutUsViewModels;
        }

        /// <summary>
        /// Get Course details For Contact Us
        /// </summary>
        /// <returns></returns>
        public ContactUsViewModel GetCourseContactUsDetails()
        {
            Course course = _unitOfWork.CourseRepository.GetMany(x => x.IsActive == true).FirstOrDefault();

            ContactUsViewModel contactUsViewModel = new ContactUsViewModel()
            {
                Email = course.Email,
                Address = course.Address,
                CourseName = course.CourseName,
                Latitude = course.Latitude,
                Longitude = course.Longitude,
                Phone = course.Phone,
                TechnicalSupportMailId = course.TechnicalSupportMailId,
                Address2= course.Address2 ?? "",
                Address2Name= course.Address2Name ?? "",
                Address2Phone= course.Address2Phone ?? ""
            };
            return contactUsViewModel;
        }

        /// <summary>
        /// Get Privacy Policy Page Details
        /// </summary>
        /// <returns></returns>
        public string GetPrivacyPolicyPageURL()
        {
            Course course = _unitOfWork.CourseRepository.GetMany(x => x.IsActive == true).FirstOrDefault();
            return course.PrivacyPolicyPageURL;
        }

        /// <summary>
        /// Get Term Condition Page Details
        /// </summary>
        /// <returns></returns>
        public string GetTermAndConditionPageURL()
        {
            Course course = _unitOfWork.CourseRepository.GetMany(x => x.IsActive == true).FirstOrDefault();
            return course.TermsConditionPageURL;
        }

        /// <summary>
        /// Get Rule Regulation Page Details
        /// </summary>
        /// <returns></returns>
        public string GetRuleAndRegulationPageURL()
        {
            Course course = _unitOfWork.CourseRepository.GetMany(x => x.IsActive == true).FirstOrDefault();
            return course.RuleAndRegulationURL;
        }
    }
}
