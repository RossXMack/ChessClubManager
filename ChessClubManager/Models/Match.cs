using System;
using System.Collections.Generic;

#nullable disable

namespace ChessClubManager.Models
{
    public partial class Match
    {
        public string Id { get; set; }
        public DateTime MatchDate { get; set; }        
        public int GamesPlayed { get; set; }
        public IList<MatchParticipant> Participants { get; set; } = new List<MatchParticipant>();

        #region Audits

        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        #endregion
    }
}
