import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { environment } from '../../environments/environment';
import { Appointment, AppointmentStatus } from '../models';
import { FileStorage } from '../models/file-storage.model';

@Injectable({
  providedIn: 'root',
})
export class AppointmentService {

  constructor(
    private http: HttpClient,
  ) {
  }

  getAppointmentById(appointmentId: string): Observable<Appointment> {
    return this.http.get<Appointment>(`${environment.eventUrl}events/appointments/${appointmentId}`);
  }

  getCompanyAppointments(status: AppointmentStatus[], companyId?: number): Observable<Appointment[]> {
    return this.http.get<Appointment[]>(`${environment.eventUrl}events/appointments/company${(companyId && '/' + companyId) || ''}`,
      { params: this.mapStatusParam(status) });
  }

  getStudentAppointments(status: AppointmentStatus[]): Observable<Appointment[]> {
    return this.http.get<Appointment[]>(`${environment.eventUrl}events/appointments/student`, { params: this.mapStatusParam(status) });
  }

  createAppointment(appointment: Appointment): Observable<Appointment> {
    return this.http.post<Appointment>(`${environment.eventUrl}events/appointments`, appointment);
  }

  uploadFileForAppointment(appointmentId: string, file: File): Observable<void> {
    const formData = new FormData();
    formData.append('file', file, file.name);
    return this.http.post<void>(`${environment.eventUrl}events/appointments/${appointmentId}/files`, formData);
  }

  getFilesForAppointment(appointmentId: string): Observable<FileStorage[]> {
    return this.http.get<FileStorage[]>(`${environment.eventUrl}events/appointments/${appointmentId}/files`);
  }

  downloadFile(appointmentId: string, fileName: string): Observable<Blob> {
    return this.http.get(`${environment.eventUrl}events/appointments/${appointmentId}/files/${fileName}`,
      { responseType: 'blob' });
  }

  updateAppointment(appointment: Appointment): Observable<void> {
    return this.http.put<void>(`${environment.eventUrl}events/appointments`, appointment);
  }

  getAllAppointments(status: AppointmentStatus[]): Observable<Appointment[]> {
    return this.http.get<Appointment[]>(`${environment.eventUrl}events/appointments`, { params: this.mapStatusParam(status) });
  }

  private mapStatusParam(status: AppointmentStatus[]): HttpParams {
    let params = new HttpParams();
    if (status && status.length) {
      params = params.set('status', status.join(','));
    }
    return params;
  }
}
