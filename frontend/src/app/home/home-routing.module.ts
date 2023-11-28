import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {HomePage} from './home.page';
import {MyGroupsComponent} from "../group/my-groups/my-groups.component";
import {CreateComponent} from "../group/create/create.component";
import {InviteComponent} from "../group/invite/invite.component";

const routes: Routes = [

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class HomePageRoutingModule {
}
