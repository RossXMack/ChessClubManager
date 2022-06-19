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

            // D1. participantRank=10 opponentRank=11 result=Draw   rankDifference=-1   expected result = no change in rank.
            var resultD1 = calcNewParticipantRanking(10, 11, MatchResult.Draw);
            Assert.Equal(10, resultD1);

            // D2. participantRank=11 opponentRank=10 result=Draw   rankDifference=1    expected result = no change in rank.
            var resultD2 = calcNewParticipantRanking(11, 10, MatchResult.Draw);
            Assert.Equal(11, resultD2);

            // D3. participantRank=10 opponentRank=15 result=Draw   difference=-5       expected result = no change in rank.
            var resultD3 = calcNewParticipantRanking(10, 15, MatchResult.Draw);
            Assert.Equal(10, resultD3);

            // D4. participantRank=15 opponentRank=10 result=Draw   difference=5        expected result = move to rank 14 (move one rank up).
            var resultD4 = calcNewParticipantRanking(15, 10, MatchResult.Draw);
            Assert.Equal(14, resultD4);

            // D5. participantRank=2 opponentRank=1 result=Draw   
            // boundary test
            var resultD5 = calcNewParticipantRanking(2, 1, MatchResult.Draw);
            Assert.Equal(2, resultD5);
        }

        [Fact]
        public void TestWinExamples()
        {
            // ** Examples - Win **

            // W1. participantRank=10 opponentRank=16 result=Win    rankDifference=-6   result= no change in rank.
            var resultW1 = calcNewParticipantRanking(10, 16, MatchResult.Win);
            Assert.Equal(10, resultW1);

            // W2. participantRank=16 opponentRank=10 result=Win    rankDifference=6    result= move to rank 13. (move up ((16-10) / 2)=3)
            var resultW2 = calcNewParticipantRanking(16, 10, MatchResult.Win);
            Assert.Equal(13, resultW2);

            // W2a. participantRank=12 opponentRank=10 result=Win    edge case? adjacent + 1
            // based on the rules this test case creates an invalid ranking. winner moves to 11, loser moves to 11.
            // making an assumption here that since the winner beat the loser, he moves forward to the losers rank.
            var resultW2a = calcNewParticipantRanking(12, 10, MatchResult.Win);
            Assert.Equal(10, resultW2a);

            // checking boundary of W2a.
            var resultW2b = calcNewParticipantRanking(13, 10, MatchResult.Win);
            Assert.Equal(12, resultW2b);

            // W3. participantRank=11 opponentRank=10 result=Win
            // Check adjacent test.
            var resultW3 = calcNewParticipantRanking(11, 10, MatchResult.Win);
            Assert.Equal(10, resultW3);
            
            // W4. participantRank=10 opponentRank=11 result=Win
            // Check adjacent test.
            var resultW4 = calcNewParticipantRanking(10, 11, MatchResult.Win);
            Assert.Equal(10, resultW4);

            // W5. participantRank=1 opponentRank=2 result=Win
            // Check boundary.
            var resultW5 = calcNewParticipantRanking(1, 2, MatchResult.Win);
            Assert.Equal(1, resultW5);

            // W6. participantRank=2 opponentRank=1 result=Win
            // Check boundary.
            var resultW6 = calcNewParticipantRanking(2, 1, MatchResult.Win);
            Assert.Equal(1, resultW6);

        }

        [Fact]
        public void TestLossExamples()
        {
            // ** Examples - Loss **

            // L1. participantRank=10 opponentRank=16 result=Loss    rankDifference=-6   result= move to rank 11 (move one rank down).
            var resultL1 = calcNewParticipantRanking(10, 16, MatchResult.Loss);
            Assert.Equal(11, resultL1);

            // L2. participantRank=16 opponentRank=10 result=Loss    rankDifference=6    result= no change in rank.
            var resultL2 = calcNewParticipantRanking(10, 16, MatchResult.Loss);
            Assert.Equal(11, resultL2);

            // L3. participantRank=11 opponentRank=10 result=Loss
            // Checking adjacent test.
            var resultL3 = calcNewParticipantRanking(11, 10, MatchResult.Loss);
            Assert.Equal(11, resultL3);

            // L4. participantRank=10 opponentRank=11 result=Loss
            // Checking adjacent test.
            var resultL4 = calcNewParticipantRanking(10, 11, MatchResult.Loss);
            Assert.Equal(11, resultL3);

            // W5. participantRank=1 opponentRank=2 result=Win
            // Check boundary.
            var resultL5 = calcNewParticipantRanking(1, 2, MatchResult.Loss);
            Assert.Equal(2, resultL5);

            // W6. participantRank=2 opponentRank=1 result=Win
            // Check boundary.
            var resultL6 = calcNewParticipantRanking(2, 1, MatchResult.Loss);
            Assert.Equal(2, resultL6);
        }

        #region Private Methods

        // Calulates and returns a new ranking based on match parameters.
        private int calcNewParticipantRanking(int participantRank, int opponentRank, MatchResult matchResult)
        {
            try
            {
                // validate initial ranks
                if (participantRank == opponentRank)
                {
                    throw new Exception("Exception - calcNewParticipantRanking(): Participants cannot be at the same initial rank");
                }

                // Work out the difference between the participants initial rankings.               
                // 1. if the difference is positive, participant is ranked lower.
                // 2. if the difference is negative, participant is ranked higher.            
                // 3. if the difference is 1 or -1, participant is adjacent. (noted for draw scenario).               
                // 4. 0 is invalid and should never occur for either participant. (see above validation).           
                var rankDifference = (participantRank - opponentRank);

                switch (matchResult)
                {
                    case MatchResult.Draw:
                        {
                            switch (rankDifference)
                            {
                                case < -1: return participantRank;                            // participant is ranked higher - same rank.
                                case > 1: return participantRank - 1;                         // participant is ranked lower - move up 1 rank.                                
                                default: return participantRank;                              // participants are adjacent - same rank.                                                                    
                            }
                        }
                    case MatchResult.Win:
                        {
                            switch (rankDifference)
                            {
                                case < 0: return participantRank;                              // participant is ranked higher - same rank.
                                case > 0: return moveRankUpByHalf(participantRank, rankDifference);  // participant is ranked lower - moveRankUpByHalf().                                                                                                       
                            }
                            break;
                        }
                    case MatchResult.Loss:
                        {
                            switch (rankDifference)
                            {
                                case < 0: return participantRank + 1;                          // participant is ranked higher - move one rank down.
                                case > 0: return participantRank;                              // participant is ranked lower - same rank.                                                                                                                                               
                            }
                            break;
                        }
                }

                throw new Exception("Exception - calcNewParticipantRanking(): Unrecognised flow path");
            }
            catch
            {
                throw;
            }
        }

        // Calculation to move participant's rank up based on ranking difference divided by 2.
        // 
        // Comments
        // =======================================================================
        // small caveat is that the difference can result in a decimal answer. 
        // the assumption I've made here is the following:
        // 
        // Example: participantRank=15 opponentRank=10 difference=(15-10) / 2 = 2.5 => resulting rank=12.5 
        // the member at 12 who is not involved in the match, I'm assuming is still seen as the higher ranked participant. 12 < 12.5
        // so I've disregarded the decimal.. meaning if result is 2.5.. result is 2.. participant will move from 15 to 13, participant 12 is uneffected.

        // ** one exception to this rule is when the participants are adjacent and the lower ranked participant wins.
        // Example: participantRank=12 opponentRank = 11 difference=(12-11) / 2 = 0.5 => resulting rank=11.5 
        // In this one case scenario the loser according to rules, loses a rank and moves to 12.. so the 11 position is vacated. 
        // as 11.5 < 12 in the adjacent scenario I've increased the winning participants rank by 1 to move him into the losers position.        
        private int moveRankUpByHalf(int participantRank, int rankDifference)
        {
            switch (rankDifference)
            {
                case 1: return participantRank - 1;                                           // adjacent so move one rank up.
                case 2: return participantRank - 2;                                           // edge case, cannot have same rank. promote winner to empty rank.
                default: return participantRank - Convert.ToInt32(rankDifference / 2);        // move up ranking by split (ignore decimal).                 
            }
        }

        #endregion
    }
}
