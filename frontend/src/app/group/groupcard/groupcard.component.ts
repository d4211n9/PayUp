import {Component, Input, OnInit} from '@angular/core';
import {Group, GroupCard} from "../group.service";
import {Router} from "@angular/router";
import {firstValueFrom} from "rxjs";

@Component({
  selector: 'app-groupcard',
  templateUrl: './groupcard.component.html',
  styleUrls: ['./groupcard.component.scss'],
})
export class GroupcardComponent  implements OnInit {


  constructor(private router: Router) {
  }

  ngOnInit() {}
  @Input() group!: GroupCard;

  openGroup() {
    this.router.navigate(['groups/'+this.group.id])
  }
}
