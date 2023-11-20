import {Component, OnInit} from '@angular/core';
import {FormBuilder, Validators} from "@angular/forms";
import {firstValueFrom} from "rxjs";
import {Group, GroupService} from "../group.service";
import {ToastController} from "@ionic/angular";

@Component({
  selector: 'app-create',
  templateUrl: './create.component.html',
  styleUrls: ['./create.component.scss'],
})
export class CreateComponent  implements OnInit {

  constructor(
    private readonly fb: FormBuilder,
    private readonly service: GroupService,
    private readonly toast: ToastController,
  ) { }

  ngOnInit() {}

  protected readonly _now = new Date().toLocaleDateString("da-DK", {timeZone: 'UTC'});

  form = this.fb.group({
    name: ['', Validators.required],
    description: ['', Validators.required],
  });

  get name() {
    return this.form.controls.name;
  }

  get description() {
    return this.form.controls.description;
  }

  async create() {
    if(this.form.invalid) return;

    var groupInfo: Group = {
      name: this.form.controls.name.value!,
      description: this.form.controls.description.value!,
      image_url: 'https://cdn-icons-png.flaticon.com/512/615/615075.png', //TODO fix hardcoding when image upload is done (also in html)
      created_date: new Date(Date.now())
    };

    const createdGroup = await firstValueFrom(this.service.create(groupInfo as Group));
    
    await (await this.toast.create({
      message: "Your group '" + createdGroup.name + "' was created successfully",
      color: "success",
      duration: 5000
    })).present();
  }
}
