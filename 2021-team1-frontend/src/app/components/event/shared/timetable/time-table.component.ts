import { Component, EventEmitter, Input, OnChanges, Output } from '@angular/core';
import { DateTime, Duration, Interval } from 'luxon';

import { Appointment, AppointmentStatus, Attendee } from '../../../../models';

@Component({
  selector: 'app-time-table',
  templateUrl: './time-table.component.html',
  styleUrls: [ './time-table.component.scss' ],
})
export class TimeTableComponent implements OnChanges {

  @Input() timeSlot: number;
  @Input() start: string;
  @Input() end: string;
  @Input() attendees: Attendee[];
  @Input() companyAppointments: Appointment[];
  @Input() studentAppointments?: Appointment[];
  @Input() studentCompanyAppointment: Appointment;
  @Input() canConfirm = false;
  @Input() showDetailedData = false;
  @Input() selectable = true;

  @Output() createAppointment: EventEmitter<Appointment> = new EventEmitter<Appointment>();
  @Output() showAppointmentInfo: EventEmitter<string> = new EventEmitter<string>();
  @Output() confirmAppointment: EventEmitter<Appointment> = new EventEmitter<Appointment>();

  timeSlots: Interval[] = [];

  AppointmentType = AppointmentType;

  private readonly dataTimeSpanFormat = 'HH:mm:ss';
  private readonly displayTimeSpanFormat = 'HH:mm';

  ngOnChanges(): void {
    this.calculateTimeIntervals();
  }

  private calculateTimeIntervals(): void {
    if (this.start && this.end && this.timeSlot && this.attendees && this.attendees.length) {
      const startDateTime = DateTime.fromFormat(this.start, this.dataTimeSpanFormat);
      const endDateTime = DateTime.fromFormat(this.end, this.dataTimeSpanFormat);
      this.timeSlots = [];

      let interval = Interval.after(startDateTime, Duration.fromObject({ minutes: this.timeSlot }));

      while (interval.isBefore(endDateTime)) {
        this.timeSlots.push(interval);
        interval = Interval.after(interval.end, Duration.fromObject({ minutes: this.timeSlot }));
      }
    }
  }

  getAppointmentForAttendeeAndTimeSlot(attendee: Attendee, timeSlot: Interval): Appointment {
    return this.findAppointmentWithCompany(attendee, timeSlot)
      || this.findCompanyAppointmentForTimeslot(attendee, timeSlot)
      || this.findStudentAppointmentForTimeSlot(timeSlot);
  }

  findCompanyAppointmentForTimeslot(attendee: Attendee, timeSlot: Interval): Appointment {
    const foundMatch = this.companyAppointments.find((appointment: Appointment) => {
      const appointmentInterval = Interval.fromDateTimes(
        DateTime.fromFormat(appointment.beginHour, this.dataTimeSpanFormat),
        DateTime.fromFormat(appointment.endHour, this.dataTimeSpanFormat),
      );
      return appointment.attendeeId === attendee.id && appointmentInterval.equals(timeSlot);
    });

    if (foundMatch) {
      return {
        ...foundMatch,
        type: foundMatch.appointmentStatus === AppointmentStatus.CONFIRMED
          ? this.returnAppointmentTypeConfirmed()
          : this.returnAppointmentTypeReserved(),
      };
    }
  }

  findAppointmentWithCompany(attendee: Attendee, timeSlot: Interval): Appointment {
    if (this.studentCompanyAppointment) {
      const appointmentInterval = Interval.fromDateTimes(
        DateTime.fromFormat(this.studentCompanyAppointment.beginHour, this.dataTimeSpanFormat),
        DateTime.fromFormat(this.studentCompanyAppointment.endHour, this.dataTimeSpanFormat),
      );

      if (appointmentInterval.equals(timeSlot) && this.studentCompanyAppointment.attendeeId === attendee.id) {
        return {
          ...this.studentCompanyAppointment,
          type: AppointmentType.COMPANY_STUDENT_MATCH,
        };
      }
    }
  }

  findStudentAppointmentForTimeSlot(timeSlot: Interval): Appointment {
    if (this.studentAppointments && this.studentAppointments.length) {
      const foundMatch = this.studentAppointments.find((appointment: Appointment) => {
        const appointmentInterval = Interval.fromDateTimes(
          DateTime.fromFormat(appointment.beginHour, this.dataTimeSpanFormat),
          DateTime.fromFormat(appointment.endHour, this.dataTimeSpanFormat),
        );
        return appointmentInterval.overlaps(timeSlot);
      });

      if (foundMatch) {
        return {
          ...foundMatch,
          type: AppointmentType.STUDENT,
        };
      }
    }
  }

  selectFreeSpot(attendee: Attendee, timeSlot: Interval): void {
    if (!this.studentCompanyAppointment) {
      this.createAppointment.emit({
        attendeeId: attendee.id,
        attendeeName: `${attendee.firstName} ${attendee.lastName}`,
        beginHour: timeSlot.start.toFormat(this.displayTimeSpanFormat),
        endHour: timeSlot.end.toFormat(this.displayTimeSpanFormat),
      } as Appointment);
    }
  }

  private returnAppointmentTypeConfirmed(): AppointmentType {
    return this.showDetailedData ? AppointmentType.COMPANY_CONFIRMED : AppointmentType.STUDENT_CONFIRMED;
  }

  private returnAppointmentTypeReserved(): AppointmentType {
    return this.showDetailedData ? AppointmentType.COMPANY_RESERVED : AppointmentType.STUDENT_RESERVED
  }
}

export enum AppointmentType {
  STUDENT_CONFIRMED = 'student-confirmed',
  COMPANY_CONFIRMED = 'company-confirmed',
  STUDENT_RESERVED = 'student-reserved',
  COMPANY_RESERVED = 'company-reserved',
  STUDENT = 'overlap',
  COMPANY_STUDENT_MATCH = 'student-company-match'
}
