import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {ExpensecardComponent} from "./expensecard/expensecard.component";
import {IonicModule} from "@ionic/angular";



@NgModule({
  declarations: [ExpensecardComponent],
  exports: [
    ExpensecardComponent
  ],
  imports: [
    CommonModule,
    IonicModule
  ]
})
export class ExpenseModule { }
