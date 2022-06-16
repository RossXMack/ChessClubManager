using ChessClubManager.Models;
using System.Collections.Generic;

namespace ChessClubManager.Interfaces
{
    public interface IRankingService
    {
        void CalcParticipantsRanking(Match match);

        void UpdateDeletedMemberRanking(Member member);
    }
}
