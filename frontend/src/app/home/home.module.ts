import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {IonicModule} from '@ionic/angular';
import {FormsModule} from '@angular/forms';
import {HomePage} from './home.page';

import {HomePageRoutingModule} from './home-routing.module';
import {MyGroupsComponent} from "../group/my-groups/my-groups.component";
import {ToolbarComponent} from "../toolbar/toolbar.component";
import {GroupModule} from "../group/group.module";


@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    HomePageRoutingModule,
    GroupModule,

  ],
  exports: [
    HomePage,
    ToolbarComponent
  ],
  declarations: [HomePage, MyGroupsComponent, ToolbarComponent]
})
export class HomePageModule {
}
