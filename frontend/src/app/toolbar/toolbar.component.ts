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
    setInterval(async () => {
      // Update lastUpdate to the current time before fetching notifications
     // await this.getNotificationsAmount();
      this.lastUpdate = new Date();//todo skal hente en int over antal noti i database eller måske bare om der er nogle
    }, 3000);
  }

  async getNotifications() {
   // this.notificationAmount = await this.notificationService.getNotificationsAmount(this.lastUpdate);
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
}
