using ChessClubManager.Interfaces;
using ChessClubManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

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
            return memberService.AddMember(member);
        }

        [HttpPut]
        public int Put([FromBody] Member member)
        {
            return memberService.UpdateMember(member);
        }

        [HttpDelete("{id}")]
        public int Delete(string id) {
            return memberService.DeleteMember(id);
        }
    }
}
