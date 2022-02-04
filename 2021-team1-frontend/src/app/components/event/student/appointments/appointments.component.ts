import { Component, OnInit, SecurityContext } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { Router } from '@angular/router';
import { forkJoin } from 'rxjs';
import { takeUntil } from 'rxjs/operators';

import { Appointment, AppointmentStatus } from '../../../../models';
import { AppointmentService } from '../../../../services/appointment.service';
import { BaseComponent } from '../../../../shared/components/base/base.component';

@Component({
  selector: 'app-appointments',
  templateUrl: './appointments.component.html',
  styleUrls: [ './appointments.component.scss' ],
})
export class AppointmentsComponent extends BaseComponent implements OnInit {

  studentActiveAppointments: Appointment[];
  studentCancelledAppointments: Appointment[];

  constructor(
    private appointmentService: AppointmentService,
    private router: Router,
    private domSanitizer: DomSanitizer
  ) {
    super();
  }

  ngOnInit(): void {
    this.fetchStudentAppointments();
  }

  private fetchStudentAppointments(): void {
    forkJoin([
      this.appointmentService.getStudentAppointments([ AppointmentStatus.RESERVED, AppointmentStatus.CONFIRMED ]),
      this.appointmentService.getStudentAppointments([ AppointmentStatus.CANCELLED ]),
    ]).pipe(
      takeUntil(this.destroy$),
    ).subscribe(([ activeAppointments, cancelledAppointments ]) => {
      this.studentActiveAppointments = activeAppointments;
      this.studentCancelledAppointments = cancelledAppointments;
    });
  }

  public routeToStudentAppointmentsPage(companyId: number, internshipId: number): void {
    this.router.navigate([ '/event/student/appointments' ], {
      queryParams:
        {
          companyId: companyId,
          internshipId: internshipId,
        },
    });
  }

  openTeamsLink(appointment: Appointment): void {
    window.open(this.domSanitizer.sanitize(SecurityContext.URL, appointment.onlineMeetingLink), '_blank');
  }
}
