import { Component, OnInit } from '@angular/core';
import {
  CreateExpense,
  CreateFullExpense,
  FullExpense,
  GroupService,
  Transaction,
  UserInGroup
} from "../../group/group.service";
import {ActivatedRoute, Router} from "@angular/router";
import {FormBuilder, Validators} from "@angular/forms";
import {ToastController} from "@ionic/angular";
import {firstValueFrom} from "rxjs";
import {AccountService} from "../../auth/account.service";

@Component({
  selector: 'app-settle',
  templateUrl: './settle.component.html',
  styleUrls: ['./settle.component.scss'],
})
export class SettleComponent  implements OnInit {
  groupId: any
  userId: any
  usersOnExpense: number[] = []
  transaction?: Transaction
  transactionList: Transaction[] = []
  myPayerTransactions: Transaction[] = []

  constructor(
    private route: ActivatedRoute,
    private accountService: AccountService,
    private readonly fb: FormBuilder,
    private readonly groupService: GroupService,
    private readonly toast: ToastController,
    private router: Router
  ) {
  }

  async ngOnInit() {
    await this.getId()
    await this.getCurrentUser()
    await this.getTransactions()
  }

  async getId() {
    const map = await firstValueFrom(this.route.paramMap)
    this.groupId = map.get('groupId')
  }

  async getCurrentUser() {
    this.userId = await this.accountService.getCurrentUser()
  }

  //TODO get the users that logged in user owes money
  async getTransactions() {
    this.transactionList = await this.groupService.getAllTransactions(this.groupId);

    this.transactionList.forEach(transaction => {
      if (transaction.payerId === this.userId) {
        this.myPayerTransactions.push(transaction);
      }
    })
  }

  handleUserSelection(event: CustomEvent) {
    if (event.detail.value !== undefined) {
      this.transaction = event.detail.value
    }
  }

  async createSettle() {
    if(this.transaction === undefined) return

    const createdExpense =
      await firstValueFrom<FullExpense>(this.groupService.createSettle(this.transaction as Transaction, this.groupId))


    await (await this.toast.create({
      message: "Your expense " + createdExpense.expense.description + " was created successfully",
      color: "success",
      duration: 5000
    })).present()
      .then(() => {
        this.router.navigate(['groups/' + this.groupId])
          .then(() => {
            location.reload();
          });
      });
  }
}
