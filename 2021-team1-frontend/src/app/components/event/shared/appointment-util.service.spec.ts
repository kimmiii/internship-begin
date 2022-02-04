import { HttpClientTestingModule } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';

import { AppointmentUtilService } from './appointment-util.service';

describe('AppointmentUtilService', () => {

  beforeEach(() => TestBed.configureTestingModule({
    imports: [MatDialogModule, HttpClientTestingModule],
    providers: [
      { provide: MAT_DIALOG_DATA, useValue: {} }
    ]
  }));

  it('should be created', () => {
    const service: AppointmentUtilService = TestBed.get(AppointmentUtilService);
    expect(service).toBeTruthy();
  });
});
