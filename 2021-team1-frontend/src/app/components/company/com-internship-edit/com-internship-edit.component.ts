import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {Title} from "@angular/platform-browser";
import {forkJoin} from 'rxjs';
import {Country} from "../../../models/country.model";
import {Specialisation} from "../../../models/specialisation.model";
import {Environment} from "../../../models/environment.model";
import {Expecation} from "../../../models/expectation.model";
import {Period} from "../../../models/period.model";
import {InternshipService} from "../../../services/internship.service";
import {ActivatedRoute, Router} from "@angular/router";
import {Internship} from "../../../models/internship.model";
import {InternshipPeriod} from "../../../models/internship-period.model";
import {InternshipSpecialisation} from "../../../models/internship-specialisation.model";
import {InternshipEnvironment} from "../../../models/internship-environment.model";
import {InternshipExpectation} from "../../../models/internship-expectation.model";
import {ContactService} from "../../../services/contact.service";
import {ProjectStatus} from "../../../models/project-status.model";
import {CustomValidators} from "src/app/validators/custom-validators";
import {Contact} from "../../../models/contact.model";
import {StateManagerService} from "../../../services/state-manager.service";

@Component({
  selector: 'app-internship-edit',
  templateUrl: './com-internship-edit.component.html',
  styleUrls: ['./com-internship-edit.component.css']
})
export class ComInternshipEditComponent implements OnInit {
  applicationForm: FormGroup;
  isAllDataFetched: boolean = false;
  btnSendClicked: boolean = false;
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
  internship: Internship;
  submitted: boolean = false;
  contactList: Contact[];
  contactObject: Contact;
  researchTopicDescriptions: string[] = [];

  constructor(private formBuilder: FormBuilder, private internshipService: InternshipService,
              private router: Router, private route: ActivatedRoute, private contactService: ContactService,
              private titleService: Title, private stateManagerService: StateManagerService) { }

  ngOnInit() {
    this.titleService.setTitle('Stagebeheer | Stageaanvraag wijzigen');
    this.fetchData();
  }

  fetchData() {
    let countryObservably = this.internshipService.getCountries();
    let specialisationObservably = this.internshipService.getSpecialisations();
    let envObservably = this.internshipService.getEnvironments();
    let expObservably = this.internshipService.getExpectations();
    let periodObservably = this.internshipService.getPeriods();
    let statusObservably = this.internshipService.getStatus();

    forkJoin([countryObservably, specialisationObservably, envObservably, expObservably, periodObservably, statusObservably]).subscribe(results => {
      this.countryList = results[0];
      this.specialisationList = results[1];
      this.environmentList = results[2];
      this.expectationList = results[3];
      this.periodList = results[4];
      this.projectStatusList = results[5];

      // Seperate call <- results response can only have 6 results
      this.contactService.getContactByCompanyId(this.stateManagerService.companyId).subscribe(data => this.contactList = data);

      let internshipId: string = this.route.snapshot.paramMap.get('internshipId');
      this.internshipService.getInternshipById(internshipId).subscribe(res => {
        if (res.error != null) {
          window.alert(`Er is iets fout gegaan bij het laden van internship met id ${internshipId}. U wordt naar de start-pagina omgeleid.`);
          this.router.navigateByUrl('/company/internships');
        } else {
          this.internship = res;
          console.log(`Internship succesvol opgehaald: `, this.internship);
          this.fetchContact();
        }

        this.splitResearchTopicDescriptions();
      });
    });
  }

  fetchContact() {
    this.contactService.getContactById(this.internship.contactPersonId).subscribe(res => {
      if (res.error != null) {
        window.alert(`Er is iets fout gegaan bij het laden van de contactpersoon. U wordt naar de start-pagina omgeleid.`);
        this.router.navigateByUrl('/company/internships');
      } else {
        this.contactObject = res;
        console.log(`Contact succesvol opgehaald:`, this.contactObject);

        this.isAllDataFetched = true;
        this.initializeAllCheckedLists();
        this.createForm();
      }
    });
  }

