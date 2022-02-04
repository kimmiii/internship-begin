import { Component, OnInit } from '@angular/core';
import {Internship} from "../../../models/internship.model";
import {InternshipService} from "../../../services/internship.service";
import {Contact} from "../../../models/contact.model";
import {Router} from "@angular/router";
import {Title} from "@angular/platform-browser";
import {StateManagerService} from "../../../services/state-manager.service";
import {FormBuilder, FormGroup} from "@angular/forms";

@Component({
  selector: 'app-start',
  templateUrl: './coor-internships.component.html',
  styleUrls: ['./coor-internships.component.css']
})
export class CoorInternshipsComponent implements OnInit {
  isAllDataFetched: boolean = false;
  countVisibleInternships: number = 0;
  loadedInternshipItems: number = 0;
  allInternshipItemsAreLoaded: boolean = false;
  internshipList: Internship[];
  companyEvaluateInternshipList: Internship[] = [];
  companyReviewInternshipList: Internship[] = [];
  promotorEvaluateInternshipList: Internship[] = [];
  promotorReviewInternshipList: Internship[] = [];
  contact: Contact;
  filterIndex: number;
  academicYears: string[] = [];
  selectedAcademicYear: string;
  periodFilterForm: FormGroup;
  formLoaded: boolean = false;
  selectedPeriod: number;

  constructor(private internshipService: InternshipService, private router: Router, private titleService: Title,
              private stateManagerService: StateManagerService, private formBuilder: FormBuilder) { }

  ngOnInit() {
    this.filterIndex = this.stateManagerService.selectedFilterIndexInternshipsCoordinator;
    this.titleService.setTitle('Stagebeheer | Stageaanvragen');
    this.fetchData();
  }

  fetchData() {
    this.allInternshipItemsAreLoaded = false;
    this.internshipService.getAcademicYears().subscribe(data => {
      this.academicYears = data;

      if (this.stateManagerService.selectedFilterAcademicYearCoordinator == "") {
        this.selectedAcademicYear = this.academicYears[0];
      } else {
        this.selectedAcademicYear = this.stateManagerService.selectedFilterAcademicYearCoordinator;
      }

      this.fetchInternships(this.filterIndex);
      this.createForm();
    });
  }

  fetchInternships(filterIndex: number) {
    this.clearLists();

    switch(filterIndex) {
      case 1: // All applications
        this.internshipService.getInternships().subscribe(data => {
          console.log(`Alle stageaanvragen zijn succesvol opgehaald.`);
          this.internshipList = data;
          this.isAllDataFetched = true;
          this.filterInternships();
        });
        break;
      case 2: // Applications with specialisation Elektronica-ICT
        this.internshipService.getBySpecialisation('EICT').subscribe(data => {
          console.log(`Alle stageaanvragen met afstudeerrichting Elektronica-ICT zijn succesvol opgehaald.`);
          this.internshipList = data;
          this.isAllDataFetched = true;
          this.filterInternships();
        });
        break;
      case 3: // Application with NO specialisation Elektronica-ICT
        this.internshipService.getNotBySpecialisationEICT().subscribe(data => {
          console.log(`Alle stageaanvragen zonder afstudeerrichting Elektronica-ICT zijn succesvol opgehaald.`);
          this.internshipList = data;
          this.isAllDataFetched = true;
          this.filterInternships();
        });
        break;
    }
  }

  filterInternships() {
    this.selectedPeriod = this.filterPeriod.value;
    this.stateManagerService.selectedFilterPeriodCoordinator = this.selectedPeriod;
    this.clearLists();

    for (let internship of this.internshipList) {
      // Define selectedPeriodValue
      let selectedPeriodValue: number = 0;

      if (this.selectedPeriod != 0) {
        if (internship.internshipPeriod.length == 2) { // SEM1+SEM2
          selectedPeriodValue = 1;
        } else {
          if (internship.internshipPeriod[0].periodId == 2) { // SEM1
            selectedPeriodValue = 2;
          } else {
            selectedPeriodValue = 3;
          }
        }
      }

      if (this.selectedPeriod == selectedPeriodValue) {
        if (internship.academicYear == this.selectedAcademicYear) {
          if (internship.projectStatus.code == 'NEW') {
            this.companyEvaluateInternshipList.push(internship);
            this.countVisibleInternships++;
          }

          if (internship.projectStatus.code == 'FEE') {
            this.companyReviewInternshipList.push(internship);
            this.countVisibleInternships++;
          }

          if (internship.projectStatus.code == 'REV') {
            if (internship.internshipAssignedUser[0].user.role.code == 'COO') {
              this.promotorEvaluateInternshipList.push(internship);
              this.countVisibleInternships++;
            }

            if (internship.internshipAssignedUser[0].user.role.code == 'REV') {
              this.promotorReviewInternshipList.push(internship);
              this.countVisibleInternships++;
            }
          }
        }
      }
    }

    if (this.countVisibleInternships == 0) {
      this.allInternshipItemsAreLoaded = true;
    }
  }

  createForm() {
    this.periodFilterForm = this.formBuilder.group({
      filterPeriod: [this.stateManagerService.selectedFilterPeriodCoordinator],
    });

    this.formLoaded = true;
  }

  clearLists() {
    this.companyEvaluateInternshipList = [];
    this.companyReviewInternshipList = [];
    this.promotorEvaluateInternshipList = [];
    this.promotorReviewInternshipList = [];
    this.countVisibleInternships = 0;
    this.loadedInternshipItems = 0;
  }

  addToLoadedArray() {
    this.loadedInternshipItems++;

    if (this.countVisibleInternships == this.loadedInternshipItems) {
      this.allInternshipItemsAreLoaded = true;
    }
  }

  proceedToDetailPage(internshipId: string) {
    this.router.navigate(['coordinator/internship-details', internshipId]);
  }

  filter(event: any) {
    this.allInternshipItemsAreLoaded = false;
    this.stateManagerService.selectedFilterIndexInternshipsCoordinator = Number(event.target.value);
    this.fetchInternships(Number(event.target.value));
  }

  filterOnAcademicYear(event: any) {
    this.allInternshipItemsAreLoaded = false;
    this.selectedAcademicYear = String(event.target.value);
    this.stateManagerService.selectedFilterAcademicYearCoordinator = this.selectedAcademicYear;
    this.filterInternships();
  }

  // GET periodFilterForm
  get filterPeriod() {
    return this.periodFilterForm.controls.filterPeriod;
  }
}
