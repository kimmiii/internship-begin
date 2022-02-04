import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Title } from '@angular/platform-browser';
import { Router } from '@angular/router';
import { forkJoin } from 'rxjs';
import { CustomValidators } from 'src/app/validators/custom-validators';

import { Contact } from '../../../models/contact.model';
import { Country } from '../../../models/country.model';
import { Environment } from '../../../models/environment.model';
import { Expecation } from '../../../models/expectation.model';
import { InternshipEnvironment } from '../../../models/internship-environment.model';
import { InternshipExpectation } from '../../../models/internship-expectation.model';
import { InternshipPeriod } from '../../../models/internship-period.model';
import { InternshipSpecialisation } from '../../../models/internship-specialisation.model';
import { Internship } from '../../../models/internship.model';
import { Period } from '../../../models/period.model';
import { ProjectStatus } from '../../../models/project-status.model';
import { Specialisation } from '../../../models/specialisation.model';
import { ContactService } from '../../../services/contact.service';
import { InternshipService } from '../../../services/internship.service';
import { StateManagerService } from '../../../services/state-manager.service';

@Component({
  selector: 'app-application-form',
  templateUrl: './com-application-form.component.html',
  styleUrls: ['./com-application-form.component.css']
})
export class ComApplicationFormComponent implements OnInit {
  applicationForm: FormGroup;
  isAllDataFetched = false;
  btnSendClicked = false;
  countryList: Country[];
  specialisationList: Specialisation[];
  environmentList: Environment[];
  expectationList: Expecation[];
  periodList: Period[];
  projectStatusList: ProjectStatus[] = [];
  checkedSpecialisationsList: boolean[];
  checkedEnvironmentsList: boolean[];
  checkedExpectationsList: boolean[];
  checkedPeriodsList: boolean[];
  internshipId: string;
  internship: Internship;
  formSubmitted = false;
  contactList: Contact[];
  errorOccurred = false;
  componentContactAddIsVisible = false;
  addedContact: Contact;

  constructor(private formBuilder: FormBuilder, private router: Router, private internshipService: InternshipService,
              private contactService: ContactService, private titleService: Title,
              private stateManagerService: StateManagerService) { }

  ngOnInit(): void {
    this.titleService.setTitle('Stagebeheer | Nieuwe stageaanvraag');
    this.fetchData();
  }

  fetchData(): void {
    const countryObservable = this.internshipService.getCountries();
    const specialisationObservable = this.internshipService.getSpecialisations();
    const envObservable = this.internshipService.getEnvironments();
    const expObservable = this.internshipService.getExpectations();
    const periodObservable = this.internshipService.getPeriods();
    const statusObservable = this.internshipService.getStatus();

    forkJoin([
      countryObservable,
      specialisationObservable,
      envObservable,
      expObservable,
      periodObservable,
      statusObservable]).subscribe(results => {
      this.countryList = results[0];
      this.specialisationList = results[1];
      this.environmentList = results[2];
      this.expectationList = results[3];
      this.periodList = results[4];
      this.projectStatusList = results[5];
      // eslint-disable-next-line no-console
      console.log('Country, Specialisation, Environments, Expectations, Period en Projectstatussen succesvol opgehaald.');

      this.fetchContacts(); // Seperate call <- results response can only have 6 results

      this.initializeAllCheckedLists();
      this.createForm();
    });
  }

  fetchContacts(): void {
    const companyId = this.stateManagerService.companyId;
    this.contactService.getContactByCompanyId(companyId).subscribe(data => {
      this.contactList = data;
      // eslint-disable-next-line no-console
      console.log('Contactpersonen succesvol opgehaald.');
      this.isAllDataFetched = true;
    });
  }

  initializeAllCheckedLists(): void {
    this.checkedSpecialisationsList = this.initializeSpecializationCheckedList();
    this.checkedEnvironmentsList = this.initializeEnvironmentCheckedList();
    this.checkedExpectationsList = this.initializeExpectationCheckedList();
    this.checkedPeriodsList = this.initializePeriodCheckedList();
  }

  initializeSpecializationCheckedList() : Array<boolean> {
    const checkedCheckboxesList: Array<boolean> = [];
    for (let index = 0; index < this.specialisationList.length; index++) {
      checkedCheckboxesList[index] = false;
    }
    return checkedCheckboxesList;
  }

