import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DomSanitizer, SafeUrl, Title } from '@angular/platform-browser';
import { forkJoin } from 'rxjs';
import { takeUntil } from 'rxjs/operators';

import { Attendee, Event, EventCompany } from '../../../../models';
import { Company } from '../../../../models/company.model';
import { CompanyService } from '../../../../services/company.service';
import { EventService } from '../../../../services/event.service';
import { StateManagerService } from '../../../../services/state-manager.service';
import { BaseComponent } from '../../../../shared/components/base/base.component';
import { NotificationService } from '../../../../shared/services/notification.service';
import { RegisteredCompanyService } from '../shared/registered-company.service';

@Component({
  selector: 'app-company-settings',
  templateUrl: './company-settings.component.html',
  styleUrls: [ './company-settings.component.scss' ],
})
export class CompanySettingsComponent extends BaseComponent implements OnInit {
  company: Company;
  registration: EventCompany;
  event: Event;
  logo: SafeUrl;

  eventSettingsFormGroup: FormGroup;
  attendeeFormGroup: FormGroup;

  readonly logoFormControlName = 'logo';
  readonly websiteFormControlName = 'website';
  readonly companyDescriptionFormControlName = 'companyDescription';
  readonly attendeesFormControlName = 'attendees';
  readonly timeslotFormControlName = 'timeSlot';
  readonly createAppointmentUntilFormControlName = 'createAppointmentUntil';
  readonly cancelAppointmentUntilFormControlName = 'cancelAppointmentUntil';
  readonly arrivalTimeFormControlName = 'arrivalTime';
  readonly departureTimeFormControlName = 'departureTime';
  readonly firstNameFormControlName = 'firstName';
  readonly lastNameFormControlName = 'lastName';

  readonly defaultValueTimeSlotLength = 20;
  readonly defaultValueMaxAppointmentHours = 24;
  readonly defaultValueMaxCancelHours = 12;
  readonly maxAmountOfAttendees = 3;

  get attendees(): FormArray {
    return this.eventSettingsFormGroup.get(this.attendeesFormControlName) as FormArray;
  }

  get arrivalTime(): string {
    return this.eventSettingsFormGroup.get(this.arrivalTimeFormControlName).value;
  }

  get departureTime(): string {
    return this.eventSettingsFormGroup.get(this.departureTimeFormControlName).value;
  }

  get canAddAttendee(): boolean {
    return this.attendees && this.attendees.length < this.maxAmountOfAttendees;
  }

  constructor( // NOSONAR
    private companyService: CompanyService,
    private eventService: EventService,
    private stateManagerService: StateManagerService,
    private formBuilder: FormBuilder,
    private notificationService: NotificationService,
    private registeredCompanyService: RegisteredCompanyService,
    private title: Title,
    private domSanitizer: DomSanitizer,
  ) {
    super();
    this.title.setTitle('Handshake Event | Instellingen');
    this.createForm();
  }

  ngOnInit(): void {
    this.fetchEventAndRegistration();
    this.fetchCompanyInfo();
    this.displayLogo();
    this.registerLogoListener();
  }

  private createForm(): void {
    this.eventSettingsFormGroup = this.formBuilder.group({
      [this.logoFormControlName]: [ null ],
      [this.websiteFormControlName]: [ null ],
      [this.companyDescriptionFormControlName]: [ null ],
      [this.arrivalTimeFormControlName]: [ null ],
      [this.departureTimeFormControlName]: [ null ],
      [this.timeslotFormControlName]: [ this.defaultValueTimeSlotLength,
        [ Validators.required, Validators.min(0), Validators.max(60) ],
      ],
      [this.createAppointmentUntilFormControlName]: [ this.defaultValueMaxAppointmentHours,
        [ Validators.required, Validators.min(0), Validators.max(48) ],
      ],
      [this.cancelAppointmentUntilFormControlName]: [ this.defaultValueMaxCancelHours,
        [ Validators.required, Validators.min(0), Validators.max(48) ],
      ],
      [this.attendeesFormControlName]: this.formBuilder.array([]),
    });

    this.attendeeFormGroup = this.formBuilder.group({
      [this.firstNameFormControlName]: [ null, Validators.required ],
      [this.lastNameFormControlName]: [ null, Validators.required ],
    });
  }

  private fetchCompanyInfo(): void {
    // retrieve basic info -> should get companyId from token
    this.companyService.getCompanyById(this.stateManagerService.companyId)
      .pipe(
        takeUntil(this.destroy$),
      ).subscribe((company: Company) => this.company = company);
  }

