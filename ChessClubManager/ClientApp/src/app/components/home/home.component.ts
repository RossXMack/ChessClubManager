import { Component, OnInit } from '@angular/core';
import { Member } from '../../models/member';
import { MemberService } from '../../services/member.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent implements OnInit {
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
}
