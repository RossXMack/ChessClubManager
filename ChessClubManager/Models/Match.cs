using System;
using System.Collections.Generic;

#nullable disable

namespace ChessClubManager.Models
{
    public partial class Match
    {
        public int Id { get; set; }
        public DateTime MatchDate { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}
