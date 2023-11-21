import {Injectable} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {environment} from "../../environments/environment";

export interface Group {
  name: string,
  description: string,
  image_url: string,
  created_date: Date
}

@Injectable()
export class GroupService {
  constructor(private readonly http: HttpClient) {
  }

  create(value: Group) {
    return this.http.post<Group>('http://localhost:5100/api/group/create', value)
  }

  get(value: Group) {
    return this.http.get<Group>(environment.apiBaseUrl + '/mygroups')
  }


  
}
