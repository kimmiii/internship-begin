import {Component, Input, OnInit} from '@angular/core';
import {Internship} from "../../../models/internship.model";
import {Router} from "@angular/router";
import {UserInternships} from "../../../models/user-internships.model";
import {StateManagerService} from "../../../services/state-manager.service";

@Component({
  selector: 'app-stu-my-internship-item',
  templateUrl: './stu-my-internship-item.component.html',
  styleUrls: ['./stu-my-internship-item.component.css']
})
export class StuMyInternshipItemComponent implements OnInit {
  @Input() internship: Internship;
  showClock: boolean = false;
  showCheckMark: boolean = false;
  showCross: boolean = false;

  constructor(private router: Router, private stateManagerService: StateManagerService) { }

  ngOnInit() {
    this.fetchUserInternships();
  }

  showInternshipDetails() {
    let internshipId = this.internship.internshipId;
    this.router.navigate(['student/internship-details', internshipId]);
  }

  fetchUserInternships() {
    let userInternships: UserInternships[] = this.internship.userInternships;

    for(let ui of userInternships) {
      if (ui.userId == this.stateManagerService.userId && ui.hireConfirmed && !ui.hireApproved && ui.rejectionFeedback == null) {
        this.showClock = true;
      }

      if (ui.userId == this.stateManagerService.userId && ui.hireApproved) {
        this.showCheckMark = true;
      }

      if (ui.userId == this.stateManagerService.userId && !ui.hireApproved && ui.rejectionFeedback != null) {
        this.showCross = true;
      }
    }
  }
}
