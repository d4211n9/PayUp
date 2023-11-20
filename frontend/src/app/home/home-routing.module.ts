import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomePage } from './home.page';
import {MyGroupsComponent} from "./mygroups/mygroups.component";

const routes: Routes = [
  {
    path: '',
    component: HomePage,
  },
  {
    path: 'mygroups',
    component: MyGroupsComponent
  }

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class HomePageRoutingModule {}
