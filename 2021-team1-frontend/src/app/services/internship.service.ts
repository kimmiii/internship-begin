import { HttpClient, HttpErrorResponse, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, of } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';

import { environment } from '../../environments/environment';
import { GetApprovedInternshipCriteriaModel } from '../models/api-models/get-approved-internship-criteria.model';
import { Country } from '../models/country.model';
import { Environment } from '../models/environment.model';
import { Expecation } from '../models/expectation.model';
import { InternshipReduced } from '../models/internship-reduced.model';
import { Technology } from '../models/internship-technology.model';
import { Internship } from '../models/internship.model';
import { Period } from '../models/period.model';
import { ProjectStatus } from '../models/project-status.model';
import { Specialisation } from '../models/specialisation.model';
import { UserFavourites } from '../models/user-favourites.model';
import { UserInternships } from '../models/user-internships.model';
import { User } from '../models/user.model';

@Injectable()
export class InternshipService {
    constructor(private http: HttpClient, private router: Router) {}

  // GET api/internships
  getInternships(): Observable<Internship[]> {
    return this.http
      .get<Internship[]>(`${environment.baseApiUrl}internships`)
      .pipe(catchError(this.handleError.bind(this)));
  }

  // GET api/events/internships
  getEventInternships(): Observable<Internship[]> {
    return this.http
      .get<Internship[]>(`${environment.eventUrl}events/internships`)
      .pipe(catchError(this.handleError.bind(this)));
  }

  // GET api/events/internships
  filterEventInternships(
    cities: string[],
    selectedCompanies: number[],
    selectedSpecialisations: number[],
    selectedTechnologies: number[])
    : Observable<Internship[]>{
    return this.http
    .get<Internship[]>(`${environment.eventUrl}events/internships`)
    .pipe(
      map(res => {
        if (cities.length > 0) {
          return res.filter(internship => cities.indexOf(internship.wpCity) !== -1);
        }
        else{
          return res;
        }
      }),
      map(res => {
        if (selectedCompanies.length > 0) {
          return res.filter(internship => selectedCompanies.indexOf(internship.companyId) !== -1);
        }
        else{
          return res;
        }
      }),
      map(res => {
        if (selectedSpecialisations.length > 0) {
          return res.filter(internship => {
            const specialisations = internship.internshipSpecialisation;
            for(let i=0; i<specialisations.length; i++){
              if(selectedSpecialisations.indexOf(specialisations[i].specialisationId) !== -1){
                  return true;
              }
            }
          })
        }
        else{
          return res;
        }
      }),
      map(res => {
        if (selectedTechnologies.length > 0) {
          return res.filter(internship => {
            const environments = internship.internshipEnvironment;
            for(let i=0; i<environments.length; i++){
              if(selectedTechnologies.indexOf(environments[i].environmentId) !== -1){
                  return true;
              }
            }
          })
        }
        else{
          return res;
        }
      }),
      catchError(this.handleError.bind(this)));
  }


  // GET events/internships/company
  getEventInternshipsByCompanyId(companyId: number): Observable<Internship[]> {
    const compareFn = (a, b) => {
      // chronologically
      if (a.createdAt < b.createdAt) {
        return -1;
      }
      // reversed chronologically
      if (a.createdAt > b.createdAt) {
        return 1;
      }
      return 0;
    };

    return this.http
      .get<Internship[]>(
        `${environment.eventUrl}events/internships/company/${companyId}`,
      )
      .pipe(tap((results) => results.sort(compareFn)));
  }

  // GET api/internships/getBySpecialisation/{specialisationCode}
  getBySpecialisation(specialisationCode: string): Observable<Internship[]> {
    return this.http
      .get<Internship[]>(
        `${environment.baseApiUrl}internships/getBySpecialisation/${specialisationCode}`,
      )
      .pipe(catchError(this.handleError.bind(this)));
  }

  // GET api/internships/getNotBySpecialisationEICT
  getNotBySpecialisationEICT(): Observable<Internship[]> {
    return this.http
      .get<Internship[]>(
        `${environment.baseApiUrl}internships/getNotBySpecialisationEICT`,
      )
      .pipe(catchError(this.handleError.bind(this)));
  }

  // GET events/internships/{id}
  getEventInternshipById(internshipId: string): any {
    return this.http
      .get<Internship>(`${environment.eventUrl}events/internships/${internshipId}`)
      .pipe(catchError((err) => this.handleError(err)));
  }

  // GET api/internships/{id}
  getInternshipById(internshipId: string): any {
    return this.http
      .get<Internship>(`${environment.baseApiUrl}internships/${internshipId}`)
      .pipe(catchError((err) => this.handleError(err)));
  }

