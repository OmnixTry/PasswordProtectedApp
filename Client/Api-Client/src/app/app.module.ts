import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import {HttpClientModule} from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LogInFormComponent } from './log-in-form/log-in-form.component';
import { CrdVerificationFromComponent } from './crd-verification-from/crd-verification-from.component';
import { PasswordStrengthMeterModule } from 'angular-password-strength-meter';


@NgModule({
  declarations: [
    AppComponent,
    LogInFormComponent,
    CrdVerificationFromComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    ReactiveFormsModule,
    HttpClientModule,
    PasswordStrengthMeterModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
