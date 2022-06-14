import { Component, OnInit} from '@angular/core';
import { Member } from '../../models/member';
import { MemberService } from '../../services/member.service';

@Component({
  selector: 'app-fetch-member',
  templateUrl: './fetch-members.component.html',
  styleUrls: ['./fetch-members.component.css']
})

export class FetchMembersComponent implements OnInit {
  public members: Member[] = [];

  constructor(private memberService: MemberService) { }

  ngOnInit(): void {
    this.getMembers();
  }

  getMembers(): void {    
    this.memberService
      .getMembers()
      .subscribe((result) => (this.members = result));
  }

  delete(memberId: string): void {
    const ans = confirm(
      'Do you want to delete the member with Id: ' + memberId
    )
    if (ans) {
      this.memberService.deleteMember(memberId).subscribe(
        () => {
          this.getMembers();
        },
        (error) => console.error(error)
      );
    }
  }
}