  createForm() {
    this.applicationForm = this.formBuilder.group({
      company: this.formBuilder.group({
        companyStreet: [this.internship.wpStreet, [Validators.minLength(3), Validators.maxLength(50)]],
        companyHouseNumber: [this.internship.wpHouseNr, [Validators.maxLength(50), Validators.min(1), Validators.max(1000)]],
        companyMailboxNumber: [this.internship.wpBusNr, [Validators.maxLength(10)]],
        companyZipCode: [this.internship.wpZipCode, [Validators.minLength(4), Validators.min(1), Validators.maxLength(50)]],
        companyCity: [this.internship.wpCity, [Validators.minLength(2), Validators.maxLength(50)]],
        companyCountry: [this.internship.wpCountry],
      }),
      contact: this.formBuilder.group({
        contactCombobox: [this.contactObject.contactId, [Validators.required]]
      }),
      promotor: this.formBuilder.group({
        promotorfirstname: [this.internship.promotorFirstname, [Validators.required, Validators.maxLength(50)]],
        promotorSurname: [this.internship.promotorSurname, [Validators.required, Validators.maxLength(50)]],
        promotorFunction: [this.internship.promotorFunction, [Validators.required, Validators.minLength(2), Validators.maxLength(100)]],
        promotorEmail: [this.internship.promotorEmail, [Validators.required, Validators.email, Validators.minLength(3), Validators.maxLength(64)]]
      }),
      topic: this.formBuilder.group({
        topicTitle: [this.internship.researchTopicTitle, [Validators.required, Validators.maxLength(80)]],
        topicSpecialisations: this.formBuilder.array(
          this.specialisationList.map(() => this.formBuilder.control('')),
          [CustomValidators.multipleCheckboxRequireOne]
        ),
        topicAssignment: [this.internship.assignmentDescription, [Validators.required, Validators.maxLength(1500)]],
        topicEnvironments: this.formBuilder.array(
          this.environmentList.map(() => this.formBuilder.control('')),
          [CustomValidators.multipleCheckboxRequireOne]
        ),
        topicEnvironmentOther: [this.internship.internshipEnvironmentOthers, [Validators.maxLength(100)]],
        topicDetails: [this.internship.technicalDetails, [Validators.maxLength(500)]],
        topicConditions: [this.internship.conditions, [Validators.maxLength(255)]],
        topicResearchTheme: [this.researchTopicDescriptions[0], [Validators.required, Validators.maxLength(500)]],
      }),
      other: this.formBuilder.group({
        otherExpectations: this.formBuilder.array(
          this.expectationList.map(() => this.formBuilder.control(''))
        ),
        otherCountStudent: [this.internship.totalInternsRequired, [Validators.required]],
        secondTopicResearchTheme: [this.researchTopicDescriptions[1], [Validators.maxLength(500)]],
        otherStudents: [this.internship.contactStudentName, [Validators.maxLength(50)]],
        otherRemarks: [this.internship.remark, [Validators.maxLength(500)]],
        otherPeriod: this.formBuilder.array(
          this.periodList.map(() => this.formBuilder.control('')),
          [CustomValidators.multipleCheckboxRequireOne]
        ),
      })
    });

    console.log(`Edit-form succesvol aangemaakt.`);
  }

  initializeAllCheckedLists() {
    this.checkedSpecialisationsList = this.initializeSpecializationCheckedList();
    this.checkedEnvironmentsList = this.initializeEnvironmentCheckedList();
    this.checkedExpectationsList = this.initializeExpectationCheckedList();
    this.checkedPeriodsList = this.initializePeriodCheckedList();
  }

