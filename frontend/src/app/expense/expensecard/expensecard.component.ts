import {Component, Input, OnInit} from '@angular/core';
import {Expense} from "../../group/group.service";
import {DatePipe} from "@angular/common";

@Component({
  selector: 'expensecard',
  templateUrl: './expensecard.component.html',
  styleUrls: ['./expensecard.component.scss'],
})
export class ExpensecardComponent  implements OnInit {

  constructor() {
  }

  ngOnInit() {
  }

  @Input() expense!: Expense;

}
