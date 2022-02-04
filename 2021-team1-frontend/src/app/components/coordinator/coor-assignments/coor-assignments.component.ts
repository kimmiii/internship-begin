import { Component, OnInit } from '@angular/core';
import {InternshipService} from "../../../services/internship.service";
import {UserInternships} from "../../../models/user-internships.model";
import {Title} from "@angular/platform-browser";
import {ActivatedRoute} from "@angular/router";

@Component({
  selector: 'app-coor-assignments',
  templateUrl: './coor-assignments.component.html',
  styleUrls: ['./coor-assignments.component.css']
})
export class CoorAssignmentsComponent implements OnInit {
  isAllDataFetched: boolean = false;
  allUserInternshipsList: UserInternships[] = [];
  userInternshipsNotApprovedList: UserInternships[] = [];
  userInternshipsApprovedList: UserInternships[] = [];
  userInternshipsRejectedList: UserInternships[] = [];
  showSpinner: boolean = false;
  showConfirmationPopupApprovalHireRequest: boolean = false;

  constructor(private internshipService: InternshipService, private titleService: Title, private route: ActivatedRoute) { }

  ngOnInit() {
    this.showSpinner = true;
    this.titleService.setTitle('Stagebeheer | Toewijzingen');

    this.fetchUserInternships();

    // Route coming from coor-user-internship-details (after confirmation)
    if (this.route.snapshot.paramMap.get('confirmed')) {
      this.showConfirmationPopupApprovalHireRequest = true;
      this.hideConfirmationPopup();
    }
  }

  fetchUserInternships() {
    // TODO deze call haalt niet meer alle nodige stages op
    this.internshipService.getUserInternships().subscribe(data => {
      this.allUserInternshipsList = data;

      for (let i=0; i < this.allUserInternshipsList.length; i++) {
        if (!this.allUserInternshipsList[i].hireApproved) { // Only show student+internship when student is not yet assigned to the internship
          if (this.allUserInternshipsList[i].evaluatedAt == null && this.allUserInternshipsList[i].hireConfirmed) { // Only when student is not yet evaluated by stage coordinator
            this.userInternshipsNotApprovedList.push(this.allUserInternshipsList[i]);
          } else if (this.allUserInternshipsList[i].rejectionFeedback != null) { // Only show student+internship when student is rejected
            this.userInternshipsRejectedList.push(this.allUserInternshipsList[i]);
          }
        } else { // Only show student+internship when student is assigned to the internship
          this.userInternshipsApprovedList.push(this.allUserInternshipsList[i]);
        }
      }

      this.isAllDataFetched = true;
      this.showSpinner = false;
    })
  }

  hideConfirmationPopup() {
    setTimeout(() => { this.showConfirmationPopupApprovalHireRequest = false }, 5000);
  }
}
