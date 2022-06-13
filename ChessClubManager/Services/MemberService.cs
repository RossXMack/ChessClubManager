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

        #endregion

        #region Constructor

        public MemberService(ChessClubManagerContext dbContext)
        {
            this.dbContext = dbContext;
        }

        #endregion

        #region CRUD Methods

        public IEnumerable<Member> GetAllMembers()
        {
            try
            {
                return dbContext.Members?.ToList().OrderBy(x => x.Id);
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
                #region Validate Member Info
                
                if (member.Birthday > DateTime.Now)
                {
                    throw new Exception("Member Validation Exception: Birthday cannot be in the future");
                }

                #endregion

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
                Member mem = dbContext.Members.Find(id);
                dbContext.Members.Remove(mem);

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
