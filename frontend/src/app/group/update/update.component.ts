import {Component, OnInit} from '@angular/core';
import {finalize, firstValueFrom} from "rxjs";
import {FormBuilder, Validators} from "@angular/forms";
import {GroupService, GroupUpdate} from "../group.service";
import {ActivatedRoute, Router} from "@angular/router";
import {HttpEventType} from "@angular/common/http";

@Component({
  selector: 'group-update',
  templateUrl: './update.component.html',
  styleUrls: ['./update.component.scss'],
})
export class UpdateComponent  implements OnInit {
  id: any;
  loading: boolean = true;
  uploading: boolean = false;
  uploadProgress: number | null = null;

  form = this.fb.group({
    name: ['', Validators.required],
    description: ['', Validators.required],
    image: [null as File | null],
  });
  imageUrl: string | ArrayBuffer | null = null;

  constructor(
    private readonly fb: FormBuilder,
    private readonly service: GroupService,
    private route: ActivatedRoute,
    private router: Router
  ) {
  }

  async ngOnInit() {
    await this.getId()
    const group = await firstValueFrom(this.service.getGroup(this.id));
    this.form.patchValue(group);
    this.imageUrl = group.imageUrl ?? null;
    this.loading = false;
  }

  async getId() {
    const map = await firstValueFrom(this.route.paramMap)
    this.id = map.get('groupId')
  }

  get name() {
    return this.form.controls.name
  }

  get description() {
    return this.form.controls.description
  }

  onFileChanged($event: Event) {
    const files = ($event.target as HTMLInputElement).files;
    if (!files) return;
    this.form.patchValue({image: files[0]});
    this.form.controls.image.updateValueAndValidity();
    const reader = new FileReader();
    reader.readAsDataURL(files[0]);
    reader.onload = () => {
      this.imageUrl = reader.result;
    }
  }

  submit() {
    if (this.form.invalid) return;
    this.uploading = true;
    this.service.update(this.form.value as GroupUpdate, this.id)
      .pipe(finalize(() => {
        this.uploading = false;
        this.uploadProgress = null;
        this.router.navigate(['/groups/'+this.id])
      }))
      .subscribe(event => {
        if (event.type == HttpEventType.UploadProgress) {
          this.uploadProgress = Math.round(100 * (event.loaded / (event.total ?? 1)))
        } else if (event.type == HttpEventType.Response && event.body) {
          this.form.patchValue(event.body);
        }
      });
  }
}
