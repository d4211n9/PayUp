import {Injectable} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {AccountService, User} from "../auth/account.service";

export interface FullUser {
  id: number;
  email: String;
  fullName: string;
  phoneNumber: string,
  created: Date,
  profileUrl: string,
}

@Injectable()
export class ProfileService {
  constructor(private readonly http: HttpClient,) {
  }

  getCurrentUser() {
    return this.http.get<FullUser>('http://localhost:5100/api/user/currentuser');
  }

}
