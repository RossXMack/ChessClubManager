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
                // todo: checkForNulls.. all over.

                dbContext.Members.Remove(member);

                // we need to update the rank list after deleting the member as each person behind him has to move up in rank to fill the gap.
                this.rankingService.UpdateRankList(member.CurrentRank);

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
