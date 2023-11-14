import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {RegisterComponent} from "./register/register.component";
import {LoginComponent} from "./login/login.component";
import {IonicModule} from "@ionic/angular";
import {RouterLink} from "@angular/router";
import {TokenService} from "../../services/TokenService";
import {ReactiveFormsModule} from "@angular/forms";
import {HTTP_INTERCEPTORS, HttpClient, HttpClientModule, HttpHandler} from "@angular/common/http";
import {AuthHttpInterceptor} from "../../interceptors/auth-http-interceptor";
import {AccountService} from "./account.service";
import {ErrorHttpInterceptor} from "../../interceptors/error-http-interceptors";



@NgModule({
  declarations: [RegisterComponent, LoginComponent],
  imports: [
    CommonModule,
    IonicModule,
    RouterLink,
    ReactiveFormsModule,
      HttpClientModule
  ],
  providers: [

    {provide: HTTP_INTERCEPTORS, useClass: ErrorHttpInterceptor, multi: true },
      AccountService
  ]
})
export class AuthModule { }
