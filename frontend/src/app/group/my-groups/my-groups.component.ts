import {Component, Input, OnInit} from '@angular/core';
import {Group} from "../group.service";

@Component({
  selector: 'app-my-groups',
  templateUrl: './my-groups.component.html',
  styleUrls: ['./my-groups.component.scss'],
})
export class MyGroupsComponent  implements OnInit {

  constructor() { }

  ngOnInit() {}

  @Input() group!: Group;

  mygroups: Group[] = [];



}
