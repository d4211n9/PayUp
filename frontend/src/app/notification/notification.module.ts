import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {HTTP_INTERCEPTORS} from "@angular/common/http";
import {ErrorHttpInterceptor} from "../../interceptors/error-http-interceptors";
import {NotificationService} from "./notification.service";
import {NotificationComponent} from "./notification.component";
import {IonicModule} from "@ionic/angular";



@NgModule({
  declarations: [NotificationComponent],
  imports: [
    CommonModule,
    IonicModule
  ],
  providers: [
    {provide: HTTP_INTERCEPTORS, useClass: ErrorHttpInterceptor, multi: true},
    NotificationService
  ]
})
export class NotificationModule {}
