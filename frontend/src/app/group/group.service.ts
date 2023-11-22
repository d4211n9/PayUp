import {Injectable} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {environment} from "../../environments/environment";
import {firstValueFrom} from "rxjs";

export interface Group {
  name: string,
  description: string,
  imageUrl: string,
  createdDate: Date
}

export interface Expense {
  description: string,
  amount: number,
  createdDate: Date
}

@Injectable()
export class GroupService {
  constructor(private readonly http: HttpClient) {
  }

  create(value: Group) {
    return this.http.post<Group>(environment.apiBaseUrl+'group/create', value)
  }

  async getAllExpenses(groupId: string) {
    const call = this.http.get<Expense[]>(environment.apiBaseUrl+'group/'+groupId+'/expenses');
    return await firstValueFrom<Expense[]>(call);
  }

  async getGroup(groupId: string) {
      const call = this.http.get<Group>(environment.apiBaseUrl+'group/'+groupId);
      return await firstValueFrom<Group>(call);
  }

}
