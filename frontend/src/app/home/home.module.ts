import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {IonicModule} from '@ionic/angular';
import {FormsModule} from '@angular/forms';
import {HomePage} from './home.page';

import {HomePageRoutingModule} from './home-routing.module';
import {GroupModule} from '../group/group.module';
import {ProfileComponent} from "../profile/profile.component";
import {ProfileModule} from "../profile/profile.module";


@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    HomePageRoutingModule,
    GroupModule,
    ProfileModule
  ],
  declarations: [HomePage]
})
export class HomePageModule {
}
