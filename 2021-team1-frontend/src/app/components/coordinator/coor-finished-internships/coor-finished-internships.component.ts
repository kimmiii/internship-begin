import {Component, OnInit} from '@angular/core';
import {Internship} from "../../../models/internship.model";
import {InternshipService} from "../../../services/internship.service";
import {Router} from "@angular/router";
import {Title} from "@angular/platform-browser";
import {StateManagerService} from "../../../services/state-manager.service";
import {Specialisation} from "../../../models/specialisation.model";
import {FormBuilder, FormGroup} from "@angular/forms";
import {forkJoin} from "rxjs";

@Component({
  selector: 'app-coor-finished-internships',
  templateUrl: './coor-finished-internships.component.html',
  styleUrls: ['./coor-finished-internships.component.css']
})
export class CoorFinishedInternshipsComponent implements OnInit {
  isAllDataFetched: boolean = false;
  countVisibleInternships: number = 0;
  loadedInternshipItems: number = 0;
  areAllInternshipItemsLoaded: boolean = false;
  internshipList: Internship[];
  approvedInternshipList: Internship[] = [];
  rejectedInternshipList: Internship[] = [];
  totalInternships: number;
  filterIndex: number;
  specialisationList: Specialisation[] = [];
  specialisationListLoaded: boolean;
  readonly COUNT_FILTEROPTIONS: number = 4;
  academicYears: string[] = [];
  selectedAcademicYear: string;
  periodFilterForm: FormGroup;
  formLoaded: boolean = false;
  selectedPeriod: number;

  constructor(private internshipService: InternshipService, private router: Router, private titleService: Title,
              private stateManagerService: StateManagerService, private formBuilder: FormBuilder) { }

  ngOnInit() {
    this.specialisationListLoaded = false;
    this.titleService.setTitle('Stagebeheer | Stageopdrachten');
    this.filterIndex = this.stateManagerService.selectedFilterIndexFinInternshipsCoordinator;

    this.fetchData();
  }

  fetchData() {
    this.areAllInternshipItemsLoaded = false;
    let specialisationsObservable = this.internshipService.getSpecialisations();
    let academicYearsSpecialisation = this.internshipService.getAcademicYears();

    forkJoin([specialisationsObservable, academicYearsSpecialisation]).subscribe(results => {
      this.specialisationList = results[0];
      this.academicYears = results[1];

      this.specialisationListLoaded = true;

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
    this.areAllInternshipItemsLoaded = false;
    this.clearLists();

    switch(filterIndex) {
      case 1: // All applications
        this.internshipService.getInternships().subscribe(data => {
          console.log('Alle stageopdrachten zijn succesvol opgehaald.');
          this.internshipList = data;
          this.isAllDataFetched = true;
          this.filterInternships();
        });
        break;
      case 2: // Applications that are completed
        this.internshipList = [];
        this.internshipService.getInternships().subscribe(data => {
          console.log('Alle stageopdrachten die volledig volzet zijn, zijn succesvol opgehaald.');
          for(let internship of data) {
            if (internship.completed || internship.projectStatus.code == 'REJ') {
              this.internshipList.push(internship);
            }
          }
          this.isAllDataFetched = true;
          this.filterInternships();
        });
        break;
      case 3: // Applications that are not completed
        this.internshipList = [];
        this.internshipService.getInternships().subscribe(data => {
          console.log('Alle stageopdrachten die nog niet volledig volzet zijn, zijn succesvol opgehaald.');
          for(let internship of data) {
            if (!internship.completed || internship.projectStatus.code == 'REJ') {
              this.internshipList.push(internship);
            }
          }
          this.isAllDataFetched = true;
          this.filterInternships();
        });
        break;
      default: // When specialisation is selected
        this.showInternshipsBasedOnSelectedSpecialisation(filterIndex);
        break;
    }
  }

  createForm() {
    this.periodFilterForm = this.formBuilder.group({
      filterPeriod: [this.stateManagerService.selectedFilterPeriodCoordinator],
    });

    this.formLoaded = true;
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
          if (internship.internshipPeriod[0].periodId == 2) {
            selectedPeriodValue = 2; // SEM1
          } else {
            selectedPeriodValue = 3; // SEM2
          }
        }
      }

      if (this.selectedPeriod == selectedPeriodValue) {
        if (internship.academicYear == this.selectedAcademicYear) {
          switch (internship.projectStatus.code) {
            case 'REJ':
              this.rejectedInternshipList.push(internship);
              this.countVisibleInternships++;
              break;
            case 'APP':
              this.approvedInternshipList.push(internship);
              this.countVisibleInternships++;
              break;
            default:
              break;
          }
        }
      }
    }

    if (this.countVisibleInternships == 0) {
      this.areAllInternshipItemsLoaded = true;
    }
  }

  clearLists() {
    this.approvedInternshipList = [];
    this.rejectedInternshipList = [];
    this.countVisibleInternships = 0;
    this.loadedInternshipItems = 0;
  }

  showInternshipsBasedOnSelectedSpecialisation(filterIndex: number) {
    let index: number = filterIndex - this.COUNT_FILTEROPTIONS;
    let selectedSpecialisationCode: string = this.specialisationList[index].code;

    this.internshipService.getBySpecialisation(selectedSpecialisationCode).subscribe(data => {
      console.log(`Alle stageopdrachten met afstudeerrichtingcode ${selectedSpecialisationCode} zijn succesvol opgehaald.`);
      this.internshipList = data;
      this.filterInternships();
      this.isAllDataFetched = true;
    });
  }

  addToLoadedArray() {
    this.loadedInternshipItems++;

    if (this.countVisibleInternships == this.loadedInternshipItems) {
      this.areAllInternshipItemsLoaded = true;
      this.isAllDataFetched = true;
    }
  }

  proceedToDetailPage(internshipId: string) {
    this.router.navigate(['coordinator/internship-details', internshipId]);
  }

  filter(event: any) {
    this.areAllInternshipItemsLoaded = false;
    this.stateManagerService.selectedFilterIndexFinInternshipsCoordinator = Number(event.target.value);
    this.fetchInternships(Number(event.target.value));
  }

  filterOnAcademicYear(event: any) {
    this.areAllInternshipItemsLoaded = false;
    this.selectedAcademicYear = String(event.target.value);
    this.stateManagerService.selectedFilterAcademicYearCoordinator = this.selectedAcademicYear;
    this.filterInternships();
  }

  // GET periodFilterForm
  get filterPeriod() {
    return this.periodFilterForm.controls.filterPeriod;
  }
}
