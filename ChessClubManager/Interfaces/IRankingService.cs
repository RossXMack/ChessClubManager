using ChessClubManager.Models;
using System.Collections.Generic;

namespace ChessClubManager.Interfaces
{
    public interface IRankingService
    {
        void CalculateNewRankings(Match match);
    }
}
