import {Component, Input, OnInit} from '@angular/core';
import {Balance} from "../group.service";

@Component({
  selector: 'balancecard',
  templateUrl: './balancecard.component.html',
  styleUrls: ['./balancecard.component.scss'],
})
export class BalancecardComponent  implements OnInit {

  constructor() { }

  ngOnInit() {}

  @Input() balance!: Balance
}
