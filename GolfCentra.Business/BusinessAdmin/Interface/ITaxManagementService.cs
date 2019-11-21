using System;
using System.Collections.Generic;
using GolfCentra.ViewModel.Admin;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.Business.BusinessAdmin.Interface
{
    public interface  ITaxManagementService
    {
        List<TaxManagementViewModel> GetAllTaxType();
        bool SaveTaxTypeDetails(string taxTypeName, decimal percentage, long uniqueSessionId);
        bool UpdateTaxTypeDetails(long id, String taxType, decimal percentage, long uniqueSessionId);
        bool DeleteTaxTypeDetails(long id, long uniqueSessionId);
    }
}