  // GET events/company/{id}/logo
  getCompanyLogoById(internshipId: string): any {
    return this.http
      .get<string>(`${environment.baseApiUrl}company/${internshipId}/logo`)
      .pipe(catchError((err) => this.handleError(err)));
  }

  // GET api/internships/{id}/getCountFavourites
  getCountFavouriteInternshipsById(internshipId: string): any {
    return this.http
      .get<number>(
        `${environment.baseApiUrl}internships/${internshipId}/getCountFavourites`,
      )
      .pipe(catchError((err) => this.handleError(err)));
  }

  // GET api/internships/{id}/GetCountFavouritesByStudentId
  getCountFavouritesByStudentId(studentId: number): any {
    return this.http
      .get<number>(
        `${environment.baseApiUrl}internships/${studentId}/getCountFavouritesByStudentId`,
      )
      .pipe(catchError((err) => this.handleError(err)));
  }

  // GET api/internships/{id}/GetApplicationsByStudentId
  getApplicationsByStudentId(studentId: number): any {
    return this.http
      .get<number>(
        `${environment.baseApiUrl}internships/${studentId}/GetApplicationsByStudentId`,
      )
      .pipe(catchError((err) => this.handleError(err)));
  }

  //GET api/internships/getappliedstudents/{internshipId}
  getAppliedStudents(internshipId: string): Observable<User[]> {
    return this.http
      .get<User[]>(
        `${environment.baseApiUrl}internships/getappliedstudents/${internshipId}`,
      )
      .pipe(catchError(this.handleError.bind(this)));
  }

  // GET api/internships/getCountAppliedStudents/{internshipId}
  getCountAppliedStudents(internshipId: string): any {
    return this.http
      .get<number>(
        `${environment.baseApiUrl}internships/getCountAppliedStudents/${internshipId}`,
      )
      .pipe(catchError((err) => this.handleError(err)));
  }

  // GET api/internships/{companyId}/getbycompany
  getInternshipByCompanyId(companyId: number): Observable<Internship[]> {
    const compareFn = (a, b) => {
      // chronologically
      if (a.createdAt < b.createdAt) {
        return -1;
      }
      // reversed chronologically
      if (a.createdAt > b.createdAt) {
        return 1;
      }
      return 0;
    };

    return this.http
      .get<Internship[]>(
        `${environment.baseApiUrl}internships/${companyId}/getbycompany`,
      )
      .pipe(tap((results) => results.sort(compareFn)));
  }

  // GET api/internships/{reviewerId}/getbyreviewer
  getInternshipsByReviewerId(reviewerId: number): Observable<Internship[]> {
    return this.http
      .get<Internship[]>(
        `${environment.baseApiUrl}internships/${reviewerId}/getbyreviewer`,
      )
      .pipe(catchError(this.handleError.bind(this)));
  }

  // GET api/internships/specialisaton
  getSpecialisations(): Observable<Specialisation[]> {
    return this.http
      .get<Specialisation[]>(
        `${environment.baseApiUrl}internships/specialisaton`,
      )
      .pipe(catchError(this.handleError.bind(this)));
  }

  // GET api/filters/technologies
  getEventTechnologies(): Observable<Technology[]> {
    return this.http
      .get<Technology[]>(`${environment.eventUrl}filters/environments`)
      .pipe(catchError(this.handleError.bind(this)));
  }

  // GET api/internships/environment
  getEnvironments(): Observable<Environment[]> {
    return this.http
      .get<Environment[]>(`${environment.baseApiUrl}internships/environment`)
      .pipe(catchError(this.handleError.bind(this)));
  }

  // GET api/internships/expectation
  getExpectations(): Observable<Expecation[]> {
    return this.http
      .get<Expecation[]>(`${environment.baseApiUrl}internships/expectation`)
      .pipe(catchError(this.handleError.bind(this)));
  }

  // GET api/internships/period
  getPeriods(): Observable<Period[]> {
    return this.http
      .get<Period[]>(`${environment.baseApiUrl}internships/period`)
      .pipe(catchError(this.handleError.bind(this)));
  }

  // GET api/internships/{id}/pdf
  createPDFByInternshipId(internshipId: string): any {
    return this.http
      .get(`${environment.baseApiUrl}internships/${internshipId}/pdf`, {
        responseType: 'blob',
      })
      .pipe(catchError((err) => this.handleError(err)));
  }

  // GET api/internships/projectstatus
  getStatus(): Observable<ProjectStatus[]> {
    return this.http
      .get<ProjectStatus[]>(
        `${environment.baseApiUrl}internships/projectstatus`,
      )
      .pipe(catchError(this.handleError.bind(this)));
  }

