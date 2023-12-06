import {Component, Input, OnInit} from '@angular/core';
import {Transaction} from "../../group.service";

@Component({
  selector: 'app-payer-transfor-card',
  templateUrl: './payer-transfor-card.component.html',
  styleUrls: ['./payer-transfor-card.component.scss'],
})
export class PayerTransforCardComponent  implements OnInit {

  @Input()   transaction!: Transaction
  constructor() { }

  ngOnInit() {}

}
