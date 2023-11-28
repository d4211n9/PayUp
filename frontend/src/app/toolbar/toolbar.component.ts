import { Component, OnInit } from '@angular/core';
import {Router} from "@angular/router";
import {AccountService, User} from "../auth/account.service";
import {AuthGuard} from "../../services/AuthGuard";

@Component({
  selector: 'toolbar',
  templateUrl: './toolbar.component.html',
  styleUrls: ['./toolbar.component.scss'],
})
export class ToolbarComponent implements OnInit {
  loggedInUser: User | undefined
  isUserLoaded: boolean = false

  constructor(
    private router: Router,
    private service: AccountService,
    private authGuard: AuthGuard
  ) {}

  async ngOnInit() {
    this.getCurrentUser()
  }

  async getCurrentUser() {
    if(this.authGuard.isLoggedIn()) {
      this.loggedInUser = await this.service.getCurrentUser()
    } else {
      this.loggedInUser = undefined
    }
    this.isUserLoaded = true
  }

  toHome() {
    this.router.navigate(['/groups'])
  }

  toProfile() {
    this.router.navigate(['/profile'])
  }

  toLogin() {
    this.router.navigate(['/login'])
  }

  async logout() {
    this.service.logout()
  }
}