  // GET api/internships/projectstatus/getstatusbyid?statusId={id}
  getStatusById(statusId: number): Observable<ProjectStatus> {
    return this.http
      .get<ProjectStatus>(
        `${environment.baseApiUrl}internships/projectstatus/getstatusbyid?statusId=${statusId}`,
      )
      .pipe(catchError(this.handleError.bind(this)));
  }

  // GET api/generic/country
  getCountries(): Observable<Country[]> {
    const compareOnName = (a, b) => {
      // alphabetically
      if (a.name < b.name) {
        return -1;
      }
      // reversed alphabetically
      if (a.name > b.name) {
        return 1;
      }
      return 0;
    };

    return this.http
      .get<Country[]>(`${environment.baseApiUrl}generic/country`)
      .pipe(tap((results) => results.sort(compareOnName)));
  }

  // GET api/internships/getUserInternshipByInternshipIdAndStudentId/{internshipId}/{studentId}
  getUserInternshipByInternshipIdAndStudentId(
    internshipId: number,
    studentId: number,
  ): Observable<UserInternships> {
    return this.http
      .get<UserInternships>(
        `${environment.baseApiUrl}internships/getUserInternshipByInternshipIdAndStudentId/${internshipId}/${studentId}`,
      )
      .pipe(catchError(this.handleError.bind(this)));
  }

  // GET api/internships/getHireRequestedInternshipsByStudentId/{studentId}
  getHireRequestedInternshipsByStudentId(
    studentId: number,
  ): Observable<UserInternships[]> {
    return this.http
      .get<UserInternships[]>(
        `${environment.baseApiUrl}internships/getHireRequestedInternshipsByStudentId/${studentId}`,
      )
      .pipe(catchError(this.handleError.bind(this)));
  }

  // GET api/internships/SetHireRequestedToTrue/{internshipId}/{userId}
  setHireRequestedToTrue(
    internshipId: number,
    userId: number,
  ): Observable<UserInternships[]> {
    return this.http
      .get<UserInternships[]>(
        `${environment.baseApiUrl}internships/setHireRequestedToTrue/${internshipId}/${userId}`,
      )
      .pipe(catchError(this.handleError.bind(this)));
  }

  // GET api/internhips/SetHireConfirmedToTrue/{internshipId}/{userId}
  setHireConfirmedToTrue(
    internshipId: number,
    userId: number,
  ): Observable<UserInternships[]> {
    return this.http
      .get<UserInternships[]>(
        `${environment.baseApiUrl}internships/SetHireConfirmedToTrue/${internshipId}/${userId}`,
      )
      .pipe(catchError(this.handleError.bind(this)));
  }

  // GET api/internships/SetHireApprovedToTrue/{internshipId}/{userId}
  setHireApprovedToTrue(internshipId: number, userId: number): Observable<any> {
    return this.http
      .get<any>(
        `${environment.baseApiUrl}internships/setHireApprovedToTrue/${internshipId}/${userId}`,
      )
      .pipe(catchError(this.handleError.bind(this)));
  }

  // GET api/internships/studentConfirmedHireRequest/{studentId}
  studentConfirmedHireRequest(studentId: number): Observable<boolean> {
    return this.http
      .get<boolean>(
        `${environment.baseApiUrl}internships/studentConfirmedHireRequest/${studentId}`,
      )
      .pipe(catchError(this.handleError.bind(this)));
  }

  // GET api/internships/studentApprovedHireRequest/{studentId}
  studentApprovedHireRequest(studentId: number): Observable<boolean> {
    return this.http
      .get<boolean>(
        `${environment.baseApiUrl}internships/studentApprovedHireRequest/${studentId}`,
      )
      .pipe(catchError(this.handleError.bind(this)));
  }

  // GET api/internships/GetUserInternships
  getUserInternships(): Observable<UserInternships[]> {
    return this.http
      .get<UserInternships[]>(
        `${environment.baseApiUrl}internships/GetUserInternships`,
      )
      .pipe(catchError(this.handleError.bind(this)));
  }

  // GET api/internships/getInternshipIdByInternshipAssignedUser/{userId}
  getInternshipIdByInternshipAssignedUser(userId: number): Observable<number> {
    return this.http
      .get<number>(
        `${environment.baseApiUrl}internships/getInternshipIdByInternshipAssignedUser/${userId}`,
      )
      .pipe(catchError(this.handleError.bind(this)));
  }

  // GET api/internships/GetAcademicYears
  getAcademicYears(): Observable<string[]> {
    return this.http
      .get(`${environment.baseApiUrl}internships/GetAcademicYears`)
      .pipe(catchError(this.handleError.bind(this)));
  }

