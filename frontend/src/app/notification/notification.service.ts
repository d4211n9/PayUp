import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {environment} from "../../environments/environment";
import {firstValueFrom} from "rxjs";

export interface GroupInviteNotification {
  groupId: number,
  groupName: string,
  groupDescription: string,
  groupImage: string,
  senderId: number,
  senderEmail: string,
  senderFullName: string,
  senderProfileImage: string,
  inviteReceived: Date,
}

@Injectable()
export class NotificationService {

  constructor(private readonly http: HttpClient) { }

  async getGroupInviteNotifications() {
    let call = this.http.get<GroupInviteNotification[]>(environment.apiBaseUrl + "/user/groupinvitations");
    return await firstValueFrom<GroupInviteNotification[]>(call);
  }
}
