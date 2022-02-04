import { Component, OnInit } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute, Params } from '@angular/router';
import { forkJoin, Observable, of } from 'rxjs';
import { catchError, filter, switchMap, takeUntil, tap } from 'rxjs/operators';

import { Appointment, AppointmentStatus, Event, EventCompany } from '../../../../models';
import { FileStorage } from '../../../../models/file-storage.model';
import { AppointmentService } from '../../../../services/appointment.service';
import { EventService } from '../../../../services/event.service';
import { BaseComponent } from '../../../../shared/components/base/base.component';
import { NotificationService } from '../../../../shared/services/notification.service';
import { AppointmentUtilService } from '../../shared/appointment-util.service';
import { AppointmentData, CreateAppointmentModalComponent } from '../create-appointment-modal/create-appointment-modal.component';

@Component({
  selector: 'app-student-appointment',
  templateUrl: './student-appointment.component.html',
  styleUrls: [ './student-appointment.component.scss' ],
})
export class StudentAppointmentComponent extends BaseComponent implements OnInit {
  activeEvent: Event;
  companyAppointmentInfo: EventCompany;
  companyAppointments: Appointment[];
  studentAppointments: Appointment[];
  studentCompanyAppointment: Appointment;

  companyId: number;
  internshipId: number;

  CV: File;
  letter: File;
  appointmentFiles$: Observable<FileStorage[]> = of([]);

  constructor(
    private appointmentService: AppointmentService,
    private eventService: EventService,
    private dialog: MatDialog,
    private activatedRoute: ActivatedRoute,
    private notificationService: NotificationService,
    private title: Title,
    private appointmentUtilService: AppointmentUtilService,
  ) {
    super();
    this.title.setTitle('Handshake Event | Afspraken');
    this.activatedRoute.queryParams
      .pipe(
        takeUntil(this.destroy$),
      ).subscribe((queryParams: Params) => {
      if (queryParams.companyId) {
        this.companyId = Number(queryParams.companyId);
      }
      if (queryParams.internshipId) {
        this.internshipId = Number(queryParams.internshipId);
      }
    });
  }

  ngOnInit(): void {
    this.fetchAppointmentData();
  }

  private fetchAppointmentData(): void {
    forkJoin([
      this.eventService.getActiveEvent(),
      this.eventService.getCompanyRegistration(this.companyId),
      this.appointmentService.getCompanyAppointments([ AppointmentStatus.RESERVED, AppointmentStatus.CONFIRMED ], this.companyId),
      this.appointmentService.getStudentAppointments([ AppointmentStatus.RESERVED, AppointmentStatus.CONFIRMED ]),
    ])
      .pipe(
        takeUntil(this.destroy$),
      ).subscribe(([ activeEvent, companyAppointmentInfo, companyAppointments, studentAppointments ]) => {
      this.activeEvent = activeEvent;
      this.companyAppointmentInfo = companyAppointmentInfo;
      this.studentCompanyAppointment = studentAppointments
        .find((appointment: Appointment) => appointment.companyId === this.companyId);
      this.studentAppointments = studentAppointments
        .filter((appointment: Appointment) => appointment.companyId !== this.companyId);
      this.companyAppointments = companyAppointments;

      if (this.studentCompanyAppointment) {
        this.fetchAppointmentFiles();
      }
    });
  }

  createAppointment(appointment: Appointment): void {
    const newAppointment: Appointment = {
      ...appointment,
      companyId: this.companyId,
      companyName: this.companyAppointmentInfo.companyName,
      internshipId: this.internshipId,
    };
    const dialogConfig = new MatDialogConfig();
    dialogConfig.width = '800px';
    dialogConfig.data = newAppointment;
    const dialogRef = this.dialog.open(CreateAppointmentModalComponent, dialogConfig);

    dialogRef
      .afterClosed()
      .pipe(
        filter((appointmentData: AppointmentData) => !!appointmentData),
        tap((appointmentData: AppointmentData) => {
          this.CV = appointmentData.CV;
          this.letter = appointmentData.letter;
        }),
        switchMap((appointmentData: AppointmentData) => this.appointmentService.createAppointment(appointmentData.appointment)),
        switchMap((newAppointment: Appointment) => // NOSONAR
          forkJoin([
            this.uploadFileIfAvailable(newAppointment, this.CV),
            this.uploadFileIfAvailable(newAppointment, this.letter),
          ]),
        ),
        takeUntil(this.destroy$),
      )
      .subscribe(() => {
        this.notificationService.showSuccessSnackBar(`Afspraak gemaakt bij ${this.companyAppointmentInfo.companyName}`);
        this.fetchAppointmentData();
      });
  }

  private uploadFileIfAvailable(appointment: Appointment, file: File): Observable<File | null> {
    if (file) {
      return this.appointmentService.uploadFileForAppointment(appointment.id, file)
        .pipe(catchError(() => {
          this.notificationService.showErrorSnackBar(`${file.name} uploaden mislukt`);
          return of(null);
        }));
    }
    return of(null);
  }

  private fetchAppointmentFiles(): void {
    this.appointmentFiles$ = this.appointmentService.getFilesForAppointment(this.studentCompanyAppointment.id);
  }

  cancelAppointment(): void {
    this.appointmentUtilService.cancelAppointment(this.companyAppointmentInfo.companyName, this.studentCompanyAppointment)
      .pipe(
        takeUntil(this.destroy$),
      ).subscribe(() => {
      this.notificationService.showSuccessSnackBar('Je afspraak is geannuleerd');
      this.fetchAppointmentData();
    });
  }

  canMakeAppointment(): boolean {
    return this.companyAppointmentInfo && this.companyAppointmentInfo.attendees && this.companyAppointmentInfo.attendees.length
      && !!this.companyAppointmentInfo.arrivalTime && !!this.companyAppointmentInfo.departureTime && !!this.companyAppointmentInfo.timeSlot;
  }
}