  initializeEnvironmentCheckedList() : Array<boolean> {
    const checkedCheckboxesList: Array<boolean> = [];
    for (let index = 0; index < this.environmentList.length; index++) {
      checkedCheckboxesList[index] = false;
    }
    return checkedCheckboxesList;
  }

  initializeExpectationCheckedList() : Array<boolean> {
    const checkedCheckboxesList: Array<boolean> = [];
    for (let index = 0; index < this.expectationList.length; index++) {
      checkedCheckboxesList[index] = false;
    }
    return checkedCheckboxesList;
  }

  initializePeriodCheckedList() : Array<boolean> {
    const checkedCheckboxesList: Array<boolean> = [];
    for (let index = 0; index < this.periodList.length; index++) {
      checkedCheckboxesList[index] = false;
    }
    return checkedCheckboxesList;
  }

  createForm(): void {
    this.applicationForm = this.formBuilder.group({
      company: this.formBuilder.group({
        companyStreet: [null, [Validators.minLength(3), Validators.maxLength(50)]],
        companyHouseNumber: [null, [Validators.maxLength(50), Validators.min(1), Validators.max(1000)]],
        companyMailboxNumber: [null, [Validators.maxLength(10)]],
        companyZipCode: [null, [Validators.minLength(4), Validators.maxLength(50), Validators.min(1)]],
        companyCity: [null, [Validators.minLength(2), Validators.maxLength(50)]],
        companyCountry: [null],
      }),
      contact: this.formBuilder.group({
        contactCombobox: [null, [Validators.required]]
      }),
      promotor: this.formBuilder.group({
        promotorfirstname: [null, [Validators.required, Validators.maxLength(50)]],
        promotorSurname: [null, [Validators.required, Validators.maxLength(50)]],
        promotorFunction: [null, [Validators.required, Validators.minLength(2), Validators.maxLength(100)]],
        promotorEmail: [null, [Validators.required, Validators.email, Validators.minLength(3), Validators.maxLength(64)]]
      }),
      topic: this.formBuilder.group({
        topicTitle: [null, [Validators.required, Validators.maxLength(80)]],
        topicSpecialisations: this.formBuilder.array(
          this.specialisationList.map(() => this.formBuilder.control('')),
          [CustomValidators.multipleCheckboxRequireOne]
        ),
        topicAssignment: [null, [Validators.required, Validators.maxLength(1500)]],
        topicEnvironments: this.formBuilder.array(
          this.environmentList.map(() => this.formBuilder.control('')),
          [CustomValidators.multipleCheckboxRequireOne]
        ),
        topicEnvironmentOther: [null, [Validators.maxLength(100)]],
        topicDetails: [null, [Validators.maxLength(1500)]],
        topicConditions: [null, [Validators.maxLength(1500)]],
        topicResearchTheme: [null, [Validators.required, Validators.maxLength(1500)]],
      }),
      other: this.formBuilder.group({
        otherExpectations: this.formBuilder.array(
          this.expectationList.map(() => this.formBuilder.control(''))
        ),
        otherCountStudent: [null, [Validators.required]],
        secondTopicResearchTheme: [null, [Validators.maxLength(1500)]],
        otherStudents: [null, [Validators.maxLength(50)]],
        otherRemarks: [null, [Validators.maxLength(1500)]],
        otherPeriod: this.formBuilder.array(
          this.periodList.map(() => this.formBuilder.control('')),
          [CustomValidators.multipleCheckboxRequireOne]
        ),
      })
    });
    // eslint-disable-next-line no-console
    console.log('Formulier succesvol aangemaakt.');
  }

  submitForm(): void {
    this.setConditionalValidators();

    this.btnSendClicked = true;
    this.errorOccurred = false;

    if (this.applicationForm.valid) {
      if (this.contactCombobox.value !== -1) { // If valid contact is selected
        this.formSubmitted = true;
        this.addInternship();
        // eslint-disable-next-line no-console
        console.log('Internship wordt verstuurd naar API...');
      } else {
        window.alert('Geen geldige contactpersoon geselecteerd.')
      }
    }
  }

