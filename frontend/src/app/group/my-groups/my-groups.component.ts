import {Component, OnInit} from '@angular/core';
import {Group, GroupService} from "../group.service";
import {Router} from "@angular/router";

@Component({
  selector: 'app-my-groups',
  templateUrl: './my-groups.component.html',
  styleUrls: ['./my-groups.component.scss'],
})
export class MyGroupsComponent  implements OnInit {
  mygroups: Group[] = [];

  constructor(
    private readonly service: GroupService,
    private router: Router
  ) {}

  ngOnInit() {
    this.getMyGroups();
  }

  async getMyGroups() {
    this.mygroups = await this.service.getMyGroups()
  }

  toCreate() {
    this.router.navigate(['groups/create'])
  }
}
