import {Component, OnInit} from '@angular/core';
import {FormBuilder, Validators} from "@angular/forms";
import {finalize} from "rxjs";
import {CreateGroup, GroupService} from "../group.service";
import {ToastController} from "@ionic/angular";
import {HttpEventType} from "@angular/common/http";
import {Router} from "@angular/router";


@Component({
  selector: 'app-create',
  templateUrl: './create.component.html',
  styleUrls: ['./create.component.scss'],
})
export class CreateComponent  implements OnInit {

  uploading: boolean = false;
  imageUrl: string | ArrayBuffer | null = null;

  constructor(
    private readonly fb: FormBuilder,
    private readonly service: GroupService,
    private readonly toast: ToastController,
    private readonly router: Router
  ) { }

  ngOnInit() {}

  protected readonly _now = new Date().toLocaleDateString("da-DK", {timeZone: 'UTC'});

  form = this.fb.group({
    name: ['', Validators.required],
    description: ['', Validators.required],
    image: [null as File | null],
  });

  get name() {
    return this.form.controls.name;
  }

  get description() {
    return this.form.controls.description;
  }

  onFileChanged($event: Event) {
    const files = ($event.target as HTMLInputElement).files;
    if (!files) return;
    this.uploading = true;
    this.form.patchValue({image: files[0]});
    this.form.controls.image.updateValueAndValidity();
    const reader = new FileReader();
    reader.readAsDataURL(files[0]);
    reader.onload = () => {
      this.imageUrl = reader.result;
    }
    this.uploading = false;
  }

  async submit() {
    if (this.form.invalid) return;
    this.uploading = true;
    this.service.create(this.form.value as CreateGroup)
      .pipe(finalize(() => {
        this.router.navigate(['/groups']).then(() => location.reload())
      }))
      .subscribe(event => {
        if (event.type == HttpEventType.Response && event.body) {
          this.form.patchValue(event.body);
        }
      });

    await (await this.toast.create({
      message: "Your group was created successfully",
      color: "success",
      duration: 5000
    })).present()
      .then(() => location.reload);
  }
}
