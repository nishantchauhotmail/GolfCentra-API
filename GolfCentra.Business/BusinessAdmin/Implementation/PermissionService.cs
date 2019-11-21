using GolfCentra.Business.BusinessAdmin.Interface;
using GolfCentra.Core;
using GolfCentra.Core.DataBase;
using GolfCentra.ViewModel.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.Business.BusinessAdmin.Implementation
{
    public class PermissionService : IPermissionService
    {
        private readonly UnitOfWork _unitOfWork;

        public PermissionService()
        {
            _unitOfWork = new UnitOfWork();
        }
    
        /// <summary>
        /// Get All Permission Type Detail
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public PermissionMainViewModel GetAllPermissionDetails(long employeeId)
        {

            List<Page> pages = _unitOfWork.PageRepository.GetMany(x => x.IsActive == true).ToList();
            List<PageRight> pageRights = _unitOfWork.PageRightRepository.GetManyWithInclude(x => x.EmployeeId == employeeId && x.IsActive == true).ToList();
            List<PermissionViewModel> pageViewModels = new List<PermissionViewModel>();
            foreach (Page page in pages)
            {
                PermissionViewModel pageViewModel = new PermissionViewModel()
                {
                    PageId = page.PageId,
                    PageName = page.PageName,
                    IsSideMenuPage = true,

                };
                PageRight pageRight = pageRights.Find(x => x.PageId == page.PageId && x.EmployeeId == employeeId && x.IsActive == true);
                if (pageRight == null)
                {
                    pageViewModel.IsPermisson = false;
                }
                else
                {
                    pageViewModel.IsPermisson = true;
                    pageViewModel.PageRightId = pageRight.PageRightId;
                }

                pageViewModels.Add(pageViewModel);
            }


            List<Core.DataBase.Action> actions = _unitOfWork.ActionRepository.GetMany(x => x.IsActive == true).ToList();
            List<ActionRight> actionRights = _unitOfWork.ActionRightRepository.GetManyWithInclude(x => x.EmployeeId == employeeId && x.IsActive == true).ToList();

            foreach (Core.DataBase.Action action in actions)
            {
                PermissionViewModel pageViewModel = new PermissionViewModel()
                {
                    PageId = action.ActionId,
                    PageName = action.Name,
                    IsSideMenuPage = false,

                };
                ActionRight actionRight = actionRights.Find(x => x.ActionId == action.ActionId && x.EmployeeId == employeeId && x.IsActive == true);
                if (actionRight == null)
                {
                    pageViewModel.IsPermisson = false;
                }
                else
                {
                    pageViewModel.IsPermisson = true;
                    pageViewModel.PageRightId = actionRight.ActionRightId;
                }

                pageViewModels.Add(pageViewModel);
            }
            PermissionMainViewModel permissionMainViewModel = new PermissionMainViewModel()
            {
                PermissionViewModels = pageViewModels,
                EmployeeId = employeeId
            };
            return permissionMainViewModel;
        }
     
        /// <summary>
        /// Update Permission Type Detail
        /// </summary>
        /// <param name="permissionViewModels"></param>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public bool UpdatePermission(List<PermissionViewModel> permissionViewModels, long employeeId)
        {

            foreach (PermissionViewModel permission in permissionViewModels)
            {

                if (permission.IsSideMenuPage == true)
                {
                    PageRight pageRight = _unitOfWork.PageRightRepository.Get(x => x.EmployeeId == employeeId && x.PageId == permission.PageId && x.IsActive == true);
                    if (pageRight == null)
                    {
                        if (permission.IsPermisson == true)
                        {
                            PageRight pageRights = new PageRight()
                            {
                                PageId = permission.PageId,
                                EmployeeId = employeeId,
                                IsActive = true,
                                CreatedOn = System.DateTime.UtcNow

                            };
                            _unitOfWork.PageRightRepository.Insert(pageRights);
                        }

                    }
                    else
                    {
                        pageRight.IsActive = permission.IsPermisson;
                        _unitOfWork.PageRightRepository.Update(pageRight);
                    }

                }
                else
                {
                    ActionRight actionRight = _unitOfWork.ActionRightRepository.Get(x => x.EmployeeId == employeeId && x.ActionId == permission.PageId && x.IsActive == true);
                    if (actionRight == null)
                    {
                        if (permission.IsPermisson == true)
                        {
                            ActionRight actionRights = new ActionRight()
                            {
                                ActionId = permission.PageId,
                                EmployeeId = employeeId,
                                IsActive = true,
                                CreatedOn = System.DateTime.UtcNow
                            };
                            _unitOfWork.ActionRightRepository.Insert(actionRights);
                        }
                    }
                    else
                    {
                        actionRight.IsActive = permission.IsPermisson;
                        _unitOfWork.ActionRightRepository.Update(actionRight);
                    }

                }


            }
            _unitOfWork.Save();
            return true;
        }
    }
}
