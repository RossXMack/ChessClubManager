import { DatePipe } from '@angular/common';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Member } from '../../models/member';
import { MemberService } from '../../services/member.service';
import * as moment from 'moment';

@Component({
  selector: 'app-add-member',
  templateUrl: './add-member.component.html',
  styleUrls: ['./add-member.component.css']
})
export class AddMemberComponent implements OnInit {
  @ViewChild('Name', null) nameElement: ElementRef;
  memberForm: FormGroup;
  title = 'Add Member';
  memberId: string = '0';
  errorMessage: any;
  submitted = false;

  constructor(
    private fb: FormBuilder,
    private avRoute: ActivatedRoute,
    private memberService: MemberService,
    private router: Router,
    private datePipe: DatePipe
  ) {    
    if (this.avRoute.snapshot.params['id']) {
      this.memberId = this.avRoute.snapshot.params['id'];
    };

    this.memberForm = this.fb.group({
      id: [''],
      name: ['', [Validators.required]],
      surname: ['** default test surname **', [Validators.required]],
      email: ['** default test email **', [Validators.required]],
      birthday: [this.datePipe.transform(Date(), "yyy-MM-dd"), [Validators.required, this.dateValidator]],
      joinDate: [this.datePipe.transform(Date(), "yyy-MM-dd"), [Validators.required, this.dateValidator]],
      gamesPlayed: [0],
      currentRank: [0],
      created: [this.datePipe.transform(Date(), "yyy-MM-dd")],
      updated: [this.datePipe.transform(Date(), "yyy-MM-dd")]
    })
  }

  ngOnInit(): void {    
    if (this.memberId !== '0') {

      this.title = 'Edit Member';

      this.memberService.getMemberById(this.memberId).subscribe(
        (result: Member) => {
          this.memberForm.setValue(result);
          this.memberForm.controls['birthday'].setValue(this.datePipe.transform(result.birthday, "yyy-MM-dd"));
          this.memberForm.controls['joinDate'].setValue(this.datePipe.transform(result.birthday, "yyy-MM-dd"));
        },
        (error) => console.error(error)
      );
    } else {
      this.nameElement.nativeElement.focus();
    }
  }

  get registerFormControl() {
    return this.memberForm.controls;
  }

  private addMember(): void {
    this.memberService.addMember(this.memberForm.value).subscribe(
      () => {
        this.router.navigate(['fetch-members']);
      },
      (error) => console.error(error)
    );
  }

  private updateMember(): void {
    this.memberService.updateMember(this.memberForm.value).subscribe(
      () => {
        this.router.navigate(['fetch-members']);
      },
      (error) => console.error(error)
    );
  }

  save() {    
    this.submitted = true;
    if (!this.memberForm.valid) {
      return;
    }

    if (this.title === 'Add Member') {
      this.addMember();
    } else {
      if (this.title === 'Edit Member') {
        this.updateMember();
      }
    }      
  }

  public cancel() {
    this.router.navigate(['fetch-members']);
  }

  dateValidator(control): { [s: string]: boolean } {
    if (control.value) {
      const date = moment(control.value);
      const today = moment();
      if (date.isAfter(today)) {
        return { 'invalidDate': true }
      }
    }
    return null;
  }
}

