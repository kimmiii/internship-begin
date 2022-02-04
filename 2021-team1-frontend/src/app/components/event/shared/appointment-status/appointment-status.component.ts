import { Component, Input } from '@angular/core';

import { AppointmentStatus } from '../../../../models';

@Component({
  selector: 'app-appointment-status',
  templateUrl: './appointment-status.component.html',
  styleUrls: [ './appointment-status.component.scss' ],
})
export class AppointmentStatusComponent {

  @Input() appointmentStatus: AppointmentStatus;

}
