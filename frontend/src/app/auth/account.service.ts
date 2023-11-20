import {HttpClient} from "@angular/common/http";
import {Injectable} from "@angular/core";
import {Timestamp} from "rxjs";


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
  constructor(private readonly http: HttpClient) {
  }

  getCurrentUser() {
    return this.http.get<User>('/api/account/whoami');
  }

  login(value: Credentials) {
    return this.http.post<{ token: string }>('http://localhost:5100/api/account/login', value);
  }

  register(value: Registration) {
    return this.http.post<any>('http://localhost:5100/api/account/register', value);
  }
}
