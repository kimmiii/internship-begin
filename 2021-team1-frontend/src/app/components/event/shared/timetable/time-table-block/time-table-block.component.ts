import { Component, EventEmitter, Input, Output } from '@angular/core';

import { Appointment, AppointmentStatus } from '../../../../../models';
import { AppointmentType } from '../time-table.component';

@Component({
  selector: 'app-time-table-block',
  templateUrl: './time-table-block.component.html',
  styleUrls: ['./time-table-block.component.scss']
})
export class TimeTableBlockComponent {

  @Input() appointment: Appointment;
  @Input() appointmentType: AppointmentType;
  @Input() showDetailedData: boolean;

  @Output() onShowAppointmentInfoClicked: EventEmitter<string> = new EventEmitter<string>();
  @Output() onConfirmAppointmentClicked: EventEmitter<Appointment> = new EventEmitter<Appointment>();

  readonly AppointmentType = AppointmentType;
  readonly AppointmentStatus = AppointmentStatus;

  showAppointmentInfo(): void {
    this.onShowAppointmentInfoClicked.emit(this.appointment.id);
  }

  confirmAppointment(): void {
    this.onConfirmAppointmentClicked.emit(this.appointment);
  }
}
