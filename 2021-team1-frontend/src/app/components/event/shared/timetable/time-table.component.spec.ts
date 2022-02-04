import { DatePipe } from '@angular/common';
import { NO_ERRORS_SCHEMA } from '@angular/core';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { DateTime, Interval } from 'luxon';

import { Appointment } from '../../../../models';
import { TimeTableComponent } from './time-table.component';

describe('TimeTableComponent', () => {
  let component: TimeTableComponent;
  let fixture: ComponentFixture<TimeTableComponent>;

  const start = getMockDateTime(18, 30).toISOString();
  const end = getMockDateTime(22, 30).toISOString();
  const timeSlot = 20;

  function getMockDateTime(hours: number, minutes: number): Date {
    const mockDate = new Date();
    mockDate.setHours(hours, minutes);
    return mockDate;
  }

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TimeTableComponent ],
      providers: [ DatePipe ],
      schemas: [NO_ERRORS_SCHEMA]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TimeTableComponent);
    component = fixture.componentInstance;
    component.start = start;
    component.end = end;
    component.timeSlot = timeSlot;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should not return appointment when end student appointment is equal to start searched time slot', () => {
    // ARRANGE
    const appointment: Appointment = {
      beginHour: getMockDateTime(20, 20).toISOString(),
      endHour: getMockDateTime(20, 30).toISOString(),
    } as Appointment;
    component.studentAppointments = [ appointment ];

    const searchTimeSlot: Interval = Interval.fromDateTimes(
      DateTime.fromJSDate(getMockDateTime(20, 30)),
      DateTime.fromJSDate(getMockDateTime(20, 50)),
    );

    // ACT
    const result = component.findStudentAppointmentForTimeSlot(searchTimeSlot);

    // ASSERT
    expect(result).toBeFalsy();
  });

});
