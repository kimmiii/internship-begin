import { Component, OnInit } from '@angular/core';
import {Internship} from "../../../models/internship.model";
import {InternshipService} from "../../../services/internship.service";
import {Router} from "@angular/router";
import {Title} from "@angular/platform-browser";
import {StateManagerService} from "../../../services/state-manager.service";
import {InternshipAssignedUser} from "../../../models/internship-assigned-user.model";

@Component({
  selector: 'app-rev-start',
  templateUrl: './rev-internships.component.html',
  styleUrls: ['./rev-internships.component.css']
})
export class RevInternshipsComponent implements OnInit {
  isAllDataFetched: boolean = false;
  countDisplayedInternships: number;
  allInternshipItemsAreLoaded: boolean = false;
  internshipList: Internship[];
  toReviewInternshipList: Internship[] = [];
  reviewedInternshipList: Internship[] = [];
  approvedInternshipList: Internship[] = [];
  rejectedInternshipList: Internship[] = [];

  constructor(private internshipService: InternshipService, private router: Router,
              private titleService: Title, private stateManagerService: StateManagerService) { }

  ngOnInit() {
    this.countDisplayedInternships = 0;
    this.fetchInternships();
    this.titleService.setTitle('Stagebeheer | Stageaanvragen');
  }

  fetchInternships() {
    this.allInternshipItemsAreLoaded = false;
    let userId: number = this.stateManagerService.userId;

    this.internshipService.getInternshipsByReviewerId(userId).subscribe(data => {
      this.internshipList = data;
      console.log(`Internships voor reviewer met id ${userId} zijn succesvol opgehaald.`);
      this.filterInternships();

      if (this.internshipList.length === 0) {
        this.allInternshipItemsAreLoaded = true;
      }

      this.isAllDataFetched = true;
    });
  }

  filterInternships() {
    for (let internship of this.internshipList) {

      if (this.internshipAssignedToMe(internship)) {
        this.toReviewInternshipList.push(internship);
      } else {
        switch (internship.projectStatus.code) {
          case 'APP': // Approved internships
            this.approvedInternshipList.push(internship);
            break;
          case 'REJ': // Rejected internships
            this.rejectedInternshipList.push(internship);
            break;
          default: // Already reviewed insternships
            this.reviewedInternshipList.push(internship);
            break;
        }
      }
    }
  }

  internshipAssignedToMe(internship: Internship) {
    let assignedUserList: InternshipAssignedUser[] = internship.internshipAssignedUser;

    for (let assignedUser of assignedUserList) {
      let assignedUserId: number = assignedUser.userId;
      if (assignedUserId == this.stateManagerService.userId) {
        return true;
      }
    }

    return false;
  }

  addToLoadedArray() {
    this.countDisplayedInternships++;

    if (this.internshipList.length === this.countDisplayedInternships) {
      this.allInternshipItemsAreLoaded = true;
      this.isAllDataFetched = true;
    }
  }

  proceedToDetailPage(internshipId: string) {
    this.router.navigate(['reviewer/internship-details', internshipId]);
  }
}