  /*
  // GET/internships/GetInternshipsByUserId/{userId}
  getInternshipsByUserId(userId: number): Observable<Internship[]> {
      return this.http.get<Internship[]>(`${environment.baseApiUrl}internships/getInternshipsByUserId/${userId}`).pipe(
        catchError(this.handleError.bind(this))
      );
  }
   */

  // POST api/internships/approved
  getApprovedInternships(
    criteriaObject: GetApprovedInternshipCriteriaModel,
  ): Observable<HttpResponse<any>> {
    return this.http
      .post<GetApprovedInternshipCriteriaModel>(
        `${environment.baseApiUrl}internships/approved`,
        criteriaObject,
        { observe: 'response' },
      )
      .pipe(catchError(this.handleError.bind(this)));
  }

  // POST api/internships
  addInternship(internshipToAdd: Internship): Observable<HttpResponse<any>> {
    return this.http
      .post<Internship>(
        `${environment.baseApiUrl}internships`,
        internshipToAdd,
        {
          observe: 'response',
        },
      )
      .pipe(catchError(this.handleError.bind(this)));
  }

  // POST api/internships/setfavourite
  setFavourite(userFavourites: UserFavourites): Observable<HttpResponse<any>> {
    return this.http
      .post<UserFavourites>(
        `${environment.baseApiUrl}internships/setfavourite`,
        userFavourites,
        {
          observe: 'response',
        },
      )
      .pipe(catchError(this.handleError.bind(this)));
  }

  // POST api/internships/deletefavourite
  deleteFavourite(
    userFavourites: UserFavourites,
  ): Observable<HttpResponse<any>> {
    return this.http
      .post<UserFavourites>(
        `${environment.baseApiUrl}internships/deletefavourite`,
        userFavourites,
        { observe: 'response' },
      )
      .pipe(catchError(this.handleError.bind(this)));
  }

  // POST api/internships/applyInternship
  applyInternship(
    userInternships: UserInternships,
  ): Observable<HttpResponse<any>> {
    return this.http
      .post<UserInternships>(
        `${environment.baseApiUrl}internships/applyInternship`,
        userInternships,
        { observe: 'response' },
      )
      .pipe(catchError(this.handleError.bind(this)));
  }

  // POST api/internships/removeApplication
  removeApplication(
    userInternships: UserInternships,
  ): Observable<HttpResponse<any>> {
    return this.http
      .post<UserInternships>(
        `${environment.baseApiUrl}internships/removeApplication`,
        userInternships,
        { observe: 'response' },
      )
      .pipe(catchError(this.handleError.bind(this)));
  }

  // PUT api/internships/{id}
  editInternship(
    internshipId: string,
    internshipToEdit: Internship,
  ): Observable<any> {
    return this.http
      .put<Internship>(
        `${environment.baseApiUrl}internships/${internshipId}`,
        internshipToEdit,
        {
          observe: 'response',
        },
      )
      .pipe(catchError((err) => this.handleError(err)));
  }

  // PUT api/internships/projectstatus
  changeStatus(internship: InternshipReduced): Observable<any> {
    return this.http
      .put<Internship>(
        `${environment.baseApiUrl}internships/projectstatus`,
        internship,
        {
          observe: 'response',
        },
      )
      .pipe(catchError(this.handleError.bind(this)));
  }

  // PUT api/internships/SetHireApprovedToTrue/{internshipId}/{userId}
  setHireApprovedToFalse(
    internshipId: number,
    userId: number,
    userInternships: UserInternships,
  ): Observable<any> {
    return this.http
      .put<UserInternships>(
        `${environment.baseApiUrl}internships/setHireApprovedToFalse/${internshipId}/${userId}`,
        userInternships,
        { observe: 'response' },
      )
      .pipe(catchError((err) => this.handleError(err)));
  }

  // PUT api/internships/ToggleInterestingUserInternship/{internshipId}/{userId}
  toggleInterestingUserInternship(
    internshipId: number,
    userId: number,
  ): Observable<any> {
    return this.http
      .put(
        `${environment.baseApiUrl}internships/ToggleInterestingUserInternship/${internshipId}/${userId}`,
        { observe: 'response' },
      )
      .pipe(catchError((err) => this.handleError(err)));
  }

  handleError(err: HttpErrorResponse): Observable<HttpErrorResponse> {
    if (err.status === 400) {
      return of(err);
    } else if (err.status === 401) {
        this.router.navigateByUrl('/login');
      } else {
        if (err.status === 404) {
          return of(err);
        } else {
          this.router.navigateByUrl('/server-error');
        }
        if (err.status === 500) {
          return of(err);
        }
      }
  }
}
