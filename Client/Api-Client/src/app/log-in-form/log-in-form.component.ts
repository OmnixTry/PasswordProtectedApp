import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-log-in-form',
  templateUrl: './log-in-form.component.html',
  styleUrls: ['./log-in-form.component.scss']
})
export class LogInFormComponent implements OnInit {

  newUserForm = new FormGroup({});
  requestResult: any;

  constructor(fb: FormBuilder, private http: HttpClient) 
  { 
    this.newUserForm = fb.group(
      {
        email: ['', Validators.required],
        password: ['', Validators.required],
        firstName: ['', Validators.required],
        lastName: ['', Validators.required],
        location: ['', Validators.required]
      });

      
  }

  ngOnInit(): void {
  }

  onSubmit(){
    console.log(this.newUserForm.value);
    this.http.post('https://localhost:5001/api/accounts/', this.newUserForm.value).subscribe(x => { this.requestResult = 'Successs'}, e => { this.requestResult = e.error[0].description});
  }
}
