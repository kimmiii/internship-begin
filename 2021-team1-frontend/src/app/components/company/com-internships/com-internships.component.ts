import { Component, OnInit } from '@angular/core';
import {Internship} from "../../../models/internship.model";
import {InternshipService} from "../../../services/internship.service";
import {Router} from "@angular/router";
import {Title} from "@angular/platform-browser";
import {StateManagerService} from "../../../services/state-manager.service";
import {forkJoin} from "rxjs";

@Component({
  selector: 'app-start',
  templateUrl: './com-internships.component.html',
  styleUrls: ['./com-internships.component.css']
})
export class ComInternshipsComponent implements OnInit {
  allDataFetched: boolean = false;
  countVisibleInternships: number = 0;
  loadedInternshipItems: number = 0;
  allInternshipItemsAreLoaded: boolean = false;
  internshipList: Internship[];
  submittedInternshipList: Internship[] = [];
  editAskedInternshipList: Internship[] = [];
  approvedInternshipList: Internship[] = [];
  rejectedInternshipList: Internship[] = [];
  academicYears: string[] = [];
  selectedAcademicYear: string;

  constructor(private internshipService: InternshipService, private router: Router,
              private titleService: Title, private stateManagerService: StateManagerService) {
  }

  ngOnInit() {
    this.titleService.setTitle('Stagebeheer | Stageaanvragen');
    this.fetchData();
  }

  fetchData() {
    let companyId: number = this.stateManagerService.companyId;

    let academicYearsObservable = this.internshipService.getAcademicYears();
    let internshipsObservable = this.internshipService.getInternshipByCompanyId(companyId);

    forkJoin([academicYearsObservable, internshipsObservable]).subscribe(results => {
      this.academicYears = results[0];
      this.internshipList = results[1];
      console.log(`Alle internships en academiejaren zijn succesvol opgehaald.`);

      if (this.stateManagerService.selectedFilterAcademicYearCompany == "") {
        this.selectedAcademicYear = this.academicYears[0];
      } else {
        this.selectedAcademicYear = this.stateManagerService.selectedFilterAcademicYearCompany;
      }

      this.filterInternships();
    });
  }

  filterInternships() {
    this.clearLists();

    for (let internship of this.internshipList) {
      if (internship.academicYear == this.selectedAcademicYear) {
        if (internship.projectStatus.code == 'NEW' || internship.projectStatus.code == 'REV') {
          this.submittedInternshipList.push(internship);
          this.countVisibleInternships++;
        }

        if (internship.projectStatus.code == 'FEE') {
          this.editAskedInternshipList.push(internship);
          this.countVisibleInternships++;
        }

        if (internship.projectStatus.code == 'APP') {
          this.approvedInternshipList.push(internship);
          this.countVisibleInternships++;
        }

        if (internship.projectStatus.code == 'REJ') {
          this.rejectedInternshipList.push(internship);
          this.countVisibleInternships++;
        }
      }
    }

    console.log(`Alle internships zijn over de juiste tabellen verdeeld.`);
    this.allDataFetched = true;

    if (this.countVisibleInternships == 0) {
      this.allInternshipItemsAreLoaded = true;
    }
  }

  clearLists() {
    this.submittedInternshipList = [];
    this.editAskedInternshipList = [];
    this.approvedInternshipList = [];
    this.rejectedInternshipList = [];
    this.countVisibleInternships = 0;
    this.loadedInternshipItems = 0;
  }

  addToLoadedArray() {
    this.loadedInternshipItems++;

    if (this.countVisibleInternships == this.loadedInternshipItems) {
      this.allInternshipItemsAreLoaded = true;
    }
  }

  filterOnAcademicYear(event: any) {
    this.allInternshipItemsAreLoaded = false;
    this.selectedAcademicYear = String(event.target.value);
    this.stateManagerService.selectedFilterAcademicYearCompany = this.selectedAcademicYear;
    this.filterInternships();
  }

  proceedToDetailPage(internshipId: string) {
    this.router.navigate(['company/internship-details', internshipId]);
  }
}
