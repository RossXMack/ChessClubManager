using ChessClubManager.Interfaces;
using ChessClubManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChessClubManager_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MemberController : ControllerBase
    {

        private readonly IMember objMember;

        public MemberController(IMember member)
        {
            this.objMember = member;
        }

        [HttpGet]
        public IEnumerable<Member> Get()
        {
            return objMember.GetAllMembers();            
        }

        [HttpGet("{id}")]
        public Member Get(int id)
        {
            return objMember.GetMember(id);
        }

        [HttpPost]
        public int Post([FromBody] Member member)
        {
            return objMember.AddMember(member);
        }

        [HttpPut]
        public int Put([FromBody] Member member)
        {
            return objMember.UpdateMember(member);
        }

        [HttpDelete]
        public int Delete(int id) {
            return objMember.DeleteMember(id);
        }
    }
}
