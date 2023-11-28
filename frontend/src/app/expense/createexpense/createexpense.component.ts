import {Component, Input, OnInit} from '@angular/core';
import {Group, GroupService, UserInGroup} from "../../group/group.service";
import {HttpClient} from "@angular/common/http";
import {firstValueFrom} from "rxjs";
import {ActivatedRoute} from "@angular/router";


@Component({
  selector: 'app-createexpense',
  templateUrl: './createexpense.component.html',
  styleUrls: ['./createexpense.component.scss'],
})
export class CreateexpenseComponent  implements OnInit {
  userInGroup: UserInGroup[] = [];
  id: any;


  constructor(private route: ActivatedRoute,
              private readonly service: GroupService
  ) { }

  async ngOnInit() {
    await this.getId()
    this.getUsersInGroup("1");
  }

  async getId() {
    const map = await firstValueFrom(this.route.paramMap)
    this.id = map.get('groupId')
  }

  async getUsersInGroup(groupId: string) {
    this.userInGroup = await this.service.getUserInGroup(groupId)
  }




}
