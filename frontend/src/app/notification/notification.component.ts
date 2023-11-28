import { Component, OnInit } from '@angular/core';
import {GroupInviteNotification, NotificationService} from "./notification.service";

@Component({
  selector: 'app-notification',
  templateUrl: './notification.component.html',
  styleUrls: ['./notification.component.scss'],
})
export class NotificationComponent  implements OnInit {
  groupInviteNotifications: GroupInviteNotification[] = [];

  constructor(private readonly notificationService: NotificationService) { }

  ngOnInit() {
    this.getNotifications();
  }

  async getNotifications() {
    await this.getGroupInviteNotifications();
  }

  async getGroupInviteNotifications() {
    this.groupInviteNotifications = await this.notificationService.getGroupInviteNotifications();
  }
}
