import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { AcademicYear, Event, EventLocation } from '../../../../models';
import { EventService } from '../../../../services/event.service';
import { BaseComponent } from '../../../../shared/components/base/base.component';

@Component({
  selector: 'app-create-event-modal',
  templateUrl: './create-event-modal.component.html',
  styleUrls: [ './create-event-modal.component.scss' ],
})
export class CreateEventModalComponent extends BaseComponent implements OnInit {
  createEventForm: FormGroup;

  locations: EventLocation[] = [ EventLocation.ONLINE, EventLocation.ON_CAMPUS ];
  academicYears$: Observable<AcademicYear[]>;
  academicYearsWithEvent: string[];

  todayDate: Date;
  EventLocation = EventLocation;

  readonly nameFormControlName = 'name';
  readonly dateFormControlName = 'dateEvent';
  readonly locationFormControlName = 'location';
  readonly academicYearFormControlName = 'academicYearId';
  readonly startHourFormControlName = 'startHour';
  readonly endHourFormControlName = 'endHour';

  constructor(
    public dialogRef: MatDialogRef<CreateEventModalComponent>,
    private formBuilder: FormBuilder,
    private eventService: EventService,
    @Inject(MAT_DIALOG_DATA) academicYearsWithEvents: string[],
  ) {
    super();
    this.createForm();
    this.todayDate = new Date();
    this.academicYearsWithEvent = academicYearsWithEvents;
  }

  ngOnInit(): void {
    this.getAcademicYears();
  }

  private createForm(): void {
    this.createEventForm = this.formBuilder.group({
      [this.nameFormControlName]: [ null, Validators.required ],
      [this.dateFormControlName]: [ null, Validators.required ],
      [this.locationFormControlName]: [ null, Validators.required ],
      [this.academicYearFormControlName]: [ null, Validators.required ],
      [this.startHourFormControlName]: [ null, Validators.required ],
      [this.endHourFormControlName]: [ null, Validators.required ],
    });
  }

  private getAcademicYears(): void {
    this.academicYears$ = this.eventService
      .getAcademicYears()
      .pipe(
        map((academicYears: AcademicYear[]) => academicYears
          .filter((academicYear: AcademicYear) => !this.academicYearsWithEvent.includes(academicYear.id))),
      );
  }

  cancel(): void {
    this.dialogRef.close();
  }

  generateEvent(): void {
    this.dialogRef.close({
      ...this.createEventForm.value,
      isActivated: false,
    } as Event);
  }
}
