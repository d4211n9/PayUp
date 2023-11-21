import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {CreateComponent} from "./create/create.component";
import {IonicModule} from "@ionic/angular";
import {ReactiveFormsModule} from "@angular/forms";
import {GroupService} from "./group.service";
import {HTTP_INTERCEPTORS} from "@angular/common/http";
import {ErrorHttpInterceptor} from "../../interceptors/error-http-interceptors";
import {GroupcardComponent} from "./groupcard/groupcard.component";


@NgModule({
  declarations: [CreateComponent, GroupcardComponent],
  imports: [
    CommonModule,
    IonicModule,
    ReactiveFormsModule
  ],
  exports: [
    GroupcardComponent
  ],
  providers: [
    {provide: HTTP_INTERCEPTORS, useClass: ErrorHttpInterceptor, multi: true},
    GroupService
  ]
})
export class GroupModule {
}
