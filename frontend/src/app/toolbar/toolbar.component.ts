import {Router} from "@angular/router";
import {Component, ComponentRef, OnInit, ViewChild} from '@angular/core';
import {PopoverOptions, IonSelect, PopoverController} from '@ionic/angular';


@Component({
  selector: 'toolbar',
  templateUrl: './toolbar.component.html',
  styleUrls: ['./toolbar.component.scss'],
})
export class ToolbarComponent  implements OnInit {

  @ViewChild('mySelect') mySelect!: IonSelect;

  // Define popover options
  popoverOptions: PopoverOptions<any> = {
    cssClass: 'my-custom-class',
    component: this.mySelect, // Provide the IonSelect component as the popover content
    event: undefined,
  };


  constructor(private router: Router, private popoverController: PopoverController) { }

  ngOnInit() {}

  toHome() {
    this.router.navigate(['/groups'])
  }

  toProfile() {
    this.router.navigate(['/profile'])
  }

    protected readonly onclick = onclick;



  openSelect(event: any) {
    // Set the event property for positioning
    this.popoverOptions.event = event;

    // Open the ion-select popover
    this.mySelect.open(event);
  }
}
