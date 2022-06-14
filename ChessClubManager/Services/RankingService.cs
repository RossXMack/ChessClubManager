using ChessClubManager.Enums;
using ChessClubManager.Interfaces;
using ChessClubManager.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChessClubManager.DataAccess
{
    public class RankingService : IRankingService
    {
        #region Members

        private readonly ChessClubManagerContext dbContext;

        #endregion

        #region Constructor

        public RankingService(ChessClubManagerContext dbContext)
        {
            this.dbContext = dbContext;
        }

        #endregion

        #region Public Methods
        // Calculates the new rankings of each player based on the supplied match param, then updates affected current rankings in members table.
        public void CalculateNewRankings(Match match)
        {
            try
            {
                // get player ranks
                var player1Rank = dbContext.Members.Find(match.Participants[0].MemberId).CurrentRank;
                var player2Rank = dbContext.Members.Find(match.Participants[1].MemberId).CurrentRank;

                var player1NewRank = calculateNewPlayerRanking(player1Rank, player2Rank, match.Participants[0].MatchResult);
                var player2NewRank = calculateNewPlayerRanking(player2Rank, player1Rank, match.Participants[1].MatchResult);

                // todo: update our current rankings in the members table.
                
            }
            catch {
                throw;
            }
        }

        #endregion

        #region Private Methods

        private int calculateNewPlayerRanking(int playerRank, int opponentRank, MatchResult matchResult)
        {
            try
            {
                // validate initial ranks
                if (playerRank == opponentRank)
                {
                    throw new Exception("Exception - calculateNewPlayerRanking: Players cannot be at the same initial rank");
                }

                // Work out the difference between the players initial rankings.               
                // 1. if the difference is positive, player is ranked lower.
                // 2. if the difference is negative, player is ranked higher.            
                // 3. if the difference is 1 or -1, player is adjacent. (noted for Draw scenario).               
                // 4. 0 is invalid and should never occur for either player. (see above validation).           
                var rankDifference = (playerRank - opponentRank);
                
                switch (matchResult)
                {
                    case MatchResult.Draw:
                        {
                            switch (rankDifference)
                            {
                                case < -1:  return playerRank - 1;                          // player is ranked higher - move one rank up.
                                case > 1:   return playerRank;                              // player is ranked lower - same rank.                                
                                default:    return playerRank;                              // players are adjacent - same rank.                                                                    
                            }                            
                        }                        
                    case MatchResult.Win:
                        {
                            switch (rankDifference)
                            {
                                case < 0:   return playerRank;                              // player is ranked higher - same rank.
                                case > 0:   return moveUpHalf(playerRank, rankDifference);  // player is ranked lower - moveUpHalf().                                                                                                       
                            }
                            break;
                        }                        
                    case MatchResult.Loss:
                        {
                            switch (rankDifference)
                            {
                                case < 0:   return playerRank + 1;                          // player is ranked higher - move one rank down.
                                case > 0:   return playerRank;                              // player is ranked lower - same rank.                                                                                                                                               
                            }
                            break;
                        }                    
                }

                throw new Exception("Exception - calculateNewPlayerRanking: Unrecognised flow path");
                // todo: double check this above.
            }
            catch {
                throw;
            }
        }

        // Calculation to move players rank up based on ranking difference divided by 2.
        // 
        // Comments
        // =======================================================================
        // small caveat is that the difference can result in a decimal answer. 
        // the assumption I've made here is the following:
        // 
        // Example: playerRank=15 opponentRank=10 difference=(15-10) / 2 = 2.5 => resulting rank=12.5 
        // the member at 12 who is not involved in the match, I'm assuming is still seen as the higher ranked player. 12 < 12.5
        // so I've disregarded the decimal.. meaning if result is 2.5.. result is 2.. player will move from 15 to 13, player 12 is uneffected.

        // ** one exception to this rule is when the players are adjacent and the lower ranked player wins.
        // Example: playerRank=12 opponentRank = 11 difference=(12-11) / 2 = 0.5 => resulting rank=11.5 
        // In this one case scenario the loser according to rules, loses a rank and moves to 12.. so the 11 position is vacated. 
        // as 11.5 < 12 in the adjacent scenario I've increased the winning players rank by 1 to move him into the losers position.        
        private int moveUpHalf(int playerRank, int rankDifference)
        {
            switch (rankDifference)
            {
                case 1: return playerRank - 1;                                           // adjacent so move one rank up.
                default: return playerRank - Convert.ToInt32(rankDifference / 2);        // move up ranking by split (ignore decimal).                 
            }
        }

        #endregion
    }
}
