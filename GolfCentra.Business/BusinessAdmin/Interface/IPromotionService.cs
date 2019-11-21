using GolfCentra.ViewModel.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.Business.BusinessAdmin.Interface
{
    public interface IPromotionService
    {
        List<PromotionViewModel> GetAllPromotion();
        void SavePromotion(PromotionViewModel promotionViewModel, long uniqueSessionId);
        bool DeletePromotion(long id, long uniqueSessionId);
    }
}
