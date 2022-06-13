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

        #endregion

        #region Constructor

        public MatchService(ChessClubManagerContext dbContext)
        {
            this.dbContext = dbContext;
        }

        #endregion

        #region CRUD Methods

        public IEnumerable<Match> GetAllMatches()
        {
            try
            {
                var data = dbContext.Matches.Select(match => new {
                    match,
                    Participants = dbContext.MatchParticipants.Where(participant => participant.MatchId == match.Id).ToList()
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
                #region Validate Match Info

                if (match.Participants.Count != 2)
                {
                    throw new Exception("Match Validation Exception: Invalid Participation Count");
                }

                if ((match.Participants[0].Result != 0) && (match.Participants[0].Result == match.Participants[1].Result))
                {
                    throw new Exception("Match Validation Exception: Invalid Match Result");
                }

                if (match.MatchDate > DateTime.Now)
                {
                    throw new Exception("Match Validation Exception: Match date cannot be in the future");
                }

                #endregion

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
                }

                // add match
                dbContext.Matches.Add(match);

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
