import { Component, OnInit } from '@angular/core';
import {Router} from "@angular/router";
import {AccountService, User} from "../auth/account.service";
import {AuthGuard} from "../../services/AuthGuard";
import {PopoverController} from "@ionic/angular";
import {NotificationService} from "../notification/notification.service";

@Component({
  selector: 'toolbar',
  templateUrl: './toolbar.component.html',
  styleUrls: ['./toolbar.component.scss'],
})
export class ToolbarComponent implements OnInit {
  loggedInUser: User | undefined
  isUserLoaded: boolean = false
  lastUpdate: Date | undefined;
  notificationAmount: number | undefined;
  constructor(
    private readonly notificationService: NotificationService,
    private router: Router,
    private service: AccountService,
    private authGuard: AuthGuard,
    private popoverController: PopoverController
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
    this.popoverController.dismiss()
  }

  toLogin() {
    this.router.navigate(['/login'])
  }

  async logout() {
    this.service.logout()
    this.popoverController.dismiss()
  }

  message: any;

  cancel() {

  }

  onWillDismiss($event: any) {

  }

  protected readonly confirm = confirm;
  public name = name;

}
