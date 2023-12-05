import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {ExpensecardComponent} from "./expensecard/expensecard.component";
import {IonicModule} from "@ionic/angular";
import {CreateexpenseComponent} from "./createexpense/createexpense.component";
import {ReactiveFormsModule} from "@angular/forms";



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
        IonicModule,
        ReactiveFormsModule
    ]
})
export class ExpenseModule { }
