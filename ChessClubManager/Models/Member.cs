using System;
using System.Collections.Generic;

#nullable disable

namespace ChessClubManager.Models
{
    public partial class Member
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public DateTime Birthday { get; set; }
        public DateTime JoinDate { get; set; }
        public int GamesPlayed { get; set; }
        public int CurrentRank { get; set; }

        #region Audits

        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        #endregion
    }
}
