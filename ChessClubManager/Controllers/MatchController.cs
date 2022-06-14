using ChessClubManager.Enums;
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
    public class MatchController : ControllerBase
    {
        private readonly IMatchService matchService;

        public MatchController(IMatchService matchService)
        {
            this.matchService = matchService;
        }

        [HttpGet]
        public IEnumerable<Match> Get()
        {
            return matchService.GetAllMatches();            
        }

        [HttpGet("{id}")]
        public Match Get(string id)
        {
            return matchService.GetMatch(id);
        }

        [HttpPost]
        public int Post([FromBody] Match match)
        {
            #region Validate Match Info

            // Validate Match
            if (match.MatchDate > DateTime.Now)
            {
                throw new Exception("Exception - MatchController(Post): Match date cannot be in the future");
            }

            // Validate Participants

            if (match.Participants.Count != 2)
            {
                throw new Exception("Exception - MatchController(Post): Invalid Participation Count");
            }

            if ((string.IsNullOrEmpty(match.Participants[0].MemberId)) || (string.IsNullOrEmpty(match.Participants[1].MemberId)))
            {
                throw new Exception("Exception - MatchController(Post): Participant Member Id is a required field");
            }


            if ((match.Participants[0].MatchResult != MatchResult.Draw) && (match.Participants[0].MatchResult == match.Participants[1].MatchResult))
            {
                throw new Exception("Exception - MatchController(Post): Invalid Match Result");
            }


            #endregion

            return matchService.AddMatch(match);
        }        
    }
}
