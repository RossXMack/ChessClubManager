using ChessClubManager.Interfaces;
using ChessClubManager.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChessClubManager.DataAccess
{
    public class MemberService : IMemberService
    {
        #region Members

        private readonly ChessClubManagerContext dbContext;

        private IRankingService rankingService;

        #endregion

        #region Constructor

        public MemberService(ChessClubManagerContext dbContext, IRankingService rankingService)
        {
            this.dbContext = dbContext;
            this.rankingService = rankingService;
        }

        #endregion

        #region CRUD Methods

        public IEnumerable<Member> GetAllMembers()
        {
            try
            {
                return dbContext.Members?.ToList().OrderBy(x => x.CurrentRank);
            }
            catch
            {
                throw;
            }
        }
        
        public Member GetMember(string id)
        {
            try
            {
                Member member = dbContext.Members.Find(id);
                return member;
            }
            catch
            {
                throw;
            }
        }                

        public int AddMember(Member member)
        {
            try
            {                
                // add default member info
                member.Id = Guid.NewGuid().ToString();
                member.GamesPlayed = 0;

                // new member starts as lowest rank
                member.CurrentRank = dbContext.Members.Count() + 1;

                // update audit info
                member.Created = DateTime.Now;
                member.Updated = DateTime.Now;

                dbContext.Members.Add(member);

                dbContext.SaveChanges();

                return 0;
            }
            catch 
            {
                throw;
            }
        }

        public int UpdateMember(Member member)
        {
            try
            {                                
                // update audit info                
                member.Updated = DateTime.Now;
                dbContext.Entry(member).State = EntityState.Modified;

                dbContext.SaveChanges();

                return 0;
            }
            catch 
            {
                throw;
            }
        }

        public int DeleteMember(string id)
        {
            try
            {
                Member member = dbContext.Members.Find(id);
                if(member == null)
                {
                    throw new Exception("Exception - DeleteMember(): Invalid member id, member cannot be null");
                }

                #region Ranking
                // move the members rank to last so that when we delete him it won't leave a gap in the rank list.
                this.rankingService.UpdateDeletedMemberRanking(member);
                #endregion

                #region Cascading
                // cascade delete to matches and match participants that contain this member.
                var data = dbContext.MatchParticipants.Select(participant => new
                {
                    participant,
                    match = dbContext.Matches.Select(match => new {
                        match,
                        participants = dbContext.MatchParticipants.Where(participant => participant.MatchId == match.Id).ToList()
                    }).Where(m => m.match.Id == participant.MatchId).FirstOrDefault()
                }).Where(p => p.participant.MemberId == id).ToList();

                if (data != null)
                {
                    data.ForEach(item =>
                    {
                        item.match.participants.ForEach(p =>
                        {
                            dbContext.MatchParticipants.Remove(p);
                        });

                        dbContext.Matches.Remove(item.match.match);
                    });
                }
                #endregion

                dbContext.Members.Remove(member);

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
