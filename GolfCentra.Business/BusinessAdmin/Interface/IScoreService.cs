using GolfCentra.ViewModel.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.Business.BusinessAdmin.Interface
{
    public interface IScoreService
    {
        List<ScoreDetailsViewModel> GetScoreDetailByAdvanceSearch(ScoreSearchViewModel scoreSearchViewModel);
        List<ScoreDetailsViewModel> GetScoreDetailReportByAdvanceSearch(ScoreSearchViewModel scoreSearchViewModel);
        ScoreDetailsViewModel GetScoreByScoreId(ScoreSearchViewModel scoreSearchViewModel);
        ScoreDetailsViewModel GetScoreCardDataForScorePost(long scoreId);
        bool UpdateScoreDetails(ScoreDetailsViewModel scoreDetailsViewModel, long uniqueSessionId);
    }
}
