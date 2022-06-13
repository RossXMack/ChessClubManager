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
            return matchService.AddMatch(match);
        }        
    }
}
