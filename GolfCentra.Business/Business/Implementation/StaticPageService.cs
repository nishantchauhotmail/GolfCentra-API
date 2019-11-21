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
    public class StaticPageService : IStaticPageService
    {
        private readonly UnitOfWork _unitOfWork;

        public StaticPageService()
        {
            _unitOfWork = new UnitOfWork();
        }

        //Not IN USE
        /// <summary>
        /// Get Rule   And Regulation From DB 
        /// </summary>
        /// <returns></returns>
        public List<string> GetRuleAndRegulation()
        {
            List<RuleAndRegulation> ruleAndRegulations = _unitOfWork.RuleAndRegulationRepository.GetMany(x => x.IsActive == true).OrderBy(x => x.OrderNo).ToList();
            List<string> ruleAndRegulationViewModels = new List<string>();
            foreach (RuleAndRegulation ruleAndRegulation in ruleAndRegulations)
            {
                ruleAndRegulationViewModels.Add(ruleAndRegulation.Text);
            }
            return ruleAndRegulationViewModels;
        }
    }
}
