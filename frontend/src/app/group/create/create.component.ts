import {Component, OnInit} from '@angular/core';
import {FormBuilder, Validators} from "@angular/forms";
import {firstValueFrom} from "rxjs";
import {Group, GroupService} from "../group.service";

@Component({
  selector: 'app-create',
  templateUrl: './create.component.html',
  styleUrls: ['./create.component.scss'],
})
export class CreateComponent  implements OnInit {

  constructor(
    private readonly fb: FormBuilder,
    private readonly service: GroupService
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
    var groupInfo: Group = {
      name: this.form.controls.name.value!,
      description: this.form.controls.description.value!,
      imageUrl: 'https://cdn-icons-png.flaticon.com/512/615/615075.png', //TODO fix hardcoding when image upload is done (also in html)
    };

    if(this.form.invalid) return;

    await firstValueFrom(this.service.create(groupInfo as Group));
  }
}
