import {NgModule} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';
import {RouteReuseStrategy} from '@angular/router';

import {IonicModule, IonicRouteStrategy} from '@ionic/angular';

import {AppComponent} from './app.component';
import {AppRoutingModule} from './app-routing.module';
import {ErrorHttpInterceptor} from "../interceptors/error-http-interceptors";
import {HTTP_INTERCEPTORS} from "@angular/common/http";
import {AuthModule} from "./auth/auth.module";
import {TokenService} from "../services/TokenService";
import {AuthHttpInterceptor} from "../interceptors/auth-http-interceptor";
import {GroupModule} from "./group/group.module";
import {HomePageModule} from "./home/home.module";
import {ProfileModule} from "./profile/profile.module";
import {UserModule} from "./user/user.module";
import {NotificationModule} from "./notification/notification.module";

@NgModule({
  declarations: [AppComponent],
  imports: [BrowserModule,
    IonicModule.forRoot(),
    AppRoutingModule,
    AuthModule,
    GroupModule,
    ProfileModule,
    HomePageModule,
    UserModule,
    NotificationModule
  ],
  providers: [
    {provide: HTTP_INTERCEPTORS, useClass: ErrorHttpInterceptor, multi: true},
    {provide: RouteReuseStrategy, useClass: IonicRouteStrategy},
    {provide: HTTP_INTERCEPTORS, useClass: AuthHttpInterceptor, multi: true},
    TokenService,],
  bootstrap: [AppComponent],

})
export class AppModule {
}
