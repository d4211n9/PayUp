import {Component, Input, OnInit} from '@angular/core';
import {Expense, FullExpense, UserOnExpense} from "../../group/group.service";
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

  @Input() expense!: FullExpense;

  openExpense() {
    console.log("TODO HAHahah goteeeem")
  }

  getOwnShare(id: number) {
    var share: number | undefined;
    var userOnExpense = this.expense.usersOnExpense.find((u) => u.userId === id);
    if(userOnExpense == undefined) {
      share = undefined;
    } else if (userOnExpense.amount > 0) {
      share = userOnExpense.amount
    } else {
      share = -1*userOnExpense.amount
    }
    return share;
  }

  getOweOrLent(id: number) {
    var message: string;
    var userOnExpense = this.expense.usersOnExpense.find((u) => u.userId === id);
    if(userOnExpense == undefined) {
      message = "Not involved"
    } else if (userOnExpense.amount > 0) {
      message = "You lent"
    } else {
      message = "You owe"
    }
    return message;
  }
}
