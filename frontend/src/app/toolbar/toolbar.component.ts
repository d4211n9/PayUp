import { Component, OnInit } from '@angular/core';
import {Router} from "@angular/router";

@Component({
  selector: 'toolbar',
  templateUrl: './toolbar.component.html',
  styleUrls: ['./toolbar.component.scss'],
})
export class ToolbarComponent  implements OnInit {

  constructor(private router: Router) { }

  ngOnInit() {}

  toHome() {
    this.router.navigate(['/groups'])
  }

  toProfile() {
    this.router.navigate(['/profile'])
  }
}
