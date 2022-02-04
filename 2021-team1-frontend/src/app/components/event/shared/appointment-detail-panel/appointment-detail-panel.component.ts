import { Component, EventEmitter, Input, OnChanges, Output, SecurityContext, SimpleChanges } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { DomSanitizer } from '@angular/platform-browser';
import { takeUntil } from 'rxjs/operators';

import { Appointment, AppointmentStatus, Event, EventCompany } from '../../../../models';
import { FileStorage } from '../../../../models/file-storage.model';
import { AppointmentService } from '../../../../services/appointment.service';
import { BaseComponent } from '../../../../shared/components/base/base.component';
import { AppointmentUtilService } from '../appointment-util.service';

@Component({
  selector: 'app-appointment-detail-panel',
  templateUrl: './appointment-detail-panel.component.html',
  styleUrls: [ './appointment-detail-panel.component.scss' ],
})
export class AppointmentDetailPanelComponent extends BaseComponent implements OnChanges {

  @Input() showAppointmentDetailPanel: boolean;
  @Input() appointmentDetail: Appointment;
  @Input() appointmentFiles: FileStorage[];
  @Input() activeEvent: Event;
  @Input() companyRegistrationInfo: EventCompany;
  @Input() closeable = true;
  @Input() canUpdateAppointment = true;

  @Output() onCancelAppointmentClicked: EventEmitter<Appointment> = new EventEmitter<Appointment>();
  @Output() onConfirmAppointmentClicked: EventEmitter<Appointment> = new EventEmitter<Appointment>();
  @Output() onCloseAppointmentClicked: EventEmitter<boolean> = new EventEmitter<boolean>();
  @Output() onSaveAppointmentClicked: EventEmitter<Appointment> = new EventEmitter<Appointment>();

  expandedAppointmentDetail = false;

  editAppointmentFormGroup: FormGroup;

  readonly teamsLinkFormControlName = 'onlineMeetingLink';

  readonly AppointmentStatus = AppointmentStatus;

  get teamsLink(): string {
    return this.editAppointmentFormGroup.get(this.teamsLinkFormControlName).value;
  }

  constructor(
    private appointmentService: AppointmentService,
    private formBuilder: FormBuilder,
    private domSanitizer: DomSanitizer
  ) {
    super();
    this.createForm();
  }

  ngOnChanges(simpleChanges: SimpleChanges): void {
    if (simpleChanges.appointmentDetail) {
      if (this.canUpdateAppointment && this.appointmentDetail) {
        this.editAppointmentFormGroup.patchValue({
          [this.teamsLinkFormControlName]: this.appointmentDetail.onlineMeetingLink,
        });
      }
    }
  }

  openFile(file: FileStorage): void {
    this.appointmentService.downloadFile(this.appointmentDetail.id, file.fileName)
      .pipe(
        takeUntil(this.destroy$),
      ).subscribe((appointmentFile: Blob) => {
      const data = new Blob([ appointmentFile ], { type: 'application/pdf' });

      const url = window.URL.createObjectURL(data);
      window.open(url);
    });
  }

  hideAppointmentDetail(): void {
    if (this.showAppointmentDetailPanel) {
      this.onCloseAppointmentClicked.emit(true);
    }
  }

  expandAppointmentDetail(): void {
    this.expandedAppointmentDetail = !this.expandedAppointmentDetail;
  }

  getDetailPanelClasses(): string[] {
    if (this.showAppointmentDetailPanel) {
      const openDetailClass = 'appointment-detail--open';
      return this.expandedAppointmentDetail
        ? [ openDetailClass, 'appointment-detail--expanded' ]
        : [ openDetailClass, 'appointment-detail--normal' ];
    } else {
      return [ 'appointment-detail--closed' ];
    }
  }

  canCancelAppointment(): boolean {
    return AppointmentUtilService.canCancelAppointment(this.activeEvent, this.companyRegistrationInfo);
  }

  cancelAppointment(): void {
    this.onCancelAppointmentClicked.emit(this.appointmentDetail);
  }

  confirmAppointment(): void {
    this.onConfirmAppointmentClicked.emit(this.appointmentDetail);
  }

  getAppointmentTitle(): string {
    if (this.appointmentDetail) {
      if (this.appointmentDetail.studentName) {
        return `Afspraak met ${this.appointmentDetail.studentName}`;
      }
      return `Jouw afspraak bij ${this.appointmentDetail.companyName}`;
    }
  }

  private createForm(): void {
    this.editAppointmentFormGroup = this.formBuilder.group({
      [this.teamsLinkFormControlName]: [ null ],
    });
  }

  saveAppointment(): void {
    this.onSaveAppointmentClicked.emit({
      ...this.appointmentDetail,
      onlineMeetingLink: this.teamsLink,
    });
  }

  openTeamsLink(): void {
    window.open(this.domSanitizer.sanitize(SecurityContext.URL, this.appointmentDetail.onlineMeetingLink), '_blank');
  }
}
