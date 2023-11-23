import {Injectable} from "@angular/core";
import {HttpClient} from "@angular/common/http";

export interface Group {
  name: string,
  description: string,
  image_url: string,
  created_date: Date
}

export interface GroupInvitation {
  groupId: number
  receiverId: number
}

@Injectable()
export class GroupService {
  constructor(private readonly http: HttpClient) {
  }

  create(value: Group) {
    return this.http.post<Group>('http://localhost:5100/api/group/create', value)
  }

  invite(group_invite: GroupInvitation) {
    return this.http.post<boolean>('http://localhost:5100/api/group/invite', group_invite);
  }
}
