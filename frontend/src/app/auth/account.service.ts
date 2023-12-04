import {HttpClient} from "@angular/common/http";
import {Injectable} from "@angular/core";
import {firstValueFrom, Timestamp} from "rxjs";
import {environment} from "../../environments/environment";
import {TokenService} from "../../services/TokenService";


export interface User {
  email: string;
  fullName: string;
  phoneNumber: string;
  created: Timestamp<any>;
  profileUrl: string | null;

}

export interface Credentials {
  email: string;
  password: string;
}

export interface Registration {
  email: String;
  fullName: string;
  password: string;
  phoneNumber: string,
  created: Date,
  profileUrl: string,
}

@Injectable()
export class AccountService {
  constructor(
    private readonly http: HttpClient,
    private readonly token: TokenService
  ) {
  }

  getCurrentUser() {
    return firstValueFrom(this.http.get<User>(environment.apiBaseUrl+'/user/currentuser'));
  }

  login(value: Credentials) {
    return this.http.post<{ token: string }>(environment.apiBaseUrl+'/account/login', value);
  }

  register(value: Registration) {
    return this.http.post<any>(environment.apiBaseUrl+'/account/register', value);
  }

  logout() {
    this.token.clearToken()
    location.reload()
  }
}
