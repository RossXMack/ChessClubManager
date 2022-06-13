import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Member } from '../../models/member';
import { MemberService } from '../../services/member.service';

@Component({
  selector: 'app-add-member',
  templateUrl: './add-member.component.html',
  styleUrls: ['./add-member.component.css']
})
export class AddMemberComponent implements OnInit {

  memberForm: FormGroup;
  title = 'Create';
  memberId: string = '0';
  errorMessage: any;
  submitted = false;

  constructor(
    private fb: FormBuilder,
    private avRoute: ActivatedRoute,
    private memberService: MemberService,
    private router: Router
  ) {    
    if (this.avRoute.snapshot.params['id']) {
      this.memberId = this.avRoute.snapshot.params['id'];
    };

    this.memberForm = this.fb.group({
      id: [''],
      name: ['', [Validators.required]],
      surname: ['', [Validators.required]],
      email: [''],
      birthday: [''],
      joinDate: ['']
    })
  }

  ngOnInit(): void {
    debugger
    if (this.memberId !== '0') {

      this.title = 'Edit';

      this.memberService.getMemberById(this.memberId).subscribe(
        (result: Member) => {
          this.memberForm.setValue(result);
        },
        (error) => console.error(error)
      );
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

    if (this.title === 'Create') {
      this.addMember();
    } else {
      if (this.title === 'Edit') {
        this.updateMember();
      }
    }      
  }

  public cancel() {
    this.router.navigate(['fetch-members']);
  }
}

