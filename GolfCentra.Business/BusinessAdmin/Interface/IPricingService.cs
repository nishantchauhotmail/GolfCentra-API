using GolfCentra.ViewModel.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.Business.BusinessAdmin.Interface
{
    public interface IPricingService
    {
        void SavePricing(UpdatePricingViewModel updatePricingViewModel);
        List<UpdatePricingViewModel> SearchAllPricing();
        void SaveNationalHoildayPricing(UpdatePricingViewModel updatePricingViewModel);
        void SavePricing(PricingViewModel pricingViewModel,long uniqueSessionId);
        PricingViewModel SearchPricing(PricingViewModel pricingViewModel);
        List<PricingViewModel> GetAllPricing();
        void SavePricingMuliple(PricingViewModel pricingViewModel, long uniqueSessionId);

    }
}
