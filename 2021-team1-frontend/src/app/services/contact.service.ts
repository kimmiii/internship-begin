import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, of } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';

import { environment } from '../../environments/environment';
import { Contact } from '../models/contact.model';

@Injectable()
export class ContactService {
  constructor(private http: HttpClient, private router: Router) {}

  // GET api/contact/{id}
  getContactById(contactId: number): any {
    return this.http
      .get<Contact>(`${environment.baseApiUrl}contact/${contactId}`)
      .pipe(catchError((err) => this.handleError(err)));
  }

  // GET api/contact/{id}/getbycompany
  getContactByCompanyId(companyId: number): Observable<Contact[]> {
    const compareFn = (a, b) => {
      // Alfabetically
      if (a.surname < b.surname) {
        return -1;
      }
      // Reversed alphabetically
      if (a.surname > b.surname) {
        return 1;
      }
      return 0;
    };

    return this.http
      .get<Contact[]>(
        `${environment.baseApiUrl}contact/${companyId}/getbycompany`,
      )
      .pipe(tap((results) => results.sort(compareFn)));
  }

  // GET api/contact/SendReminderEmail/{reviewerId}/{internshipId}
  sendReminderEmail(reviewerId: number, internshipId: number): any {
    return this.http
      .get(
        `${environment.baseApiUrl}contact/sendreminderemail/${reviewerId}/${internshipId}`,
      )
      .pipe(catchError((err) => this.handleError(err)));
  }

  // POST api/contact
  addContact(contactToAdd: Contact): any {
    return this.http
      .post<Contact>(
        `${environment.baseApiUrl}contact`,
        contactToAdd,
        { observe: 'response' },
      )
      .pipe(catchError((err) => this.handleError(err)));
  }

  // PUT api/contact/{id}remove
  deleteContact(contactId: number): any {
    return this.http
      .put(
        `${environment.baseApiUrl}contact/${contactId}/remove`,
        '',
        { observe: 'response' },
      )
      .pipe(catchError((err) => this.handleError(err)));
  }

  handleError(err: HttpErrorResponse) {
    if (err.status == 400) {
      return of(err);
    } else {
      if (err.status == 401) {
        this.router.navigateByUrl('/login');
      } else {
        if (err.status == 404) {
          return of(err);
        } else {
          this.router.navigateByUrl('/server-error');
        }
      }
    }
  }
}