  private displayLogo(): void {
    this.companyService
      .downloadLogoFromCompany(this.stateManagerService.companyId)
      .pipe(
        takeUntil(this.destroy$),
      ).subscribe((logo: Blob) => {
      if (logo) {
        const objectURL = URL.createObjectURL(logo);
        this.logo = this.domSanitizer.bypassSecurityTrustUrl(objectURL);
      }
    });
  }

  private fetchEventAndRegistration(): void {
    forkJoin([
      this.eventService.getActiveEvent(),
      this.eventService.getCompanyRegistration(),
    ]).pipe(
      takeUntil(this.destroy$),
    ).subscribe(([ activeEvent, companyRegistration ]) => {
      if (activeEvent) {
        this.event = activeEvent;
        this.patchEventHours();
      }

      if (companyRegistration) {
        this.registration = companyRegistration;
        this.patchValues();
        this.disableFormControlsForUpdate();
      }
    });
  }

  private patchEventHours(): void {
    this.eventSettingsFormGroup.patchValue({
      [this.arrivalTimeFormControlName]: this.event.startHour,
      [this.departureTimeFormControlName]: this.event.endHour,
    });
  }

  private patchValues(): void {
    this.eventSettingsFormGroup.patchValue({
      [this.websiteFormControlName]: this.registration.website,
      [this.companyDescriptionFormControlName]: this.registration.companyDescription,
      [this.arrivalTimeFormControlName]: this.registration.arrivalTime,
      [this.departureTimeFormControlName]: this.registration.departureTime,
      [this.timeslotFormControlName]: this.registration.timeSlot,
      [this.createAppointmentUntilFormControlName]: this.registration.createAppointmentUntil,
      [this.cancelAppointmentUntilFormControlName]: this.registration.cancelAppointmentUntil,
    });

    if (this.registration.attendees && this.registration.attendees.length) {
      this.registration.attendees.forEach((attendee: Attendee) => this.addAttendee(attendee));
    }
  }

  private disableFormControlsForUpdate(): void {
    this.eventSettingsFormGroup.get(this.websiteFormControlName).disable();
    this.eventSettingsFormGroup.get(this.companyDescriptionFormControlName).disable();
    this.eventSettingsFormGroup.get(this.arrivalTimeFormControlName).disable();
    this.eventSettingsFormGroup.get(this.departureTimeFormControlName).disable();
    this.eventSettingsFormGroup.get(this.timeslotFormControlName).disable();
    this.eventSettingsFormGroup.get(this.createAppointmentUntilFormControlName).disable();
    this.eventSettingsFormGroup.get(this.cancelAppointmentUntilFormControlName).disable();
  }

  removeAttendee(index: number): void {
    this.attendees.removeAt(index);
  }

  addAttendee(attendee: Attendee): void {
    if (this.canAddAttendee) {
      this.attendees.push(this.formBuilder.control(attendee));
      this.attendeeFormGroup.reset();
    }
  }

  saveSettings(): void {
    const eventCompany: EventCompany = {
      ...this.eventSettingsFormGroup.value,
    };

    if (!this.registration) {
      this.eventService.registerCompany(eventCompany)
        .pipe(
          takeUntil(this.destroy$),
        ).subscribe(() => {
        this.notificationService.showSuccessSnackBar('Instellingen succesvol opgeslagen');
        this.attendees.clear();
        this.fetchEventAndRegistration();
        this.registeredCompanyService.isCompanyRegistered.next(true);
      });
    } else {
      const attendeeArray = this.attendees.value as Array<Attendee>;
      forkJoin(attendeeArray
        .filter((attendee: Attendee) => !attendee.id)
        .map((attendee: Attendee) => this.eventService.addAttendee(attendee)))
        .pipe(
          takeUntil(this.destroy$)
        ).subscribe(() => {
        this.notificationService.showSuccessSnackBar('Aanwezigen toegevoegd');
        this.attendees.clear();
        this.fetchEventAndRegistration();
      })
    }
  }

  private registerLogoListener(): void {
    this.eventSettingsFormGroup.get(this.logoFormControlName).valueChanges
      .pipe(
        takeUntil(this.destroy$),
      ).subscribe((logo: File) => {
      if (logo) {
        this.companyService.uploadLogoForCompany(logo)
          .pipe(
            takeUntil(this.destroy$),
          ).subscribe(() => {
          this.displayLogo();
        });
      }
    });
  }
}
