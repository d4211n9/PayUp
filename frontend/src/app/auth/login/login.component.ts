import {Component, OnInit} from '@angular/core';
import {FormBuilder, Validators} from "@angular/forms";
import {ToastController} from "@ionic/angular";
import {firstValueFrom} from "rxjs";
import {AccountService, Credentials} from "../account.service";
import {TokenService} from "../../../services/TokenService";
import {Router} from "@angular/router";
import {ToolbarComponent} from "../../toolbar/toolbar.component";
import {HomePageModule} from "../../home/home.module";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent implements OnInit {

  readonly form = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(7)]],
  });

  constructor(
    private readonly fb: FormBuilder,
    private readonly toast: ToastController,
    private readonly service: AccountService,
    private readonly token: TokenService,
    private readonly router: Router,
    private readonly tb: ToolbarComponent
  ) {
  }

  get email() {
    return this.form.controls.email;
  }

  get password() {
    return this.form.controls.password;
  }

  ngOnInit() {
  }

  async submit() {
    if (this.form.invalid) return;
    const {token} = await firstValueFrom(this.service.login(this.form.value as Credentials));
    this.token.setToken(token);

    await (await this.toast.create({
      message: "Welcome back!",
      color: "success",
      duration: 5000
    })).present();

    await this.router.navigate(['/groups'])

    //Refresh toolbar to show logged-in user
    this.tb.loggedInUser = await this.service.getCurrentUser()
    location.reload()
  }
}
