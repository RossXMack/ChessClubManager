import { Component, OnInit } from '@angular/core';
import { DatePipe } from '@angular/common';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { FlatMatch, Match, MatchResult, Participant } from '../../models/match';
import { Member } from '../../models/member';
import { MatchService } from '../../services/match.service';
import { MemberService } from '../../services/member.service';

@Component({
  selector: 'app-add-match',
  templateUrl: './add-match.component.html',
  styleUrls: ['./add-match.component.css']
})
export class AddMatchComponent implements OnInit {
  
  matchForm: FormGroup;  
  title = 'Create';
  matchId: string = '0';
  errorMessage: any;
  submitted = false;
  memberStubList: { id: string; fullname: string; }[] = [];

  constructor(
    private fb: FormBuilder,    
    private matchService: MatchService,
    private memberService: MemberService,
    private router: Router,
    private datePipe: DatePipe
  ) {       
    this.matchForm = this.fb.group({
      id: [''],
      matchDate: [this.datePipe.transform(Date(), "yyy-MM-dd")],
      gamesPlayed: [1],      
      participant1Id: [''],
      participant1Result: [''],            
      participant2Id: [''],
      participant2Result: ['']      
    })
  }

  ngOnInit(): void {
    this.getMemberList();
  }
  
  get registerFormControl() {
    return this.matchForm.controls;
  }

  public cancel() {
    this.router.navigate(['fetch-matches']);
  }

  private getMemberList() {
    this.memberService
      .getMembers()
      .subscribe((data: Member[]) => {
        data.map(d => {
          this.memberStubList.push({ id: d.id, fullname: d.name + " " + d.surname });
          debugger

        })
      })
      
  }

  private addMatch(): void {    
    debugger
    // assign to flat
    var flatMatch = this.matchForm.value;
    
    this.matchService.addMatch(this.convertFromFlat(flatMatch)).subscribe(
      () => {
        this.router.navigate(['fetch-matches']);
      },
      (error) => console.error(error)
    );
  }

  private convertFromFlat(flatMatch: FlatMatch): Match {
    var match = new Match();
    match.id = '0';
    match.matchDate = flatMatch.matchDate;
    match.gamesPlayed = Number(flatMatch.gamesPlayed);

    var participant1 = new Participant();
    participant1.id = '0';
    participant1.matchId = '0';
    participant1.memberId = flatMatch.participant1Id;
    participant1.matchResult = Number(flatMatch.participant1Result);
    match.participants.push(participant1);

    var participant2 = new Participant();
    participant2.id = '0';
    participant2.matchId = '0';
    participant2.memberId = flatMatch.participant2Id;
    participant2.matchResult = Number(flatMatch.participant2Result);
    match.participants.push(participant2);

    return match;
  }

  save() {
    debugger
    this.submitted = true;
    if (!this.matchForm.valid) {
      return;
    }
    
    this.addMatch();          
  }  
}

