using ChessClubManager.Models;
using System.Collections.Generic;

namespace ChessClubManager.Interfaces
{
    public interface IMember
    {        
        int AddMember(Member member);

        Member GetMember(int id);

        IEnumerable<Member> GetAllMembers();

        int UpdateMember(Member member);
        
        int DeleteMember(int id);
    }
}
