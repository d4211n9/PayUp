import { Component, OnInit } from '@angular/core';
import {LoginComponent} from "../login/login.component";
import {FormBuilder, Validators} from "@angular/forms";
import {firstValueFrom} from "rxjs";
import {Credentials} from "../account.service";
import {TokenService} from "../../../services/TokenService";

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
})
export class RegisterComponent  implements OnInit {

  readonly form = this.fb.group({
    name: ['', Validators.required],
    email: ['', [Validators.required, Validators.email]],
    phone: ['', Validators.required],
    password: ['', Validators.required],
    repeatPassword: ['', Validators.required],
  });

  get name() { return this.form.controls.name; }
  get email() { return this.form.controls.email; }
  get phone() { return this.form.controls.phone; }
  get password() { return this.form.controls.password; }
  get repeatPassword() { return this.form.controls.repeatPassword; }


  constructor(
      private readonly fb: FormBuilder,
      private readonly token: TokenService,
  ) { }

  ngOnInit() {}


  async register() {
    if (this.form.invalid) return;
    const {token} = await firstValueFrom(this.service.login(this.form.value as Credentials));
    this.token.setToken(token);


    (await this.toast.create({
      message: "Welcome back!",
      color: "success",
      duration: 5000
    })).present();
  }
}
