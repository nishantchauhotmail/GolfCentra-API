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
    public class ScoreService : IScoreService
    {
        private readonly UnitOfWork _unitOfWork;

        public ScoreService()
        {
            _unitOfWork = new UnitOfWork();
        }
        #region Old Code
        /// <summary>
        /// Save Score Details
        /// </summary>
        /// <param name="scoreViewModels"></param>
        /// <param name="golferId"></param>
        /// <param name="grossTotal"></param>
        /// <param name="roundTotal"></param>
        /// <returns></returns>
        public bool SaveScore(List<ScoreViewModel> scoreViewModels, long golferId, long grossTotal, long roundTotal)
        {
            Tuple<long, long> total = CalculateGrossTotal(scoreViewModels);
            if (total.Item1 != roundTotal)
                throw new Exception("Round Total Miscalculated.");

            if (total.Item2 != grossTotal)
                throw new Exception("Gross Total Miscalculated.");
            if (roundTotal == 0)
                throw new Exception("Please Provide Round Total");
            Score score = new Score()
            {
                GolferId = golferId,
                ScoreSubmittedDate = System.DateTime.UtcNow,
                ScoreSubmittedTime = System.DateTime.UtcNow.TimeOfDay,
                HoleTypeId = 2,
                IsActive = true,
                CreatedOn = System.DateTime.UtcNow,
            };

            foreach (ScoreViewModel scoreViewModel in scoreViewModels)
            {
                if (scoreViewModel.HoleNumber == 1 && scoreViewModel.Storkes != 0)
                    score.ScoreHole1.Add(HoleScore(scoreViewModel, new ScoreHole1()));
                if (scoreViewModel.HoleNumber == 2 && scoreViewModel.Storkes != 0)
                    score.ScoreHole2.Add(HoleScore(scoreViewModel, new ScoreHole2()));
                if (scoreViewModel.HoleNumber == 3 && scoreViewModel.Storkes != 0)
                    score.ScoreHole3.Add(HoleScore(scoreViewModel, new ScoreHole3()));
                if (scoreViewModel.HoleNumber == 4 && scoreViewModel.Storkes != 0)
                    score.ScoreHole4.Add(HoleScore(scoreViewModel, new ScoreHole4()));
                if (scoreViewModel.HoleNumber == 5 && scoreViewModel.Storkes != 0)
                    score.ScoreHole5.Add(HoleScore(scoreViewModel, new ScoreHole5()));
                if (scoreViewModel.HoleNumber == 6 && scoreViewModel.Storkes != 0)
                    score.ScoreHole6.Add(HoleScore(scoreViewModel, new ScoreHole6()));
                if (scoreViewModel.HoleNumber == 7 && scoreViewModel.Storkes != 0)
                    score.ScoreHole7.Add(HoleScore(scoreViewModel, new ScoreHole7()));
                if (scoreViewModel.HoleNumber == 8 && scoreViewModel.Storkes != 0)
                    score.ScoreHole8.Add(HoleScore(scoreViewModel, new ScoreHole8()));
                if (scoreViewModel.HoleNumber == 9 && scoreViewModel.Storkes != 0)
                    score.ScoreHole9.Add(HoleScore(scoreViewModel, new ScoreHole9()));
                if (scoreViewModel.HoleNumber == 10 && scoreViewModel.Storkes != 0)
                    score.ScoreHole10.Add(HoleScore(scoreViewModel, new ScoreHole10()));
                if (scoreViewModel.HoleNumber == 11 && scoreViewModel.Storkes != 0)
                    score.ScoreHole11.Add(HoleScore(scoreViewModel, new ScoreHole11()));
                if (scoreViewModel.HoleNumber == 12 && scoreViewModel.Storkes != 0)
                    score.ScoreHole12.Add(HoleScore(scoreViewModel, new ScoreHole12()));
                if (scoreViewModel.HoleNumber == 13 && scoreViewModel.Storkes != 0)
                    score.ScoreHole13.Add(HoleScore(scoreViewModel, new ScoreHole13()));
                if (scoreViewModel.HoleNumber == 14 && scoreViewModel.Storkes != 0)
                    score.ScoreHole14.Add(HoleScore(scoreViewModel, new ScoreHole14()));
                if (scoreViewModel.HoleNumber == 15 && scoreViewModel.Storkes != 0)
                    score.ScoreHole15.Add(HoleScore(scoreViewModel, new ScoreHole15()));
                if (scoreViewModel.HoleNumber == 16 && scoreViewModel.Storkes != 0)
                    score.ScoreHole16.Add(HoleScore(scoreViewModel, new ScoreHole16()));
                if (scoreViewModel.HoleNumber == 17 && scoreViewModel.Storkes != 0)
                    score.ScoreHole17.Add(HoleScore(scoreViewModel, new ScoreHole17()));
                if (scoreViewModel.HoleNumber == 18 && scoreViewModel.Storkes != 0)
                    score.ScoreHole18.Add(HoleScore(scoreViewModel, new ScoreHole18()));
                if (scoreViewModel.HoleNumber == 19 && scoreViewModel.Storkes != 0)
                    score.ScoreHole19.Add(HoleScore(scoreViewModel, new ScoreHole19()));
                if (scoreViewModel.HoleNumber == 20 && scoreViewModel.Storkes != 0)
                    score.ScoreHole20.Add(HoleScore(scoreViewModel, new ScoreHole20()));
                if (scoreViewModel.HoleNumber == 21 && scoreViewModel.Storkes != 0)
                    score.ScoreHole21.Add(HoleScore(scoreViewModel, new ScoreHole21()));
                if (scoreViewModel.HoleNumber == 22 && scoreViewModel.Storkes != 0)
                    score.ScoreHole22.Add(HoleScore(scoreViewModel, new ScoreHole22()));
                if (scoreViewModel.HoleNumber == 23 && scoreViewModel.Storkes != 0)
                    score.ScoreHole23.Add(HoleScore(scoreViewModel, new ScoreHole23()));
                if (scoreViewModel.HoleNumber == 24 && scoreViewModel.Storkes != 0)
                    score.ScoreHole24.Add(HoleScore(scoreViewModel, new ScoreHole24()));
                if (scoreViewModel.HoleNumber == 25 && scoreViewModel.Storkes != 0)
                    score.ScoreHole25.Add(HoleScore(scoreViewModel, new ScoreHole25()));
                if (scoreViewModel.HoleNumber == 26 && scoreViewModel.Storkes != 0)
                    score.ScoreHole26.Add(HoleScore(scoreViewModel, new ScoreHole26()));
                if (scoreViewModel.HoleNumber == 27 && scoreViewModel.Storkes != 0)
                    score.ScoreHole27.Add(HoleScore(scoreViewModel, new ScoreHole27()));
            }
            _unitOfWork.ScoreRepository.Insert(score);
            _unitOfWork.Save();

            return true;
        }


        /// <summary>
        /// Get Score's Details By Golfer Id
        /// </summary>
        /// <param name="golfeId"></param>
        /// <returns></returns>
        public List<ScoreViewModel> GetScoreList(long golfeId)
        {
            List<Score> scores = _unitOfWork.ScoreRepository.GetMany(x => x.IsActive == true && x.GolferId == golfeId).OrderByDescending(x => x.ScoreSubmittedDate).ToList();

            List<ScoreViewModel> scoreViewModels = new List<ScoreViewModel>();

            foreach (Score score in scores)
            {
                ScoreViewModel scoreViewModel = new ScoreViewModel();

                if (score.BookingId == null)
                {
                    //Old
                    scoreViewModel.ScoreDate = score.ScoreSubmittedDate.ToShortDateString();
                    scoreViewModel.Time = Core.Helper.DateHelper.ConvertSystemDateToCurrent(score.ScoreSubmittedDate).ToString("hh':'mm");

//scoreViewModel.ScoreId = score.ScoreId;
                    scoreViewModel.ENS = Core.Helper.Crypto.EncryptStringAES(score.ScoreId.ToString());
                }
                //New
                else
                {
                    scoreViewModel.ScoreDate = score.BookingId != null ? score.Booking.TeeOffDate.ToShortDateString() : "";
                    scoreViewModel.Time = score.BookingId != null ? score.Booking.TeeOffSlot : "";
                   // scoreViewModel.ScoreId = score.ScoreId;
                    scoreViewModel.ENS = Core.Helper.Crypto.EncryptStringAES(score.ScoreId.ToString());
                }

                scoreViewModels.Add(scoreViewModel);
            }

            return scoreViewModels;
        }

        /// <summary>
        /// Get Score Card Details By Score Id
        /// </summary>
        /// <param name="scoreId"></param>
        /// <param name="golferId"></param>
        /// <returns></returns>
        public Tuple<long, long, List<ScoreViewModel>> GetScoreCardDetailsByScoreId(string id, long golferId)
        {
            long scoreId = Convert.ToInt64(Core.Helper.Crypto.DecryptStringAES(id));
            Score scores = _unitOfWork.ScoreRepository.Get(x => x.ScoreId == scoreId && x.GolferId == golferId && x.IsActive == true);
            if (scores == null)
                throw new Exception("No Score Found");
            List<ScoreViewModel> scoreViewModels = new List<ScoreViewModel>();

            if (scores.ScoreHole1 != null && scores.ScoreHole1.Count() != 0)
            {
                ScoreViewModel scoreViewModel = new ScoreViewModel()
                {
                    HoleNumber = 1,
                    Par = scores.ScoreHole1.Where(x => x.ScoreId == scoreId).Select(x => x.Par).FirstOrDefault().GetValueOrDefault(),
                    Storkes = scores.ScoreHole1.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault(),
                    HoleName = scores.ScoreHole1.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                };
                if (scoreViewModel.Storkes != 0)
                    scoreViewModels.Add(scoreViewModel);
            }
            if (scores.ScoreHole2 != null && scores.ScoreHole2.Count() != 0)
            {
                ScoreViewModel scoreViewModel = new ScoreViewModel()
                {
                    HoleNumber = 2,
                    Par = scores.ScoreHole2.Where(x => x.ScoreId == scoreId).Select(x => x.Par).FirstOrDefault().GetValueOrDefault(),
                    Storkes = scores.ScoreHole2.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault(),
                    HoleName = scores.ScoreHole2.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                };
                if (scoreViewModel.Storkes != 0)
                    scoreViewModels.Add(scoreViewModel);
            }
            if (scores.ScoreHole3 != null && scores.ScoreHole3.Count() != 0)
            {
                ScoreViewModel scoreViewModel = new ScoreViewModel()
                {
                    HoleNumber = 3,
                    Par = scores.ScoreHole3.Where(x => x.ScoreId == scoreId).Select(x => x.Par).FirstOrDefault().GetValueOrDefault(),
                    Storkes = scores.ScoreHole3.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault(),
                    HoleName = scores.ScoreHole3.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                };
                if (scoreViewModel.Storkes != 0)
                    scoreViewModels.Add(scoreViewModel);
            }
            if (scores.ScoreHole4 != null && scores.ScoreHole4.Count() != 0)
            {
                ScoreViewModel scoreViewModel = new ScoreViewModel()
                {
                    HoleNumber = 4,
                    Par = scores.ScoreHole4.Where(x => x.ScoreId == scoreId).Select(x => x.Par).FirstOrDefault().GetValueOrDefault(),
                    HoleName = scores.ScoreHole4.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                    Storkes = scores.ScoreHole4.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault()
                };
                if (scoreViewModel.Storkes != 0)
                    scoreViewModels.Add(scoreViewModel);
            }
            if (scores.ScoreHole5 != null && scores.ScoreHole5.Count() != 0)
            {
                ScoreViewModel scoreViewModel = new ScoreViewModel()
                {
                    HoleNumber = 5,
                    Par = scores.ScoreHole5.Where(x => x.ScoreId == scoreId).Select(x => x.Par).FirstOrDefault().GetValueOrDefault(),
                    HoleName = scores.ScoreHole5.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                    Storkes = scores.ScoreHole5.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault()
                };
                if (scoreViewModel.Storkes != 0)
                    scoreViewModels.Add(scoreViewModel);
            }
            if (scores.ScoreHole6 != null && scores.ScoreHole6.Count() != 0)
            {
                ScoreViewModel scoreViewModel = new ScoreViewModel()
                {
                    HoleNumber = 6,
                    Par = scores.ScoreHole6.Where(x => x.ScoreId == scoreId).Select(x => x.Par).FirstOrDefault().GetValueOrDefault(),
                    HoleName = scores.ScoreHole6.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                    Storkes = scores.ScoreHole6.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault()
                };
                if (scoreViewModel.Storkes != 0)
                    scoreViewModels.Add(scoreViewModel);
            }
            if (scores.ScoreHole7 != null && scores.ScoreHole7.Count() != 0)
            {
                ScoreViewModel scoreViewModel = new ScoreViewModel()
                {
                    HoleNumber = 7,
                    Par = scores.ScoreHole7.Where(x => x.ScoreId == scoreId).Select(x => x.Par).FirstOrDefault().GetValueOrDefault(),
                    HoleName = scores.ScoreHole7.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                    Storkes = scores.ScoreHole7.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault()
                };
                if (scoreViewModel.Storkes != 0)
                    scoreViewModels.Add(scoreViewModel);
            }
            if (scores.ScoreHole8 != null && scores.ScoreHole8.Count() != 0)
            {
                ScoreViewModel scoreViewModel = new ScoreViewModel()
                {
                    HoleNumber = 8,
                    Par = scores.ScoreHole8.Where(x => x.ScoreId == scoreId).Select(x => x.Par).FirstOrDefault().GetValueOrDefault(),
                    HoleName = scores.ScoreHole8.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                    Storkes = scores.ScoreHole8.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault()
                };
                if (scoreViewModel.Storkes != 0)
                    scoreViewModels.Add(scoreViewModel);
            }
            if (scores.ScoreHole9 != null && scores.ScoreHole9.Count() != 0)
            {
                ScoreViewModel scoreViewModel = new ScoreViewModel()
                {
                    HoleNumber = 9,
                    Par = scores.ScoreHole9.Where(x => x.ScoreId == scoreId).Select(x => x.Par).FirstOrDefault().GetValueOrDefault(),
                    HoleName = scores.ScoreHole9.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                    Storkes = scores.ScoreHole9.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault()
                };
                if (scoreViewModel.Storkes != 0)
                    scoreViewModels.Add(scoreViewModel);
            }
            if (scores.ScoreHole10 != null && scores.ScoreHole10.Count() != 0)
            {
                ScoreViewModel scoreViewModel = new ScoreViewModel()
                {
                    HoleNumber = 10,
                    Par = scores.ScoreHole10.Where(x => x.ScoreId == scoreId).Select(x => x.Par).FirstOrDefault().GetValueOrDefault(),
                    HoleName = scores.ScoreHole10.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                    Storkes = scores.ScoreHole10.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault()
                };
                if (scoreViewModel.Storkes != 0)
                    scoreViewModels.Add(scoreViewModel);
            }
            if (scores.ScoreHole11 != null && scores.ScoreHole11.Count() != 0)
            {
                ScoreViewModel scoreViewModel = new ScoreViewModel()
                {
                    HoleNumber = 11,
                    Par = scores.ScoreHole11.Where(x => x.ScoreId == scoreId).Select(x => x.Par).FirstOrDefault().GetValueOrDefault(),
                    HoleName = scores.ScoreHole11.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                    Storkes = scores.ScoreHole11.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault()
                };
                if (scoreViewModel.Storkes != 0)
                    scoreViewModels.Add(scoreViewModel);
            }
            if (scores.ScoreHole12 != null && scores.ScoreHole12.Count() != 0)
            {
                ScoreViewModel scoreViewModel = new ScoreViewModel()
                {
                    HoleNumber = 12,
                    Par = scores.ScoreHole12.Where(x => x.ScoreId == scoreId).Select(x => x.Par).FirstOrDefault().GetValueOrDefault(),
                    HoleName = scores.ScoreHole12.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                    Storkes = scores.ScoreHole12.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault()
                };
                if (scoreViewModel.Storkes != 0)
                    scoreViewModels.Add(scoreViewModel);
            }
            if (scores.ScoreHole13 != null && scores.ScoreHole13.Count() != 0)
            {
                ScoreViewModel scoreViewModel = new ScoreViewModel()
                {
                    HoleNumber = 13,
                    Par = scores.ScoreHole13.Where(x => x.ScoreId == scoreId).Select(x => x.Par).FirstOrDefault().GetValueOrDefault(),
                    HoleName = scores.ScoreHole13.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                    Storkes = scores.ScoreHole13.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault()
                };
                if (scoreViewModel.Storkes != 0)
                    scoreViewModels.Add(scoreViewModel);
            }
            if (scores.ScoreHole14 != null && scores.ScoreHole14.Count() != 0)
            {
                ScoreViewModel scoreViewModel = new ScoreViewModel()
                {
                    HoleNumber = 14,
                    Par = scores.ScoreHole14.Where(x => x.ScoreId == scoreId).Select(x => x.Par).FirstOrDefault().GetValueOrDefault(),
                    HoleName = scores.ScoreHole14.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                    Storkes = scores.ScoreHole14.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault()
                };
                if (scoreViewModel.Storkes != 0)
                    scoreViewModels.Add(scoreViewModel);
            }
            if (scores.ScoreHole15 != null && scores.ScoreHole15.Count() != 0)
            {
                ScoreViewModel scoreViewModel = new ScoreViewModel()
                {
                    HoleNumber = 15,
                    Par = scores.ScoreHole15.Where(x => x.ScoreId == scoreId).Select(x => x.Par).FirstOrDefault().GetValueOrDefault(),
                    HoleName = scores.ScoreHole15.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                    Storkes = scores.ScoreHole15.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault()
                };
                if (scoreViewModel.Storkes != 0)
                    scoreViewModels.Add(scoreViewModel);
            }
            if (scores.ScoreHole16 != null && scores.ScoreHole16.Count() != 0)
            {
                ScoreViewModel scoreViewModel = new ScoreViewModel()
                {
                    HoleNumber = 16,
                    Par = scores.ScoreHole16.Where(x => x.ScoreId == scoreId).Select(x => x.Par).FirstOrDefault().GetValueOrDefault(),
                    HoleName = scores.ScoreHole16.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                    Storkes = scores.ScoreHole16.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault()
                };
                if (scoreViewModel.Storkes != 0)
                    scoreViewModels.Add(scoreViewModel);
            }
            if (scores.ScoreHole17 != null && scores.ScoreHole17.Count() != 0)
            {
                ScoreViewModel scoreViewModel = new ScoreViewModel()
                {
                    HoleNumber = 17,
                    Par = scores.ScoreHole17.Where(x => x.ScoreId == scoreId).Select(x => x.Par).FirstOrDefault().GetValueOrDefault(),
                    HoleName = scores.ScoreHole17.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                    Storkes = scores.ScoreHole17.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault()
                };
                if (scoreViewModel.Storkes != 0)
                    scoreViewModels.Add(scoreViewModel);
            }
            if (scores.ScoreHole18 != null && scores.ScoreHole18.Count() != 0)
            {
                ScoreViewModel scoreViewModel = new ScoreViewModel()
                {
                    HoleNumber = 18,
                    Par = scores.ScoreHole18.Where(x => x.ScoreId == scoreId).Select(x => x.Par).FirstOrDefault().GetValueOrDefault(),
                    HoleName = scores.ScoreHole18.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                    Storkes = scores.ScoreHole18.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault()
                };
                if (scoreViewModel.Storkes != 0)
                    scoreViewModels.Add(scoreViewModel);
            }
            if (scores.ScoreHole19 != null && scores.ScoreHole19.Count() != 0)
            {
                ScoreViewModel scoreViewModel = new ScoreViewModel()
                {
                    HoleNumber = 19,
                    Par = scores.ScoreHole19.Where(x => x.ScoreId == scoreId).Select(x => x.Par).FirstOrDefault().GetValueOrDefault(),
                    HoleName = scores.ScoreHole19.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                    Storkes = scores.ScoreHole19.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault()
                };
                if (scoreViewModel.Storkes != 0)
                    scoreViewModels.Add(scoreViewModel);
            }
            if (scores.ScoreHole20 != null && scores.ScoreHole20.Count() != 0)
            {
                ScoreViewModel scoreViewModel = new ScoreViewModel()
                {
                    HoleNumber = 20,
                    Par = scores.ScoreHole20.Where(x => x.ScoreId == scoreId).Select(x => x.Par).FirstOrDefault().GetValueOrDefault(),
                    HoleName = scores.ScoreHole20.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                    Storkes = scores.ScoreHole20.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault()
                };
                if (scoreViewModel.Storkes != 0)
                    scoreViewModels.Add(scoreViewModel);
            }
            if (scores.ScoreHole21 != null && scores.ScoreHole21.Count() != 0)
            {
                ScoreViewModel scoreViewModel = new ScoreViewModel()
                {
                    HoleNumber = 21,
                    Par = scores.ScoreHole21.Where(x => x.ScoreId == scoreId).Select(x => x.Par).FirstOrDefault().GetValueOrDefault(),
                    HoleName = scores.ScoreHole21.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                    Storkes = scores.ScoreHole21.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault()
                };
                if (scoreViewModel.Storkes != 0)
                    scoreViewModels.Add(scoreViewModel);
            }
            if (scores.ScoreHole22 != null && scores.ScoreHole22.Count() != 0)
            {
                ScoreViewModel scoreViewModel = new ScoreViewModel()
                {
                    HoleNumber = 22,
                    Par = scores.ScoreHole22.Where(x => x.ScoreId == scoreId).Select(x => x.Par).FirstOrDefault().GetValueOrDefault(),
                    HoleName = scores.ScoreHole22.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                    Storkes = scores.ScoreHole22.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault()
                };
                if (scoreViewModel.Storkes != 0)
                    scoreViewModels.Add(scoreViewModel);
            }
            if (scores.ScoreHole23 != null && scores.ScoreHole23.Count() != 0)
            {
                ScoreViewModel scoreViewModel = new ScoreViewModel()
                {
                    HoleNumber = 23,
                    Par = scores.ScoreHole23.Where(x => x.ScoreId == scoreId).Select(x => x.Par).FirstOrDefault().GetValueOrDefault(),
                    HoleName = scores.ScoreHole23.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                    Storkes = scores.ScoreHole23.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault()
                };
                if (scoreViewModel.Storkes != 0)
                    scoreViewModels.Add(scoreViewModel);
            }
            if (scores.ScoreHole24 != null && scores.ScoreHole24.Count() != 0)
            {
                ScoreViewModel scoreViewModel = new ScoreViewModel()
                {
                    HoleNumber = 24,
                    Par = scores.ScoreHole24.Where(x => x.ScoreId == scoreId).Select(x => x.Par).FirstOrDefault().GetValueOrDefault(),
                    HoleName = scores.ScoreHole24.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                    Storkes = scores.ScoreHole24.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault()
                };
                if (scoreViewModel.Storkes != 0)
                    scoreViewModels.Add(scoreViewModel);
            }
            if (scores.ScoreHole25 != null && scores.ScoreHole25.Count() != 0)
            {
                ScoreViewModel scoreViewModel = new ScoreViewModel()
                {
                    HoleNumber = 25,
                    Par = scores.ScoreHole25.Where(x => x.ScoreId == scoreId).Select(x => x.Par).FirstOrDefault().GetValueOrDefault(),
                    HoleName = scores.ScoreHole25.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                    Storkes = scores.ScoreHole25.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault()
                };
                if (scoreViewModel.Storkes != 0)
                    scoreViewModels.Add(scoreViewModel);
            }
            if (scores.ScoreHole26 != null && scores.ScoreHole26.Count() != 0)
            {
                ScoreViewModel scoreViewModel = new ScoreViewModel()
                {
                    HoleNumber = 26,
                    Par = scores.ScoreHole26.Where(x => x.ScoreId == scoreId).Select(x => x.Par).FirstOrDefault().GetValueOrDefault(),
                    HoleName = scores.ScoreHole26.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                    Storkes = scores.ScoreHole26.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault()
                };
                if (scoreViewModel.Storkes != 0)
                    scoreViewModels.Add(scoreViewModel);
            }
            if (scores.ScoreHole27 != null && scores.ScoreHole27.Count() != 0)
            {
                ScoreViewModel scoreViewModel = new ScoreViewModel()
                {
                    HoleNumber = 27,
                    Par = scores.ScoreHole27.Where(x => x.ScoreId == scoreId).Select(x => x.Par).FirstOrDefault().GetValueOrDefault(),
                    HoleName = scores.ScoreHole27.Where(x => x.ScoreId == scoreId).Select(x => x.HoleName).FirstOrDefault() ?? "",
                    Storkes = scores.ScoreHole27.Where(x => x.ScoreId == scoreId).Select(x => x.Storkes).FirstOrDefault().GetValueOrDefault()
                };
                if (scoreViewModel.Storkes != 0)
                    scoreViewModels.Add(scoreViewModel);
            }

            Tuple<long, long> ScoreCal = CalculateGrossTotal(scoreViewModels);
            return new Tuple<long, long, List<ScoreViewModel>>(ScoreCal.Item1, ScoreCal.Item2, scoreViewModels);
        }


        #endregion
        #region Common Code

        /// <summary>
        /// Convert ScoreViewModel To Database Score[1 to 27] Model
        /// </summary>
        /// <param name="scoreViewModel"></param>
        /// <param name="model"></param>
        /// <returns></returns>
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
            return model;
        }


        /// <summary>
        /// Calculate Round Total And Gross Total
        /// </summary>
        /// <param name="scoreViewModels"></param>
        /// <returns></returns>
        private Tuple<long, long> CalculateGrossTotal(List<ScoreViewModel> scoreViewModels)
        {
            long grossScore = 0, par = 0, total = 0;
            foreach (ScoreViewModel score in scoreViewModels)
            {
                if (score.Storkes != 0)
                {
                    par += score.Par;
                    total += score.Storkes.GetValueOrDefault();
                }
            }
            grossScore = total - par;
            return new Tuple<long, long>(total, grossScore);
        }

        #endregion
        #region New Code
        public ScoreDetailsViewModel GetScoreCardDataForScorePost(string bookingId)
        {
            long id = Convert.ToInt64(Core.Helper.Crypto.DecryptStringAES(bookingId));
            ScoreDetailsViewModel scoreDetailsViewModel = new ScoreDetailsViewModel();
            List<ScoreViewModel> holeViewList = new List<ScoreViewModel>();
            scoreDetailsViewModel.ENB = bookingId;
            Booking booking = _unitOfWork.BookingRepository.Get(x => x.BookingId == id);
            CoursePairing coursePairing = _unitOfWork.CoursePairingRepository.Get(x => x.CoursePairingId == booking.CoursePairingId);
            scoreDetailsViewModel.BookingId = id;
           
            if (coursePairing.HoleTypeId == (int)Core.Helper.Enum.EnumHoleType.Hole9)
            {
                scoreDetailsViewModel.CoursePairingName = coursePairing.CourseName1.Value;
               List <HoleInformation> holeInformations = _unitOfWork.HoleInformationRepository.GetMany(x => x.IsActive == true && x.CourseNameId == coursePairing.StartCourseNameId).ToList();
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
                scoreDetailsViewModel.CoursePairingName = coursePairing.CourseName1.Value +" - " + coursePairing.CourseName.Value;
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

        public bool SaveScore(List<ScoreViewModel> scoreViewModels, long golferId, long grossTotal, long roundTotal, string id)
        {
            long bookingId = Convert.ToInt64(Core.Helper.Crypto.DecryptStringAES(id));
            Tuple<long, long> total = CalculateGrossTotal(scoreViewModels);
            if (total.Item1 != roundTotal)
                throw new Exception("Round Total Miscalculated.");

            if (total.Item2 != grossTotal)
                throw new Exception("Gross Total Miscalculated.");
            if (roundTotal == 0)
                throw new Exception("Please Provide Round Total");

            Golfer golfer = _unitOfWork.GolferRepository.Get(x => x.GolferId == golferId);
            Booking booking = _unitOfWork.BookingRepository.Get(x => x.BookingId == bookingId);
            if (booking.GolferId == golferId)
            {
                if (booking.ScoreId != null)
                {
                    throw new Exception("Already Score Posted.");
                }
            }
            else
            {

                foreach (var item in booking.BookingPlayerDetails)
                {
                    if (item.Player2 == golfer.ClubMemberId)
                    {
                        if (item.Player2ScoreId != null)
                        {
                            throw new Exception("Already Score Posted.");
                        }

                    }
                    if (item.Player3 == golfer.ClubMemberId)
                    {
                        if (item.Player3ScoreId != null)
                        {
                            throw new Exception("Already Score Posted.");
                        }
                    }
                    if (item.Player4 == golfer.ClubMemberId)
                    {
                        if (item.Player4ScoreId != null)
                            if (item.Player4ScoreId != null)
                            {
                                throw new Exception("Already Score Posted.");
                            }
                    }
                }


            }

            Score score = new Score()
            {
                GolferId = golferId,
                ScoreSubmittedDate = System.DateTime.UtcNow,
                ScoreSubmittedTime = System.DateTime.UtcNow.TimeOfDay,
                HoleTypeId = 2,
                IsActive = true,
                CreatedOn = System.DateTime.UtcNow,
                BookingId = bookingId
            };
            using (var scope = _unitOfWork.BeginTransaction())
            {

                foreach (ScoreViewModel scoreViewModel in scoreViewModels)
                {
                    if (scoreViewModel.HoleNumber == 1 && scoreViewModel.Storkes != 0)
                        score.ScoreHole1.Add(HoleScore(scoreViewModel, new ScoreHole1()));
                    if (scoreViewModel.HoleNumber == 2 && scoreViewModel.Storkes != 0)
                        score.ScoreHole2.Add(HoleScore(scoreViewModel, new ScoreHole2()));
                    if (scoreViewModel.HoleNumber == 3 && scoreViewModel.Storkes != 0)
                        score.ScoreHole3.Add(HoleScore(scoreViewModel, new ScoreHole3()));
                    if (scoreViewModel.HoleNumber == 4 && scoreViewModel.Storkes != 0)
                        score.ScoreHole4.Add(HoleScore(scoreViewModel, new ScoreHole4()));
                    if (scoreViewModel.HoleNumber == 5 && scoreViewModel.Storkes != 0)
                        score.ScoreHole5.Add(HoleScore(scoreViewModel, new ScoreHole5()));
                    if (scoreViewModel.HoleNumber == 6 && scoreViewModel.Storkes != 0)
                        score.ScoreHole6.Add(HoleScore(scoreViewModel, new ScoreHole6()));
                    if (scoreViewModel.HoleNumber == 7 && scoreViewModel.Storkes != 0)
                        score.ScoreHole7.Add(HoleScore(scoreViewModel, new ScoreHole7()));
                    if (scoreViewModel.HoleNumber == 8 && scoreViewModel.Storkes != 0)
                        score.ScoreHole8.Add(HoleScore(scoreViewModel, new ScoreHole8()));
                    if (scoreViewModel.HoleNumber == 9 && scoreViewModel.Storkes != 0)
                        score.ScoreHole9.Add(HoleScore(scoreViewModel, new ScoreHole9()));
                    if (scoreViewModel.HoleNumber == 10 && scoreViewModel.Storkes != 0)
                        score.ScoreHole10.Add(HoleScore(scoreViewModel, new ScoreHole10()));
                    if (scoreViewModel.HoleNumber == 11 && scoreViewModel.Storkes != 0)
                        score.ScoreHole11.Add(HoleScore(scoreViewModel, new ScoreHole11()));
                    if (scoreViewModel.HoleNumber == 12 && scoreViewModel.Storkes != 0)
                        score.ScoreHole12.Add(HoleScore(scoreViewModel, new ScoreHole12()));
                    if (scoreViewModel.HoleNumber == 13 && scoreViewModel.Storkes != 0)
                        score.ScoreHole13.Add(HoleScore(scoreViewModel, new ScoreHole13()));
                    if (scoreViewModel.HoleNumber == 14 && scoreViewModel.Storkes != 0)
                        score.ScoreHole14.Add(HoleScore(scoreViewModel, new ScoreHole14()));
                    if (scoreViewModel.HoleNumber == 15 && scoreViewModel.Storkes != 0)
                        score.ScoreHole15.Add(HoleScore(scoreViewModel, new ScoreHole15()));
                    if (scoreViewModel.HoleNumber == 16 && scoreViewModel.Storkes != 0)
                        score.ScoreHole16.Add(HoleScore(scoreViewModel, new ScoreHole16()));
                    if (scoreViewModel.HoleNumber == 17 && scoreViewModel.Storkes != 0)
                        score.ScoreHole17.Add(HoleScore(scoreViewModel, new ScoreHole17()));
                    if (scoreViewModel.HoleNumber == 18 && scoreViewModel.Storkes != 0)
                        score.ScoreHole18.Add(HoleScore(scoreViewModel, new ScoreHole18()));
                    if (scoreViewModel.HoleNumber == 19 && scoreViewModel.Storkes != 0)
                        score.ScoreHole19.Add(HoleScore(scoreViewModel, new ScoreHole19()));
                    if (scoreViewModel.HoleNumber == 20 && scoreViewModel.Storkes != 0)
                        score.ScoreHole20.Add(HoleScore(scoreViewModel, new ScoreHole20()));
                    if (scoreViewModel.HoleNumber == 21 && scoreViewModel.Storkes != 0)
                        score.ScoreHole21.Add(HoleScore(scoreViewModel, new ScoreHole21()));
                    if (scoreViewModel.HoleNumber == 22 && scoreViewModel.Storkes != 0)
                        score.ScoreHole22.Add(HoleScore(scoreViewModel, new ScoreHole22()));
                    if (scoreViewModel.HoleNumber == 23 && scoreViewModel.Storkes != 0)
                        score.ScoreHole23.Add(HoleScore(scoreViewModel, new ScoreHole23()));
                    if (scoreViewModel.HoleNumber == 24 && scoreViewModel.Storkes != 0)
                        score.ScoreHole24.Add(HoleScore(scoreViewModel, new ScoreHole24()));
                    if (scoreViewModel.HoleNumber == 25 && scoreViewModel.Storkes != 0)
                        score.ScoreHole25.Add(HoleScore(scoreViewModel, new ScoreHole25()));
                    if (scoreViewModel.HoleNumber == 26 && scoreViewModel.Storkes != 0)
                        score.ScoreHole26.Add(HoleScore(scoreViewModel, new ScoreHole26()));
                    if (scoreViewModel.HoleNumber == 27 && scoreViewModel.Storkes != 0)
                        score.ScoreHole27.Add(HoleScore(scoreViewModel, new ScoreHole27()));
                }
                _unitOfWork.ScoreRepository.Insert(score);
                _unitOfWork.Save();

                if (booking.GolferId == golferId)
                {
                    booking.ScoreId = score.ScoreId;
                }
                else
                {

                    foreach (var item in booking.BookingPlayerDetails)
                    {
                        if (item.Player2 == golfer.ClubMemberId)
                        {
                            item.Player2ScoreId = score.ScoreId;
                        }
                        if (item.Player3 == golfer.ClubMemberId)
                        {
                            item.Player3ScoreId = score.ScoreId;
                        }
                        if (item.Player4 == golfer.ClubMemberId)
                        {
                            item.Player4ScoreId = score.ScoreId;
                        }
                    }


                }
                _unitOfWork.BookingRepository.Update(booking);
                _unitOfWork.Save();
                scope.EndTransaction();
            }
            return true;
        }

        public List<BookingViewModel> GetBookingListForScorePost(long golferId)
        {
            List<BookingViewModel> bookingViewModels = new List<BookingViewModel>();
            List<Booking> bookings = new List<Booking>();
            DateTime currentDateTime = Core.Helper.DateHelper.ConvertSystemDate();
            Golfer golfer = _unitOfWork.GolferRepository.Get(x => x.GolferId == golferId);

            bookings = _unitOfWork.BookingRepository.GetMany(x => x.GolferId == golferId && x.TeeOffDate < currentDateTime && x.BookingTypeId==(int)Core.Helper.Enum.EnumBookingType.BTT && x.CoursePairingId != null && TimeSpan.Parse(x.TeeOffSlot) <= TimeSpan.Parse(currentDateTime.Hour.ToString() + ":" + currentDateTime.Minute.ToString()) && x.BookingStatusId == (int)Core.Helper.Enum.EnumBookingStatus.Confirm && x.IsActive == true && x.ScoreId == null).OrderByDescending(x => x.TeeOffDate).ToList();
            List<Booking> otherBooking = GetAllOtherBookingDetails(golfer.ClubMemberId, (int)Core.Helper.Enum.EnumSearchBooking.Completed);
            bookings.AddRange(otherBooking);
            bookings.OrderByDescending(x => x.TeeOffDate).ToList();
            foreach (Booking booking in bookings)
            {
                BookingViewModel bookingViewModel = new BookingViewModel()
                {
                    BookingId = booking.BookingId,
                    Time = booking.TeeOffSlot,
                    Date = booking.TeeOffDate.ToShortDateString(),
                    NoOfPlayer = booking.NoOfPlayer,
                    PaymentGatewayBookingId = booking.PaymentGatewayBookingId,
                    BookedBy = booking.Golfer.Name + " " + booking.Golfer.LastName,
                    ENB = Core.Helper.Crypto.EncryptStringAES(booking.BookingId.ToString())
                };
                bookingViewModels.Add(bookingViewModel);
            };

            return bookingViewModels;
        }

        private List<Booking> GetAllOtherBookingDetails(string MId, long bookingSearchTypeId)
        {
            List<Booking> bookings = new List<Booking>();
            DateTime currentDateTime = Core.Helper.DateHelper.ConvertSystemDate();
            DateTime startDate = new DateTime(currentDateTime.Year, currentDateTime.Month, currentDateTime.Day, 0, 0, 0);
            DateTime endDate = new DateTime(currentDateTime.Year, currentDateTime.Month, currentDateTime.Day, 23, 59, 59);

            List<BookingPlayerDetail> bookingPlayerDetails = _unitOfWork.BookingPlayerDetailRepository.GetMany(x => ((x.Player2 == MId && x.Player2ScoreId == null) || (x.Player3 == MId && x.Player3ScoreId == null) || (x.Player4 == MId && x.Player4ScoreId == null)) && x.IsActive == true).ToList();


            foreach (var booking in bookingPlayerDetails)
            {
                if (bookingSearchTypeId == (int)Core.Helper.Enum.EnumSearchBooking.Cancelled)
                {
                    bookings.AddRange(_unitOfWork.BookingRepository.GetMany(x => x.BookingId == booking.BookingId && x.BookingStatusId == (int)Core.Helper.Enum.EnumBookingStatus.Cancelled && x.IsActive == true).OrderByDescending(x => x.UpdatedOn).ToList());
                }
                else
                {
                    if (bookingSearchTypeId == (int)Core.Helper.Enum.EnumSearchBooking.Completed)
                    {
                        bookings.AddRange(_unitOfWork.BookingRepository.GetMany(x => x.BookingId == booking.BookingId && x.TeeOffDate < currentDateTime && TimeSpan.Parse(x.TeeOffSlot) <= TimeSpan.Parse(currentDateTime.Hour.ToString() + ":" + currentDateTime.Minute.ToString()) && x.BookingStatusId == (int)Core.Helper.Enum.EnumBookingStatus.Confirm && x.IsActive == true).OrderByDescending(x => x.TeeOffDate).ToList());
                    }
                    else if (bookingSearchTypeId == (int)Core.Helper.Enum.EnumSearchBooking.Upcoming)
                    {
                        DateTime nextDay = currentDateTime.AddDays(1);
                        DateTime date = new DateTime(nextDay.Year, nextDay.Month, nextDay.Day, 0, 0, 0);
                        bookings.AddRange(_unitOfWork.BookingRepository.GetMany(x => x.BookingId == booking.BookingId && x.TeeOffDate >= date && x.BookingStatusId == (int)Core.Helper.Enum.EnumBookingStatus.Confirm && x.IsActive == true).OrderBy(x => x.TeeOffDate).ToList());

                    }
                    else if (bookingSearchTypeId == (int)Core.Helper.Enum.EnumSearchBooking.Today)
                    {

                        bookings.AddRange(_unitOfWork.BookingRepository.GetMany(x => x.BookingId == booking.BookingId && x.TeeOffDate >= startDate && TimeSpan.Parse(x.TeeOffSlot) > TimeSpan.Parse(currentDateTime.Hour.ToString() + ":" + currentDateTime.Minute.ToString()) && x.TeeOffDate <= endDate && x.BookingStatusId == (int)Core.Helper.Enum.EnumBookingStatus.Confirm && x.IsActive == true).OrderBy(x => x.TeeOffSlot).ToList());
                    }
                }


            }
            return bookings.Where(x=> x.BookingTypeId == (int)Core.Helper.Enum.EnumBookingType.BTT && x.CoursePairingId != null).ToList();
        }
        #endregion
    }
}
