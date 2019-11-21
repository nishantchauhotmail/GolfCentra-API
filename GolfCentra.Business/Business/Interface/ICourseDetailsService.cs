using GolfCentra.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.Business.Business.Interface
{
    public interface ICourseDetailsService
    {
        List<HoleViewModel> CourseGPSDetails(long courseNameTypeId);
        List<ScoreViewModel> GetCourseDetailForScorePost(long courseHoleTypeId);
        List<AboutUsViewModel> CourseAboutUsDetails();
        ContactUsViewModel GetCourseContactUsDetails();
        string GetPrivacyPolicyPageURL();
        string GetTermAndConditionPageURL();
        string GetRuleAndRegulationPageURL();
    }
}
