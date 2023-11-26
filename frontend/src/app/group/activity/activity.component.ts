import {Component, OnInit} from '@angular/core';
import {Balance, FullExpense, Group, GroupService} from "../group.service";
import {firstValueFrom} from "rxjs";
import {ActivatedRoute} from "@angular/router";

@Component({
  selector: 'app-activity',
  templateUrl: './activity.component.html',
  styleUrls: ['./activity.component.scss'],
})
export class ActivityComponent implements OnInit {
  expenses: FullExpense[] = []
  balances: Balance[] = []
  group: Group | undefined;
  id: any;
  subpage = 'activity';
  members: number[] = []; //TODO skal ændres til users, brugte bare til at populate balances tabben
  loading: boolean = true;

  constructor(
    private route: ActivatedRoute,
    private readonly service: GroupService,
  ) {
  }

  async ngOnInit() {
    await this.getId()
    this.getGroup()
    await this.getAllExpenses()
    this.getBalances()
    this.loading = false
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

  async getBalances() {
    this.balances = await this.service.getBalances(this.id)
  }
}
