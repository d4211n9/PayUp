import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {environment} from "../../environments/environment";
import {firstValueFrom} from "rxjs";
import {EnumValue} from "@angular/compiler-cli/src/ngtsc/partial_evaluator";

export interface GroupInviteNotification {
  category: EnumValue,
  subject: string,
  body: string,
  description: string,
  footer: string,
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
