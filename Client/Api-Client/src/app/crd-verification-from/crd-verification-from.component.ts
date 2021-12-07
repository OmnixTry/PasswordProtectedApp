import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-crd-verification-from',
  templateUrl: './crd-verification-from.component.html',
  styleUrls: ['./crd-verification-from.component.scss']
})
export class CrdVerificationFromComponent implements OnInit {

  newUserForm = new FormGroup({});
  requestResult: any;

  get pwdErrors(): any{
    return this.newUserForm.controls.password.errors;
  }

  constructor(fb: FormBuilder, private http: HttpClient) 
  { 
    this.newUserForm = fb.group(
      {
        username: ['', Validators.required],
        password: ['', Validators.required]
      });

      
  }

  ngOnInit(): void {
  }

  onSubmit(){
    console.log(this.newUserForm.value);
    this.http.post('https://localhost:5001/api/Authorization/login/', this.newUserForm.value, {responseType:'text'}).subscribe(x => { 
      this.requestResult = x
    }, e => { 
      this.requestResult = e.error// 'oops!! usernme or password are wrong'
    });
  }

}
