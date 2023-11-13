import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {RegisterComponent} from "./register/register.component";
import {LoginComponent} from "./login/login.component";
import {IonicModule} from "@ionic/angular";
import {RouterLink} from "@angular/router";



@NgModule({
  declarations: [RegisterComponent, LoginComponent],
  imports: [
    CommonModule,
    IonicModule,
    RouterLink
  ]
})
export class AuthModule { }
