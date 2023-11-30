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
  id: any;
  group: Group | undefined;
  expenses: FullExpense[] = []
  balances: Balance[] = []
  subpage = 'activity';
  loading: boolean = true;
  balancesLoaded: boolean = false;

  constructor(
    private route: ActivatedRoute,
    private readonly service: GroupService,
  ) {
  }

  async ngOnInit() {
    await this.getId()
    this.group = await firstValueFrom(this.service.getGroup(this.id))
    await this.getAllExpenses()
    this.loading = false
  }

  async segmentChanged(ev: any) {
    if (ev.detail.value === 'balances' && !this.balancesLoaded) {
      this.loading = true
      await this.getBalances()
      this.balancesLoaded = true
      this.loading = false
    }
  }

  async getId() {
    const map = await firstValueFrom(this.route.paramMap)
    this.id = map.get('groupId')
  }

  async getAllExpenses() {
    this.expenses = await this.service.getAllExpenses(this.id)
  }

  async getBalances() {
    this.balances = await this.service.getBalances(this.id)
  }
}
