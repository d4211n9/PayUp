import {Router} from "@angular/router";
import {Component, OnInit, ViewChild} from '@angular/core';
import {IonSelect, PopoverController, PopoverOptions} from '@ionic/angular';

@Component({
  selector: 'toolbar',
  templateUrl: './toolbar.component.html',
  styleUrls: ['./toolbar.component.scss'],
})
export class ToolbarComponent  implements OnInit {

  constructor(private router: Router, private popoverController: PopoverController) { }

  ngOnInit() {}

  toHome() {
    this.router.navigate(['/groups'])
  }

  toProfile() {
    this.router.navigate(['/profile'])
  }

    protected readonly onclick = onclick;


  @ViewChild('mySelect') mySelect: IonSelect;

  // Define popover options
  popoverOptions: PopoverOptions = {
    cssClass: 'my-custom-class', // Optional custom class for styling
    positionStrategy: 'fixed', // Use 'fixed' position strategy
    event: event, // Event for positioning (can be MouseEvent or undefined)
    componentProps: {}, // Optional properties to pass to the popover component
  };


  openSelect(event: any) {
    // Set the event property for positioning
    this.popoverOptions.event = event;

    // Open the ion-select popover
    this.mySelect.open(event);
  }
}
}
