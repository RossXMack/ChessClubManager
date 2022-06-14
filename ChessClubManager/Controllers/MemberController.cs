using ChessClubManager.Interfaces;
using ChessClubManager.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace ChessClubManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MemberController : ControllerBase
    {

        private readonly IMemberService memberService;

        public MemberController(IMemberService memberService)
        {
            this.memberService = memberService;
        }

        [HttpGet]
        public IEnumerable<Member> Get()
        {
            return memberService.GetAllMembers();            
        }

        [HttpGet("{id}")]
        public Member Get(string id)
        {
            return memberService.GetMember(id);
        }

        [HttpPost]
        public int Post([FromBody] Member member)
        {
            #region Validate Member Info

            if (string.IsNullOrEmpty(member.Name))
            {
                throw new Exception("Exception - MemberController(Post): Member name is a required field");
            }

            if (string.IsNullOrEmpty(member.Surname))
            {
                throw new Exception("Exception - MemberController(Post): Member surname is a required field");
            }

            if (string.IsNullOrEmpty(member.Email))
            {
                throw new Exception("Exception - MemberController(Post): Member email is a required field");
            }

            if (member.Birthday > DateTime.Now)
            {
                throw new Exception("Exception - MemberController(Post): Birthday cannot be in the future");
            }

            #endregion

            return memberService.AddMember(member);
        }

        [HttpPut]
        public int Put([FromBody] Member member)
        {
            #region Validate Member Info

            if (string.IsNullOrEmpty(member.Name))
            {
                throw new Exception("Exception - MemberController(Put): Member name is a required field");
            }

            if (string.IsNullOrEmpty(member.Surname))
            {
                throw new Exception("Exception - MemberController(Put): Member surname is a required field");
            }

            if (string.IsNullOrEmpty(member.Email))
            {
                throw new Exception("Exception - MemberController(Put): Member email is a required field");
            }

            if (member.Birthday > DateTime.Now)
            {
                throw new Exception("Exception - MemberController(Put): Birthday cannot be in the future");
            }

            #endregion

            return memberService.UpdateMember(member);
        }

        [HttpDelete("{id}")]
        public int Delete(string id) {
            return memberService.DeleteMember(id);
        }
    }
}
