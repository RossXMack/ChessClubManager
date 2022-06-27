import { Component, ElementRef, OnInit, ViewChild} from '@angular/core';
import { Member } from '../../models/member';
import { MemberService } from '../../services/member.service';

@Component({
  selector: 'app-fetch-member',
  templateUrl: './fetch-members.component.html',
  styleUrls: ['./fetch-members.component.css']
})

export class FetchMembersComponent implements OnInit {
  @ViewChild('AddMember', null) addMemElement: ElementRef;
  public members: Member[] = [];

  constructor(private memberService: MemberService) { }

  ngOnInit(): void {
    this.getMembers();

    this.addMemElement.nativeElement.focus();
  }

  getMembers(): void {    
    this.memberService
      .getMembers()
      .subscribe((result) => (this.members = result));
  }

  delete(memberId: string, memberName: string): void {
    const ans = confirm(
      'Do you want to delete the member with Name: ' + memberName
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
