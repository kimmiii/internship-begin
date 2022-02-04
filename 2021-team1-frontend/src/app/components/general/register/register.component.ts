import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Title } from '@angular/platform-browser';
import { Router } from '@angular/router';

import { Register } from '../../../models/api-models/register.model';
import { Company } from '../../../models/company.model';
import { Contact } from '../../../models/contact.model';
import { Country } from '../../../models/country.model';
import { AccountService } from '../../../services/account.service';
import { CompanyService } from '../../../services/company.service';
import { ContactService } from '../../../services/contact.service';
import { InternshipService } from '../../../services/internship.service';
import { StateManagerService } from '../../../services/state-manager.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  isAllDataFetched = false;
  registerForm: FormGroup;
  registerButtonClicked = false;
  countryList: Country[];
  submitButtonDisabled = false;
  emailAlreadyUsed = false;
  accountRegistrationErrorOccurred = false;
  vatNumberErrorOccurred = false;
  contactErrorOccurred = false;
  contactErrorMessage: string;

  constructor(public titleService: Title, private stateManagerService: StateManagerService,
              private formBuilder: FormBuilder, private internshipService: InternshipService,
              private router: Router, private accountService: AccountService, private companyService: CompanyService,
              private contactService: ContactService) { }

  ngOnInit(): void {
    this.stateManagerService.reset();
    this.titleService.setTitle('Stagebeheer - Registreren');
    this.fetchData();
    this.createForm();
  }

  fetchData(): void {
    this.internshipService.getCountries().subscribe(data => {
      this.countryList = data;
      this.isAllDataFetched = true;
    })
  }

  createForm(): void {
    this.registerForm = this.formBuilder.group({
      loginData: this.formBuilder.group({
        email: [null, [Validators.required, Validators.email, Validators.minLength(3), Validators.maxLength(64)]],
        password: [null, [
          Validators.required,
          Validators.pattern('^(?=[^A-Z]*[A-Z])(?=[^a-z]*[a-z])(?=[^0-9]*[0-9])(?=.*[-_!@#\\$%\\^&\\*]).{8,}$'),
          Validators.maxLength(50)]],
        passwordConfirmation: [null, [Validators.required, Validators.maxLength(50)]]
      }, { validator: this.checkIfBothPasswordsMatch('password', 'passwordConfirmation') }),
      company: this.formBuilder.group({
        companyName: [null, [Validators.required, Validators.maxLength(50)]],
        companyStreet: [null, [Validators.required, Validators.minLength(3), Validators.maxLength(50)]],
        companyHouseNumber: [null, [Validators.required, Validators.maxLength(50), Validators.min(1), Validators.max(1000)]],
        companyBusNumber: [null, [Validators.maxLength(10)]],
        companyZipCode: [null, [Validators.required, Validators.minLength(4), Validators.min(1), Validators.maxLength(50)]],
        companyCity: [null, [Validators.required, Validators.minLength(2), Validators.maxLength(50)]],
        companyCountry: [null, [Validators.required]],
        companyVatNumber: [null, [Validators.maxLength(50)]],
        companyPhoneNumber: [null, [Validators.required, Validators.pattern('\\+[0-9]{10,11}')]],
        companyEmail: [null, [Validators.required, Validators.email, Validators.maxLength(63)]],
        companyCountEmployees: [null, [Validators.required, Validators.min(0)]],
        companyCountITEmployees: [null, [Validators.required, Validators.min(0)]],
        companyCountMentors: [null, [Validators.required, Validators.min(0)]]
      }),
      contact: this.formBuilder.group({
        contactFirstName: [null,  [Validators.required, Validators.maxLength(50)]],
        contactSurname: [null, [Validators.required, Validators.maxLength(50)]],
        contactFunction: [null, [Validators.required, Validators.minLength(2), Validators.maxLength(100)]],
        contactPhoneNumber: [null, [Validators.required, Validators.pattern('\\+[0-9]{10,11}')]],
        contactEmail: [null, [Validators.required, Validators.email, Validators.maxLength(63)]]
      })
    })
  }

  checkIfBothPasswordsMatch(password: string, passwordConfirmation: string) {
    return (group: FormGroup) => {
      const passwordInput = group.controls[password];
      const passwordConfirmationInput = group.controls[passwordConfirmation];

      if (passwordInput.value !== passwordConfirmationInput.value) {
        return passwordConfirmationInput.setErrors({ notEquivalent: true });
      } else {
        return passwordConfirmationInput.setErrors(null);
      }
    }
  }

  register(): void {
    this.registerButtonClicked = true;
    this.accountRegistrationErrorOccurred = false;
    this.vatNumberErrorOccurred = false;
    this.contactErrorOccurred = false;

    if (this.registerForm.valid) {
      this.submitButtonDisabled = true;
      this.addUser();
    }
  }

  addUser(): void {
    const user: Register = this.createRegisterObject();
    this.accountService.registerUser(user).subscribe(res => {
      if (res.toString().includes('409')) {
        this.emailAlreadyUsed = true;
        this.submitButtonDisabled = false;
      } else if (res.toString().includes('40') || res.toString().includes('50')) {
          this.accountRegistrationErrorOccurred = true;
          this.submitButtonDisabled = false;
        } else {
          const userId = res.body.userId;
          this.addCompany(userId);
        }
    });
  }

  createRegisterObject(): Register {
    const userEmailAddress: string = this.email.value;
    const userPass: string = this.password.value;
    const confirmPassword: string = this.passwordConfirmation.value;

    return new Register(userEmailAddress, userPass, confirmPassword);
  }

  addCompany(userId: number): void {
    const company: Company = this.createCompanyObject(userId);
    this.companyService.addCompany(company).subscribe(res => {
      if (res.error !== null) {
        this.vatNumberErrorOccurred = true;
        this.submitButtonDisabled = false;
      } else {
        const companyId = res.body.companyId;
        this.addContact(companyId);
      }
    });
  }

  createCompanyObject(userIdInput): Company {
    const name: string = this.companyName.value;
    const street: string = this.companyStreet.value;
    const houseNr: string = this.companyHouseNumber.value.toString();
    const busNr: string = this.companyBusNumber.value;
    const zipCode: string = this.companyZipCode.value;
    const city: string = this.companyCity.value;
    const country: string = this.companyCountry.value;
    const vatNumber: string = this.companyVatNumber.value;
    const email: string = this.companyEmail.value;
    const phoneNumber: string = this.companyPhoneNumber.value;
    const totalEmployees: number = this.companyCountEmployees.value;
    const totalITEmployees: number = this.companyCountITEmployees.value;
    const totalITEmployeesActive: number = this.companyCountMentors.value;

    return new Company(name, street, houseNr, busNr, zipCode, city, country, vatNumber, email, phoneNumber,
      totalEmployees, totalITEmployees, totalITEmployeesActive, userIdInput);
  }

  addContact(companyId: number): void {
    const contact: Contact = this.createContactObject(companyId);
    this.contactService.addContact(contact).subscribe(res => {
      if (res.error !== null) {
        this.submitButtonDisabled = false;
        this.contactErrorMessage = res.error.message;
        this.contactErrorOccurred = true;
      } else {
        this.clearRegisterForm();
      }
    });
  }

  createContactObject(companyIdInput: number): Contact {
    const companyId: number = companyIdInput;
    const firstname: string = this.contactFirstName.value;
    const surname: string = this.contactSurname.value;
    const email: string = this.contactEmail.value;
    const phoneNumber: string = this.contactPhoneNumber.value;
    const contactFunction: string = this.contactFunction.value;

    return new Contact(companyId, firstname, surname, email, phoneNumber, contactFunction);
  }

  clearRegisterForm(): void {
    this.registerButtonClicked = false;
    this.registerForm.reset();
    this.submitButtonDisabled = false;
    this.proceedToRegisterSuccessFulPage();
  }

  proceedToRegisterSuccessFulPage(): void {
    this.router.navigateByUrl('register-successful');
  }

  proceedToLoginPage(): void {
    this.router.navigateByUrl('login');
  }

  resetFlags(): void {
    this.emailAlreadyUsed = false;
  }

  // GETTERS for registerForm
  get loginData() {
    return <FormGroup>this.registerForm.controls.loginData;
  }
  get email() {
    return this.loginData.controls.email;
  }
  get password() {
    return this.loginData.controls.password;
  }
  get passwordConfirmation() {
    return this.loginData.controls.passwordConfirmation;
  }
  get company() {
    return <FormGroup>this.registerForm.controls.company;
  }
  get companyName() {
    return this.company.controls.companyName;
  }
  get companyStreet() {
    return this.company.controls.companyStreet;
  }
  get companyHouseNumber() {
    return this.company.controls.companyHouseNumber;
  }
  get companyBusNumber() {
    return this.company.controls.companyBusNumber;
  }
  get companyZipCode() {
    return this.company.controls.companyZipCode;
  }
  get companyCity() {
    return this.company.controls.companyCity;
  }
  get companyCountry() {
    return this.company.controls.companyCountry;
  }
  get companyVatNumber() {
    return this.company.controls.companyVatNumber;
  }
  get companyPhoneNumber() {
    return this.company.controls.companyPhoneNumber;
  }
  get companyEmail() {
    return this.company.controls.companyEmail;
  }
  get companyCountEmployees() {
    return this.company.controls.companyCountEmployees;
  }
  get companyCountITEmployees() {
    return this.company.controls.companyCountITEmployees;
  }
  get companyCountMentors() {
    return this.company.controls.companyCountMentors;
  }
  get contact() {
    return <FormGroup>this.registerForm.controls.contact;
  }
  get contactFirstName() {
    return this.contact.controls.contactFirstName;
  }
  get contactSurname() {
    return this.contact.controls.contactSurname;
  }
  get contactFunction() {
    return this.contact.controls.contactFunction;
  }
  get contactPhoneNumber() {
    return this.contact.controls.contactPhoneNumber;
  }
  get contactEmail() {
    return this.contact.controls.contactEmail;
  }
}
