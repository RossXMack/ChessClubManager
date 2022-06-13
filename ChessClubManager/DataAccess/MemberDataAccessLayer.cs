﻿using ChessClubManager.Interfaces;
using ChessClubManager.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChessClubManager.DataAccess
{
    public class MemberDataAccessLayer : IMember
    {
        private readonly ChessClubManagerContext db;

        public MemberDataAccessLayer(ChessClubManagerContext db)
        {
            this.db = db;
        }

        #region Update Methods

        public int AddMember(Member member)
        {
            try
            {
                db.Members.Add(member);
                db.SaveChanges();
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
                db.Entry(member).State = EntityState.Modified;
                db.SaveChanges();
                return 0;
            }
            catch 
            {
                throw;
            }
        }

        public int DeleteMember(int id)
        {
            try
            {
                Member mem = db.Members.Find(id);
                db.Members.Remove(mem);
                db.SaveChanges();
                return 0;
            }
            catch 
            {
                throw;
            }
        }

        #endregion

        #region Read Methods

        public IEnumerable<Member> GetAllMembers()
        {
            try
            {
                return db.Members?.ToList().OrderBy(x => x.Id);
            }
            catch 
            {
                throw;
            }            
        }

        public Member GetMember(int id)
        {
            try
            {
                Member member = db.Members.Find(id);
                return member;
            }
            catch 
            {
                throw;
            }
        }

        #endregion
    }
}
