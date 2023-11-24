import {NgModule} from '@angular/core';
import {PreloadAllModules, RouterModule, Routes} from '@angular/router';
import {RegisterComponent} from "./auth/register/register.component";
import {LoginComponent} from "./auth/login/login.component";
import {InviteComponent} from "./group/invite/invite.component";
import {CreateComponent} from "./group/create/create.component";
import {ProfileComponent} from "./profile/profile.component";
import {ActivityComponent} from "./group/activity/activity.component";
import {MyGroupsComponent} from "./group/my-groups/my-groups.component";
import {HomePage} from "./home/home.page";


const routes: Routes = [
  {
    path: 'home',
    loadChildren: () => import('./home/home.module').then(m => m.HomePageModule)
  },
  {
    path: '',
    redirectTo: 'home',
    pathMatch: 'full'
  },
  {
    path: '',
    component: HomePage,
  },
  {
    path: 'register',
    component: RegisterComponent
  },
  {
    path: 'login',
    component: LoginComponent
  },
  {
    path: 'group/invite/:groupid',
    component: InviteComponent
  },
  {
    path: 'mygroups',
    component: MyGroupsComponent
  },
  {
    path: 'create',
    component: CreateComponent
  },
  {
    path: 'profile',
    component: ProfileComponent
  },
  {
    path: 'group/:groupId/expenses',
    component: ActivityComponent
  }
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes, {preloadingStrategy: PreloadAllModules})
  ],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
