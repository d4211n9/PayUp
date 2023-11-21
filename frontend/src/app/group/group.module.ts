import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {CreateComponent} from "./create/create.component";
import {IonicModule} from "@ionic/angular";
import {ReactiveFormsModule} from "@angular/forms";
import {GroupService} from "./group.service";
import {HTTP_INTERCEPTORS} from "@angular/common/http";
import {ErrorHttpInterceptor} from "../../interceptors/error-http-interceptors";
import {ExpenseModule} from "../expense/expense.module";
import {ActivityComponent} from "./activity/activity.component";


@NgModule({
  declarations: [CreateComponent, ActivityComponent],
  imports: [
    CommonModule,
    IonicModule,
    ReactiveFormsModule,
    ExpenseModule
  ],
  providers: [
    {provide: HTTP_INTERCEPTORS, useClass: ErrorHttpInterceptor, multi: true},
    GroupService
  ]
})
export class GroupModule {
}
