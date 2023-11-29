import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {IonicModule} from '@ionic/angular';
import {FormsModule} from '@angular/forms';
import {HomePage} from './home.page';

import {HomePageRoutingModule} from './home-routing.module';
import {ToolbarComponent} from "../toolbar/toolbar.component";
import {GroupModule} from "../group/group.module";
import {ProfileModule} from "../profile/profile.module";
import {AuthModule} from "../auth/auth.module";
import {UserModule} from "../user/user.module";
import {NotificationModule} from "../notification/notification.module";


@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    HomePageRoutingModule,
    GroupModule,
    ProfileModule,
    AuthModule,
    UserModule,
    NotificationModule
  ],
  exports: [
    HomePage,
    ToolbarComponent,

  ],
  declarations: [
    HomePage,
    ToolbarComponent
  ]
})
export class HomePageModule {
}
