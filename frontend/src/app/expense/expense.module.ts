import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {ExpensecardComponent} from "./expensecard/expensecard.component";
import {IonicModule} from "@ionic/angular";
import {CreateexpenseComponent} from "./createexpense/createexpense.component";



@NgModule({
  declarations: [
    ExpensecardComponent,
    CreateexpenseComponent
  ],
  exports: [
    ExpensecardComponent,
  ],
  imports: [
    CommonModule,
    IonicModule
  ]
})
export class ExpenseModule { }
