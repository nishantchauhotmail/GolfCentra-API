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
    public class PageService : IPageService
    {

        private readonly UnitOfWork _unitOfWork;

        public PageService()
        {
            _unitOfWork = new UnitOfWork();
        }
      
        /// <summary>
        /// Get All Page Detail
        /// </summary>
        /// <returns></returns>
        public List<PageViewModel> GetAllPageDetails()
        {
            List<Page> pages = _unitOfWork.PageRepository.GetManyWithInclude(x => x.IsActive == true).ToList();
            List<PageViewModel> pageViewModels = new List<PageViewModel>();
            foreach (Page page in pages)
            {
                PageViewModel pageViewModel = new PageViewModel()
                {
                    PageId = page.PageId,
                    Ordering = page.Ordering,
                    PageName = page.PageName,
                };
                pageViewModels.Add(pageViewModel);
            }
            return pageViewModels;
        }

        /// <summary>
        /// Update Odering Of Pages
        /// </summary>
        /// <param name="pageViewModel"></param>
        /// <returns></returns>
        public bool UpdatePageOrdering(PageViewModel pageViewModel)
        {
            Page page = _unitOfWork.PageRepository.Get(x => x.PageId == pageViewModel.PageId);
            page.Ordering = pageViewModel.Ordering;
            _unitOfWork.PageRepository.Update(page);
            _unitOfWork.Save();
            return true;
        }

     
    }
}
