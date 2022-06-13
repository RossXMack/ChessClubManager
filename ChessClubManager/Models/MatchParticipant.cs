using System;
using System.Collections.Generic;

#nullable disable

namespace ChessClubManager.Models
{
    public partial class MatchParticipant
    {
        public string Id { get; set; }
        public string MatchId { get; set; }
        public string MemberId { get; set; }
        public int Result { get; set; }

        #region Audits

        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        #endregion
    }
}
