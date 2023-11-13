import { Component, OnInit } from '@angular/core';
import {LoginComponent} from "../login/login.component";

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
})
export class RegisterComponent  implements OnInit {

  constructor() { }

  ngOnInit() {}

  protected readonly LoginComponent = LoginComponent;
}
