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

    constructor(
        private route: ActivatedRoute,
        private readonly service: GroupService,
    ) {}

    async ngOnInit() {
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

    async getGroup(){
        this.group = await this.service.getGroup(this.id)
    }
}
