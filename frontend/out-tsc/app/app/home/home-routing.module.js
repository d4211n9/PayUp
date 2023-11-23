import { __decorate } from "tslib";
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { HomePage } from './home.page';
import { MyGroupsComponent } from "../group/my-groups/my-groups.component";
const routes = [
    {
        path: '',
        component: HomePage,
    },
    {
        path: 'mygroups',
        component: MyGroupsComponent
    }
];
let HomePageRoutingModule = class HomePageRoutingModule {
};
HomePageRoutingModule = __decorate([
    NgModule({
        imports: [RouterModule.forChild(routes)],
        exports: [RouterModule]
    })
], HomePageRoutingModule);
export { HomePageRoutingModule };
//# sourceMappingURL=home-routing.module.js.map