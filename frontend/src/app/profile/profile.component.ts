import { Component, OnInit } from '@angular/core';
import {FullUser, ProfileService} from "./profile.service";
import {firstValueFrom, Observable} from "rxjs";

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss'],
})
export class ProfileComponent  implements OnInit {


   fullUser$?: Observable<FullUser>;
  constructor(private readonly service: ProfileService,) {

  }

  ngOnInit() {

     this.fullUser$ = this.service.getCurrentUser();


   }


}
