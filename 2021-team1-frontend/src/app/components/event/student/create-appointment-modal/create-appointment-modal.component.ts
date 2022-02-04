import { Component, Inject } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';

import { Appointment } from '../../../../models';

@Component({
  selector: 'app-create-appointment-modal',
  templateUrl: './create-appointment-modal.component.html',
  styleUrls: [ './create-appointment-modal.component.scss' ],
})
export class CreateAppointmentModalComponent {
  appointment: Appointment;

  createAppointmentForm: FormGroup;

  readonly cvFormControlName = 'CV';
  readonly letterFormControlName = 'letter';
  readonly commentFormControlName = 'comment';

  get CV(): File {
    return this.createAppointmentForm.get(this.cvFormControlName).value;
  }

  get letter(): File {
    return this.createAppointmentForm.get(this.letterFormControlName).value;
  }

  get comment(): string {
    return this.createAppointmentForm.get(this.commentFormControlName).value;
  }

  constructor(
    private formBuider: FormBuilder,
    private dialogRef: MatDialogRef<CreateAppointmentModalComponent>,
    @Inject(MAT_DIALOG_DATA) appointment: Appointment,
  ) {
    this.createForm();
    this.appointment = appointment;
  }

  private createForm(): void {
    this.createAppointmentForm = this.formBuider.group({
      [this.cvFormControlName]: [ null ],
      [this.letterFormControlName]: [ null ],
      [this.commentFormControlName]: [ null ],
    });
  }

  cancel(): void {
    this.dialogRef.close();
  }

  createAppointment(): void {
    this.dialogRef.close({
      appointment: {
        ...this.appointment,
        comment: this.comment,
      } as Appointment,
      CV: this.CV,
      letter: this.letter
    } as AppointmentData);
  }
}

export interface AppointmentData {
  appointment: Appointment;
  CV: File;
  letter: File;
}
