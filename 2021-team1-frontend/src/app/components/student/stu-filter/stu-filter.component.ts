import {Component, EventEmitter, OnInit, Output} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {InternshipService} from "../../../services/internship.service";
import {forkJoin} from "rxjs";
import {Specialisation} from "../../../models/specialisation.model";
import {Environment} from "../../../models/environment.model";
import {Period} from "../../../models/period.model";
import {Company} from "../../../models/company.model";
import {CompanyService} from "../../../services/company.service";
import {Expecation} from "../../../models/expectation.model";
import {InternshipPeriod} from "../../../models/internship-period.model";
import {InternshipSpecialisation} from "../../../models/internship-specialisation.model";
import {InternshipEnvironment} from "../../../models/internship-environment.model";
import {InternshipExpectation} from "../../../models/internship-expectation.model";
import {StateManagerService} from "../../../services/state-manager.service";
import {FilteredInternshipModel} from "../../../models/filtered-internship.model";
import {UserFavourites} from "../../../models/user-favourites.model";
import {UserInternships} from "../../../models/user-internships.model";

@Component({
  selector: 'app-stu-filter',
  templateUrl: './stu-filter.component.html',
  styleUrls: ['./stu-filter.component.css']
})
export class StuFilterComponent implements OnInit {
  @Output() filterLoaded: EventEmitter<any> = new EventEmitter<any>();
  @Output() filterNotLoaded: EventEmitter<any> = new EventEmitter<any>();
  @Output() executeFiltering: EventEmitter<FilteredInternshipModel> = new EventEmitter<FilteredInternshipModel>();
  filterForm: FormGroup;
  isAllDataFetched: boolean = false;
  companyList: Company[];
  specialisationList: Specialisation[];
  environmentList: Environment[];
  periodList: Period[];
  expectationList: Expecation[];
  checkedSpecialisationList: boolean[];
  checkedEnvironmentList: boolean[];
  checkedPeriodList: boolean[];
  checkedExpectationList: boolean[];

  constructor(private formBuilder: FormBuilder, private internshipService: InternshipService, private companyService: CompanyService,
              private stateManagerService: StateManagerService) { }

  ngOnInit() {
    this.fetchData();
  }

  fetchData() {
    let specialisationObservable = this.internshipService.getSpecialisations();
    let environmentObservable = this.internshipService.getEnvironments();
    let periodObservable = this.internshipService.getPeriods();
    let companyObservable = this.companyService.getActiveCompanies();
    let expectationObservable = this.internshipService.getExpectations();

    forkJoin([specialisationObservable, environmentObservable, periodObservable, companyObservable, expectationObservable]).subscribe( results => {
      this.specialisationList = results[0];
      this.environmentList = results[1];
      this.periodList = results[2];
      this.companyList = results[3];
      this.expectationList = results[4];

      this.initializeAllCheckedLists();
      this.createForm();

      this.isAllDataFetched = true;
      this.filterLoaded.emit();
    });
  }

  initializeAllCheckedLists() {
    this.checkedSpecialisationList = this.initializeCheckedSpecialisationList();
    this.checkedEnvironmentList = this.initializeCheckedEnvironmentList();
    this.checkedPeriodList = this.initializeCheckedPeriodList();
    this.checkedExpectationList = this.initializeCheckedExpectationList();
  }

  initializeCheckedSpecialisationList(): Array<boolean> {
    let checkedCheckboxesList: Array<boolean> = [];

    for (let index = 0; index < this.specialisationList.length; index++){
      if (this.stateManagerService.filterInternshipSpecialisation.length > 0) {
        checkedCheckboxesList[index] = this.stateManagerService.filterInternshipSpecialisation[index];
      } else {
        checkedCheckboxesList[index] = false;
      }
    }
    return checkedCheckboxesList;
  }

  initializeCheckedEnvironmentList(): Array<boolean> {
    let checkedCheckboxesList: Array<boolean> = [];

    for (let index = 0; index < this.environmentList.length; index++){
      if (this.stateManagerService.filterInternshipEnvironment.length > 0) {
        checkedCheckboxesList[index] = this.stateManagerService.filterInternshipEnvironment[index];
      } else {
        checkedCheckboxesList[index] = false;
      }
    }
    return checkedCheckboxesList;
  }

  initializeCheckedPeriodList(): Array<boolean> {
    let checkedCheckboxesList: Array<boolean> = [];

    for (let index = 0; index < this.periodList.length; index++){
      if (this.stateManagerService.filterInternshipPeriod.length > 0) {
        checkedCheckboxesList[index] = this.stateManagerService.filterInternshipPeriod[index];
      } else {
        checkedCheckboxesList[index] = false;
      }
    }
    return checkedCheckboxesList;
  }

  initializeCheckedExpectationList(): Array<boolean> {
    let checkedCheckboxesList: Array<boolean> = [];

    for (let index = 0; index < this.expectationList.length; index++){
      if (this.stateManagerService.filterExpectationList.length > 0) {
        checkedCheckboxesList[index] = this.stateManagerService.filterExpectationList[index];
      } else {
        checkedCheckboxesList[index] = false;
      }
    }
    return checkedCheckboxesList;
  }

