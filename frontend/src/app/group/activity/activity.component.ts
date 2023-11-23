import {Component, OnInit} from '@angular/core';
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
  members: number[] = []; //TODO skal ændres til users, brugte bare til at populate balances tabben

  constructor(
    private route: ActivatedRoute,
    private readonly service: GroupService,
  ) {
  }

  async ngOnInit() {
    await this.getId()
    this.getAllExpenses()
    this.getGroup()

    this.generateItems() //TODO: skift til get members & balances
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

  private generateItems() {
    const count = this.members.length + 1;
    for (let i = 0; i < 50; i++) {
      this.members.push(count + i);
    }
  }
}
