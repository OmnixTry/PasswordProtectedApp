import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { lowerCaseValidator, numbersValidator, specialSymbolValidator, upperCaseValidator } from '../validators';

@Component({
  selector: 'app-log-in-form',
  templateUrl: './log-in-form.component.html',
  styleUrls: ['./log-in-form.component.scss']
})
export class LogInFormComponent implements OnInit {

  newUserForm = new FormGroup({});
  requestResult: any;

  get passValue():string {
    return this.newUserForm.value.password;
  }

  get pwdErrors(): any{
    return this.newUserForm.controls.password.errors;
  }

  get strErrors(): any {
    return this.newUserForm.controls?.strength?.errors;
  }

  onStrengthChange(change: any){
    console.log(change);
    console.log(this.newUserForm);
    this.newUserForm.get('strength')?.setValue(change);
  }

  constructor(fb: FormBuilder, private http: HttpClient) 
  { 
    this.newUserForm = fb.group(
      {
        email: ['', Validators.required],
        password: ['', { validators: [Validators.required, Validators.minLength(10), Validators.maxLength(64), upperCaseValidator(), lowerCaseValidator(), numbersValidator(), specialSymbolValidator()]}],
        firstName: ['', Validators.required],
        lastName: ['', Validators.required],
        location: ['', Validators.required],
        strength: [null, Validators.min(3)]
      });

      console.log(this.newUserForm.errors);
  }

  ngOnInit(): void {
    console.log(this.newUserForm.errors);
  }

  onSubmit(){
    console.log(this.newUserForm.value);
    this.http.post('https://localhost:5001/api/accounts/', this.newUserForm.value).subscribe(x => { this.requestResult = 'Successs'}, e => { this.requestResult = e.error[0].description});
  }
}
