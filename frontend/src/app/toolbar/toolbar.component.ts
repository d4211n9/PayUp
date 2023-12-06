import {Component, OnInit, ViewChild} from '@angular/core';
import {Router} from "@angular/router";
import {AccountService, User} from "../auth/account.service";
import {AuthGuard} from "../../services/AuthGuard";
import {IonModal, PopoverController} from "@ionic/angular";
import {NotificationService, NotificationSettingsDto} from "../notification/notification.service";
import {firstValueFrom} from "rxjs";


@Component({
  selector: 'toolbar',
  templateUrl: './toolbar.component.html',
  styleUrls: ['./toolbar.component.scss'],
})
export class ToolbarComponent implements OnInit {
  loggedInUser: User | undefined
  isUserLoaded: boolean = false
  lastUpdate: Date | undefined;
  notificationAmount: number | undefined;
  @ViewChild(IonModal) modal: IonModal | undefined;
  constructor(
    private notificationService: NotificationService,
    private router: Router,
    private service: AccountService,
    private authGuard: AuthGuard,
    private popoverController: PopoverController,

  ) {}

  async ngOnInit() {
    this.getCurrentUser()

  }

  async getCurrentUser() {
    if(this.authGuard.isLoggedIn()) {
      this.loggedInUser = await this.service.getCurrentUser()
    } else {
      this.loggedInUser = undefined
    }
    this.isUserLoaded = true
  }

  toHome() {
    this.router.navigate(['/groups'])
  }

  toProfile() {
    this.router.navigate(['/profile'])
    this.popoverController.dismiss()
  }

  toLogin() {
    this.router.navigate(['/login'])
  }

  async logout() {
    this.service.logout()
    this.popoverController.dismiss()
  }


  cancel() {
    this.modal!.dismiss(null, 'cancel');
  }

  async confirm() {
    // Create the NotificationSettingsDto object
    const settings: NotificationSettingsDto = {
      userId: 0, //is replaced in backend with session data anyway
      inviteNotification: this.InviteAppToggle,
      inviteNotificationEmail: this.InviteEmailToggle,
      expenseNotification: this.ExpenseAppToggle,
      expenseNotificationEmail: this.ExpenseEmailToggle
    };

     let isEdit = await firstValueFrom(this.notificationService.updateUserNotificationSettings(settings));
     if (isEdit)
       this.modal!.dismiss('fewfwe', 'confirm');
  }

  public settingsData: NotificationSettingsDto | undefined;
  InviteAppToggle: any;
  InviteEmailToggle: any;
  ExpenseAppToggle: any;
  ExpenseEmailToggle: any;

  async getSettings() {
    this.settingsData = await this.notificationService.getNotificationSettings();

    this.InviteAppToggle = this.settingsData.inviteNotification;
    this.InviteEmailToggle = this.settingsData.inviteNotificationEmail;
    this.ExpenseAppToggle = this.settingsData.expenseNotification;
    this.ExpenseEmailToggle = this.settingsData.expenseNotificationEmail;
  }

  setNewSettings() {
    return false;
  }
}
