import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Title } from '@angular/platform-browser';
import { Router } from '@angular/router';
import jwtDecode, { JwtPayload } from 'jwt-decode';
import { ObservableInput } from 'rxjs';
import { catchError, finalize, takeUntil } from 'rxjs/operators';

import { Login } from '../../../models/api-models/login.model';
import { JWTClaims, JWTToken } from '../../../models/jwt.model';
import { RoleCode } from '../../../models/role.model';
import { AccountService } from '../../../services/account.service';
import { StateManagerService } from '../../../services/state-manager.service';
import { BaseComponent } from '../../../shared/components/base/base.component';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: [ './login.component.css' ],
})
export class LoginComponent extends BaseComponent implements OnInit {
  signInForm: FormGroup;
  errorMessage: string;
  showSpinner = false;

  readonly emailFormControlName = 'userEmailAddress';
  readonly passwordFormControlName = 'password';

  readonly incorrectCredentials = 'Verkeerd e-mailadres en/of wachtwoord.';
  readonly accountNotActivated = 'Uw account is nog niet geactiveerd door de PXL-stagecoördinator.';
  readonly companyNotActivated = 'Uw bedrijf is nog niet geactiveerd door de PXL-stagecoördinator.';
  readonly accountError = 'U hebt geen geldig account om aan te melden op dit platform.';

  constructor(
    private accountService: AccountService,
    private stateManagerService: StateManagerService,
    private formBuilder: FormBuilder,
    private router: Router,
    public titleService: Title,
  ) {
    super();
  }

  ngOnInit(): void {
    this.titleService.setTitle('Stagebeheer | Aanmelden');
    this.resetErrorMessage();
    this.createForm();
  }

  createForm(): void {
    this.signInForm = this.formBuilder.group({
      [this.emailFormControlName]: [ null, [ Validators.required, Validators.email ] ],
      [this.passwordFormControlName]: [ null, [ Validators.required ] ],
    });
  }

  signIn(): void {
    this.showSpinner = true;

    const login: Login = this.signInForm.value;
    this.resetForm();

    this.accountService.login(login)
      .pipe(
        catchError((error: HttpErrorResponse) => this.handleError(error)),
        finalize(() => this.showSpinner = false),
        takeUntil(this.destroy$),
      )
      .subscribe((jwtToken: JWTToken) => {
        this.login(jwtToken.token);
      });
  }

  private handleError(error: HttpErrorResponse): ObservableInput<HttpErrorResponse> {
    if (error.status === 401) {
      this.errorMessage = this.incorrectCredentials;
    } else {
      this.errorMessage = this.accountError;
    }

    throw error;
  }

  login(token: string): void {
    if (token) {
      const claims = jwtDecode<JwtPayload>(token) as JWTClaims;
      if (this.validateUser(claims)) {
        this.stateManagerService.isLoggedIn = true;
        this.stateManagerService.setTokenAndClaims(token, claims);
        this.navigateToDashboard();
      }
    } else {
      this.errorMessage = this.accountError;
    }
  }

  validateUser(claims: JWTClaims): boolean {
    if (!claims.isUserActivated) {
      this.errorMessage = this.accountNotActivated;
    }

    if (claims.roleCode === RoleCode.COM && !claims.isCompanyActivated) {
      this.errorMessage = this.companyNotActivated;
    }

    return !this.errorMessage;
  }

  navigateToDashboard(): void {
    this.router.navigateByUrl('/');

    this.showSpinner = false;
  }

  resetForm(): void {
    this.signInForm.reset();
  }

  resetErrorMessage(): void {
    this.errorMessage = '';
  }
}
