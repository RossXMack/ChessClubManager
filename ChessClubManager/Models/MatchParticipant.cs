using System;
using System.Collections.Generic;

#nullable disable

namespace ChessClubManager.Models
{
    public partial class MatchParticipant
    {
        public int Id { get; set; }
        public int MatchId { get; set; }
        public int MemberId { get; set; }
        public int Result { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}
