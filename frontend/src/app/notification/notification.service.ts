import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {environment} from "../../environments/environment";
import {firstValueFrom, Observable} from "rxjs";
import {EnumValue} from "@angular/compiler-cli/src/ngtsc/partial_evaluator";
import {FullUser} from "../profile/profile.service";
import {GroupInvitation} from "../group/group.service";

export interface Notification {
  subject: string,
  body: string,
  footer: string,
  inviteReceived: Date,
  category: NotificationCategory
}

export enum NotificationCategory {
  GroupInvite = 1,
}

export interface GroupInviteDto{
  accepted: boolean,
  groupId: number
}

@Injectable()
export class NotificationService {

  constructor(private readonly http: HttpClient) { }

  async getNotifications(lastUpdate?: Date): Promise<Notification[]> {
    let url = environment.apiBaseUrl + "/user/notifications";

    if (lastUpdate) {
      const formattedLastUpdate = lastUpdate.toISOString();
      url += `?lastUpdate=${formattedLastUpdate}`;
    }
    // Make the HTTP request
    const call = this.http.get<Notification[]>(url);

    // Wait for the observable to complete and return the result
    return await firstValueFrom<Notification[]>(call);
  }

  acceptInvite(isAccepted: boolean, groupId: number): Observable<boolean> {
    let group_invite: GroupInviteDto = {
      accepted: isAccepted,
      groupId: groupId
    }
      return this.http.post<boolean>('http://localhost:5100/api/user/accept-invite', group_invite);
    }
}
