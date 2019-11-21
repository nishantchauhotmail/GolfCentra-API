using GolfCentra.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.Business.Business.Interface
{
    public interface IScoreService
    {
        bool SaveScore(List<ScoreViewModel> scoreViewModels, long golferId, long grossTotal, long roundTotal);
        List<ScoreViewModel> GetScoreList(long golfeId);
        Tuple<long, long, List<ScoreViewModel>> GetScoreCardDetailsByScoreId(string id, long golferId);
        List<BookingViewModel> GetBookingListForScorePost(long golferId);
        bool SaveScore(List<ScoreViewModel> scoreViewModels, long golferId, long grossTotal, long roundTotal, string id);
        ScoreDetailsViewModel GetScoreCardDataForScorePost(string bookingId);
    }
}
