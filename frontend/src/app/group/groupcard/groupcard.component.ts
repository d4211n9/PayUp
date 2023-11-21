import {Component, Input, OnInit} from '@angular/core';
import {Group} from "../group.service";

@Component({
  selector: 'app-groupcard',
  templateUrl: './groupcard.component.html',
  styleUrls: ['./groupcard.component.scss'],
})
export class GroupcardComponent  implements OnInit {

  constructor() { }

  ngOnInit() {}
  @Input() mygroups!: Group;
}
