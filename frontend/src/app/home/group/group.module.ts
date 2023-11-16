import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {InviteComponent} from "./invite/invite.component";
import {IonicModule} from "@ionic/angular";



@NgModule({
  declarations: [
    InviteComponent
  ],
  imports: [
    CommonModule,
    IonicModule
  ]
})
export class GroupModule { }