  createForm() {
    this.filterForm = this.formBuilder.group({
      hideCompletedInternships: [this.stateManagerService.hideCompletedInternships],
      showFavouritesApplied: [this.stateManagerService.filterShowFavouritesAppliedValue],
      company: [this.stateManagerService.filterCompany],
      internshipTitleDescription: [this.stateManagerService.filterAssignmentDescription, [Validators.maxLength(500)]],
      specialisations: this.formBuilder.array(
        this.specialisationList.map(() => this.formBuilder.control(''))
      ),
      environments: this.formBuilder.array(
        this.environmentList.map(() => this.formBuilder.control(''))
      ),
      otherEnvironment: [this.stateManagerService.filterInternshipEnvironmentOther, [Validators.maxLength(100)]],
      periods: this.formBuilder.array(
        this.periodList.map(() => this.formBuilder.control(''))
      ),
      expectations: this.formBuilder.array(
        this.expectationList.map(() => this.formBuilder.control(''))
      )
    });
  }

  filter() {
    this.fillStateManager();
    let filteredInternship: FilteredInternshipModel = this.createFilteredInternshipModel();
    this.executeFiltering.emit(filteredInternship);
    this.filterLoaded.emit();
  }

  createFilteredInternshipModel(): FilteredInternshipModel {
    let companyId = this.stateManagerService.filterCompany;
    let internshipEnvironmentOthers = this.stateManagerService.filterInternshipEnvironmentOther;
    let assignmentDescription = this.stateManagerService.filterAssignmentDescription;
    let internshipPeriod: InternshipPeriod[] = [];
    let internshipSpecialisation: InternshipSpecialisation[] = [];
    let internshipEnvironment: InternshipEnvironment[] = [];
    let internshipExpectation: InternshipExpectation[] = [];
    let userFavourites: UserFavourites[] = [];
    let userInternships: UserInternships[] = [];

    for(let i = 0; i < this.checkedPeriodList.length; i++) {
      if (this.checkedPeriodList[i] == true) {
        internshipPeriod.push(new InternshipPeriod(this.periodList[i].periodId));
      }
      this.stateManagerService.filterInternshipPeriod[i] = this.checkedPeriodList[i];
    }

    for(let i = 0; i < this.checkedSpecialisationList.length; i++) {
      if (this.checkedSpecialisationList[i] == true) {
        internshipSpecialisation.push(new InternshipSpecialisation(this.specialisationList[i].specialisationId));
      }
      this.stateManagerService.filterInternshipSpecialisation[i] = this.checkedSpecialisationList[i];
    }

    for(let i = 0; i < this.checkedEnvironmentList.length; i++) {
      if (this.checkedEnvironmentList[i] == true) {
        internshipEnvironment.push(new InternshipEnvironment(this.environmentList[i].environmentId));
      }
      this.stateManagerService.filterInternshipEnvironment[i] = this.checkedEnvironmentList[i];
    }

    for(let i = 0; i < this.checkedExpectationList.length; i++) {
      if (this.checkedExpectationList[i] == true) {
        internshipExpectation.push(new InternshipExpectation(this.expectationList[i].expectationId));
      }
      this.stateManagerService.filterExpectationList[i] = this.checkedExpectationList[i];
    }

    if (this.showFavouritesApplied.value == 1) {
      let userFavouritesObject = new UserFavourites(this.stateManagerService.userId);
      userFavourites.push(userFavouritesObject);
    }

    if (this.showFavouritesApplied.value == 2) {
      let userInternshipsObject = new UserInternships(this.stateManagerService.userId);
      userInternships.push(userInternshipsObject);
    }

    let filteredInternship: FilteredInternshipModel = new FilteredInternshipModel(companyId, internshipEnvironmentOthers, assignmentDescription, internshipPeriod,
      internshipSpecialisation, internshipEnvironment, internshipExpectation, userFavourites, userInternships);

    if (this.hideCompletedInternships.value) {
      filteredInternship.hideCompletedInternships = true;
    }

   return filteredInternship
  }

  fillStateManager() {
    this.stateManagerService.filterCompany = Number(this.company.value);
    if (this.otherEnvironment.value == null) {
      this.stateManagerService.filterInternshipEnvironmentOther = '';
    } else {
      this.stateManagerService.filterInternshipEnvironmentOther = this.otherEnvironment.value;
    }

    if (this.internshipTitleDescription.value == null) {
      this.stateManagerService.filterAssignmentDescription = '';
    } else {
      this.stateManagerService.filterAssignmentDescription = this.internshipTitleDescription.value;
    }

    this.stateManagerService.filterShowFavouritesAppliedValue = this.showFavouritesApplied.value;
    this.stateManagerService.hideCompletedInternships = this.hideCompletedInternships.value;
  }

  resetFilter() {
    this.filterNotLoaded.emit();
    this.stateManagerService.filterShowFavouritesAppliedValue = 0;
    this.filterForm.reset();
    this.filter();
  }

  // GETTER filterForm
  get hideCompletedInternships() {
    return this.filterForm.controls.hideCompletedInternships;
  }
  get showFavouritesApplied() {
    return this.filterForm.controls.showFavouritesApplied;
  }
  get company() {
    return this.filterForm.controls.company;
  }
  get internshipTitleDescription() {
    return this.filterForm.controls.internshipTitleDescription;
  }
  get specialisations() {
    return this.filterForm.controls.specialisations;
  }
  get environments() {
    return this.filterForm.controls.environments;
  }
  get otherEnvironment() {
    return this.filterForm.controls.otherEnvironment;
  }
  get periods() {
    return this.filterForm.controls.periods;
  }
  get expectations() {
    return this.filterForm.controls.expectations;
  }
}
