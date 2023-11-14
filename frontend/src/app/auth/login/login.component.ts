import { Component, OnInit } from '@angular/core';
import {FormBuilder, Validators} from "@angular/forms";
import {ToastController} from "@ionic/angular";
import {firstValueFrom} from "rxjs";
import {AccountService, Credentials} from "../account.service";
import {TokenService} from "../../../services/TokenService";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent  implements OnInit {

  readonly form = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', Validators.required],
  });

  get email() { return this.form.controls.email; }
  get password() { return this.form.controls.password; }


  constructor(
      private readonly fb: FormBuilder,
      private readonly toast: ToastController,
      private readonly service: AccountService,
      private readonly token: TokenService
  ) {}

  ngOnInit() {}

  async submit() {
    if (this.form.invalid) return;
    const { token } = await firstValueFrom(this.service.login(this.form.value as Credentials));
    this.token.setToken(token);


    await (await this.toast.create({
      message: "Welcome back!",
      color: "success",
      duration: 5000
    })).present();
  }


}
