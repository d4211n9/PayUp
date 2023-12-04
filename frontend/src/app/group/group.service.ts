import {Injectable} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {environment} from "../../environments/environment";
import {firstValueFrom} from "rxjs";

export interface Group {
  id: number,
  name: string,
  description: string,
  imageUrl: string | null,
  createdDate: Date
}

export interface UserInGroup {
  id: number,
  fullName: string,
  imageUrl: string
}

export interface CreateGroup {
  name: string,
  description: string,
  imageUrl: string | null,
  createdDate: Date
}

export interface GroupUpdate {
  name: string;
  description: string;
  imageUrl: File | null;
}

export interface CreateExpense {
  groupId: number,
  description: string,
  amount: number,
  createdDate: Date
}

export interface CreateFullExpense {
  expense: CreateExpense,
  userIdsOnExpense: number[]
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

export interface GroupInvitation {
  groupId: number
  receiverId: number
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
    const formData = new FormData();
    Object.entries(value).forEach(([key, value]) =>
      formData.append(key, value)
    );

    return this.http.post<Group>(environment.apiBaseUrl+'/group/create', formData, {
      reportProgress: true,
      observe: 'events'
    });
  }

    createExpense(value: CreateFullExpense) {
    return this.http.post<FullExpense>(environment.apiBaseUrl+'/expense', value);
}


  async getAllExpenses(groupId: number) {
    const call = this.http.get<FullExpense[]>(environment.apiBaseUrl+'/group/'+groupId+'/expenses');
    return await firstValueFrom<FullExpense[]>(call);
  }

  getGroup(groupId: number) {
      return this.http.get<Group>(environment.apiBaseUrl+'/group/'+groupId);

  }

  async getUserInGroup(groupId: string) {
    const call = this.http.get<UserInGroup[]>(environment.apiBaseUrl +"/group/"+groupId+"/users");
    return await firstValueFrom<UserInGroup[]>(call);
  }


  async getBalances(groupId: number) {
    const call = this.http.get<Balance[]>(environment.apiBaseUrl+'/group/'+groupId+'/balances')
    return await firstValueFrom<Balance[]>(call);
  }
  invite(group_invite: GroupInvitation) {
    return this.http.post<boolean>('http://localhost:5100/api/group/invite', group_invite);
  }

  update(value: GroupUpdate, groupId: number) {
    const formData = new FormData();
    Object.entries(value).forEach(([key, value]) =>
      formData.append(key, value)
    );
    return this.http.put<GroupUpdate>(environment.apiBaseUrl+'/group/'+groupId+'/update', formData, {
      reportProgress: true,
      observe: 'events'
    });
  }
}
