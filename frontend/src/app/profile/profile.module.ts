import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {IonicModule} from "@ionic/angular";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {ProfileComponent} from "./profile.component";
import {ProfileService} from "./profile.service";
import {HTTP_INTERCEPTORS, HttpClientModule} from "@angular/common/http";
import {ErrorHttpInterceptor} from "../../interceptors/error-http-interceptors";
import {AuthHttpInterceptor} from "../../interceptors/auth-http-interceptor";

@NgModule({
  declarations: [ProfileComponent],
  imports: [
    CommonModule,
    IonicModule,
    ReactiveFormsModule,
    HttpClientModule,
    FormsModule
  ],
  providers: [
    {provide: HTTP_INTERCEPTORS, useClass: ErrorHttpInterceptor, multi: true},
    { provide: HTTP_INTERCEPTORS, useClass: AuthHttpInterceptor, multi: true },
    ProfileService
  ]
})
export class ProfileModule { }
