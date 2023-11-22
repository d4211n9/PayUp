import {Component, Input, OnInit} from '@angular/core';
import {Expense, Group, GroupService} from "../group.service";
import {firstValueFrom} from "rxjs";
import {ActivatedRoute} from "@angular/router";

@Component({
  selector: 'app-activity',
  templateUrl: './activity.component.html',
  styleUrls: ['./activity.component.scss'],
})
export class ActivityComponent implements OnInit {
  expenses: Expense[] = []
  group: Group | undefined;
  id: any;
  subpage = 'activity';

  constructor(
    private route: ActivatedRoute,
    private readonly service: GroupService,
  ) {
  }

  async ngOnInit() {
    //TODO:
    // Skal det her optimeres ifht. antal DB kald?
    // getAllExpenses og getGroup bruger hver to kald:
    // 1) verificere logged in user er medlem i gruppen
    // 2) udfør respektiv opgave
    // Det vil sige der pt. bliver lavet 4 kald så snart man går ind på en gruppe.
    // Og det bliver nok væsentligt mere i takt med mere info skal hentes (balances/totals/groupmembers)
    await this.getId()
    this.getAllExpenses()
    this.getGroup()
  }

  async getId() {
    const map = await firstValueFrom(this.route.paramMap)
    this.id = map.get('groupId')
  }

  async getAllExpenses() {
    this.expenses = await this.service.getAllExpenses(this.id)
  }

  async getGroup() {
    this.group = await this.service.getGroup(this.id)
  }
}