  setConditionalValidators(): void {
    // EnvironmentOther
    if (this.checkedEnvironmentsList[this.checkedEnvironmentsList.length-1] === true) { // OtherEnvironment selected
      this.topicEnvironmentOther.setValidators([Validators.required, Validators.maxLength(100)]);
      this.topicEnvironmentOther.updateValueAndValidity();
    } else {
      this.topicEnvironmentOther.setValidators([Validators.maxLength(100)]);
      this.topicEnvironmentOther.updateValueAndValidity();
      this.topicEnvironmentOther.setValue(null);
    }

    // SecondTopicResearchTheme
    if (this.otherCountStudent.value === 2) {
      this.secondTopicResearchTheme.setValidators([Validators.required, Validators.maxLength(500)]);
      this.secondTopicResearchTheme.updateValueAndValidity();
    } else {
      this.secondTopicResearchTheme.setValidators([Validators.maxLength(500)]);
      this.secondTopicResearchTheme.updateValueAndValidity();
      this.secondTopicResearchTheme.setValue(null);
    }
  }

  addInternship(): void {
    const internship: Internship = this.createInternshipRequestObject();
    // eslint-disable-next-line no-console
    console.log('Internship verzonden naar API:', internship);

    this.internshipService.addInternship(internship).subscribe(res => {
      // eslint-disable-next-line no-console
      console.log('Internship succesvol toegevoegd aan database:', internship);

      this.proceedToInternshipDetails(res.body.internshipId);
      this.resetForm();
    });
  }

  createInternshipRequestObject(): Internship {
    const companyId: number = this.stateManagerService.companyId;
    const contactPersonId: number = this.contactCombobox.value;
    const promotorFirstname: string = this.promotorfirstname.value;
    const promotorSurname: string = this.promotorSurname.value;
    const promotorFunction: string = this.promotorFunction.value;
    const promotorEmail: string = this.promotorEmail.value;
    const projectStatusId: number = this.projectStatusList.filter(x => x.code === 'NEW')[0].projectStatusId;
    const wpStreet: string = this.companyStreet.value;
    const wpHouseNr: string = this.companyHouseNumber.value;
    const wpBusNr: string = this.companyMailboxNumber.value;
    const wpZipCode: string = this.companyZipCode.value;
    const wpCity: string = this.companyCity.value;
    const wpCountry: string = this.companyCountry.value;
    const internshipEnvironmentOthers: string = this.topicEnvironmentOther.value;
    const assignmentDescription: string = this.topicAssignment.value;
    const technicalDetails: string = this.topicDetails.value;
    const conditions: string = this.topicConditions.value;
    const totalInternRequired: number = this.otherCountStudent.value;
    const contactStudentName: string = this.otherStudents.value;
    const remark: string = this.otherRemarks.value;
    const topicTitle: string = this.topicTitle.value;
    let researchTheme: string = this.topicResearchTheme.value;

    // When 2 students for internships => second research theme
    if (this.secondTopicResearchTheme !== null) {
      researchTheme += `~${this.secondTopicResearchTheme.value}`;
    }

    const internshipPeriod: InternshipPeriod[] = [];
    const internshipSpecialisation: InternshipSpecialisation[] = [];
    const internshipEnvironment: InternshipEnvironment[] = [];
    const internshipExpectation: InternshipExpectation[] = [];

    for(let i = 0; i < this.checkedSpecialisationsList.length; i++) {
      if (this.checkedSpecialisationsList[i] === true) {
        internshipSpecialisation.push(new InternshipSpecialisation(this.specialisationList[i].specialisationId));
      }
    }

    for(let i = 0; i < this.checkedEnvironmentsList.length; i++) {
      if (this.checkedEnvironmentsList[i] === true) {
        internshipEnvironment.push(new InternshipEnvironment(this.environmentList[i].environmentId));
      }
    }

    for(let i = 0; i < this.checkedExpectationsList.length; i++) {
      if (this.checkedExpectationsList[i] === true) {
        internshipExpectation.push(new InternshipExpectation(this.expectationList[i].expectationId));
      }
    }

    for(let i = 0; i < this.checkedPeriodsList.length; i++) {
      if (this.checkedPeriodsList[i] === true) {
        internshipPeriod.push(new InternshipPeriod(this.periodList[i].periodId));
      }
    }

    return new Internship(companyId, contactPersonId, promotorFirstname, promotorSurname, promotorFunction, promotorEmail,
      projectStatusId, wpStreet, wpHouseNr, wpBusNr, wpZipCode, wpCity, wpCountry,
      internshipEnvironmentOthers, assignmentDescription, technicalDetails, conditions, totalInternRequired, contactStudentName,
      remark, topicTitle, researchTheme, internshipPeriod, internshipSpecialisation, internshipEnvironment, internshipExpectation);
  }

