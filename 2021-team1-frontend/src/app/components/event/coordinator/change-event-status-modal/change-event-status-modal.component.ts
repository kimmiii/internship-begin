import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';

import { Event } from '../../../../models';

@Component({
  selector: 'app-change-event-status-modal',
  templateUrl: './change-event-status-modal.component.html',
})
export class ChangeEventStatusModalComponent {
  stateAction: StateAction;

  constructor(
    public dialogRef: MatDialogRef<ChangeEventStatusModalComponent>,
    @Inject(MAT_DIALOG_DATA) event: Event,
  ) {
    this.stateAction = event.isActivated
      ? StateAction.DEACTIVE
      : StateAction.ACTIVATE;
  }

  cancel(): void {
    this.dialogRef.close();
  }
}

export enum StateAction {
  ACTIVATE = 'activeren',
  DEACTIVE = 'deactiveren',
}
