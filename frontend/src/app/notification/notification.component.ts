import { Component, OnInit } from '@angular/core';
import {GroupInviteNotification, NotificationService} from "./notification.service";
import {Router} from "@angular/router";

@Component({
  selector: 'app-notification',
  templateUrl: './notification.component.html',
  styleUrls: ['./notification.component.scss'],
})
export class NotificationComponent  implements OnInit {
  groupInviteNotifications: GroupInviteNotification[] = [];

  constructor(
    private readonly notificationService: NotificationService,
    private readonly router: Router
  ) { }

  ngOnInit() {
    this.getNotifications();
  }

  async getNotifications() {
    await this.getGroupInviteNotifications();
  }

  async getGroupInviteNotifications() {
    this.groupInviteNotifications = await this.notificationService.getGroupInviteNotifications();
  }

  toGroup(groupId: string) {
    this.router.navigateByUrl('/groups/'+groupId)
  }
}