  initializeSpecializationCheckedList() : Array<boolean> {
    let checkedCheckboxesList: Array<boolean> = [];
    for (let index = 0; index < this.specialisationList.length; index++) {
      checkedCheckboxesList[index] = false;

      this.internship.internshipSpecialisation.forEach(spec => {
        if (spec.specialisationId == this.specialisationList[index].specialisationId) checkedCheckboxesList[index] = true;
      });

    }
    return checkedCheckboxesList;
  }

  initializeEnvironmentCheckedList() : Array<boolean> {
    let checkedCheckboxesList: Array<boolean> = [];
    for (let index = 0; index < this.environmentList.length; index++) {
      checkedCheckboxesList[index] = false;

      this.internship.internshipEnvironment.forEach(env => {
        if (env.environmentId == this.environmentList[index].environmentId) checkedCheckboxesList[index] = true;
      });
    }
    return checkedCheckboxesList;
  }

  initializeExpectationCheckedList() : Array<boolean> {
    let checkedCheckboxesList: Array<boolean> = [];
    for (let index = 0; index < this.expectationList.length; index++) {
      checkedCheckboxesList[index] = false;

      this.internship.internshipExpectation.forEach(exp => {
        if (exp.expectationId == this.expectationList[index].expectationId) checkedCheckboxesList[index] = true;
      });
    }
    return checkedCheckboxesList;
  }

  initializePeriodCheckedList() : Array<boolean> {
    let checkedCheckboxesList: Array<boolean> = [];
    for (let index = 0; index < this.periodList.length; index++) {
      checkedCheckboxesList[index] = false;

      this.internship.internshipPeriod.forEach(per => {
        if (per.periodId == this.periodList[index].periodId) checkedCheckboxesList[index] = true;
      });
    }
    return checkedCheckboxesList;
  }

  editForm() {
    this.setConditionalValidators();
    this.btnSendClicked = true;

    if (this.applicationForm.valid) {
      console.log('Edit form is geldig.');
      this.submitted = true;

      this.editInternship();
    }
  }

  setConditionalValidators() {
    // EnvironmentOther
    if (this.checkedEnvironmentsList[this.checkedEnvironmentsList.length - 1] == true) { // OtherEnvironment selected
      this.topicEnvironmentOther.setValidators([Validators.required, Validators.maxLength(100)]);
      this.topicEnvironmentOther.updateValueAndValidity();
    } else {
      this.topicEnvironmentOther.setValidators([Validators.maxLength(100)]);
      this.topicEnvironmentOther.updateValueAndValidity();
      this.topicEnvironmentOther.setValue(null);
    }

    // SecondTopicResearchTheme
    if (this.otherCountStudent.value == 2) {
      this.secondTopicResearchTheme.setValidators([Validators.required, Validators.maxLength(500)]);
      this.secondTopicResearchTheme.updateValueAndValidity();
    } else {
      this.secondTopicResearchTheme.setValidators([Validators.maxLength(500)]);
      this.secondTopicResearchTheme.updateValueAndValidity();
      this.secondTopicResearchTheme.setValue(null);
    }
  }

  editInternship() {
    let internshipRequest: Internship = this.createInternshipRequestObject();
    console.log("Internship naar de API:", internshipRequest);

    this.internshipService.editInternship(this.internship.internshipId, internshipRequest).subscribe(res => {
      if (res.error != null) {
        window.alert('Het is niet mogelijk om deze stageaanvraag te wijzigen. U wordt naar de startpagina geleid.');
        this.router.navigateByUrl('/company/internships');
      } else {
        console.log('Internship succesvol gewijzigd: ', internshipRequest);
        this.btnSendClicked = false;

        this.applicationForm.reset();
        this.proceedToInternshipDetails(this.internship.internshipId);
      }
    });
  }

