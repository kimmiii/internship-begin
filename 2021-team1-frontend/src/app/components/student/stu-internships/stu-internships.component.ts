import {Component, OnInit} from '@angular/core';
import {InternshipService} from "../../../services/internship.service";
import {Internship} from "../../../models/internship.model";
import {Title} from "@angular/platform-browser";
import {Router} from "@angular/router";
import {StateManagerService} from "../../../services/state-manager.service";
import {GetApprovedInternshipCriteriaModel} from "../../../models/api-models/get-approved-internship-criteria.model";
import {PageCriteriaModel} from "../../../models/page-criteria.model";
import {FilteredInternshipModel} from "../../../models/filtered-internship.model";

@Component({
  selector: 'app-stu-start',
  templateUrl: './stu-internships.component.html',
  styleUrls: ['./stu-internships.component.css']
})
export class StuInternshipsComponent implements OnInit {
  isAllDataFetched: boolean = false;
  internshipList: Internship[];
  readonly COUNT_INTERNSHIPS_PER_PAGE: number = 10;
  countPages: number;
  countPagesArray: number[] = [];
  showFilter: boolean = false;
  filterLoaded: boolean = false;
  internshipItemLoaded: boolean = false;
  pageNumber: number = 1;
  countLoadedInternships: number = 0;

  constructor(private internshipService: InternshipService, private titleService: Title, private router: Router,
              private stateManagerService: StateManagerService) { }

  ngOnInit() {
    this.titleService.setTitle('Stagebeheer | Alle stageopdrachten');
    this.fetchData();
  }

  fetchData() {
    this.countLoadedInternships = 0;
    this.countPagesArray = [];
    this.internshipList = [];
    let criteriaObject: GetApprovedInternshipCriteriaModel = this.createCriteriaObject(); // Object that will be sent for filtering

    this.internshipService.getApprovedInternships(criteriaObject).subscribe(res => {
      console.log('Internships zijn succesvol opgehaald.');
      let internships = res.body.internships;

      this.showInternships(internships, res);
    });
  }

  createCriteriaObject(): GetApprovedInternshipCriteriaModel {
    let pageCriteriaObject: PageCriteriaModel = new PageCriteriaModel(this.COUNT_INTERNSHIPS_PER_PAGE, this.pageNumber);
    let filterCriteria: FilteredInternshipModel = this.stateManagerService.filteredInternship;

    return new GetApprovedInternshipCriteriaModel(pageCriteriaObject, filterCriteria);
  }

  filter(event: FilteredInternshipModel) {
    this.countLoadedInternships = 0;
    this.isAllDataFetched = false;
    this.stateManagerService.filteredInternship = event;
    this.stateManagerService.hideCompletedInternships = event.hideCompletedInternships;
    this.countPagesArray = [];
    this.internshipList = [];
    this.pageNumber = 1;
    let criteriaObject: PageCriteriaModel = new PageCriteriaModel(this.COUNT_INTERNSHIPS_PER_PAGE, this.pageNumber);
    let filterCriteriaObject: GetApprovedInternshipCriteriaModel = new GetApprovedInternshipCriteriaModel(criteriaObject, event);

    this.internshipService.getApprovedInternships(filterCriteriaObject).subscribe(res => {
      console.log('Goedgekeurde internships zijn succesvol opgehaald.');
      let internships = res.body.internships;
      this.showInternships(internships, res);
    });
  }

  showInternships(internships: Internship[], res) {
    // Hide completed internships
    if (this.stateManagerService.hideCompletedInternships) {
      for(let internship of internships) {
        if (!internship.completed) {
          this.internshipList.push(internship);
        }
      }
    } else {
      this.internshipList = internships;
    }

    this.countPages = res.body.pages;
    for (let i = 0; i < this.countPages; i++) {
      this.countPagesArray.push(i);
    }
    this.isAllDataFetched = true;
  }

  updateIndex(pageIndex: number) {
    this.pageNumber = pageIndex;
    this.fetchData();
  }

  decreasePageIndex() {
    if (this.pageNumber > 1) {
      this.pageNumber -= 1;
      this.fetchData();
    }
  }

  increasePageIndex() {
    if (this.pageNumber < this.countPages) {
      this.pageNumber += 1;
      this.fetchData();
    }
  }

  proceedToDetailPage(internshipId: string) {
    this.router.navigate(['student/internship-details', internshipId]);
  }

  toggleFilter() {
    this.showFilter = !this.showFilter;
  }

  hideSpinnerFilter() {
    this.filterLoaded = true;
  }

  showSpinnerFilter() {
    this.filterLoaded = false;
  }

  hideSpinnerInternshipItem() {
    this.countLoadedInternships++;

    if (this.internshipList.length == this.countLoadedInternships) {
      this.internshipItemLoaded = true;
    }
  }
}
