import {Component, Input, OnInit} from '@angular/core';
import {Transaction} from "../../group.service";

@Component({
  selector: 'app-payee-transfor-card',
  templateUrl: './payee-transfor-card.component.html',
  styleUrls: ['./payee-transfor-card.component.scss'],
})
export class PayeeTransforCardComponent  implements OnInit {

  @Input()   transaction!: Transaction
  constructor() { }

  ngOnInit() {}

}
