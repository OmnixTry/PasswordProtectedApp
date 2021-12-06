import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import {HttpClientModule} from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LogInFormComponent } from './log-in-form/log-in-form.component';
import { CrdVerificationFromComponent } from './crd-verification-from/crd-verification-from.component';

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
    HttpClientModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
