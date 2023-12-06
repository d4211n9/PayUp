import {Injectable} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {environment} from "../../environments/environment";
import {GroupUpdate} from "../group/group.service";

export interface FullUser {
  id: number,
  email: String,
  fullName: string,
  phoneNumber: string,
  created: Date,
  profileUrl: string | null,
}

export interface EditUserDto {
  email: String,
  fullName: string,
  phoneNumber: string,
}

export interface EditUserImg {
  imageUrl: File | null
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

  editUserImage(value: EditUserImg) {
    const formData = new FormData();
    Object.entries(value).forEach(([key, value]) =>
      formData.append(key, value)
    );
    return this.http.put<EditUserImg>(environment.apiBaseUrl+'/user/profileimage', formData, {
      reportProgress: true,
      observe: 'events'
    });
  }
}
