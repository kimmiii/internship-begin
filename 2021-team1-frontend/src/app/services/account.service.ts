import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { environment } from 'src/environments/environment';

import { Login } from '../models/api-models/login.model';
import { Register } from '../models/api-models/register.model';
import { JWTToken } from '../models/jwt.model';
import { User } from '../models/user.model';

@Injectable()
export class AccountService {

  constructor(private http: HttpClient, private router: Router) { }

  // GET api/user/getreviewers
  getReviewers(): Observable<User[]> {
    return this.http.get<User[]>(`${environment.baseApiUrl}user/getreviewers`).pipe(
      catchError(this.handleError.bind(this))
    );
  }

  // GET api/user/getstudents
  getStudents(): Observable<User[]> {
    return this.http.get<User[]>(`${environment.baseApiUrl}user/getstudents`).pipe(
      catchError(this.handleError.bind(this))
    );
  }

  // GET api/user/removecv/id
  removecV(id: number): Observable<any> {
    return this.http.get(`${environment.baseApiUrl}user/removecv/${id}`, { observe: 'response' }).pipe(
      catchError(err => this.handleError(err))
    );
  }

  // GET: api/user/GetUserById/{userId}
  getUserById(userId: number): Observable<any> {
    return this.http.get<User>(`${environment.baseApiUrl}user/getuserbyid/${userId}`).pipe(
      catchError(err => this.handleError(err))
    );
  }

  // GET api/user/downloadCV/{studentId}
  downloadCV(studentId: number): Observable<Blob> {
    return this.http.get(`${environment.baseApiUrl}user/downloadCV/${studentId}`,
      {  responseType: 'blob' });
  }

  // GET api/account/GetCsvTemplate/{userTypeCode}
  getCsvTemplate(userTypeCode: string): Observable<Blob> {
    return this.http.get(`${environment.baseApiUrl}User/GetCsvTemplate/${userTypeCode}`, { responseType: 'blob' });
  }

  // POST api/account/login
  login(login: Login): Observable<JWTToken> {
    return this.http.post<JWTToken>(`${environment.baseApiUrl}account/login`, login);
  }

  // POST api/account/register/company
  registerUser(user: Register): any {
    return this.http.post<Register>(`${environment.baseApiUrl}account/register/company`, user, { observe: 'response' }).pipe(
      catchError(err => this.handleError(err))
    );
  }

  // POST api/account/postcsvlector
  addLectorsByCSV(file: File): any {
    const formData: FormData = new FormData();
    formData.append('postedFile', file);

    return this.http.post<any>(`${environment.baseApiUrl}account/postcsvlector`, formData, { observe: 'response' }).pipe(
      catchError(err => this.handleError(err))
    );
  }

  // POST api/account/postcsvstudent
  addStudentsByCSV(file: File): any {
    const formData: FormData = new FormData();
    formData.append('postedFile', file);

    return this.http.post<any>(`${environment.baseApiUrl}account/postcsvstudent`, formData, { observe: 'response' }).pipe(
      catchError(err => this.handleError(err))
    );
  }

  // POST api/user/uploadpdf
  uploadPDF(file: File, id: number): any {
    const formData: FormData = new FormData();
    formData.append('postedPDF', file);

    return this.http.post<any>(`${environment.baseApiUrl}user/uploadpdf/${id}`, formData, { observe: 'response' }).pipe(
      catchError(err => this.handleError((err)))
    );
  }


  handleError(err: HttpErrorResponse) {
    if (err.status == 400) {
      return of(err);
    } else if (err.status == 401) {
      return of(err.message);
    } else if (err.status == 409) {
        return of(err.message);
      } else {
        this.router.navigateByUrl('/server-error');
      }
  }
}
