using System;
using System.Collections.Generic;
using GolfCentra.ViewModel.Admin;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.Business.BusinessAdmin.Interface
{
    public interface IEquipmentService
    {
        List<EquipmentViewModel> GetAllEquipmentType();
   
        bool SaveEquipmentDetails(EquipmentViewModel equipmentViewModel, long uniqueSessionId);
        bool UpdateEquipmentDetails(EquipmentViewModel equipmentViewModel, long uniqueSessionId);
        bool DeleteEquipmentDetails(long id, long uniqueSessionId);
    }
}
