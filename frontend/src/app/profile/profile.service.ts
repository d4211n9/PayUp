import {Injectable} from "@angular/core";
import {HttpClient} from "@angular/common/http";

export interface FullUser {
  id: number,
  email: String,
  fullName: string,
  phoneNumber: string,
  created: Date,
  profileUrl: string,
}

export interface EditUserDto {
  email: String,
  fullName: string,
  phoneNumber: string,
  profileUrl: string,
}

@Injectable()
export class ProfileService {
  constructor(private readonly http: HttpClient,) {
  }

  getCurrentUser() {
    return this.http.get<FullUser>('http://localhost:5100/api/user/currentuser');
  }

  editCurrentUser(user: EditUserDto) {
    return this.http.put<FullUser>('http://localhost:5100/api/user/profileinfo', user);
  }
}
