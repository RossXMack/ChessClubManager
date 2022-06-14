using ChessClubManager.Enums;
using System;
using Xunit;

namespace ChessClubManager.Tests
{
    public class RankingServiceTests
    {        
        [Fact]
        public void TestDrawExamples()
        {
            // ** Examples - Draw **

            // D1. playerRank=10 opponentRank=11 result=Draw   rankDifference=-1   expected result = no change in rank.
            var resultD1 = calculateNewPlayerRanking(10, 11, MatchResult.Draw);
            Assert.Equal(10, resultD1);

            // D2. playerRank=11 opponentRank=10 result=Draw   rankDifference=1    expected result = no change in rank.
            var resultD2 = calculateNewPlayerRanking(11, 10, MatchResult.Draw);
            Assert.Equal(11, resultD2);

            // D3. playerRank=10 opponentRank=15 result=Draw   difference=-5       expected result = no change in rank.
            var resultD3 = calculateNewPlayerRanking(10, 15, MatchResult.Draw);
            Assert.Equal(10, resultD3);

            // D4. playerRank=15 opponentRank=10 result=Draw   difference=5        expected result = move to rank 14 (move one rank up).
            var resultD4 = calculateNewPlayerRanking(15, 10, MatchResult.Draw);
            Assert.Equal(14, resultD4);
        }

        [Fact]
        public void TestWinExamples()
        {
            // ** Examples - Win **

            // W1. playerRank=10 opponentRank=16 result=Win    rankDifference=-6   result= no change in rank.
            var resultW1 = calculateNewPlayerRanking(10, 16, MatchResult.Win);
            Assert.Equal(10, resultW1);

            // W2. playerRank=16 opponentRank=10 result=Win    rankDifference=6    result= move to rank 13. (move up ((16-10) / 2)=3)
            var resultW2 = calculateNewPlayerRanking(16, 10, MatchResult.Win);
            Assert.Equal(13, resultW2);

            // W3. playerRank=12 opponentRank=11 result=Win
            // Checking adjacent test.
            var resultW3 = calculateNewPlayerRanking(12, 11, MatchResult.Win);
            Assert.Equal(11, resultW3);

            // W4. playerRank=11 opponentRank=12 result=Win
            // Checking adjacent test.
            var resultW4 = calculateNewPlayerRanking(11, 12, MatchResult.Win);
            Assert.Equal(11, resultW3);
        }

        [Fact]
        public void TestLossExamples()
        {
            // ** Examples - Loss **

            // L1. playerRank=10 opponentRank=16 result=Loss    rankDifference=-6   result= move to rank 11 (move one rank down).
            var resultL1 = calculateNewPlayerRanking(10, 16, MatchResult.Loss);
            Assert.Equal(11, resultL1);

            // L2. playerRank=16 opponentRank=10 result=Loss    rankDifference=6    result= no change in rank.
            var resultL2 = calculateNewPlayerRanking(10, 16, MatchResult.Loss);
            Assert.Equal(11, resultL2);

            // L3. playerRank=12 opponentRank=11 result=Loss
            // Checking adjacent test.
            var resultL3 = calculateNewPlayerRanking(12, 11, MatchResult.Loss);
            Assert.Equal(12, resultL3);

            // L4. playerRank=11 opponentRank=12 result=Loss
            // Checking adjacent test.
            var resultL4 = calculateNewPlayerRanking(11, 12, MatchResult.Loss);
            Assert.Equal(12, resultL3);
        }

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
                                case < -1: return playerRank;                             // player is ranked higher - same rank.
                                case > 1: return playerRank - 1;                          // player is ranked lower - move one rank up.                                
                                default: return playerRank;                               // players are adjacent - same rank.                                                                    
                            }
                        }
                    case MatchResult.Win:
                        {
                            switch (rankDifference)
                            {
                                case < 0: return playerRank;                              // player is ranked higher - same rank.
                                case > 0: return moveUpHalf(playerRank, rankDifference);  // player is ranked lower - moveUpHalf().                                                                                                       
                            }
                            break;
                        }
                    case MatchResult.Loss:
                        {
                            switch (rankDifference)
                            {
                                case < 0: return playerRank + 1;                          // player is ranked higher - move one rank down.
                                case > 0: return playerRank;                              // player is ranked lower - same rank.                                                                                                                                               
                            }
                            break;
                        }
                }

                throw new Exception("Exception - calculateNewPlayerRanking: Unrecognised flow path");                
            }
            catch
            {
                throw;
            }
        }
               
        private int moveUpHalf(int playerRank, int rankDifference)
        {            
            switch (rankDifference)
            {
                case 1: return playerRank - 1;                                           // adjacent so move one rank up.
                default: return playerRank - Convert.ToInt32(rankDifference/2);          // move up ranking by split (ignore decimal).                 
            }
        }

        #endregion
    }
}
