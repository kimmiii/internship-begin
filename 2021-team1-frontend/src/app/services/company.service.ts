import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, ObservableInput, of } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { environment } from '../../environments/environment';
import { Company } from '../models/company.model';
import { Location } from '../models/location.model';

@Injectable()
export class CompanyService {
  constructor(private http: HttpClient, private router: Router) {}

  // GET api/company
  getAllCompanies(): Observable<Company[]> {
    return this.http
      .get<Company[]>(`${environment.baseApiUrl}company`)
      .pipe(catchError(this.handleError.bind(this)));
  }

  // GET api/filters/companies
  getAllEventCompanies(): Observable<Company[]> {
    return this.http
      .get<Company[]>(`${environment.eventUrl}filters/companies`)
      .pipe(catchError(this.handleError.bind(this)));
  }

  // GET api/filters/locations
  getEventLocations(): Observable<Location[]> {
    return this.http
      .get<Location[]>(`${environment.eventUrl}filters/locations`)
      .pipe(catchError(this.handleError.bind(this)));
  }

  // GET api/company/{id}
  getCompanyById(companyId: number): Observable<Company> {
    return this.http
      .get<Company>(`${environment.baseApiUrl}company/${companyId}`)
      .pipe(catchError(this.handleError.bind(this)));
  }

  // GET api/company/inactive
  getNewCompanies(): Observable<Company[]> {
    return this.http
      .get<Company[]>(`${environment.baseApiUrl}company/new`)
      .pipe(catchError(this.handleError.bind(this)));
  }

  // GET api/company/active
  getActiveCompanies(): Observable<Company[]> {
    return this.http
      .get<Company[]>(`${environment.baseApiUrl}company/active`)
      .pipe(catchError(this.handleError.bind(this)));
  }

  // GET api/company/evaluatedInactive
  getRejectedCompanies(): Observable<Company[]> {
    return this.http
      .get<Company[]>(`${environment.baseApiUrl}company/evaluatedInactive`)
      .pipe(catchError(this.handleError.bind(this)));
  }

  // GET api/company/getCounters/{companyId}
  getCounters(companyId: number): Observable<number[]> {
    return this.http
      .get<number[]>(
        `${environment.baseApiUrl}company/getCounters/${companyId}`,
      )
      .pipe(catchError(this.handleError.bind(this)));
  }

  // POST api/company
  addCompany(company: Company): any {
    return this.http
      .post<Company>(`${environment.baseApiUrl}company`, company, {
        observe: 'response',
      })
      .pipe(catchError((err) => this.handleError(err)));
  }

  // POST events/company/logo
  uploadLogoForCompany(file: File): Observable<void> {
    const formData = new FormData();
    formData.append('file', file, file.name);
    return this.http.post<void>(`${environment.eventUrl}events/company/logo`, formData);
  }

  // GET events/company/{id}/logo
  downloadLogoFromCompany(companyId: number): Observable<Blob> {
    return this.http.get(`${environment.eventUrl}events/company/${companyId}/logo`,
      { responseType: 'blob' });
  }

  // PUT api/company/{id}/approve
  approveCompany(companyId: number): any {
    return this.http
      .put(`${environment.baseApiUrl}company/${companyId}/approve`, '', {
        observe: 'response',
      })
      .pipe(catchError((err) => this.handleError(err)));
  }

  // PUT api/company/{id}/reject
  rejectCompany(companyId: number, evaluationFeedback: string): any {
    return this.http
      .put(
        `${environment.baseApiUrl}company/${companyId}/reject?evaluationFeedback=${evaluationFeedback}`,
        '',
        { observe: 'response' },
      )
      .pipe(catchError(this.handleError.bind(this)));
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
