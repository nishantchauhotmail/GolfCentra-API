using GolfCentra.Business.BusinessAdmin.Interface;
using GolfCentra.Core;
using GolfCentra.Core.DataBase;
using GolfCentra.ViewModel.Admin;
using GolfCentra.ViewModel.Admin.LoginActivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.Business.BusinessAdmin.Implementation
{
    public class MemberTypeService : IMemberTypeService
    {
        private readonly UnitOfWork _unitOfWork;

        public MemberTypeService()
        {
            _unitOfWork = new UnitOfWork();
        }

        /// <summary>
        /// Get All Member Type
        /// </summary>
        /// <returns></returns>
        public List<MemberTypeViewModel> GetAllMemberType()
        {
            List<MemberTypeViewModel> memberTypeViewModels = new List<MemberTypeViewModel>();
            List<MemberType> memberTypes = _unitOfWork.MemberTypeRepository.GetMany(x => x.IsActive == true).ToList();

            foreach (var item in memberTypes)
            {
                MemberTypeViewModel memberTypeViewModel = new MemberTypeViewModel()
                {
                    MemberTypeId = item.MemberTypeId,
                    Name = item.Name,
                    ValueToShow = item.ValueToShow
                };
                memberTypeViewModels.Add(memberTypeViewModel);
            }
            return memberTypeViewModels;
        }

        /// <summary>
        /// Save member Type
        /// </summary>
        /// <param name="memberTypeName"></param>
        /// <returns></returns>
        public bool SaveMemberTypeDetails(string memberTypeName,string memberTypeValue,long uniqueSessionId)
        {

            MemberType memberTypeDB = _unitOfWork.MemberTypeRepository.Get(x => x.Name.Trim().ToLower() == memberTypeName.Trim().ToLower() && x.IsActive == true);
            if (memberTypeDB != null)
                throw new Exception("Member Type Already Exist");
            MemberType memberType = new MemberType()
            {
                Name = memberTypeName,
            
                IsActive = true,
                CreatedOn = System.DateTime.UtcNow,
                ValueToShow =memberTypeValue
            };
            _unitOfWork.MemberTypeRepository.Insert(memberType);
            _unitOfWork.Save();
            try
            {

                SessionActivityPageViewModel sessionActivityPageViewModel = new SessionActivityPageViewModel()
                {
                    ControllerName = "Save Member Type ",
                    ActionName = "Save",
                    PerformOn = memberType.MemberTypeId.ToString(),
                    LoginHistoryId = uniqueSessionId,

                    Info = "Created a MemberType with id- " + memberType.MemberTypeId.ToString()
                };
                new Common.AddSessionActivity().SaveSessionActivity(sessionActivityPageViewModel);

            }
            catch (Exception)
            {

            }
            return true;
        }

        /// <summary>
        /// Update Member Type
        /// </summary>
        /// <param name="id"></param>
        /// <param name="memberType"></param>
        /// <returns></returns>
        public bool UpdateMemberTypeDetails(long id, String memberType,string valueToShow,long uniqueSessionId)
        {
            MemberType memberTypeDB1 = _unitOfWork.MemberTypeRepository.Get(x => x.Name.Trim().ToLower() == memberType.Trim().ToLower() && x.IsActive == true);
            if (memberTypeDB1 != null)
                throw new Exception("Member Type Already Exist");
            MemberType memberTypeDB = _unitOfWork.MemberTypeRepository.Get(x => x.MemberTypeId == id && x.IsActive == true);
            if (memberTypeDB == null)
                throw new Exception("Member Type  Not Exist");
          

            memberTypeDB.ValueToShow = valueToShow;
            memberTypeDB.Name = memberType;
            _unitOfWork.MemberTypeRepository.Update(memberTypeDB);
            _unitOfWork.Save();

            try
            {

                SessionActivityPageViewModel sessionActivityPageViewModel = new SessionActivityPageViewModel()
                {
                    ControllerName = "Update Member Type ",
                    ActionName = "Update",
                    PerformOn = memberTypeDB.MemberTypeId.ToString(),
                    LoginHistoryId = uniqueSessionId,

                    Info = "Updated a MemberType with id- " + memberTypeDB.MemberTypeId.ToString()
                };
                new Common.AddSessionActivity().SaveSessionActivity(sessionActivityPageViewModel);

            }
            catch (Exception)
            {

            }
            return true;
        }

        /// <summary>
        /// Delete Member Type
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteMemberTypeDetails(long id,long uniqueSessionId)
        {

            MemberType memberTypeDB = _unitOfWork.MemberTypeRepository.Get(x => x.MemberTypeId == id && x.IsActive == true);
            if (memberTypeDB == null)
                throw new Exception("Member Type Not Exist");
            memberTypeDB.IsActive = false;
            memberTypeDB.UpdatedOn = System.DateTime.UtcNow;
            _unitOfWork.MemberTypeRepository.Update(memberTypeDB);
            _unitOfWork.Save();
            try
            {

                SessionActivityPageViewModel sessionActivityPageViewModel = new SessionActivityPageViewModel()
                {
                    ControllerName = "Delete Member Type ",
                    ActionName = "Delete",
                    PerformOn = memberTypeDB.MemberTypeId.ToString(),
                    LoginHistoryId = uniqueSessionId,

                    Info = "Delete a MemberType with id- " + memberTypeDB.MemberTypeId.ToString()
                };
                new Common.AddSessionActivity().SaveSessionActivity(sessionActivityPageViewModel);

            }
            catch (Exception)
            {

            }
            return true;
        }
    }
}
