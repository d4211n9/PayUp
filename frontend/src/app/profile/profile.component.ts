import {Component, OnInit} from '@angular/core';
import {EditUserDto, FullUser, ProfileService} from "./profile.service";
import {Observable} from "rxjs";
import {FormBuilder, Validators} from "@angular/forms";
import {ToastController} from "@ionic/angular";

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss'],
})
export class ProfileComponent  implements OnInit {


   fullUser$!: Observable<FullUser>;
  editMode = false;
  editedUser!: EditUserDto;
  constructor(private readonly service: ProfileService,
              private readonly fb: FormBuilder,
              private readonly toast: ToastController,) {}

  ngOnInit() {
     this.fullUser$ = this.service.getCurrentUser();
   }

  form = this.fb.group({
    email: ['', Validators.required],
    fullName: ['', Validators.required],
    phone: ['', Validators.required],
    imageUrl: [''],
  });



  enterEditMode() {
    this.editMode = true;
    // Copy the values to the editedUser object
    this.fullUser$!.subscribe(user => {this.editedUser = { ...user}
      this.form.controls.imageUrl.setValue(user.profileUrl);});

  }

  exitEditMode() {
    this.editMode = false;
  }

  async saveChanges() {
    var user: EditUserDto = {
      email: this.form.controls.email.value!,
      fullName: this.form.controls.fullName.value!,
      phoneNumber: this.form.controls.phone.value!,
      profileUrl: this.form.controls.imageUrl.value!
    };

    this.fullUser$ = this.service.editCurrentUser(user as EditUserDto);

    await (await this.toast.create({
      message: "Your profile was updated successfully",
      color: "success",
      duration: 5000
    })).present();
    this.exitEditMode();
  }
}
