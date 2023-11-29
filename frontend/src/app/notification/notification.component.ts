import { Component, OnInit } from '@angular/core';
import {Notification, NotificationCategory, NotificationService} from "./notification.service";
import {Router} from "@angular/router";
import {firstValueFrom, interval} from "rxjs";

@Component({
  selector: 'app-notification',
  templateUrl: './notification.component.html',
  styleUrls: ['./notification.component.scss'],
})
export class NotificationComponent  implements OnInit {
  notifications: Notification[] = [];
  lastUpdate: Date | undefined;

  constructor(
    private readonly notificationService: NotificationService,
    private readonly router: Router
  ) {
  }


  ngOnInit() {
    this.getNotifications();
    setInterval(async () => {
      // Update lastUpdate to the current time before fetching notifications
      await this.getNotifications();
      this.lastUpdate = new Date();
    }, 3000);
  }

  async getNotifications() {
    this.notifications = await this.notificationService.getNotifications(this.lastUpdate);
  }

  toGroup(groupId: string) {
    this.router.navigateByUrl('/groups/' + groupId)
  }

   async acceptInvite(id: string) {
     await firstValueFrom(this.notificationService.acceptInvite(true, Number(id)))

     // If the acceptance is successful, remove the notification from the array
     this.notifications = this.notifications.filter(noti => noti.footer !== id);
   }

  async declineInvite(id: string) {
    await firstValueFrom(this.notificationService.acceptInvite(false, Number(id)));
    // If the acceptance is successful, remove the notification from the array
    this.notifications = this.notifications.filter(noti => noti.footer !== id);
  }
}

