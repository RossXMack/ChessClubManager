using ChessClubManager.Models;
using System.Collections.Generic;

namespace ChessClubManager.Interfaces
{
    public interface IMatchService
    {        
        int AddMatch(Match match);

        Match GetMatch(string id);

        IEnumerable<Match> GetAllMatches();                        
    }
}
