import { HttpClientTestingModule } from '@angular/common/http/testing';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterTestingModule } from '@angular/router/testing';

import { Login } from '../../../models/api-models/login.model';
import { AccountService } from '../../../services/account.service';
import { CompanyService } from '../../../services/company.service';
import { StateManagerService } from '../../../services/state-manager.service';
import { LoginComponent } from './login.component';

import Spy = jasmine.Spy;

describe('LoginComponent', () => {
  let component: LoginComponent;
  let fixture: ComponentFixture<LoginComponent>;
  let email;
  let password;
  const EMAIL_VALUE = 'test@test.com';
  const PASSWORD_VALUE = 'Test123*';
  let accountService: AccountService;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [ ReactiveFormsModule, FormsModule, RouterTestingModule, HttpClientTestingModule ],
      declarations: [ LoginComponent ],
      providers: [
        AccountService,
        StateManagerService,
        CompanyService
      ],
    })
      .compileComponents();
  }));

  beforeEach(async(() => {
    fixture = TestBed.createComponent(LoginComponent);
    component = fixture.componentInstance;

    accountService = TestBed.get(AccountService);

    component.ngOnInit();
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should have as title `Stagebeheer - Aanmelden`', async(() => {
    expect(component.titleService.getTitle()).toEqual('Stagebeheer | Aanmelden');
  }));

  /*
  it('email should be invalid when empty', () => {
    initFormControls();
    expect(email.errors['required']).toBeTruthy();
  });
   */

  /*
  it('email should be invalid when no email address', () => {
    initFormControls();
    email.setValue('test');
    expect(email.errors['email']).toBeTruthy();
  });
   */

  /*
  it('email should be valid when email address', () => {
    initFormControls();
    email.setValue(EMAIL_VALUE);

    expect(email.valid).toBeTruthy();
  });
  */

  it('password should be invalid when empty', () => {
    initFormControls();
    expect(password.errors['required']).toBeTruthy();
  });

  it('password should be valid when input', () => {
    initFormControls();
    password.setValue(PASSWORD_VALUE);

    expect(password.valid).toBeTruthy();
  });

  it('form should be invalid when empty', () => {
    expect(component.signInForm.valid).toBeFalsy();
  });

  /*
  it('should create login object successfully', () => {
    initFormControlsWithValues();

    const login: Login = component.signInForm.value;
    expect(login.userEmailAddress).toBe(EMAIL_VALUE);
    expect(login.password).toBe(PASSWORD_VALUE);
  });
   */

  /*
  it('should call login method from accountService when submitting valid form', () => {
    initFormControlsWithValues();

    const accountSpy: Spy = spyOn(accountService, 'login').and.callThrough();
    component.signIn();
    expect(accountSpy).toHaveBeenCalledWith({
      userEmailAddress: EMAIL_VALUE,
      password: PASSWORD_VALUE,
    } as Login);
  });
   */

  function initFormControls() {
    email = component.signInForm.controls['email'];
    password = component.signInForm.controls['password'];
  }

  function initFormControlsWithValues() {
    initFormControls();
    email.setValue(EMAIL_VALUE);
    password.setValue(PASSWORD_VALUE);
  }
});