  createInternshipRequestObject(): Internship {
    let companyId: number = this.internship.companyId;
    let contactPersonId: number = this.contactCombobox.value;
    let promotorFirstname: string = this.promotorfirstname.value;
    let promotorSurname: string = this.promotorSurname.value;
    let promotorFunction: string = this.promotorFunction.value;
    let promotorEmail: string = this.promotorEmail.value;
    let projectStatusId: number = this.projectStatusList.filter(x => x.code == 'NEW')[0].projectStatusId;
    let wpStreet: string = this.companyStreet.value;
    let wpHouseNr: string = this.companyHouseNumber.value;
    let wpBusNr: string = this.companyMailboxNumber.value;
    let wpZipCode: string = this.companyZipCode.value;
    let wpCity: string = this.companyCity.value;
    let wpCountry: string = this.companyCountry.value;
    let internshipEnvironmentOthers: string = this.checkedEnvironmentsList[this.checkedEnvironmentsList.length-1] ? this.topicEnvironmentOther.value : null;
    let assignmentDescription: string = this.topicAssignment.value;
    let technicalDetails: string = this.topicDetails.value;
    let conditions: string = this.topicConditions.value;
    let totalInternRequired: number = this.otherCountStudent.value;
    let contactStudentName: string = this.otherStudents.value;
    let remark: string = this.otherRemarks.value;
    let topicTitle: string = this.topicTitle.value;
    let researchTheme: string = this.topicResearchTheme.value;

    // When 2 students for internships => second research theme
    if (this.secondTopicResearchTheme.value != null) {
      researchTheme += `~${this.secondTopicResearchTheme.value}`;
    }

    let internshipPeriod: InternshipPeriod[] = [];
    let internshipSpecialisation: InternshipSpecialisation[] = [];
    let internshipEnvironment: InternshipEnvironment[] = [];
    let internshipExpectation: InternshipExpectation[] = [];
    let internshipId: string = this.internship.internshipId;

    for(let i = 0; i < this.checkedSpecialisationsList.length; i++) {
      if (this.checkedSpecialisationsList[i] == true) {
        internshipSpecialisation.push(new InternshipSpecialisation(this.specialisationList[i].specialisationId));
      }
    }

    for(let i = 0; i < this.checkedEnvironmentsList.length; i++) {
      if (this.checkedEnvironmentsList[i] == true) {
        internshipEnvironment.push(new InternshipEnvironment(this.environmentList[i].environmentId));
      }
    }

    for(let i = 0; i < this.checkedExpectationsList.length; i++) {
      if (this.checkedExpectationsList[i] == true) {
        internshipExpectation.push(new InternshipExpectation(this.expectationList[i].expectationId));
      }
    }

    for(let i = 0; i < this.checkedPeriodsList.length; i++) {
      if (this.checkedPeriodsList[i] == true) {
        internshipPeriod.push(new InternshipPeriod(this.periodList[i].periodId));
      }
    }

    let newInternship: Internship =  new Internship(companyId, contactPersonId, promotorFirstname, promotorSurname, promotorFunction, promotorEmail,
      projectStatusId, wpStreet, wpHouseNr, wpBusNr, wpZipCode, wpCity, wpCountry, internshipEnvironmentOthers, assignmentDescription, technicalDetails, conditions, totalInternRequired, contactStudentName,
      remark, topicTitle, researchTheme, internshipPeriod, internshipSpecialisation, internshipEnvironment, internshipExpectation);

    newInternship.internshipId = internshipId;
    newInternship.internalFeedback = this.internship.internalFeedback;
    newInternship.externalFeedback = this.internship.externalFeedback;
    newInternship.academicYear = this.internship.academicYear;

    return newInternship;
  }

  proceedToInternshipDetails(internshipId: string) {
    this.router.navigate(['/company/internship-details', internshipId]);
  }

  splitResearchTopicDescriptions() {
    if (this.internship.totalInternsRequired == 2) {
      this.researchTopicDescriptions = this.internship.researchTopicDescription.split('~');
    } else {
      this.researchTopicDescriptions[0] = this.internship.researchTopicDescription;
    }
  }

  // Getters for edit form
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
