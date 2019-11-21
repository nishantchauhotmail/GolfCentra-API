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
    public class ScoreService : IScoreService
    {
        private readonly UnitOfWork _unitOfWork;

        public ScoreService()
        {
            _unitOfWork = new UnitOfWork();
        }

        /// <summary>
        /// Get All Score Type By advance Search
        /// </summary>
        /// <param name="scoreSearchViewModel"></param>
        /// <returns></returns>
        public List<ScoreDetailsViewModel> GetScoreDetailByAdvanceSearch(ScoreSearchViewModel scoreSearchViewModel)
        {

            List<ScoreDetailsViewModel> scoreDetailsViewModels = new List<ScoreDetailsViewModel>();
            List<Score> scores = new List<Score>();
            if (scoreSearchViewModel.SearchTypeId == (int)Core.Helper.Enum.EnumAdminScoreSearchType.Date)
            {
                DateTime startDate = new DateTime(scoreSearchViewModel.Date.Year, scoreSearchViewModel.Date.Month, scoreSearchViewModel.Date.Day, 0, 0, 0);
                DateTime endDate = new DateTime(scoreSearchViewModel.Date.Year, scoreSearchViewModel.Date.Month, scoreSearchViewModel.Date.Day, 23, 59, 59);
                scores = _unitOfWork.ScoreRepository.GetMany(x => x.ScoreSubmittedDate >= startDate && x.ScoreSubmittedDate <= endDate && x.IsActive == true).ToList();

            }
            else if (scoreSearchViewModel.SearchTypeId == (int)Core.Helper.Enum.EnumAdminScoreSearchType.EmailDate)
            {
                DateTime startDate = new DateTime(scoreSearchViewModel.Date.Year, scoreSearchViewModel.Date.Month, scoreSearchViewModel.Date.Day, 0, 0, 0);
                DateTime endDate = new DateTime(scoreSearchViewModel.Date.Year, scoreSearchViewModel.Date.Month, scoreSearchViewModel.Date.Day, 23, 59, 59);
                scores = _unitOfWork.ScoreRepository.GetMany(x => x.Golfer.Email.ToLower() == scoreSearchViewModel.Email.ToLower() && x.ScoreSubmittedDate >= startDate && x.ScoreSubmittedDate <= endDate && x.IsActive == true).ToList();

            }
            else if (scoreSearchViewModel.SearchTypeId == (int)Core.Helper.Enum.EnumAdminScoreSearchType.DateFilter)
            {

                DateTime startDate = new DateTime(scoreSearchViewModel.StartDate.Year, scoreSearchViewModel.StartDate.Month, scoreSearchViewModel.StartDate.Day, 0, 0, 0);
                DateTime endDate = new DateTime(scoreSearchViewModel.EndDate.Year, scoreSearchViewModel.EndDate.Month, scoreSearchViewModel.EndDate.Day, 23, 59, 59);

                scores = _unitOfWork.ScoreRepository.GetMany(x => x.ScoreSubmittedDate >= scoreSearchViewModel.StartDate && x.ScoreSubmittedDate <= scoreSearchViewModel.EndDate && x.IsActive == true).ToList();

            }
            else if (scoreSearchViewModel.SearchTypeId == (int)Core.Helper.Enum.EnumAdminScoreSearchType.EmailArray)
            {
                foreach (var email in scoreSearchViewModel.EmailList)
                {
                    scores.AddRange(_unitOfWork.ScoreRepository.GetMany(x => x.Golfer.Email.ToLower() == email.ToLower() && x.IsActive == true).ToList());
                }
            }

            foreach (Score score in scores)
            {
                List<ScoreViewModel> scoreViewModels = new List<ScoreViewModel>();
                long scoreId = score.ScoreId;
                if (score.ScoreHole1 != null && score.ScoreHole1.Count() != 0)
                {
                    ScoreViewModel scoreViewModel = new ScoreViewModel()
                    {
                        HoleNumber = 1,
                        Par = _unitOfWork.ParStorkeRepository.Get(x => x.HoleNumberId == 1).Par,
                        Storkes = score.ScoreHole1.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault(),
                        HoleName = score.ScoreHole1.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                    };
                    if (scoreViewModel.Storkes != 0)
                        scoreViewModels.Add(scoreViewModel);
                }
                if (score.ScoreHole2 != null && score.ScoreHole2.Count() != 0)
                {
                    ScoreViewModel scoreViewModel = new ScoreViewModel()
                    {
                        HoleNumber = 2,
                        Par = _unitOfWork.ParStorkeRepository.Get(x => x.HoleNumberId == 2).Par,
                        Storkes = score.ScoreHole2.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault(),
                        HoleName = score.ScoreHole2.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                    };
                    if (scoreViewModel.Storkes != 0)
                        scoreViewModels.Add(scoreViewModel);
                }
                if (score.ScoreHole3 != null && score.ScoreHole3.Count() != 0)
                {
                    ScoreViewModel scoreViewModel = new ScoreViewModel()
                    {
                        HoleNumber = 3,
                        Par = _unitOfWork.ParStorkeRepository.Get(x => x.HoleNumberId == 3).Par,
                        Storkes = score.ScoreHole3.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault(),
                        HoleName = score.ScoreHole3.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                    };
                    if (scoreViewModel.Storkes != 0)
                        scoreViewModels.Add(scoreViewModel);
                }
                if (score.ScoreHole4 != null && score.ScoreHole4.Count() != 0)
                {
                    ScoreViewModel scoreViewModel = new ScoreViewModel()
                    {
                        HoleNumber = 4,
                        Par = _unitOfWork.ParStorkeRepository.Get(x => x.HoleNumberId == 4).Par,
                        HoleName = score.ScoreHole4.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                        Storkes = score.ScoreHole4.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault()
                    };
                    if (scoreViewModel.Storkes != 0)
                        scoreViewModels.Add(scoreViewModel);
                }
                if (score.ScoreHole5 != null && score.ScoreHole5.Count() != 0)
                {
                    ScoreViewModel scoreViewModel = new ScoreViewModel()
                    {
                        HoleNumber = 5,
                        Par = _unitOfWork.ParStorkeRepository.Get(x => x.HoleNumberId == 5).Par,
                        HoleName = score.ScoreHole5.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                        Storkes = score.ScoreHole5.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault()
                    };
                    if (scoreViewModel.Storkes != 0)
                        scoreViewModels.Add(scoreViewModel);
                }
                if (score.ScoreHole6 != null && score.ScoreHole6.Count() != 0)
                {
                    ScoreViewModel scoreViewModel = new ScoreViewModel()
                    {
                        HoleNumber = 6,
                        Par = _unitOfWork.ParStorkeRepository.Get(x => x.HoleNumberId == 6).Par,
                        HoleName = score.ScoreHole6.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                        Storkes = score.ScoreHole6.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault()
                    };
                    if (scoreViewModel.Storkes != 0)
                        scoreViewModels.Add(scoreViewModel);
                }
                if (score.ScoreHole7 != null && score.ScoreHole7.Count() != 0)
                {
                    ScoreViewModel scoreViewModel = new ScoreViewModel()
                    {
                        HoleNumber = 7,
                        Par = _unitOfWork.ParStorkeRepository.Get(x => x.HoleNumberId == 7).Par,
                        HoleName = score.ScoreHole7.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                        Storkes = score.ScoreHole7.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault()
                    };
                    if (scoreViewModel.Storkes != 0)
                        scoreViewModels.Add(scoreViewModel);
                }
                if (score.ScoreHole8 != null && score.ScoreHole8.Count() != 0)
                {
                    ScoreViewModel scoreViewModel = new ScoreViewModel()
                    {
                        HoleNumber = 8,
                        Par = _unitOfWork.ParStorkeRepository.Get(x => x.HoleNumberId == 8).Par,
                        HoleName = score.ScoreHole8.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                        Storkes = score.ScoreHole8.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault()
                    };
                    if (scoreViewModel.Storkes != 0)
                        scoreViewModels.Add(scoreViewModel);
                }
                if (score.ScoreHole9 != null && score.ScoreHole9.Count() != 0)
                {
                    ScoreViewModel scoreViewModel = new ScoreViewModel()
                    {
                        HoleNumber = 9,
                        Par = _unitOfWork.ParStorkeRepository.Get(x => x.HoleNumberId == 9).Par,
                        HoleName = score.ScoreHole9.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                        Storkes = score.ScoreHole9.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault()
                    };
                    if (scoreViewModel.Storkes != 0)
                        scoreViewModels.Add(scoreViewModel);
                }
                if (score.ScoreHole10 != null && score.ScoreHole10.Count() != 0)
                {
                    ScoreViewModel scoreViewModel = new ScoreViewModel()
                    {
                        HoleNumber = 10,
                        Par = _unitOfWork.ParStorkeRepository.Get(x => x.HoleNumberId == 10).Par,
                        HoleName = score.ScoreHole10.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                        Storkes = score.ScoreHole10.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault()
                    };
                    if (scoreViewModel.Storkes != 0)
                        scoreViewModels.Add(scoreViewModel);
                }
                if (score.ScoreHole11 != null && score.ScoreHole11.Count() != 0)
                {
                    ScoreViewModel scoreViewModel = new ScoreViewModel()
                    {
                        HoleNumber = 11,
                        Par = _unitOfWork.ParStorkeRepository.Get(x => x.HoleNumberId == 11).Par,
                        HoleName = score.ScoreHole11.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                        Storkes = score.ScoreHole11.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault()
                    };
                    if (scoreViewModel.Storkes != 0)
                        scoreViewModels.Add(scoreViewModel);
                }
                if (score.ScoreHole12 != null && score.ScoreHole12.Count() != 0)
                {
                    ScoreViewModel scoreViewModel = new ScoreViewModel()
                    {
                        HoleNumber = 12,
                        Par = _unitOfWork.ParStorkeRepository.Get(x => x.HoleNumberId == 12).Par,
                        HoleName = score.ScoreHole12.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                        Storkes = score.ScoreHole12.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault()
                    };
                    if (scoreViewModel.Storkes != 0)
                        scoreViewModels.Add(scoreViewModel);
                }
                if (score.ScoreHole13 != null && score.ScoreHole13.Count() != 0)
                {
                    ScoreViewModel scoreViewModel = new ScoreViewModel()
                    {
                        HoleNumber = 13,
                        Par = _unitOfWork.ParStorkeRepository.Get(x => x.HoleNumberId == 13).Par,
                        HoleName = score.ScoreHole13.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                        Storkes = score.ScoreHole13.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault()
                    };
                    if (scoreViewModel.Storkes != 0)
                        scoreViewModels.Add(scoreViewModel);
                }
                if (score.ScoreHole14 != null && score.ScoreHole14.Count() != 0)
                {
                    ScoreViewModel scoreViewModel = new ScoreViewModel()
                    {
                        HoleNumber = 14,
                        Par = _unitOfWork.ParStorkeRepository.Get(x => x.HoleNumberId == 14).Par,
                        HoleName = score.ScoreHole14.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                        Storkes = score.ScoreHole14.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault()
                    };
                    if (scoreViewModel.Storkes != 0)
                        scoreViewModels.Add(scoreViewModel);
                }
                if (score.ScoreHole15 != null && score.ScoreHole15.Count() != 0)
                {
                    ScoreViewModel scoreViewModel = new ScoreViewModel()
                    {
                        HoleNumber = 15,
                        Par = _unitOfWork.ParStorkeRepository.Get(x => x.HoleNumberId == 15).Par,
                        HoleName = score.ScoreHole15.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                        Storkes = score.ScoreHole15.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault()
                    };
                    if (scoreViewModel.Storkes != 0)
                        scoreViewModels.Add(scoreViewModel);
                }
                if (score.ScoreHole16 != null && score.ScoreHole16.Count() != 0)
                {
                    ScoreViewModel scoreViewModel = new ScoreViewModel()
                    {
                        HoleNumber = 16,
                        Par = _unitOfWork.ParStorkeRepository.Get(x => x.HoleNumberId == 16).Par,
                        HoleName = score.ScoreHole16.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                        Storkes = score.ScoreHole16.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault()
                    };
                    if (scoreViewModel.Storkes != 0)
                        scoreViewModels.Add(scoreViewModel);
                }
                if (score.ScoreHole17 != null && score.ScoreHole17.Count() != 0)
                {
                    ScoreViewModel scoreViewModel = new ScoreViewModel()
                    {
                        HoleNumber = 17,
                        Par = _unitOfWork.ParStorkeRepository.Get(x => x.HoleNumberId == 17).Par,
                        HoleName = score.ScoreHole17.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                        Storkes = score.ScoreHole17.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault()
                    };
                    if (scoreViewModel.Storkes != 0)
                        scoreViewModels.Add(scoreViewModel);
                }
                if (score.ScoreHole18 != null && score.ScoreHole18.Count() != 0)
                {
                    ScoreViewModel scoreViewModel = new ScoreViewModel()
                    {
                        HoleNumber = 18,
                        Par = _unitOfWork.ParStorkeRepository.Get(x => x.HoleNumberId == 18).Par,
                        HoleName = score.ScoreHole18.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                        Storkes = score.ScoreHole18.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault()
                    };
                    if (scoreViewModel.Storkes != 0)
                        scoreViewModels.Add(scoreViewModel);
                }
                if (score.ScoreHole19 != null && score.ScoreHole19.Count() != 0)
                {
                    ScoreViewModel scoreViewModel = new ScoreViewModel()
                    {
                        HoleNumber = 19,
                        Par = _unitOfWork.ParStorkeRepository.Get(x => x.HoleNumberId == 19).Par,
                        HoleName = score.ScoreHole19.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                        Storkes = score.ScoreHole19.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault()
                    };
                    if (scoreViewModel.Storkes != 0)
                        scoreViewModels.Add(scoreViewModel);
                }
                if (score.ScoreHole20 != null && score.ScoreHole20.Count() != 0)
                {
                    ScoreViewModel scoreViewModel = new ScoreViewModel()
                    {
                        HoleNumber = 20,
                        Par = _unitOfWork.ParStorkeRepository.Get(x => x.HoleNumberId == 20).Par,
                        HoleName = score.ScoreHole20.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                        Storkes = score.ScoreHole20.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault()
                    };
                    if (scoreViewModel.Storkes != 0)
                        scoreViewModels.Add(scoreViewModel);
                }
                if (score.ScoreHole21 != null && score.ScoreHole21.Count() != 0)
                {
                    ScoreViewModel scoreViewModel = new ScoreViewModel()
                    {
                        HoleNumber = 21,
                        Par = _unitOfWork.ParStorkeRepository.Get(x => x.HoleNumberId == 21).Par,
                        HoleName = score.ScoreHole21.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                        Storkes = score.ScoreHole21.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault()
                    };
                    if (scoreViewModel.Storkes != 0)
                        scoreViewModels.Add(scoreViewModel);
                }
                if (score.ScoreHole22 != null && score.ScoreHole22.Count() != 0)
                {
                    ScoreViewModel scoreViewModel = new ScoreViewModel()
                    {
                        HoleNumber = 22,
                        Par = _unitOfWork.ParStorkeRepository.Get(x => x.HoleNumberId == 22).Par,
                        HoleName = score.ScoreHole22.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                        Storkes = score.ScoreHole22.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault()
                    };
                    if (scoreViewModel.Storkes != 0)
                        scoreViewModels.Add(scoreViewModel);
                }
                if (score.ScoreHole23 != null && score.ScoreHole23.Count() != 0)
                {
                    ScoreViewModel scoreViewModel = new ScoreViewModel()
                    {
                        HoleNumber = 23,
                        Par = _unitOfWork.ParStorkeRepository.Get(x => x.HoleNumberId == 23).Par,
                        HoleName = score.ScoreHole23.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                        Storkes = score.ScoreHole23.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault()
                    };
                    if (scoreViewModel.Storkes != 0)
                        scoreViewModels.Add(scoreViewModel);
                }
                if (score.ScoreHole24 != null && score.ScoreHole24.Count() != 0)
                {
                    ScoreViewModel scoreViewModel = new ScoreViewModel()
                    {
                        HoleNumber = 24,
                        Par = _unitOfWork.ParStorkeRepository.Get(x => x.HoleNumberId == 24).Par,
                        HoleName = score.ScoreHole24.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                        Storkes = score.ScoreHole24.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault()
                    };
                    if (scoreViewModel.Storkes != 0)
                        scoreViewModels.Add(scoreViewModel);
                }
                if (score.ScoreHole25 != null && score.ScoreHole25.Count() != 0)
                {
                    ScoreViewModel scoreViewModel = new ScoreViewModel()
                    {
                        HoleNumber = 25,
                        Par = _unitOfWork.ParStorkeRepository.Get(x => x.HoleNumberId == 25).Par,
                        HoleName = score.ScoreHole25.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                        Storkes = score.ScoreHole25.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault()
                    };
                    if (scoreViewModel.Storkes != 0)
                        scoreViewModels.Add(scoreViewModel);
                }
                if (score.ScoreHole26 != null && score.ScoreHole26.Count() != 0)
                {
                    ScoreViewModel scoreViewModel = new ScoreViewModel()
                    {
                        HoleNumber = 26,
                        Par = _unitOfWork.ParStorkeRepository.Get(x => x.HoleNumberId == 26).Par,
                        HoleName = score.ScoreHole26.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                        Storkes = score.ScoreHole26.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault()
                    };
                    if (scoreViewModel.Storkes != 0)
                        scoreViewModels.Add(scoreViewModel);
                }
                if (score.ScoreHole27 != null && score.ScoreHole27.Count() != 0)
                {
                    ScoreViewModel scoreViewModel = new ScoreViewModel()
                    {
                        HoleNumber = 27,
                        Par = _unitOfWork.ParStorkeRepository.Get(x => x.HoleNumberId == 27).Par,
                        HoleName = score.ScoreHole27.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                        Storkes = score.ScoreHole27.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault()
                    };
                    if (scoreViewModel.Storkes != 0)
                        scoreViewModels.Add(scoreViewModel);
                }

                ScoreDetailsViewModel scoreDetailsViewModel = new ScoreDetailsViewModel()
                {
                    //ScoreDate = score.ScoreSubmittedDate.ToShortDateString(),
                    // Time = score.ScoreSubmittedTime.ToString("hh':'mm"),
                    ScoreId = score.ScoreId,
                    Email = score.Golfer.Email,
                    UserName = score.Golfer.Name,
                    SubmittedDate = score.ScoreSubmittedDate,
                    Time = score.ScoreSubmittedTime,
                    ScoreViewModels = scoreViewModels
                };
                scoreDetailsViewModels.Add(scoreDetailsViewModel);
            }
            return scoreDetailsViewModels;
        }


        public List<ScoreDetailsViewModel> GetScoreDetailReportByAdvanceSearch(ScoreSearchViewModel scoreSearchViewModel)
        {
            List<ScoreDetailsViewModel> scoreDetailsViewModels = new List<ScoreDetailsViewModel>();
            List<Score> scores = new List<Score>();
            if (scoreSearchViewModel.Date.Year != 0001)
            {
                DateTime startDate = new DateTime(scoreSearchViewModel.Date.Year, scoreSearchViewModel.Date.Month, scoreSearchViewModel.Date.Day, 0, 0, 0);
                DateTime endDate = new DateTime(scoreSearchViewModel.Date.Year, scoreSearchViewModel.Date.Month, scoreSearchViewModel.Date.Day, 23, 59, 59);
                scores = _unitOfWork.ScoreRepository.GetMany(x => x.BookingId != null && x.IsActive == true).Where(x => x.Booking.BookingDate >= startDate && x.Booking.BookingDate <= endDate).ToList();

            }

            if (scoreSearchViewModel.CoursePairingId != 0)
            {
                if (scores.Count() == 0)
                {
                    scores = _unitOfWork.ScoreRepository.GetMany(x => x.BookingId != null && x.Booking.CoursePairingId == scoreSearchViewModel.CoursePairingId && x.IsActive == true).ToList();

                }
                else
                {
                    scores = scores.Where(x => x.Booking.CoursePairingId == scoreSearchViewModel.CoursePairingId && x.IsActive == true).ToList();
                }

            }
            if (scoreSearchViewModel.GolferName != "" && scoreSearchViewModel.GolferName != null)
            {
                if (scores.Count() == 0)
                {
                    scores = _unitOfWork.ScoreRepository.GetMany(x => x.Golfer.Name.ToLower() == scoreSearchViewModel.GolferName.ToLower() && x.IsActive == true).ToList();

                }
                else
                {
                    scores = scores.Where(x => x.Golfer.Name.ToLower() == scoreSearchViewModel.GolferName.ToLower() && x.IsActive == true).ToList();

                }
            }
            if (scoreSearchViewModel.MemberShipId != "" && scoreSearchViewModel.MemberShipId != null)
            {
                if (scores.Count() == 0)
                {
                    scores = _unitOfWork.ScoreRepository.GetMany(x => x.Golfer.ClubMemberId == scoreSearchViewModel.MemberShipId && x.IsActive == true).ToList();

                }
                else
                {
                    scores = scores.Where(x => x.Golfer.ClubMemberId == scoreSearchViewModel.MemberShipId && x.IsActive == true).ToList();
                }
            }



            foreach (Score score in scores)
            {
                List<ScoreViewModel> scoreViewModels = new List<ScoreViewModel>();
                long scoreId = score.ScoreId;
                if (score.ScoreHole1 != null && score.ScoreHole1.Count() != 0)
                {
                    ScoreViewModel scoreViewModel = new ScoreViewModel()
                    {
                        HoleNumber = 1,
                        Par = _unitOfWork.ParStorkeRepository.Get(x => x.HoleNumberId == 1).Par,
                        Storkes = score.ScoreHole1.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault(),
                        HoleName = score.ScoreHole1.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                    };
                    if (scoreViewModel.Storkes != 0)
                        scoreViewModels.Add(scoreViewModel);
                }
                if (score.ScoreHole2 != null && score.ScoreHole2.Count() != 0)
                {
                    ScoreViewModel scoreViewModel = new ScoreViewModel()
                    {
                        HoleNumber = 2,
                        Par = _unitOfWork.ParStorkeRepository.Get(x => x.HoleNumberId == 2).Par,
                        Storkes = score.ScoreHole2.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault(),
                        HoleName = score.ScoreHole2.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                    };
                    if (scoreViewModel.Storkes != 0)
                        scoreViewModels.Add(scoreViewModel);
                }
                if (score.ScoreHole3 != null && score.ScoreHole3.Count() != 0)
                {
                    ScoreViewModel scoreViewModel = new ScoreViewModel()
                    {
                        HoleNumber = 3,
                        Par = _unitOfWork.ParStorkeRepository.Get(x => x.HoleNumberId == 3).Par,
                        Storkes = score.ScoreHole3.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault(),
                        HoleName = score.ScoreHole3.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                    };
                    if (scoreViewModel.Storkes != 0)
                        scoreViewModels.Add(scoreViewModel);
                }
                if (score.ScoreHole4 != null && score.ScoreHole4.Count() != 0)
                {
                    ScoreViewModel scoreViewModel = new ScoreViewModel()
                    {
                        HoleNumber = 4,
                        Par = _unitOfWork.ParStorkeRepository.Get(x => x.HoleNumberId == 4).Par,
                        HoleName = score.ScoreHole4.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                        Storkes = score.ScoreHole4.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault()
                    };
                    if (scoreViewModel.Storkes != 0)
                        scoreViewModels.Add(scoreViewModel);
                }
                if (score.ScoreHole5 != null && score.ScoreHole5.Count() != 0)
                {
                    ScoreViewModel scoreViewModel = new ScoreViewModel()
                    {
                        HoleNumber = 5,
                        Par = _unitOfWork.ParStorkeRepository.Get(x => x.HoleNumberId == 5).Par,
                        HoleName = score.ScoreHole5.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                        Storkes = score.ScoreHole5.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault()
                    };
                    if (scoreViewModel.Storkes != 0)
                        scoreViewModels.Add(scoreViewModel);
                }
                if (score.ScoreHole6 != null && score.ScoreHole6.Count() != 0)
                {
                    ScoreViewModel scoreViewModel = new ScoreViewModel()
                    {
                        HoleNumber = 6,
                        Par = _unitOfWork.ParStorkeRepository.Get(x => x.HoleNumberId == 6).Par,
                        HoleName = score.ScoreHole6.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                        Storkes = score.ScoreHole6.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault()
                    };
                    if (scoreViewModel.Storkes != 0)
                        scoreViewModels.Add(scoreViewModel);
                }
                if (score.ScoreHole7 != null && score.ScoreHole7.Count() != 0)
                {
                    ScoreViewModel scoreViewModel = new ScoreViewModel()
                    {
                        HoleNumber = 7,
                        Par = _unitOfWork.ParStorkeRepository.Get(x => x.HoleNumberId == 7).Par,
                        HoleName = score.ScoreHole7.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                        Storkes = score.ScoreHole7.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault()
                    };
                    if (scoreViewModel.Storkes != 0)
                        scoreViewModels.Add(scoreViewModel);
                }
                if (score.ScoreHole8 != null && score.ScoreHole8.Count() != 0)
                {
                    ScoreViewModel scoreViewModel = new ScoreViewModel()
                    {
                        HoleNumber = 8,
                        Par = _unitOfWork.ParStorkeRepository.Get(x => x.HoleNumberId == 8).Par,
                        HoleName = score.ScoreHole8.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                        Storkes = score.ScoreHole8.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault()
                    };
                    if (scoreViewModel.Storkes != 0)
                        scoreViewModels.Add(scoreViewModel);
                }
                if (score.ScoreHole9 != null && score.ScoreHole9.Count() != 0)
                {
                    ScoreViewModel scoreViewModel = new ScoreViewModel()
                    {
                        HoleNumber = 9,
                        Par = _unitOfWork.ParStorkeRepository.Get(x => x.HoleNumberId == 9).Par,
                        HoleName = score.ScoreHole9.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                        Storkes = score.ScoreHole9.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault()
                    };
                    if (scoreViewModel.Storkes != 0)
                        scoreViewModels.Add(scoreViewModel);
                }
                if (score.ScoreHole10 != null && score.ScoreHole10.Count() != 0)
                {
                    ScoreViewModel scoreViewModel = new ScoreViewModel()
                    {
                        HoleNumber = 10,
                        Par = _unitOfWork.ParStorkeRepository.Get(x => x.HoleNumberId == 10).Par,
                        HoleName = score.ScoreHole10.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                        Storkes = score.ScoreHole10.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault()
                    };
                    if (scoreViewModel.Storkes != 0)
                        scoreViewModels.Add(scoreViewModel);
                }
                if (score.ScoreHole11 != null && score.ScoreHole11.Count() != 0)
                {
                    ScoreViewModel scoreViewModel = new ScoreViewModel()
                    {
                        HoleNumber = 11,
                        Par = _unitOfWork.ParStorkeRepository.Get(x => x.HoleNumberId == 11).Par,
                        HoleName = score.ScoreHole11.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                        Storkes = score.ScoreHole11.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault()
                    };
                    if (scoreViewModel.Storkes != 0)
                        scoreViewModels.Add(scoreViewModel);
                }
                if (score.ScoreHole12 != null && score.ScoreHole12.Count() != 0)
                {
                    ScoreViewModel scoreViewModel = new ScoreViewModel()
                    {
                        HoleNumber = 12,
                        Par = _unitOfWork.ParStorkeRepository.Get(x => x.HoleNumberId == 12).Par,
                        HoleName = score.ScoreHole12.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                        Storkes = score.ScoreHole12.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault()
                    };
                    if (scoreViewModel.Storkes != 0)
                        scoreViewModels.Add(scoreViewModel);
                }
                if (score.ScoreHole13 != null && score.ScoreHole13.Count() != 0)
                {
                    ScoreViewModel scoreViewModel = new ScoreViewModel()
                    {
                        HoleNumber = 13,
                        Par = _unitOfWork.ParStorkeRepository.Get(x => x.HoleNumberId == 13).Par,
                        HoleName = score.ScoreHole13.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                        Storkes = score.ScoreHole13.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault()
                    };
                    if (scoreViewModel.Storkes != 0)
                        scoreViewModels.Add(scoreViewModel);
                }
                if (score.ScoreHole14 != null && score.ScoreHole14.Count() != 0)
                {
                    ScoreViewModel scoreViewModel = new ScoreViewModel()
                    {
                        HoleNumber = 14,
                        Par = _unitOfWork.ParStorkeRepository.Get(x => x.HoleNumberId == 14).Par,
                        HoleName = score.ScoreHole14.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                        Storkes = score.ScoreHole14.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault()
                    };
                    if (scoreViewModel.Storkes != 0)
                        scoreViewModels.Add(scoreViewModel);
                }
                if (score.ScoreHole15 != null && score.ScoreHole15.Count() != 0)
                {
                    ScoreViewModel scoreViewModel = new ScoreViewModel()
                    {
                        HoleNumber = 15,
                        Par = _unitOfWork.ParStorkeRepository.Get(x => x.HoleNumberId == 15).Par,
                        HoleName = score.ScoreHole15.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                        Storkes = score.ScoreHole15.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault()
                    };
                    if (scoreViewModel.Storkes != 0)
                        scoreViewModels.Add(scoreViewModel);
                }
                if (score.ScoreHole16 != null && score.ScoreHole16.Count() != 0)
                {
                    ScoreViewModel scoreViewModel = new ScoreViewModel()
                    {
                        HoleNumber = 16,
                        Par = _unitOfWork.ParStorkeRepository.Get(x => x.HoleNumberId == 16).Par,
                        HoleName = score.ScoreHole16.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                        Storkes = score.ScoreHole16.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault()
                    };
                    if (scoreViewModel.Storkes != 0)
                        scoreViewModels.Add(scoreViewModel);
                }
                if (score.ScoreHole17 != null && score.ScoreHole17.Count() != 0)
                {
                    ScoreViewModel scoreViewModel = new ScoreViewModel()
                    {
                        HoleNumber = 17,
                        Par = _unitOfWork.ParStorkeRepository.Get(x => x.HoleNumberId == 17).Par,
                        HoleName = score.ScoreHole17.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                        Storkes = score.ScoreHole17.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault()
                    };
                    if (scoreViewModel.Storkes != 0)
                        scoreViewModels.Add(scoreViewModel);
                }
                if (score.ScoreHole18 != null && score.ScoreHole18.Count() != 0)
                {
                    ScoreViewModel scoreViewModel = new ScoreViewModel()
                    {
                        HoleNumber = 18,
                        Par = _unitOfWork.ParStorkeRepository.Get(x => x.HoleNumberId == 18).Par,
                        HoleName = score.ScoreHole18.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                        Storkes = score.ScoreHole18.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault()
                    };
                    if (scoreViewModel.Storkes != 0)
                        scoreViewModels.Add(scoreViewModel);
                }
                if (score.ScoreHole19 != null && score.ScoreHole19.Count() != 0)
                {
                    ScoreViewModel scoreViewModel = new ScoreViewModel()
                    {
                        HoleNumber = 19,
                        Par = _unitOfWork.ParStorkeRepository.Get(x => x.HoleNumberId == 19).Par,
                        HoleName = score.ScoreHole19.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                        Storkes = score.ScoreHole19.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault()
                    };
                    if (scoreViewModel.Storkes != 0)
                        scoreViewModels.Add(scoreViewModel);
                }
                if (score.ScoreHole20 != null && score.ScoreHole20.Count() != 0)
                {
                    ScoreViewModel scoreViewModel = new ScoreViewModel()
                    {
                        HoleNumber = 20,
                        Par = _unitOfWork.ParStorkeRepository.Get(x => x.HoleNumberId == 20).Par,
                        HoleName = score.ScoreHole20.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                        Storkes = score.ScoreHole20.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault()
                    };
                    if (scoreViewModel.Storkes != 0)
                        scoreViewModels.Add(scoreViewModel);
                }
                if (score.ScoreHole21 != null && score.ScoreHole21.Count() != 0)
                {
                    ScoreViewModel scoreViewModel = new ScoreViewModel()
                    {
                        HoleNumber = 21,
                        Par = _unitOfWork.ParStorkeRepository.Get(x => x.HoleNumberId == 21).Par,
                        HoleName = score.ScoreHole21.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                        Storkes = score.ScoreHole21.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault()
                    };
                    if (scoreViewModel.Storkes != 0)
                        scoreViewModels.Add(scoreViewModel);
                }
                if (score.ScoreHole22 != null && score.ScoreHole22.Count() != 0)
                {
                    ScoreViewModel scoreViewModel = new ScoreViewModel()
                    {
                        HoleNumber = 22,
                        Par = _unitOfWork.ParStorkeRepository.Get(x => x.HoleNumberId == 22).Par,
                        HoleName = score.ScoreHole22.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                        Storkes = score.ScoreHole22.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault()
                    };
                    if (scoreViewModel.Storkes != 0)
                        scoreViewModels.Add(scoreViewModel);
                }
                if (score.ScoreHole23 != null && score.ScoreHole23.Count() != 0)
                {
                    ScoreViewModel scoreViewModel = new ScoreViewModel()
                    {
                        HoleNumber = 23,
                        Par = _unitOfWork.ParStorkeRepository.Get(x => x.HoleNumberId == 23).Par,
                        HoleName = score.ScoreHole23.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                        Storkes = score.ScoreHole23.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault()
                    };
                    if (scoreViewModel.Storkes != 0)
                        scoreViewModels.Add(scoreViewModel);
                }
                if (score.ScoreHole24 != null && score.ScoreHole24.Count() != 0)
                {
                    ScoreViewModel scoreViewModel = new ScoreViewModel()
                    {
                        HoleNumber = 24,
                        Par = _unitOfWork.ParStorkeRepository.Get(x => x.HoleNumberId == 24).Par,
                        HoleName = score.ScoreHole24.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                        Storkes = score.ScoreHole24.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault()
                    };
                    if (scoreViewModel.Storkes != 0)
                        scoreViewModels.Add(scoreViewModel);
                }
                if (score.ScoreHole25 != null && score.ScoreHole25.Count() != 0)
                {
                    ScoreViewModel scoreViewModel = new ScoreViewModel()
                    {
                        HoleNumber = 25,
                        Par = _unitOfWork.ParStorkeRepository.Get(x => x.HoleNumberId == 25).Par,
                        HoleName = score.ScoreHole25.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                        Storkes = score.ScoreHole25.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault()
                    };
                    if (scoreViewModel.Storkes != 0)
                        scoreViewModels.Add(scoreViewModel);
                }
                if (score.ScoreHole26 != null && score.ScoreHole26.Count() != 0)
                {
                    ScoreViewModel scoreViewModel = new ScoreViewModel()
                    {
                        HoleNumber = 26,
                        Par = _unitOfWork.ParStorkeRepository.Get(x => x.HoleNumberId == 26).Par,
                        HoleName = score.ScoreHole26.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                        Storkes = score.ScoreHole26.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault()
                    };
                    if (scoreViewModel.Storkes != 0)
                        scoreViewModels.Add(scoreViewModel);
                }
                if (score.ScoreHole27 != null && score.ScoreHole27.Count() != 0)
                {
                    ScoreViewModel scoreViewModel = new ScoreViewModel()
                    {
                        HoleNumber = 27,
                        Par = _unitOfWork.ParStorkeRepository.Get(x => x.HoleNumberId == 27).Par,
                        HoleName = score.ScoreHole27.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                        Storkes = score.ScoreHole27.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault()
                    };
                    if (scoreViewModel.Storkes != 0)
                        scoreViewModels.Add(scoreViewModel);
                }

                ScoreDetailsViewModel scoreDetailsViewModel = new ScoreDetailsViewModel()
                {
                    //ScoreDate = score.ScoreSubmittedDate.ToShortDateString(),
                    // Time = score.ScoreSubmittedTime.ToString("hh':'mm"),
                    ScoreId = score.ScoreId,
                    Email = score.Golfer.Email,
                    UserName = score.Golfer.Name,
                    SubmittedDate = score.ScoreSubmittedDate,
                    Time = score.ScoreSubmittedTime,
                    ScoreViewModels = scoreViewModels,
                    CoursePairingName = score.BookingId != null ? (score.Booking.CoursePairingId != null ? (score.Booking.CoursePairing.CourseName1.Value) + (score.Booking.CoursePairing.EndCourseNameId != null ? " - " + score.Booking.CoursePairing.CourseName.Value : "") : "") : "",
                    MemberShipId = score.Golfer.ClubMemberId,
                    DateOfPlay = score.BookingId != null ? score.Booking.TeeOffDate : new DateTime(),
                    PlayTime = score.BookingId != null ? score.Booking.TeeOffSlot : ""

                };
                scoreDetailsViewModels.Add(scoreDetailsViewModel);
            }
            return scoreDetailsViewModels;
        }


        public ScoreDetailsViewModel GetScoreByScoreId(ScoreSearchViewModel scoreSearchViewModel)
        {
            Score score = new Score();
            score = _unitOfWork.ScoreRepository.Get(x => x.ScoreId == scoreSearchViewModel.ScoreId && x.IsActive == true);
            ScoreDetailsViewModel scoreDetailsViewModel = new ScoreDetailsViewModel()
            {
                //ScoreDate = score.ScoreSubmittedDate.ToShortDateString(),
                // Time = score.ScoreSubmittedTime.ToString("hh':'mm"),
                ScoreId = score.ScoreId,
                Email = score.Golfer.Email,
                UserName = score.Golfer.Name,
                SubmittedDate = score.ScoreSubmittedDate,
                Time = score.ScoreSubmittedTime,

                CoursePairingName = score.BookingId != null ? (score.Booking.CoursePairingId != null ? (score.Booking.CoursePairing.CourseName1.Value) + (score.Booking.CoursePairing.EndCourseNameId != null ? " - " + score.Booking.CoursePairing.CourseName.Value : "") : "") : "",
                MemberShipId = score.Golfer.ClubMemberId,
                DateOfPlay = score.BookingId != null ? score.Booking.TeeOffDate : new DateTime(),
                PlayTime = score.BookingId != null ? score.Booking.TeeOffSlot : ""

            };

            List<ScoreViewModel> scoreViewModels = new List<ScoreViewModel>();
            long scoreId = score.ScoreId;
            if (score.ScoreHole1 != null && score.ScoreHole1.Count() != 0)
            {
                ScoreViewModel scoreViewModel = new ScoreViewModel()
                {
                    HoleNumber = 1,
                    Par = _unitOfWork.ParStorkeRepository.Get(x => x.HoleNumberId == 1).Par,
                    Storkes = score.ScoreHole1.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault(),
                    HoleName = score.ScoreHole1.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                    ScoreId = scoreId
                };
                if (scoreViewModel.Storkes != 0)
                    scoreViewModels.Add(scoreViewModel);
            }
            if (score.ScoreHole2 != null && score.ScoreHole2.Count() != 0)
            {
                ScoreViewModel scoreViewModel = new ScoreViewModel()
                {
                    HoleNumber = 2,
                    Par = _unitOfWork.ParStorkeRepository.Get(x => x.HoleNumberId == 2).Par,
                    Storkes = score.ScoreHole2.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault(),
                    HoleName = score.ScoreHole2.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                    ScoreId = scoreId
                };
                if (scoreViewModel.Storkes != 0)
                    scoreViewModels.Add(scoreViewModel);
            }
            if (score.ScoreHole3 != null && score.ScoreHole3.Count() != 0)
            {
                ScoreViewModel scoreViewModel = new ScoreViewModel()
                {
                    HoleNumber = 3,
                    Par = _unitOfWork.ParStorkeRepository.Get(x => x.HoleNumberId == 3).Par,
                    Storkes = score.ScoreHole3.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault(),
                    HoleName = score.ScoreHole3.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                    ScoreId = scoreId
                };
                if (scoreViewModel.Storkes != 0)
                    scoreViewModels.Add(scoreViewModel);
            }
            if (score.ScoreHole4 != null && score.ScoreHole4.Count() != 0)
            {
                ScoreViewModel scoreViewModel = new ScoreViewModel()
                {
                    HoleNumber = 4,
                    Par = _unitOfWork.ParStorkeRepository.Get(x => x.HoleNumberId == 4).Par,
                    HoleName = score.ScoreHole4.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                    Storkes = score.ScoreHole4.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault(),
                    ScoreId = scoreId
                };
                if (scoreViewModel.Storkes != 0)
                    scoreViewModels.Add(scoreViewModel);
            }
            if (score.ScoreHole5 != null && score.ScoreHole5.Count() != 0)
            {
                ScoreViewModel scoreViewModel = new ScoreViewModel()
                {
                    HoleNumber = 5,
                    Par = _unitOfWork.ParStorkeRepository.Get(x => x.HoleNumberId == 5).Par,
                    HoleName = score.ScoreHole5.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                    Storkes = score.ScoreHole5.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault(),
                    ScoreId = scoreId
                };
                if (scoreViewModel.Storkes != 0)
                    scoreViewModels.Add(scoreViewModel);
            }
            if (score.ScoreHole6 != null && score.ScoreHole6.Count() != 0)
            {
                ScoreViewModel scoreViewModel = new ScoreViewModel()
                {
                    HoleNumber = 6,
                    Par = _unitOfWork.ParStorkeRepository.Get(x => x.HoleNumberId == 6).Par,
                    HoleName = score.ScoreHole6.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                    Storkes = score.ScoreHole6.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault(),
                    ScoreId = scoreId
                };
                if (scoreViewModel.Storkes != 0)
                    scoreViewModels.Add(scoreViewModel);
            }
            if (score.ScoreHole7 != null && score.ScoreHole7.Count() != 0)
            {
                ScoreViewModel scoreViewModel = new ScoreViewModel()
                {
                    HoleNumber = 7,
                    Par = _unitOfWork.ParStorkeRepository.Get(x => x.HoleNumberId == 7).Par,
                    HoleName = score.ScoreHole7.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                    Storkes = score.ScoreHole7.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault(),
                    ScoreId = scoreId
                };
                if (scoreViewModel.Storkes != 0)
                    scoreViewModels.Add(scoreViewModel);
            }
            if (score.ScoreHole8 != null && score.ScoreHole8.Count() != 0)
            {
                ScoreViewModel scoreViewModel = new ScoreViewModel()
                {
                    HoleNumber = 8,
                    Par = _unitOfWork.ParStorkeRepository.Get(x => x.HoleNumberId == 8).Par,
                    HoleName = score.ScoreHole8.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                    Storkes = score.ScoreHole8.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault(),
                    ScoreId = scoreId
                };
                if (scoreViewModel.Storkes != 0)
                    scoreViewModels.Add(scoreViewModel);
            }
            if (score.ScoreHole9 != null && score.ScoreHole9.Count() != 0)
            {
                ScoreViewModel scoreViewModel = new ScoreViewModel()
                {
                    HoleNumber = 9,
                    Par = _unitOfWork.ParStorkeRepository.Get(x => x.HoleNumberId == 9).Par,
                    HoleName = score.ScoreHole9.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                    Storkes = score.ScoreHole9.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault(),
                    ScoreId = scoreId
                };
                if (scoreViewModel.Storkes != 0)
                    scoreViewModels.Add(scoreViewModel);
            }
            if (score.ScoreHole10 != null && score.ScoreHole10.Count() != 0)
            {
                ScoreViewModel scoreViewModel = new ScoreViewModel()
                {
                    HoleNumber = 10,
                    Par = _unitOfWork.ParStorkeRepository.Get(x => x.HoleNumberId == 10).Par,
                    HoleName = score.ScoreHole10.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                    Storkes = score.ScoreHole10.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault(),
                    ScoreId = scoreId
                };
                if (scoreViewModel.Storkes != 0)
                    scoreViewModels.Add(scoreViewModel);
            }
            if (score.ScoreHole11 != null && score.ScoreHole11.Count() != 0)
            {
                ScoreViewModel scoreViewModel = new ScoreViewModel()
                {
                    HoleNumber = 11,
                    Par = _unitOfWork.ParStorkeRepository.Get(x => x.HoleNumberId == 11).Par,
                    HoleName = score.ScoreHole11.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                    Storkes = score.ScoreHole11.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault(),
                    ScoreId = scoreId
                };
                if (scoreViewModel.Storkes != 0)
                    scoreViewModels.Add(scoreViewModel);
            }
            if (score.ScoreHole12 != null && score.ScoreHole12.Count() != 0)
            {
                ScoreViewModel scoreViewModel = new ScoreViewModel()
                {
                    HoleNumber = 12,
                    Par = _unitOfWork.ParStorkeRepository.Get(x => x.HoleNumberId == 12).Par,
                    HoleName = score.ScoreHole12.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                    Storkes = score.ScoreHole12.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault(),
                    ScoreId = scoreId
                };
                if (scoreViewModel.Storkes != 0)
                    scoreViewModels.Add(scoreViewModel);
            }
            if (score.ScoreHole13 != null && score.ScoreHole13.Count() != 0)
            {
                ScoreViewModel scoreViewModel = new ScoreViewModel()
                {
                    HoleNumber = 13,
                    Par = _unitOfWork.ParStorkeRepository.Get(x => x.HoleNumberId == 13).Par,
                    HoleName = score.ScoreHole13.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                    Storkes = score.ScoreHole13.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault(),
                    ScoreId = scoreId
                };
                if (scoreViewModel.Storkes != 0)
                    scoreViewModels.Add(scoreViewModel);
            }
            if (score.ScoreHole14 != null && score.ScoreHole14.Count() != 0)
            {
                ScoreViewModel scoreViewModel = new ScoreViewModel()
                {
                    HoleNumber = 14,
                    Par = _unitOfWork.ParStorkeRepository.Get(x => x.HoleNumberId == 14).Par,
                    HoleName = score.ScoreHole14.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                    Storkes = score.ScoreHole14.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault(),
                    ScoreId = scoreId
                };
                if (scoreViewModel.Storkes != 0)
                    scoreViewModels.Add(scoreViewModel);
            }
            if (score.ScoreHole15 != null && score.ScoreHole15.Count() != 0)
            {
                ScoreViewModel scoreViewModel = new ScoreViewModel()
                {
                    HoleNumber = 15,
                    Par = _unitOfWork.ParStorkeRepository.Get(x => x.HoleNumberId == 15).Par,
                    HoleName = score.ScoreHole15.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                    Storkes = score.ScoreHole15.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault(),
                    ScoreId = scoreId
                };
                if (scoreViewModel.Storkes != 0)
                    scoreViewModels.Add(scoreViewModel);
            }
            if (score.ScoreHole16 != null && score.ScoreHole16.Count() != 0)
            {
                ScoreViewModel scoreViewModel = new ScoreViewModel()
                {
                    HoleNumber = 16,
                    Par = _unitOfWork.ParStorkeRepository.Get(x => x.HoleNumberId == 16).Par,
                    HoleName = score.ScoreHole16.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                    Storkes = score.ScoreHole16.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault(),
                    ScoreId = scoreId
                };
                if (scoreViewModel.Storkes != 0)
                    scoreViewModels.Add(scoreViewModel);
            }
            if (score.ScoreHole17 != null && score.ScoreHole17.Count() != 0)
            {
                ScoreViewModel scoreViewModel = new ScoreViewModel()
                {
                    HoleNumber = 17,
                    Par = _unitOfWork.ParStorkeRepository.Get(x => x.HoleNumberId == 17).Par,
                    HoleName = score.ScoreHole17.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                    Storkes = score.ScoreHole17.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault(),
                    ScoreId = scoreId
                };
                if (scoreViewModel.Storkes != 0)
                    scoreViewModels.Add(scoreViewModel);
            }
            if (score.ScoreHole18 != null && score.ScoreHole18.Count() != 0)
            {
                ScoreViewModel scoreViewModel = new ScoreViewModel()
                {
                    HoleNumber = 18,
                    Par = _unitOfWork.ParStorkeRepository.Get(x => x.HoleNumberId == 18).Par,
                    HoleName = score.ScoreHole18.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                    Storkes = score.ScoreHole18.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault(),
                    ScoreId = scoreId
                };
                if (scoreViewModel.Storkes != 0)
                    scoreViewModels.Add(scoreViewModel);
            }
            if (score.ScoreHole19 != null && score.ScoreHole19.Count() != 0)
            {
                ScoreViewModel scoreViewModel = new ScoreViewModel()
                {
                    HoleNumber = 19,
                    Par = _unitOfWork.ParStorkeRepository.Get(x => x.HoleNumberId == 19).Par,
                    HoleName = score.ScoreHole19.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                    Storkes = score.ScoreHole19.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault()
                };
                if (scoreViewModel.Storkes != 0)
                    scoreViewModels.Add(scoreViewModel);
            }
            if (score.ScoreHole20 != null && score.ScoreHole20.Count() != 0)
            {
                ScoreViewModel scoreViewModel = new ScoreViewModel()
                {
                    HoleNumber = 20,
                    Par = _unitOfWork.ParStorkeRepository.Get(x => x.HoleNumberId == 20).Par,
                    HoleName = score.ScoreHole20.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                    Storkes = score.ScoreHole20.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault()
                };
                if (scoreViewModel.Storkes != 0)
                    scoreViewModels.Add(scoreViewModel);
            }
            if (score.ScoreHole21 != null && score.ScoreHole21.Count() != 0)
            {
                ScoreViewModel scoreViewModel = new ScoreViewModel()
                {
                    HoleNumber = 21,
                    Par = _unitOfWork.ParStorkeRepository.Get(x => x.HoleNumberId == 21).Par,
                    HoleName = score.ScoreHole21.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                    Storkes = score.ScoreHole21.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault()
                };
                if (scoreViewModel.Storkes != 0)
                    scoreViewModels.Add(scoreViewModel);
            }
            if (score.ScoreHole22 != null && score.ScoreHole22.Count() != 0)
            {
                ScoreViewModel scoreViewModel = new ScoreViewModel()
                {
                    HoleNumber = 22,
                    Par = _unitOfWork.ParStorkeRepository.Get(x => x.HoleNumberId == 22).Par,
                    HoleName = score.ScoreHole22.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                    Storkes = score.ScoreHole22.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault()
                };
                if (scoreViewModel.Storkes != 0)
                    scoreViewModels.Add(scoreViewModel);
            }
            if (score.ScoreHole23 != null && score.ScoreHole23.Count() != 0)
            {
                ScoreViewModel scoreViewModel = new ScoreViewModel()
                {
                    HoleNumber = 23,
                    Par = _unitOfWork.ParStorkeRepository.Get(x => x.HoleNumberId == 23).Par,
                    HoleName = score.ScoreHole23.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                    Storkes = score.ScoreHole23.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault()
                };
                if (scoreViewModel.Storkes != 0)
                    scoreViewModels.Add(scoreViewModel);
            }
            if (score.ScoreHole24 != null && score.ScoreHole24.Count() != 0)
            {
                ScoreViewModel scoreViewModel = new ScoreViewModel()
                {
                    HoleNumber = 24,
                    Par = _unitOfWork.ParStorkeRepository.Get(x => x.HoleNumberId == 24).Par,
                    HoleName = score.ScoreHole24.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                    Storkes = score.ScoreHole24.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault()
                };
                if (scoreViewModel.Storkes != 0)
                    scoreViewModels.Add(scoreViewModel);
            }
            if (score.ScoreHole25 != null && score.ScoreHole25.Count() != 0)
            {
                ScoreViewModel scoreViewModel = new ScoreViewModel()
                {
                    HoleNumber = 25,
                    Par = _unitOfWork.ParStorkeRepository.Get(x => x.HoleNumberId == 25).Par,
                    HoleName = score.ScoreHole25.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                    Storkes = score.ScoreHole25.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault()
                };
                if (scoreViewModel.Storkes != 0)
                    scoreViewModels.Add(scoreViewModel);
            }
            if (score.ScoreHole26 != null && score.ScoreHole26.Count() != 0)
            {
                ScoreViewModel scoreViewModel = new ScoreViewModel()
                {
                    HoleNumber = 26,
                    Par = _unitOfWork.ParStorkeRepository.Get(x => x.HoleNumberId == 26).Par,
                    HoleName = score.ScoreHole26.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                    Storkes = score.ScoreHole26.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault()
                };
                if (scoreViewModel.Storkes != 0)
                    scoreViewModels.Add(scoreViewModel);
            }
            if (score.ScoreHole27 != null && score.ScoreHole27.Count() != 0)
            {
                ScoreViewModel scoreViewModel = new ScoreViewModel()
                {
                    HoleNumber = 27,
                    Par = _unitOfWork.ParStorkeRepository.Get(x => x.HoleNumberId == 27).Par,
                    HoleName = score.ScoreHole27.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                    Storkes = score.ScoreHole27.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault()
                };
                if (scoreViewModel.Storkes != 0)
                    scoreViewModels.Add(scoreViewModel);
            }

            scoreDetailsViewModel.ScoreViewModels = scoreViewModels;
            return scoreDetailsViewModel;
        }


        public bool UpdateScoreDetails(ScoreDetailsViewModel scoreDetailsViewModel,long uniqueSessionId)
        {
            Score scores = _unitOfWork.ScoreRepository.Get(x => x.ScoreId == scoreDetailsViewModel.ScoreId && x.IsActive == true);
            foreach (ScoreViewModel scoreHole in scoreDetailsViewModel.ScoreViewModels)
            {
                scoreHole.ScoreId = scores.ScoreId;
                if (scoreHole.HoleNumber == 1)
                {
                    ScoreHole1 scoreHole1 = _unitOfWork.ScoreHole1Repository.Get(x => x.ScoreId == scoreHole.ScoreId);
                    if (scoreHole1 != null)
                    { //Update

                        _unitOfWork.ScoreHole1Repository.Update(HoleScore(scoreHole, scoreHole1));
                    }
                    else
                    {
                        //Add
                        if (scoreHole.HoleNumber == 1 && scoreHole.Storkes != 0)
                        {
                            _unitOfWork.ScoreHole1Repository.Insert(HoleScore(scoreHole, new ScoreHole1()));
                        }
                    }
                }

                if (scoreHole.HoleNumber == 2)
                {

                    ScoreHole2 scoreHole2 = _unitOfWork.ScoreHole2Repository.Get(x => x.ScoreId == scoreHole.ScoreId);
                    if (scoreHole2 != null)
                    { //Update

                        _unitOfWork.ScoreHole2Repository.Update(HoleScore(scoreHole, scoreHole2));
                    }

                    else
                    {
                        //Add
                        if (scoreHole.HoleNumber == 2 && scoreHole.Storkes != 0)
                        {
                            _unitOfWork.ScoreHole2Repository.Insert(HoleScore(scoreHole, new ScoreHole2()));
                        }
                    }

                }


                if (scoreHole.HoleNumber == 3)
                {

                    ScoreHole3 scoreHole3 = _unitOfWork.ScoreHole3Repository.Get(x => x.ScoreId == scoreHole.ScoreId);
                    if (scoreHole3 != null)
                    { //Update

                        _unitOfWork.ScoreHole3Repository.Update(HoleScore(scoreHole, scoreHole3));
                    }

                    else
                    {
                        //Add
                        //Add
                        if (scoreHole.HoleNumber == 3 && scoreHole.Storkes != 0)
                        {
                            _unitOfWork.ScoreHole3Repository.Insert(HoleScore(scoreHole, new ScoreHole3()));
                        }
                    }

                }
                if (scoreHole.HoleNumber == 4)
                {

                    ScoreHole4 scoreHole4 = _unitOfWork.ScoreHole4Repository.Get(x => x.ScoreId == scoreHole.ScoreId);
                    if (scoreHole4 != null)
                    { //Update

                        _unitOfWork.ScoreHole4Repository.Update(HoleScore(scoreHole, scoreHole4));
                    }

                    else
                    {
                        //Add
                        if (scoreHole.HoleNumber == 4 && scoreHole.Storkes != 0)
                        {
                            _unitOfWork.ScoreHole4Repository.Insert(HoleScore(scoreHole, new ScoreHole4()));
                        }
                    }

                }
                if (scoreHole.HoleNumber == 5)
                {

                    ScoreHole5 scoreHole5 = _unitOfWork.ScoreHole5Repository.Get(x => x.ScoreId == scoreHole.ScoreId);
                    if (scoreHole5 != null)
                    { //Update

                        _unitOfWork.ScoreHole5Repository.Update(HoleScore(scoreHole, scoreHole5));
                    }

                    else
                    {
                        //Add
                        if (scoreHole.HoleNumber == 5 && scoreHole.Storkes != 0)
                        {
                            _unitOfWork.ScoreHole5Repository.Insert(HoleScore(scoreHole, new ScoreHole5()));
                        }
                    }

                }
                if (scoreHole.HoleNumber == 6)
                {

                    ScoreHole6 scoreHole6 = _unitOfWork.ScoreHole6Repository.Get(x => x.ScoreId == scoreHole.ScoreId);
                    if (scoreHole6 != null)
                    { //Update

                        _unitOfWork.ScoreHole6Repository.Update(HoleScore(scoreHole, scoreHole6));
                    }

                    else
                    {
                        //Add
                        if (scoreHole.HoleNumber == 6 && scoreHole.Storkes != 0)
                        {
                            _unitOfWork.ScoreHole6Repository.Insert(HoleScore(scoreHole, new ScoreHole6()));
                        }
                    }

                }
                if (scoreHole.HoleNumber == 7)
                {

                    ScoreHole7 scoreHole7 = _unitOfWork.ScoreHole7Repository.Get(x => x.ScoreId == scoreHole.ScoreId);
                    if (scoreHole7 != null)
                    { //Update

                        _unitOfWork.ScoreHole7Repository.Update(HoleScore(scoreHole, scoreHole7));
                    }

                    else
                    {
                        //Add
                        if (scoreHole.HoleNumber == 7 && scoreHole.Storkes != 0)
                        {
                            _unitOfWork.ScoreHole7Repository.Insert(HoleScore(scoreHole, new ScoreHole7()));
                        }
                    }

                }
                if (scoreHole.HoleNumber == 8)
                {

                    ScoreHole8 scoreHole8 = _unitOfWork.ScoreHole8Repository.Get(x => x.ScoreId == scoreHole.ScoreId);
                    if (scoreHole8 != null)
                    { //Update

                        _unitOfWork.ScoreHole8Repository.Update(HoleScore(scoreHole, scoreHole8));
                    }

                    else
                    {
                        //Add
                        if (scoreHole.HoleNumber == 8 && scoreHole.Storkes != 0)
                        {
                            _unitOfWork.ScoreHole8Repository.Insert(HoleScore(scoreHole, new ScoreHole8()));
                        }
                    }

                }
                if (scoreHole.HoleNumber == 9)
                {

                    ScoreHole9 scoreHole9 = _unitOfWork.ScoreHole9Repository.Get(x => x.ScoreId == scoreHole.ScoreId);
                    if (scoreHole9 != null)
                    { //Update

                        _unitOfWork.ScoreHole9Repository.Update(HoleScore(scoreHole, scoreHole9));
                    }

                    else
                    {
                        //Add
                        if (scoreHole.HoleNumber == 9 && scoreHole.Storkes != 0)
                        {
                            _unitOfWork.ScoreHole9Repository.Insert(HoleScore(scoreHole, new ScoreHole9()));
                        }
                    }

                }
                if (scoreHole.HoleNumber == 10)
                {

                    ScoreHole10 scoreHole10 = _unitOfWork.ScoreHole10Repository.Get(x => x.ScoreId == scoreHole.ScoreId);
                    if (scoreHole10 != null)
                    { //Update

                        _unitOfWork.ScoreHole10Repository.Update(HoleScore(scoreHole, scoreHole10));
                    }

                    else
                    {
                        //Add
                        if (scoreHole.HoleNumber == 10 && scoreHole.Storkes != 0)
                        {
                            _unitOfWork.ScoreHole10Repository.Insert(HoleScore(scoreHole, new ScoreHole10()));
                        }
                    }
                }
                if (scoreHole.HoleNumber == 11)
                {

                    ScoreHole11 scoreHole11 = _unitOfWork.ScoreHole11Repository.Get(x => x.ScoreId == scoreHole.ScoreId);
                    if (scoreHole11 != null)
                    { //Update

                        _unitOfWork.ScoreHole11Repository.Update(HoleScore(scoreHole, scoreHole11));
                    }

                    else
                    {
                        //Add
                        if (scoreHole.HoleNumber == 11 && scoreHole.Storkes != 0)
                        {
                            _unitOfWork.ScoreHole11Repository.Insert(HoleScore(scoreHole, new ScoreHole11()));
                        }
                    }
                }

                if (scoreHole.HoleNumber == 12)
                {

                    ScoreHole12 scoreHole12 = _unitOfWork.ScoreHole12Repository.Get(x => x.ScoreId == scoreHole.ScoreId);
                    if (scoreHole12 != null)
                    { //Update

                        _unitOfWork.ScoreHole12Repository.Update(HoleScore(scoreHole, scoreHole12));
                    }

                    else
                    {
                        //Add
                        if (scoreHole.HoleNumber == 12 && scoreHole.Storkes != 0)
                        {
                            _unitOfWork.ScoreHole12Repository.Insert(HoleScore(scoreHole, new ScoreHole12()));
                        }
                    }

                }
                if (scoreHole.HoleNumber == 13)
                {
                    ScoreHole13 scoreHole13 = _unitOfWork.ScoreHole13Repository.Get(x => x.ScoreId == scoreHole.ScoreId);
                    if (scoreHole13 != null)
                    { //Update

                        _unitOfWork.ScoreHole13Repository.Update(HoleScore(scoreHole, scoreHole13));
                    }

                    else
                    {
                        //Add
                        if (scoreHole.HoleNumber == 13 && scoreHole.Storkes != 0)
                        {
                            _unitOfWork.ScoreHole13Repository.Insert(HoleScore(scoreHole, new ScoreHole13()));
                        }
                    }

                }
                if (scoreHole.HoleNumber == 14)
                {

                    ScoreHole14 scoreHole14 = _unitOfWork.ScoreHole14Repository.Get(x => x.ScoreId == scoreHole.ScoreId);
                    if (scoreHole14 != null)
                    { //Update

                        _unitOfWork.ScoreHole14Repository.Update(HoleScore(scoreHole, scoreHole14));
                    }

                    else
                    {
                        //Add
                        if (scoreHole.HoleNumber == 14 && scoreHole.Storkes != 0)
                        {
                            _unitOfWork.ScoreHole14Repository.Insert(HoleScore(scoreHole, new ScoreHole14()));
                        }
                    }

                }
                if (scoreHole.HoleNumber == 15)
                {

                    ScoreHole15 scoreHole15 = _unitOfWork.ScoreHole15Repository.Get(x => x.ScoreId == scoreHole.ScoreId);
                    if (scoreHole15 != null)
                    { //Update

                        _unitOfWork.ScoreHole15Repository.Update(HoleScore(scoreHole, scoreHole15));
                    }

                    else
                    {
                        //Add
                        if (scoreHole.HoleNumber == 15 && scoreHole.Storkes != 0)
                        {
                            _unitOfWork.ScoreHole15Repository.Insert(HoleScore(scoreHole, new ScoreHole15()));
                        }
                    }

                }
                if (scoreHole.HoleNumber == 16)
                {

                    ScoreHole16 scoreHole16 = _unitOfWork.ScoreHole16Repository.Get(x => x.ScoreId == scoreHole.ScoreId);
                    if (scoreHole16 != null)
                    { //Update

                        _unitOfWork.ScoreHole16Repository.Update(HoleScore(scoreHole, scoreHole16));
                    }

                    else
                    {
                        //Add
                        if (scoreHole.HoleNumber == 16 && scoreHole.Storkes != 0)
                        {
                            _unitOfWork.ScoreHole16Repository.Insert(HoleScore(scoreHole, new ScoreHole16()));
                        }
                    }

                }

                if (scoreHole.HoleNumber == 17)
                {
                    ScoreHole17 scoreHole17 = _unitOfWork.ScoreHole17Repository.Get(x => x.ScoreId == scoreHole.ScoreId);
                    if (scoreHole17 != null)
                    { //Update

                        _unitOfWork.ScoreHole17Repository.Update(HoleScore(scoreHole, scoreHole17));
                    }

                    else
                    {
                        //Add
                        if (scoreHole.HoleNumber == 17 && scoreHole.Storkes != 0)
                        {
                            _unitOfWork.ScoreHole17Repository.Insert(HoleScore(scoreHole, new ScoreHole17()));
                        }
                    }

                }

                if (scoreHole.HoleNumber == 18)
                {

                    ScoreHole18 scoreHole18 = _unitOfWork.ScoreHole18Repository.Get(x => x.ScoreId == scoreHole.ScoreId);
                    if (scoreHole18 != null)
                    { //Update

                        _unitOfWork.ScoreHole18Repository.Update(HoleScore(scoreHole, scoreHole18));
                    }

                    else
                    {
                        //Add
                        if (scoreHole.HoleNumber == 18 && scoreHole.Storkes != 0)
                        {
                            _unitOfWork.ScoreHole18Repository.Insert(HoleScore(scoreHole, new ScoreHole18()));
                        }
                    }
                }
            }

            _unitOfWork.Save();

            try
            {

                SessionActivityPageViewModel sessionActivityPageViewModel = new SessionActivityPageViewModel()
                {
                    ControllerName = "Update Score",
                    ActionName = "Save",
                    PerformOn = scoreDetailsViewModel.ScoreId.ToString(),
                    LoginHistoryId = uniqueSessionId,

                    Info = "Update a Score with id- " + scoreDetailsViewModel.ScoreId.ToString()
                };
                new Common.AddSessionActivity().SaveSessionActivity(sessionActivityPageViewModel);

            }
            catch (Exception)
            {

            }
            return true;
        }






        public ScoreDetailsViewModel GetScoreCardDataForScorePost(long scoreId)
        {
            Score score = _unitOfWork.ScoreRepository.Get(x => x.ScoreId == scoreId && x.IsActive == true);
            long bookingId = score.BookingId.GetValueOrDefault();
            ScoreDetailsViewModel scoreDetailsViewModel = new ScoreDetailsViewModel();
            List<ScoreViewModel> holeViewList = new List<ScoreViewModel>();

            Booking booking = _unitOfWork.BookingRepository.Get(x => x.BookingId == bookingId);
            CoursePairing coursePairing = _unitOfWork.CoursePairingRepository.Get(x => x.CoursePairingId == booking.CoursePairingId);
            //    scoreDetailsViewModel.BookingId = bookingId;

            if (coursePairing.HoleTypeId == (int)Core.Helper.Enum.EnumHoleType.Hole9)
            {
                scoreDetailsViewModel.CoursePairingName = coursePairing.CourseName1.Value;
                List<HoleInformation> holeInformations = _unitOfWork.HoleInformationRepository.GetMany(x => x.IsActive == true && x.CourseNameId == coursePairing.StartCourseNameId).ToList();
                int count = 1;
                foreach (var holeInfo in holeInformations.OrderBy(x => x.HoleNumberId))
                {
                    ScoreViewModel scoreViewModel = new ScoreViewModel
                    {
                        HoleNumber = count
                    };
                    ParStorke parStorke = _unitOfWork.ParStorkeRepository.Get(x => x.IsActive == true && x.HoleNumber.Value == holeInfo.HoleNumber.Value);

                    if (parStorke != null)
                    {
                        scoreViewModel.HoleNumber = count;
                        scoreViewModel.Par = parStorke.Par;
                        scoreViewModel.StorkeIndex = parStorke.Storke;
                        scoreViewModel.Storkes = GetStrokesValue(score, count, scoreId);
                        scoreViewModel.Sand = GetSandValue(score, count, scoreId);
                        scoreViewModel.Drive = GetDriveValue(score, count, scoreId);
                        scoreViewModel.Putts = GetPuttValue(score, count, scoreId);
                    }
                    if (holeInfo != null)
                    {
                        scoreViewModel.HoleName = coursePairing.CourseName1.Value;
                    }
                    holeViewList.Add(scoreViewModel);
                    count++;
                }
                scoreDetailsViewModel.ScoreViewModels = holeViewList;
                return scoreDetailsViewModel;

            }
            else if (coursePairing.HoleTypeId == (int)Core.Helper.Enum.EnumHoleType.Hole18)
            {
                scoreDetailsViewModel.CoursePairingName = coursePairing.CourseName1.Value + " - " + coursePairing.CourseName.Value;
                List<HoleInformation> holeInformations = _unitOfWork.HoleInformationRepository.GetMany(x => x.IsActive == true && x.CourseNameId == coursePairing.StartCourseNameId).ToList();
                int count = 1;
                foreach (var holeInfo in holeInformations.OrderBy(x => x.HoleNumberId))
                {
                    ScoreViewModel scoreViewModel = new ScoreViewModel
                    {
                        HoleNumber = count
                    };
                    ParStorke parStorke = _unitOfWork.ParStorkeRepository.Get(x => x.IsActive == true && x.HoleNumber.Value == holeInfo.HoleNumber.Value);

                    if (parStorke != null)
                    {
                        scoreViewModel.HoleNumber = count;
                        scoreViewModel.Par = parStorke.Par;
                        scoreViewModel.StorkeIndex = parStorke.Storke;
                        scoreViewModel.Storkes = GetStrokesValue(score, count, scoreId);
                        scoreViewModel.Sand = GetSandValue(score, count, scoreId);
                        scoreViewModel.Drive = GetDriveValue(score, count, scoreId);
                        scoreViewModel.Putts = GetPuttValue(score, count, scoreId);
                    }
                    if (holeInfo != null)
                    {
                        scoreViewModel.HoleName = coursePairing.CourseName1.Value;
                    }
                    holeViewList.Add(scoreViewModel);
                    count++;
                }


                List<HoleInformation> holeInformations1 = _unitOfWork.HoleInformationRepository.GetMany(x => x.IsActive == true && x.CourseNameId == coursePairing.EndCourseNameId).ToList();

                foreach (var holeInfo in holeInformations1.OrderBy(x => x.HoleNumberId))
                {
                    ScoreViewModel scoreViewModel = new ScoreViewModel
                    {
                        HoleNumber = count
                    };
                    ParStorke parStorke = _unitOfWork.ParStorkeRepository.Get(x => x.IsActive == true && x.HoleNumber.Value == holeInfo.HoleNumber.Value);

                    if (parStorke != null)
                    {
                        scoreViewModel.HoleNumber = count;
                        scoreViewModel.Par = parStorke.Par;
                        scoreViewModel.StorkeIndex = parStorke.Storke;
                        scoreViewModel.Storkes = GetStrokesValue(score, count, scoreId);
                        scoreViewModel.Sand = GetSandValue(score, count, scoreId);
                        scoreViewModel.Drive = GetDriveValue(score, count, scoreId);
                        scoreViewModel.Putts = GetPuttValue(score, count, scoreId);
                    }
                    if (holeInfo != null)
                    {
                        scoreViewModel.HoleName = coursePairing.CourseName.Value;
                    }
                    holeViewList.Add(scoreViewModel);
                    count++;
                }
                scoreDetailsViewModel.ScoreViewModels = holeViewList;
                return scoreDetailsViewModel;
            }
            else
            {

                return scoreDetailsViewModel;
            }
        }


        public int GetStrokesValue(Score score, int count, long scoreId)
        {

            if (count == 1)
            {
                return score.ScoreHole1.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault();
            }
            if (count == 2)
            {
                return score.ScoreHole2.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault();
            }
            if (count == 3)
            {
                return score.ScoreHole3.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault();
            }
            if (count == 4)
            {
                return score.ScoreHole4.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault();
            }
            if (count == 5)
            {
                return score.ScoreHole4.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault();
            }
            if (count == 6)
            {
                return score.ScoreHole4.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault();
            }
            if (count == 7)
            {
                return score.ScoreHole4.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault();
            }
            if (count == 8)
            {
                return score.ScoreHole4.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault();
            }
            if (count == 9)
            {
                return score.ScoreHole4.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault();
            }
            if (count == 10)
            {
                return score.ScoreHole4.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault();
            }
            if (count == 11)
            {
                return score.ScoreHole4.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault();

            }
            if (count == 12)
            {
                return score.ScoreHole4.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault();
            }
            if (count == 13)
            {
                return score.ScoreHole4.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault();
            }
            if (count == 14)
            {
                return score.ScoreHole4.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault();
            }
            if (count == 15)
            {
                return score.ScoreHole4.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault();
            }
            if (count == 16)
            {
                return score.ScoreHole4.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault();
            }
            if (count == 17)
            {
                return score.ScoreHole4.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault();
            }
            if (count == 18)
            {
                return score.ScoreHole4.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault();
            }

            return 0;
        }


        public int GetDriveValue(Score score, int count, long scoreId)
        {

            if (count == 1)
            {
                return score.ScoreHole1.Where(x => x.ScoreId == scoreId).Select(x => x.Drive).FirstOrDefault().GetValueOrDefault();
            }
            if (count == 2)
            {
                return score.ScoreHole2.Where(x => x.ScoreId == scoreId).Select(x => x.Drive).FirstOrDefault().GetValueOrDefault();
            }
            if (count == 3)
            {
                return score.ScoreHole3.Where(x => x.ScoreId == scoreId).Select(x => x.Drive).FirstOrDefault().GetValueOrDefault();
            }
            if (count == 4)
            {
                return score.ScoreHole4.Where(x => x.ScoreId == scoreId).Select(x => x.Drive).FirstOrDefault().GetValueOrDefault();
            }
            if (count == 5)
            {
                return score.ScoreHole4.Where(x => x.ScoreId == scoreId).Select(x => x.Drive).FirstOrDefault().GetValueOrDefault();
            }
            if (count == 6)
            {
                return score.ScoreHole4.Where(x => x.ScoreId == scoreId).Select(x => x.Drive).FirstOrDefault().GetValueOrDefault();
            }
            if (count == 7)
            {
                return score.ScoreHole4.Where(x => x.ScoreId == scoreId).Select(x => x.Drive).FirstOrDefault().GetValueOrDefault();
            }
            if (count == 8)
            {
                return score.ScoreHole4.Where(x => x.ScoreId == scoreId).Select(x => x.Drive).FirstOrDefault().GetValueOrDefault();
            }
            if (count == 9)
            {
                return score.ScoreHole4.Where(x => x.ScoreId == scoreId).Select(x => x.Drive).FirstOrDefault().GetValueOrDefault();
            }
            if (count == 10)
            {
                return score.ScoreHole4.Where(x => x.ScoreId == scoreId).Select(x => x.Drive).FirstOrDefault().GetValueOrDefault();
            }
            if (count == 11)
            {
                return score.ScoreHole4.Where(x => x.ScoreId == scoreId).Select(x => x.Drive).FirstOrDefault().GetValueOrDefault();

            }
            if (count == 12)
            {
                return score.ScoreHole4.Where(x => x.ScoreId == scoreId).Select(x => x.Drive).FirstOrDefault().GetValueOrDefault();
            }
            if (count == 13)
            {
                return score.ScoreHole4.Where(x => x.ScoreId == scoreId).Select(x => x.Drive).FirstOrDefault().GetValueOrDefault();
            }
            if (count == 14)
            {
                return score.ScoreHole4.Where(x => x.ScoreId == scoreId).Select(x => x.Drive).FirstOrDefault().GetValueOrDefault();
            }
            if (count == 15)
            {
                return score.ScoreHole4.Where(x => x.ScoreId == scoreId).Select(x => x.Drive).FirstOrDefault().GetValueOrDefault();
            }
            if (count == 16)
            {
                return score.ScoreHole4.Where(x => x.ScoreId == scoreId).Select(x => x.Drive).FirstOrDefault().GetValueOrDefault();
            }
            if (count == 17)
            {
                return score.ScoreHole4.Where(x => x.ScoreId == scoreId).Select(x => x.Drive).FirstOrDefault().GetValueOrDefault();
            }
            if (count == 18)
            {
                return score.ScoreHole4.Where(x => x.ScoreId == scoreId).Select(x => x.Drive).FirstOrDefault().GetValueOrDefault();
            }
            return 0;
        }

        public int GetPuttValue(Score score, int count, long scoreId)
        {

            if (count == 1)
            {
                return score.ScoreHole1.Where(x => x.ScoreId == scoreId).Select(x => x.Putts).FirstOrDefault().GetValueOrDefault();
            }
            if (count == 2)
            {
                return score.ScoreHole2.Where(x => x.ScoreId == scoreId).Select(x => x.Putts).FirstOrDefault().GetValueOrDefault();
            }
            if (count == 3)
            {
                return score.ScoreHole3.Where(x => x.ScoreId == scoreId).Select(x => x.Putts).FirstOrDefault().GetValueOrDefault();
            }
            if (count == 4)
            {
                return score.ScoreHole4.Where(x => x.ScoreId == scoreId).Select(x => x.Putts).FirstOrDefault().GetValueOrDefault();
            }

            if (count == 5)
            {
                return score.ScoreHole4.Where(x => x.ScoreId == scoreId).Select(x => x.Putts).FirstOrDefault().GetValueOrDefault();
            }
            if (count == 6)
            {
                return score.ScoreHole4.Where(x => x.ScoreId == scoreId).Select(x => x.Putts).FirstOrDefault().GetValueOrDefault();
            }
            if (count == 7)
            {
                return score.ScoreHole4.Where(x => x.ScoreId == scoreId).Select(x => x.Putts).FirstOrDefault().GetValueOrDefault();
            }
            if (count == 8)
            {
                return score.ScoreHole4.Where(x => x.ScoreId == scoreId).Select(x => x.Putts).FirstOrDefault().GetValueOrDefault();
            }
            if (count == 9)
            {
                return score.ScoreHole4.Where(x => x.ScoreId == scoreId).Select(x => x.Putts).FirstOrDefault().GetValueOrDefault();
            }
            if (count == 10)
            {
                return score.ScoreHole4.Where(x => x.ScoreId == scoreId).Select(x => x.Putts).FirstOrDefault().GetValueOrDefault();
            }
            if (count == 11)
            {
                return score.ScoreHole4.Where(x => x.ScoreId == scoreId).Select(x => x.Putts).FirstOrDefault().GetValueOrDefault();

            }
            if (count == 12)
            {
                return score.ScoreHole4.Where(x => x.ScoreId == scoreId).Select(x => x.Putts).FirstOrDefault().GetValueOrDefault();
            }
            if (count == 13)
            {
                return score.ScoreHole4.Where(x => x.ScoreId == scoreId).Select(x => x.Putts).FirstOrDefault().GetValueOrDefault();
            }
            if (count == 14)
            {
                return score.ScoreHole4.Where(x => x.ScoreId == scoreId).Select(x => x.Putts).FirstOrDefault().GetValueOrDefault();
            }
            if (count == 15)
            {
                return score.ScoreHole4.Where(x => x.ScoreId == scoreId).Select(x => x.Putts).FirstOrDefault().GetValueOrDefault();
            }
            if (count == 16)
            {
                return score.ScoreHole4.Where(x => x.ScoreId == scoreId).Select(x => x.Putts).FirstOrDefault().GetValueOrDefault();
            }
            if (count == 17)
            {
                return score.ScoreHole4.Where(x => x.ScoreId == scoreId).Select(x => x.Putts).FirstOrDefault().GetValueOrDefault();
            }
            if (count == 18)
            {
                return score.ScoreHole4.Where(x => x.ScoreId == scoreId).Select(x => x.Putts).FirstOrDefault().GetValueOrDefault();
            }
            return 0;
        }

        public int GetSandValue(Score score, int count, long scoreId)
        {

            if (count == 1)
            {
                return score.ScoreHole1.Where(x => x.ScoreId == scoreId).Select(x => x.Sand).FirstOrDefault().GetValueOrDefault();
            }
            if (count == 2)
            {
                return score.ScoreHole2.Where(x => x.ScoreId == scoreId).Select(x => x.Sand).FirstOrDefault().GetValueOrDefault();
            }
            if (count == 3)
            {
                return score.ScoreHole3.Where(x => x.ScoreId == scoreId).Select(x => x.Sand).FirstOrDefault().GetValueOrDefault();
            }
            if (count == 4)
            {
                return score.ScoreHole4.Where(x => x.ScoreId == scoreId).Select(x => x.Sand).FirstOrDefault().GetValueOrDefault();
            }

            if (count == 5)
            {
                return score.ScoreHole4.Where(x => x.ScoreId == scoreId).Select(x => x.Sand).FirstOrDefault().GetValueOrDefault();
            }
            if (count == 6)
            {
                return score.ScoreHole4.Where(x => x.ScoreId == scoreId).Select(x => x.Sand).FirstOrDefault().GetValueOrDefault();
            }
            if (count == 7)
            {
                return score.ScoreHole4.Where(x => x.ScoreId == scoreId).Select(x => x.Sand).FirstOrDefault().GetValueOrDefault();
            }
            if (count == 8)
            {
                return score.ScoreHole4.Where(x => x.ScoreId == scoreId).Select(x => x.Sand).FirstOrDefault().GetValueOrDefault();
            }
            if (count == 9)
            {
                return score.ScoreHole4.Where(x => x.ScoreId == scoreId).Select(x => x.Sand).FirstOrDefault().GetValueOrDefault();
            }
            if (count == 10)
            {
                return score.ScoreHole4.Where(x => x.ScoreId == scoreId).Select(x => x.Sand).FirstOrDefault().GetValueOrDefault();
            }
            if (count == 11)
            {
                return score.ScoreHole4.Where(x => x.ScoreId == scoreId).Select(x => x.Sand).FirstOrDefault().GetValueOrDefault();

            }
            if (count == 12)
            {
                return score.ScoreHole4.Where(x => x.ScoreId == scoreId).Select(x => x.Sand).FirstOrDefault().GetValueOrDefault();
            }
            if (count == 13)
            {
                return score.ScoreHole4.Where(x => x.ScoreId == scoreId).Select(x => x.Sand).FirstOrDefault().GetValueOrDefault();
            }
            if (count == 14)
            {
                return score.ScoreHole4.Where(x => x.ScoreId == scoreId).Select(x => x.Sand).FirstOrDefault().GetValueOrDefault();
            }
            if (count == 15)
            {
                return score.ScoreHole4.Where(x => x.ScoreId == scoreId).Select(x => x.Sand).FirstOrDefault().GetValueOrDefault();
            }
            if (count == 16)
            {
                return score.ScoreHole4.Where(x => x.ScoreId == scoreId).Select(x => x.Sand).FirstOrDefault().GetValueOrDefault();
            }
            if (count == 17)
            {
                return score.ScoreHole4.Where(x => x.ScoreId == scoreId).Select(x => x.Sand).FirstOrDefault().GetValueOrDefault();
            }
            if (count == 18)
            {
                return score.ScoreHole4.Where(x => x.ScoreId == scoreId).Select(x => x.Sand).FirstOrDefault().GetValueOrDefault();
            }
            return 0;
        }

        private dynamic HoleScore(ScoreViewModel scoreViewModel, dynamic model)
        {
            model.Putts = scoreViewModel.Putts;
            model.Sand = scoreViewModel.Sand;
            model.Drive = scoreViewModel.Drive;
            model.Storkes = scoreViewModel.Storkes;
            model.Saves = scoreViewModel.Saves;
            model.Chips = scoreViewModel.Chips;
            model.Clubs = scoreViewModel.Clubs;
            model.Penalty = scoreViewModel.Penalty;
            model.CreatedOn = System.DateTime.UtcNow;
            model.IsActive = true;
            model.HoleName = scoreViewModel.HoleName;
            model.Par = scoreViewModel.Par;
            model.ScoreId = scoreViewModel.ScoreId;
            return model;
        }
    }
}
