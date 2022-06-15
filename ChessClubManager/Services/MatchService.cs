using ChessClubManager.Interfaces;
using ChessClubManager.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChessClubManager.DataAccess
{
    public class MatchService : IMatchService
    {
        #region Members

        private readonly ChessClubManagerContext dbContext;

        private IRankingService rankingService;

        #endregion

        #region Constructor

        public MatchService(ChessClubManagerContext dbContext, IRankingService rankingService)
        {
            this.dbContext = dbContext;
            this.rankingService = rankingService;
        }

        #endregion

        #region CRUD Methods

        public IEnumerable<Match> GetAllMatches()
        {
            try
            {                
                var data = dbContext.Matches.Select(match => new
                {
                    match,
                    Participants = dbContext.MatchParticipants.Select(participant => new { 
                        participant,
                        member = dbContext.Members.Where(member => member.Id == participant.MemberId).ToList()
                    }).Where(participant => participant.participant.MatchId == match.Id).ToList()
                }).ToList();

                var result = data.Select(oo => oo.match);
                return result;
            }
            catch
            {
                throw;
            }
        }

        public Match GetMatch(string id)
        {
            try
            {
                var data = dbContext.Matches.Select(match => new {
                    match,
                    Participants = dbContext.MatchParticipants.Where(participant => participant.MatchId == match.Id).ToList()
                }).FirstOrDefault(oo => oo.match.Id == id);

                return data.match;
            }
            catch
            {
                throw;
            }
        }
       
        public int AddMatch(Match match)
        {
            try
            {                
                // add default match info
                match.Id = Guid.NewGuid().ToString();

                // update audit info
                match.Created = DateTime.Now;
                match.Updated = DateTime.Now;
                                                     
                // add participants
                foreach (var participant in match.Participants)
                {                    
                    // add default participant info
                    participant.Id = Guid.NewGuid().ToString();
                    participant.MatchId = match.Id;

                    // update audit info
                    participant.Created = DateTime.Now;
                    participant.Updated = DateTime.Now;
                    
                    dbContext.MatchParticipants.Add(participant);

                    // update games played
                    Member member = dbContext.Members.Find(participant.MemberId);
                    member.GamesPlayed = member.GamesPlayed + match.GamesPlayed;
                    dbContext.Entry(member).State = EntityState.Modified;
                }

                // add match
                dbContext.Matches.Add(match);

                // calculate new rankings
                this.rankingService.CalculateNewRankings(match);                                

                dbContext.SaveChanges();

                return 0;
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region Private Methods
        
        #endregion
    }
}
