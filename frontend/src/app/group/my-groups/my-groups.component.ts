import {Component, Input, OnInit} from '@angular/core';
import {Group, GroupService} from "../group.service";
import {environment} from "../../../environments/environment";
import {firstValueFrom} from "rxjs";
import {HttpClient} from "@angular/common/http";
import {group} from "@angular/animations";

@Component({
  selector: 'app-my-groups',
  templateUrl: './my-groups.component.html',
  styleUrls: ['./my-groups.component.scss'],
})
export class MyGroupsComponent  implements OnInit {
  mygroups: Group[] = [];

  constructor(
    //private readonly service: GroupService,
    private readonly http: HttpClient
  ) {}

  ngOnInit() {
    this.getMyGroups();
  }

  async getMyGroups() {
    const call = this.http.get<Group[]>(environment.apiBaseUrl + "/mygroups")
    this.mygroups = await firstValueFrom<Group[]>(call);
  }

}
