using ChessClubManager.Models;
using System.Collections.Generic;

namespace ChessClubManager.Interfaces
{
    public interface IMemberService
    {        
        int AddMember(Member member);

        Member GetMember(string id);

        IEnumerable<Member> GetAllMembers();

        int UpdateMember(Member member);
        
        int DeleteMember(string id);
    }
}
