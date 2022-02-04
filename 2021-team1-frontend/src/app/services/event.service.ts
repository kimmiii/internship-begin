import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, ObservableInput, of } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { environment } from '../../environments/environment';
import { AcademicYear, Attendee, Event, EventCompany } from '../models';
import { Company } from '../models/company.model';
import { Internship } from '../models/internship.model';
import { Specialisation } from '../models/specialisation.model';

@Injectable({
  providedIn: 'root',
})
export class EventService {
  constructor(private http: HttpClient, private router: Router) {
  }

  getEvents(): Observable<Event[]> {
    return this.http.get<Event[]>(`${environment.eventUrl}events`);
  }

  getActiveEvent(): Observable<Event> {
    return this.http.get<Event>(`${environment.eventUrl}events/active`);
  }

  getAcademicYears(): Observable<AcademicYear[]> {
    return this.http.get<AcademicYear[]>(
      `${environment.eventUrl}academic-years`,
    );
  }

  addEvent(event: Event): Observable<Event> {
    return this.http.post<Event>(`${environment.eventUrl}events`, event);
  }

  updateEventStatus(event: Event): Observable<Event> {
    return this.http.put<Event>(`${environment.eventUrl}events`, event);
  }

  getCompanyInfo(): Observable<Company> {
    return this.http.get<Company>(`${environment.eventUrl}companies`);
  }

  getCompanyRegistration(id?: number): Observable<EventCompany> {
    return this.http.get<EventCompany>(`${environment.eventUrl}events/company/${id || ''}`);
  }

  registerCompany(eventCompany: EventCompany): Observable<EventCompany> {
    return this.http.post<EventCompany>(`${environment.eventUrl}events/company`, eventCompany);
  }

  addAttendee(attendee: Attendee): Observable<Attendee> {
    return this.http.post<Attendee>(`${environment.eventUrl}events/company/attendee`, attendee);
  }

  getEventSpecialisations(): Observable<Specialisation[]> {
    return this.http
      .get<Specialisation[]>(`${environment.eventUrl}filters/specialisations`)
      .pipe(catchError(this.handleError.bind(this)));
  }

  getAllApprovedInternshipsOfCompany(): Observable<Internship[]> {
    return this.http.get<Internship[]>(`${environment.eventUrl}events/internships/company`);
  }

  saveInternships(internships: Internship[]): Observable<Internship[]> {
    return this.http.put<Internship[]>(`${environment.eventUrl}internships/multi`, internships);
  }

  handleError(err: HttpErrorResponse): ObservableInput<HttpErrorResponse> {
    if (err.status === 400) {
      return of(err);
    } else if (err.status === 401) {
      this.router.navigateByUrl('/login');
    } else if (err.status === 404) {
      return of(err);
    } else {
      this.router.navigateByUrl('/server-error');
    }
  }
}
