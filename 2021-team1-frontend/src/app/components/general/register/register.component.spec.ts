

import { HttpClient, HttpClientModule, HttpHandler } from '@angular/common/http';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { RouterTestingModule } from '@angular/router/testing';

import { Register } from '../../../models/api-models/register.model';
import { Company } from '../../../models/company.model';
import { Contact } from '../../../models/contact.model';
import { AccountService } from '../../../services/account.service';
import { CompanyService } from '../../../services/company.service';
import { ContactService } from '../../../services/contact.service';
import { InternshipService } from '../../../services/internship.service';
import { StateManagerService } from '../../../services/state-manager.service';
import { RegisterComponent } from './register.component';

import Spy = jasmine.Spy;

describe('RegisterComponent', () => {
  let component: RegisterComponent;
  let fixture: ComponentFixture<RegisterComponent>;
  let loginDataFormGroup;
  let email;
  let password;
  let passwordConfirmation;
  const VALID_EMAIL = 'test@test.com';
  const VALID_PASSWORD = 'Test123*';
  let companyFormGroup;
  let phoneNumber;
  let accountService: AccountService;
  let companyService: CompanyService;
  let contactService: ContactService;
  let router: Router;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [ ReactiveFormsModule, FormsModule, HttpClientModule, RouterTestingModule ],
      declarations: [ RegisterComponent ],
      providers: [
        StateManagerService,
        InternshipService,
        HttpClient,
        HttpHandler,
        AccountService,
        CompanyService,
        ContactService
      ]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RegisterComponent);
    component = fixture.componentInstance;

    accountService = TestBed.get(AccountService);
    companyService = TestBed.get(CompanyService);
    contactService = TestBed.get(ContactService);
    router = TestBed.get(Router);

    component.ngOnInit();
    fixture.detectChanges();
  });

  afterEach(() => {
    TestBed.resetTestingModule();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should have as title `Stagebeheer - Registreren`',() => {
    expect(component.titleService.getTitle()).toEqual('Stagebeheer - Registreren');
  });

  /*
  Since it is supposed the built in validation from Angular works correctly, not all input will be tested. For demonstrating
  purposes, the email input is tested completely. For the other input fields only the custom made validation will be tested.
   */
  it('email validity should work correctly', () => {
    initFormControls();

    expect(email.errors['required']);

    email.setValue(createRandomString(10));
    expect(email.errors['email']).toBeTruthy();

    email.setValue(createRandomString(2));
    expect(email.errors['minlength']).toBeTruthy();

    email.setValue(createRandomString(65));
    expect(email.errors['maxlength']).toBeTruthy();

    email.setValue('test@test.com');
    expect(email.valid).toBeTruthy();
  });

  it('password should only be valid when input confirms regex', () => {
    initFormControls();

    password.setValue(createRandomString(7));
    expect(password.errors['pattern']).toBeTruthy();

    password.setValue(createRandomString(9));
    expect(password.errors['pattern']).toBeTruthy();

    password.setValue('Test1*');
    expect(password.errors['pattern']).toBeTruthy();

    password.setValue('test123*');
    expect(password.errors['pattern']).toBeTruthy();

    password.setValue(VALID_PASSWORD);
    expect(password.valid).toBeTruthy();
  });

  it('passwordConfirmation should be equal to password', () => {
    initFormControls();

    password.setValue(createRandomString(10));
    passwordConfirmation.setValue(createRandomString(12));
    expect(passwordConfirmation.valid).toBeFalsy();

    password.setValue(VALID_PASSWORD);
    passwordConfirmation.setValue(VALID_PASSWORD);
    expect(passwordConfirmation.valid).toBeTruthy();
  });

  it('phone number should only be valid when input confirms regex', () => {
    initFormControls();

    phoneNumber.setValue(createRandomString(10));
    expect(phoneNumber.errors['pattern']).toBeTruthy();

    phoneNumber.setValue(createRandomString(11));
    expect(phoneNumber.errors['pattern']).toBeTruthy();

    phoneNumber.setValue(createRandomString(10, true));
    expect(phoneNumber.errors['pattern']).toBeTruthy();

    phoneNumber.setValue(createRandomString(11, true));
    expect(phoneNumber.errors['pattern']).toBeTruthy();

    phoneNumber.setValue('+' + createRandomString(10, true));
    expect(phoneNumber.valid).toBeTruthy();

    phoneNumber.setValue('+' + createRandomString(11, true));
    expect(phoneNumber.valid).toBeTruthy();

    phoneNumber.setValue('+' + createRandomString(9, true));
    expect(phoneNumber.errors['pattern']).toBeTruthy();

    phoneNumber.setValue('+' + createRandomString(12, true));
    expect(phoneNumber.errors['pattern']).toBeTruthy();
  });

  it('form should be invalid when empty', () => {
    expect(component.registerForm.valid).toBeFalsy();
  });

  it('should create register object successfully', () => {
    initFormControlsWithValues();

    const register: Register = component.createRegisterObject();
    expect(register.userEmailAddress).toBe(VALID_EMAIL);
    expect(register.userPass).toBe(VALID_PASSWORD);
    expect(register.confirmPassword).toBe(VALID_PASSWORD);
  });

  it('should call registerUser method from accountService when addUser method is called', () => {
    initFormControlsWithValues();

    const accountSpy: Spy = spyOn(accountService, 'registerUser').and.callThrough();
    component.addUser();
    expect(accountSpy).toHaveBeenCalledWith(new Register(VALID_EMAIL, VALID_PASSWORD, VALID_PASSWORD));
  });

  it('should create company object successfully', () => {
    initCompanyFields();

    const company: Company = component.createCompanyObject(1);
    expect(company.userId).toBe(1);
    expect(company.name).toBe('Test company');
    expect(company.street).toBe('Test street');
    expect(company.houseNr).toBe('10');
    expect(company.busNr).toBe('A');
    expect(company.zipCode).toBe('1234');
    expect(company.city).toBe('Test city');
    expect(company.country).toBe('Test country');
    expect(company.vatNumber).toBe('123.123.123');
    expect(company.email).toBe('test@test.com');
    expect(company.phoneNumber).toBe('+12345678');
    expect(company.totalEmployees).toBe(10);
    expect(company.totalITEmployees).toBe(10);
    expect(company.totalITEmployeesActive).toBe(10);
  });

  it('should call addCompany method from companyService when addCompany method is called', () => {
    initCompanyFields();

    const companySpy: Spy = spyOn(companyService, 'addCompany').and.callThrough();
    component.addCompany(1);
    expect(companySpy).toHaveBeenCalled();
  });

  it('should create contact object successfully', () => {
    initContactFields();

    const contact: Contact = component.createContactObject(1);
    expect(contact.companyId).toBe(1);
    expect(contact.firstname).toBe('Test');
    expect(contact.surname).toBe('Test');
    expect(contact.email).toBe('test@test.com');
    expect(contact.phoneNumber).toBe('+12345678');
    expect(contact.function).toBe('Test');
  });

  it('should call addContact method from contactService when addContact method is called', () => {
    initContactFields();

    const contactSpy: Spy = spyOn(contactService, 'addContact').and.callThrough();
    component.addContact(1);
    expect(contactSpy).toHaveBeenCalled();
  });

  it('should navigate to success screen after register succesfully', () => {
    const routerSpy: Spy = spyOn(router, 'navigateByUrl').and.callThrough();
    component.proceedToRegisterSuccessFulPage();
    expect(routerSpy).toHaveBeenCalled();
  });


  function createRandomString(length: number, onlyNumbers?: boolean): string {
    let result = '';
    let characters: string;

    if (onlyNumbers) {
      characters = '0123456789';
    } else {
      characters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz';
    }

    const charactersLength: number = characters.length;
    for(let i = 0; i < length; i++) {
      result += characters.charAt(Math.floor(Math.random() * charactersLength));
    }

    return result;
  }

  function initFormControls() {
    loginDataFormGroup = <FormGroup>component.registerForm.controls.loginData;
    companyFormGroup = <FormGroup>component.registerForm.controls.company;

    email = loginDataFormGroup.controls['email'];
    password = loginDataFormGroup.controls['password'];
    passwordConfirmation = loginDataFormGroup.controls['passwordConfirmation'];
    phoneNumber = companyFormGroup.controls['companyPhoneNumber'];
  }

  function initFormControlsWithValues() {
    initFormControls();

    email.setValue(VALID_EMAIL);
    password.setValue(VALID_PASSWORD);
    passwordConfirmation.setValue(VALID_PASSWORD);
  }

  function initCompanyFields() {
    const companyFormGroup = <FormGroup>component.registerForm.controls.company;
    companyFormGroup.controls['companyName'].setValue('Test company');
    companyFormGroup.controls['companyStreet'].setValue('Test street');
    companyFormGroup.controls['companyHouseNumber'].setValue('10');
    companyFormGroup.controls['companyBusNumber'].setValue('A');
    companyFormGroup.controls['companyZipCode'].setValue('1234');
    companyFormGroup.controls['companyCity'].setValue('Test city');
    companyFormGroup.controls['companyCountry'].setValue('Test country');
    companyFormGroup.controls['companyVatNumber'].setValue('123.123.123');
    companyFormGroup.controls['companyEmail'].setValue('test@test.com');
    companyFormGroup.controls['companyPhoneNumber'].setValue('+12345678');
    companyFormGroup.controls['companyCountEmployees'].setValue(10);
    companyFormGroup.controls['companyCountITEmployees'].setValue(10);
    companyFormGroup.controls['companyCountMentors'].setValue(10);
  }

  function initContactFields() {
    const contactFormGroup = <FormGroup>component.registerForm.controls.contact;
    contactFormGroup.controls['contactFirstName'].setValue('Test');
    contactFormGroup.controls['contactSurname'].setValue('Test');
    contactFormGroup.controls['contactEmail'].setValue('test@test.com');
    contactFormGroup.controls['contactPhoneNumber'].setValue('+12345678');
    contactFormGroup.controls['contactFunction'].setValue('Test');
  }
});
