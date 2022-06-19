using ChessClubManager.Enums;
using ChessClubManager.Interfaces;
using ChessClubManager.Models;
using Microsoft.EntityFrameworkCore;
using System;
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

        // Calculates the new rankings for each participant in a match.
        public void CalcParticipantsRanking(Match match)
        {
            try
            {
                // get participant 1
                Member participant1 = dbContext.Members.Find(match.Participants[0].MemberId);
                int participant1Rank = participant1.CurrentRank;

                // get participant 2
                Member participant2 = dbContext.Members.Find(match.Participants[1].MemberId);
                int participant2Rank = participant2.CurrentRank;

                // Update Match Participant 1's ranking               
                this.UpdateParticipantRanking(participant1.Id, participant1Rank, participant2Rank, match.Participants[0].MatchResult);

                // Update Match Participant 2's ranking
                this.UpdateParticipantRanking(participant2.Id, participant2Rank, participant1Rank, match.Participants[1].MatchResult);
            }
            catch
            {
                throw;
            }
        }

        // Recalculates member rankings afer a member has been deleted.
        public void UpdateDeletedMemberRanking(Member member)
        {
            // update the rankings as if our member had been moved to last rank. This will move everyone into his old position.
            UpdateMemberRankings(member.Id, member.CurrentRank, dbContext.Members.Count() + 1);
        }

        #endregion

        #region Private Methods

        // Calculates a new ranking for given participant and updates the effected current ranks in the members table. 
        private void UpdateParticipantRanking(string participantId, int participantRank, int opponentRank, MatchResult result)
        {
            try
            {
                int participantNewRank = calcNewParticipantRanking(participantRank, opponentRank, result);

                // if our participants ranking is unchanged then return;
                if (participantNewRank != participantRank)
                {
                    UpdateMemberRankings(participantId, participantRank, participantNewRank);
                }
                else
                {
                    return;
                }
            }
            catch
            {
                throw;
            }
        }

        // Update the current member rankings in the members table based on the change in rank of this participant.
        private void UpdateMemberRankings(string memberId, int memberRank, int memberNewRank)
        {
            // get our ranking list boundaries for sublist using our ranking change - rankings above this and below this would remain uneffected.
            int higherRankBoundary = memberRank < memberNewRank ? memberRank : memberNewRank;
            int lowerRankBoundary = memberRank > memberNewRank ? memberRank : memberNewRank;

            // we don't want to db update the entire members list, so we getting a subsection of the entire list demarcated by our boundaries.
            var subList = dbContext.Members
                .Where(m => m.CurrentRank >= higherRankBoundary && m.CurrentRank <= lowerRankBoundary)
                .Select(subEntry => new TempRankEntry { Member = subEntry, sortingRank = subEntry.CurrentRank }
                ).ToList();

            // update new ranks to subList using decimal to displace previous rank correctly. 
            // if moving up in rank, previous rank moves down. 
            // if moving down in rank, previous rank moves up.
            if (memberNewRank > memberRank)
                subList.Find(m => m.Member.Id == memberId).sortingRank = Convert.ToDouble(memberNewRank) + 0.1;
            else
                subList.Find(m => m.Member.Id == memberId).sortingRank = Convert.ToDouble(memberNewRank) - 0.1;

            // sort subList by the new sorting rank column generated above.
            var sortedSubList = subList.OrderBy(s => s.sortingRank);

            // rerank the newly sorted list based on high boundary to low boundary.
            int newRank = higherRankBoundary;
            foreach (var entry in sortedSubList)
            {
                entry.Member.CurrentRank = newRank;
                this.dbContext.Entry(entry.Member).State = EntityState.Modified;
                newRank++;
            }
        }

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
                int rankDifference = (participantRank - opponentRank);
                
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
                                case < 0:   return participantRank;                              // participant is ranked higher - same rank.
                                case > 0:   return moveRankUpByHalf(participantRank, rankDifference);  // participant is ranked lower - moveRankUpByHalf().                                                                                                       
                            }
                            break;
                        }                        
                    case MatchResult.Loss:
                        {
                            switch (rankDifference)
                            {
                                case < 0:   return participantRank + 1;                          // participant is ranked higher - move one rank down.
                                case > 0:   return participantRank;                              // participant is ranked lower - same rank.                                                                                                                                               
                            }
                            break;
                        }                    
                }

                throw new Exception("Exception - calcNewParticipantRanking(): Unrecognised flow path");
            }
            catch {
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

    class TempRankEntry {
        public Member Member { get; set; }
        
        public double sortingRank { get; set; }
    }
}
