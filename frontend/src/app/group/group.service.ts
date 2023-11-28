import {Injectable} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {environment} from "../../environments/environment";
import {firstValueFrom} from "rxjs";

export interface Group {
  id: number,
  name: string,
  description: string,
  imageUrl: string,
  createdDate: Date
}

export interface UserInGroup {
  userId: number,
  fullName: string,
  imageUrl: string
}

export interface CreateGroup {
  name: string,
  description: string,
  imageUrl: string,
  createdDate: Date
}

export interface FullExpense {
  expense: Expense
  usersOnExpense: UserOnExpense[]
  loggedInUser: number
}

export interface Expense {
  id: number
  userId: number
  groupId: number
  description: string
  amount: number
  createdDate: string
  fullName: string
}

export interface UserOnExpense {
  userId: number
  expenseId: number
  amount: number
  imageUrl: string
}

export interface Balance {
  userId: number,
  fullName: string,
  imageUrl: string,
  amount: number
}

@Injectable()
export class GroupService {
  constructor(private readonly http: HttpClient) {
  }

  async getMyGroups() {
    const call = this.http.get<Group[]>(environment.apiBaseUrl + "/mygroups")
    return await firstValueFrom<Group[]>(call);
  }

  create(value: CreateGroup) {
    return this.http.post<Group>(environment.apiBaseUrl+'/group/create', value)
  }

  async getAllExpenses(groupId: number) {
    const call = this.http.get<FullExpense[]>(environment.apiBaseUrl+'/group/'+groupId+'/expenses');
    return await firstValueFrom<FullExpense[]>(call);
  }

  async getGroup(groupId: number) {
      const call = this.http.get<Group>(environment.apiBaseUrl+'/group/'+groupId);
      return await firstValueFrom<Group>(call);
  }

  async getUserInGroup(groupId: string) {
    const call = this.http.get<UserInGroup[]>(environment.apiBaseUrl +"/group/"+groupId+"/users");
    return await firstValueFrom<UserInGroup[]>(call);
  }


  async getBalances(groupId: number) {
    const call = this.http.get<Balance[]>(environment.apiBaseUrl+'/group/'+groupId+'/balances')
    return await firstValueFrom<Balance[]>(call);
  }
}




