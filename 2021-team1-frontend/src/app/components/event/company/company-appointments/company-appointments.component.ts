import { Component, OnInit } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material';
import { Title } from '@angular/platform-browser';
import { forkJoin } from 'rxjs';
import { filter, switchMap, takeUntil } from 'rxjs/operators';

import { Appointment, AppointmentStatus, Event, EventCompany } from '../../../../models';
import { FileStorage } from '../../../../models/file-storage.model';
import { AppointmentService } from '../../../../services/appointment.service';
import { EventService } from '../../../../services/event.service';
import { BaseComponent } from '../../../../shared/components/base/base.component';
import { NotificationService } from '../../../../shared/services/notification.service';
import { CancelAppointment, CancelAppointmentModalComponent } from '../cancel-appointment-modal/cancel-appointment-modal.component';

@Component({
  selector: 'app-company-appointments',
  templateUrl: './company-appointments.component.html',
  styleUrls: [ './company-appointments.component.scss' ],
})
export class CompanyAppointmentsComponent extends BaseComponent implements OnInit {
  activeEvent: Event;
  appointments: Appointment[];
  companyRegistrationInfo: EventCompany;

  showAppointmentDetail = false;
  appointmentDetail: Appointment;
  appointmentFiles: FileStorage[];

  constructor(
    private eventService: EventService,
    private appointmentService: AppointmentService,
    public dialog: MatDialog,
    private title: Title,
    private notificationService: NotificationService,
  ) {
    super();
    this.title.setTitle('Handshake Event | Agenda');
  }

  ngOnInit(): void {
    this.fetchEventAndAppointmentsData();
  }

  private fetchEventAndAppointmentsData(): void {
    forkJoin([
      this.eventService.getActiveEvent(),
      this.eventService.getCompanyRegistration(),
      this.appointmentService.getCompanyAppointments([ AppointmentStatus.RESERVED, AppointmentStatus.CONFIRMED ]),
    ]).pipe(
      takeUntil(this.destroy$),
    ).subscribe(([ activeEvent, companyRegistrationInfo, companyAppointments ]) => {
      this.activeEvent = activeEvent;
      this.companyRegistrationInfo = companyRegistrationInfo;
      this.appointments = companyAppointments;
      this.setUpdatedAppointmentDetailOrHideDetailPanel();
    });
  }

  private setUpdatedAppointmentDetailOrHideDetailPanel(): void {
    if (this.appointmentDetail) {
      this.appointmentDetail = this.appointments.find((companyAppointment: Appointment) =>
        companyAppointment.id === this.appointmentDetail.id);

      if (!this.appointmentDetail) {
        this.showAppointmentDetail = false;
      }
    }
  }

  showAppointmentDetailPanel(appointmentId: string): void {
    forkJoin([
      this.appointmentService.getAppointmentById(appointmentId),
      this.appointmentService.getFilesForAppointment(appointmentId),
    ]).pipe(
      takeUntil(this.destroy$),
    ).subscribe(([ appointment, files ]) => {
      this.showAppointmentDetail = true;
      this.appointmentDetail = appointment;
      this.appointmentFiles = files;
    });
  }

  hasUnconfirmedAppointments(appointments: Appointment[]): boolean {
    return appointments && appointments.some((appointment: Appointment) => appointment.appointmentStatus === AppointmentStatus.RESERVED);
  }

  confirmAppointment(appointment: Appointment): void {
    this.appointmentService.updateAppointment({
      ...appointment,
      appointmentStatus: AppointmentStatus.CONFIRMED,
    }).pipe(
      takeUntil(this.destroy$),
    ).subscribe(() => {
      this.notificationService.showSuccessSnackBar('De afspraak is bevestigd');
      this.fetchEventAndAppointmentsData();
    });
  }

  cancelAppointment(appointment: Appointment): void {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.width = '800px';
    dialogConfig.data = appointment;
    const dialogRef = this.dialog.open(CancelAppointmentModalComponent, dialogConfig);

    dialogRef
      .afterClosed()
      .pipe(
        filter((cancelAppointment: CancelAppointment) => !!cancelAppointment),
        switchMap((cancelAppointment: CancelAppointment) => this.appointmentService.updateAppointment({
          ...appointment,
          appointmentStatus: AppointmentStatus.CANCELLED,
          cancelMotivation: cancelAppointment.cancelMotivation,
        })),
        takeUntil(this.destroy$))
      .subscribe(() => {
        this.notificationService.showSuccessSnackBar('De afspraak is geannuleerd');
        this.fetchEventAndAppointmentsData();
      });
  }

  closeAppointmentDetailPanel(): void {
    this.showAppointmentDetail = false;
  }

  saveAppointment(appointment: Appointment): void {
    this.appointmentService.updateAppointment(appointment)
      .pipe(
        takeUntil(this.destroy$),
      ).subscribe(() => {
      this.notificationService.showSuccessSnackBar('Je wijzigingen zijn opgeslaan');
      this.fetchEventAndAppointmentsData();
    });
  }
}
