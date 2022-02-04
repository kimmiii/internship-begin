import { Component, Inject } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';

import { Appointment } from '../../../../models';

@Component({
  selector: 'app-cancel-appointment-modal',
  templateUrl: './cancel-appointment-modal.component.html',
})
export class CancelAppointmentModalComponent {
  appointment: Appointment;

  cancelFormGroup: FormGroup;

  readonly cancelMotivationFormControlName = 'cancelMotivation';

  constructor(
    private formBuider: FormBuilder,
    private dialogRef: MatDialogRef<CancelAppointmentModalComponent>,
    @Inject(MAT_DIALOG_DATA) appointment: Appointment,
  ) {
    this.createForm();
    this.appointment = appointment;
  }

  private createForm(): void {
    this.cancelFormGroup = this.formBuider.group({
      [this.cancelMotivationFormControlName]: [ null ],
    });
  }

  cancel(): void {
    this.dialogRef.close();
  }

  cancelAppointment(): void {
    this.dialogRef.close(this.cancelFormGroup.value);
  }
}

export interface CancelAppointment {
  cancelMotivation: string;
}
