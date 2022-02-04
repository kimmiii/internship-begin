import { Injectable } from '@angular/core';
import jwtDecode, { JwtPayload } from 'jwt-decode';

import { FilteredInternshipModel } from '../models/filtered-internship.model';
import { JWTClaims } from '../models/jwt.model';

@Injectable()
export class StateManagerService {
  token: string;
  claims: JWTClaims = {} as JWTClaims;

  isLoggedIn = false;

  readonly tokenStorageKey = 'token';

  get userId(): number {
    return this.claims.nameid;
  }

  get userFirstName(): string {
    return this.claims.firstName;
  }

  get userSurname(): string {
    return this.claims.surname;
  }

  get userEmailAddress(): string {
    return this.claims.email;
  }

  get roleCode(): string {
    return this.claims.roleCode;
  }

  get roleDescription(): string {
    return this.claims.roleDescription;
  }

  get companyId(): number {
    return this.claims.companyId;
  }

  get cvPresent(): boolean {
    return this.claims.cvPresent;
  }

  set cvPresent(cvPresent: boolean) {
    this.claims.cvPresent = cvPresent;
  }

  get companyName(): string {
    return this.claims.companyName;
  }

  // FILTER COORDINATOR
  valueSelectedStudentOrderByCoordinator = 1;
  selectedFilterIndexInternshipsCoordinator = 1;
  selectedFilterAcademicYearCoordinator = '';
  selectedFilterPeriodCoordinator = 0;
  selectedFilterAcademicYearCompany = '';

  // FILTER COMPANY
  valueSelectedStudentOrderByCompany = 1;
  selectedFilterIndexFinInternshipsCoordinator = 1;

  // STUDENT FILTERING
  filteredInternship: FilteredInternshipModel = null;
  hideCompletedInternships = false;
  filterShowFavouritesAppliedValue: number;
  filterCompany: number = null;
  filterInternshipEnvironmentOther = '';
  filterAssignmentDescription = '';
  filterInternshipSpecialisation: boolean[] = [];
  filterInternshipEnvironment: boolean[] = [];
  filterInternshipPeriod: boolean[] = [];
  filterExpectationList: boolean[] = [];

  constructor() {
    this.fetchStoredToken();
  }

  private fetchStoredToken(): void {
    const token = sessionStorage.getItem(this.tokenStorageKey);
    if (token && this.validateToken(token)) {
      this.token = token;
      this.isLoggedIn = true;
    } else {
      this.resetToken();
      this.isLoggedIn = false;
    }
  }

  validateToken(token: string): boolean {
    const claims = jwtDecode<JwtPayload>(token) as JWTClaims;
    this.claims = claims;

    return Date.now() < claims.exp * 1000;
  }

  setTokenAndClaims(token: string, claims: JWTClaims): void {
    this.token = token;
    this.claims = claims;
    sessionStorage.setItem(this.tokenStorageKey, token);
  }

  // Student filter
  resetFilter(): void {
    this.filteredInternship = null;
    this.hideCompletedInternships = false;
    this.filterShowFavouritesAppliedValue = 0;
    this.filterCompany = null;
    this.filterInternshipEnvironmentOther = '';
    this.filterAssignmentDescription = '';
    this.filterInternshipSpecialisation = [];
    this.filterInternshipEnvironment = [];
    this.filterInternshipPeriod = [];
    this.filterExpectationList = [];
  }

  // Other filters
  reset(): void {
    this.isLoggedIn = false;
    this.resetToken();
    this.selectedFilterIndexInternshipsCoordinator = 1;
    this.selectedFilterIndexFinInternshipsCoordinator = 1;
    this.valueSelectedStudentOrderByCoordinator = 1;
    this.valueSelectedStudentOrderByCompany = 1;
    this.selectedFilterAcademicYearCoordinator = '';
    this.selectedFilterPeriodCoordinator = 0;
    this.selectedFilterAcademicYearCompany = '';
  }

  private resetToken(): void {
    sessionStorage.removeItem(this.tokenStorageKey);
    this.token = null;
    this.claims = {} as JWTClaims;
  }
}