  proceedToInternshipDetails(internshipId: number): void {
    if (internshipId !== null) {
      this.router.navigate(['/company/internship-details', internshipId]);
      // eslint-disable-next-line no-console
      console.log(`Succesvol genavigeerd naar detailpagina van stage met id ${internshipId}.`);
    } else {
      this.errorOccurred = true;
    }
  }

  resetForm(): void {
    this.applicationForm.reset();
    this.btnSendClicked = false;
  }

  checkSelectedContact(): void {
    this.componentContactAddIsVisible = this.contactCombobox.value === -1; //-1 = 'add new contact'
  }

  contactAdded($event: Contact): void {
    this.componentContactAddIsVisible = false;
    this.addedContact = $event;
    const companyId = this.stateManagerService.companyId;

    // Refresh contact list
    this.contactService.getContactByCompanyId(companyId).subscribe(data => {
      this.contactList = data;
      // eslint-disable-next-line no-console
      console.log('Contactpersonen succesvol opgehaald.');

      const newContactId: number = this.defineIdNewContact();
      this.contactCombobox.setValue(newContactId);
    });
  }

  defineIdNewContact(): number {
    let contactId: number;

    for (const contact of this.contactList) {
      // If contact is the new added contact, then ...
      if (contact.firstname === this.addedContact.firstname &&
          contact.surname === this.addedContact.surname &&
          contact.function === this.addedContact.function &&
          contact.phoneNumber === this.addedContact.phoneNumber &&
          contact.email === this.addedContact.email) {
        contactId = contact.contactId;
      }
    }
    // eslint-disable-next-line no-console
    console.log(`Id van de nieuw toegevoegde contactpersoon is ${contactId}.`);
    return contactId;
  }

  // Getters for form
  get company() {
    return <FormGroup>this.applicationForm.controls.company;
  }
  get companyStreet() {
    return this.company.controls.companyStreet;
  }
  get companyHouseNumber() {
    return this.company.controls.companyHouseNumber;
  }
  get companyMailboxNumber() {
    return this.company.controls.companyMailboxNumber;
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
  get contact() {
    return <FormGroup>this.applicationForm.controls.contact;
  }
  get contactCombobox() {
    return this.contact.controls.contactCombobox;
  }
  get promotor() {
    return <FormGroup>this.applicationForm.controls.promotor;
  }
  get promotorfirstname() {
    return this.promotor.controls.promotorfirstname;
  }
  get promotorSurname() {
    return this.promotor.controls.promotorSurname;
  }
  get promotorFunction() {
    return this.promotor.controls.promotorFunction;
  }
  get promotorEmail() {
    return this.promotor.controls.promotorEmail;
  }
  get topic() {
    return <FormGroup>this.applicationForm.controls.topic;
  }
  get topicTitle() {
    return this.topic.controls.topicTitle;
  }
  get topicSpecialisations() {
    return this.topic.controls.topicSpecialisations;
  }
  get topicAssignment() {
    return this.topic.controls.topicAssignment;
  }
  get topicEnvironments() {
    return this.topic.controls.topicEnvironments;
  }
  get topicEnvironmentOther() {
    return this.topic.controls.topicEnvironmentOther;
  }
  get topicDetails() {
    return this.topic.controls.topicDetails;
  }
  get topicConditions() {
    return this.topic.controls.topicConditions;
  }
  get topicResearchTheme() {
    return this.topic.controls.topicResearchTheme;
  }
  get other() {
    return <FormGroup>this.applicationForm.controls.other;
  }
  get otherExpectations() {
    return this.other.controls.otherExpectations;
  }
  get otherCountStudent() {
    return this.other.controls.otherCountStudent;
  }
  get secondTopicResearchTheme() {
    return this.other.controls.secondTopicResearchTheme;
  }
  get otherStudents() {
    return this.other.controls.otherStudents;
  }
  get otherRemarks() {
    return this.other.controls.otherRemarks;
  }
  get otherPeriod() {
    return this.other.controls.otherPeriod;
  }
}
