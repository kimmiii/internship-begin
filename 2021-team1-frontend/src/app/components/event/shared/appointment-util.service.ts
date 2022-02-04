import { Injectable } from '@angular/core';
import { MatDialog } from '@angular/material';
import { DateTime, Duration } from 'luxon';
import { Observable } from 'rxjs';
import { filter, switchMap } from 'rxjs/operators';

import { Appointment, AppointmentStatus, Event, EventCompany } from '../../../models';
import { AppointmentService } from '../../../services/appointment.service';
import { ConfirmationDialogComponent } from './confirmation-dialog/confirmation-dialog.component';

@Injectable({
  providedIn: 'root',
})
export class AppointmentUtilService {

  static canCancelAppointment(event: Event, eventCompany: EventCompany): boolean {
    let dateTimeEvent = DateTime.fromISO(event.dateEvent as string);
    const startHour = DateTime.fromFormat(event.startHour, 'HH:mm:ss');
    dateTimeEvent = dateTimeEvent.set({ hour: startHour.hour, minute: startHour.minute });
    const cancelAppointmentsUntilDate = dateTimeEvent.minus(Duration.fromObject({
      hours: eventCompany.cancelAppointmentUntil,
    }));
    return DateTime.fromJSDate(new Date()) < cancelAppointmentsUntilDate;
  }

  constructor(
    private dialog: MatDialog,
    private appointmentService: AppointmentService,
  ) {
  }

  cancelAppointment(appointmentWith: string, appointment: Appointment): Observable<void> {
    return this.dialog.open(ConfirmationDialogComponent, {
      width: '500px',
      data: {
        title: 'Afspraak annuleren',
        info: `Weet je zeker dat je de afspraak met ${appointmentWith} wil annuleren?`,
      },
    }).afterClosed()
      .pipe(
        filter((confirmed: boolean) => confirmed),
        switchMap(() => this.appointmentService.updateAppointment({
            ...appointment,
            appointmentStatus: AppointmentStatus.CANCELLED,
          }),
        ));
  }
}
