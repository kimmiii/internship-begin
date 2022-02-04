import {Component, OnInit} from '@angular/core';
import {Title} from "@angular/platform-browser";
import {Internship} from "../../../models/internship.model";
import {InternshipService} from "../../../services/internship.service";
import {StateManagerService} from "../../../services/state-manager.service";
import {forkJoin} from "rxjs";
import {Router} from "@angular/router";
import {UserInternships} from "../../../models/user-internships.model";

@Component({
  selector: 'app-stu-my-internships',
  templateUrl: './stu-my-internships.component.html',
  styleUrls: ['./stu-my-internships.component.css']
})
export class StuMyInternshipsComponent implements OnInit {
  isAllDataFetched: boolean = false;
  showSpinner: boolean = false;
  myInternshipList: Internship[] = [];
  studentIsAlreadyAssignedToInternship: boolean = false;
  myInternshipId: string;

  constructor(private titleService: Title, private internshipService: InternshipService,
              private stateManagerService: StateManagerService, private router: Router) { }

  ngOnInit() {
    this.showSpinner = true;
    this.titleService.setTitle('Stagebeheer | Mijn stageopdrachten');

    this.fetchData();
  }

  fetchData() {
    let studentId: number = this.stateManagerService.userId;

    let getHireRequestedInternshipsByStudentIdObservable = this.internshipService
      .getHireRequestedInternshipsByStudentId(studentId); // GET UserInternships for student where HireRequested = true
    let studentApprovedHireRequestObservable = this.internshipService
      .studentApprovedHireRequest(studentId); // GET TRUE or FALSE if student is already assigned to internship (HireApproved = true)

    forkJoin([getHireRequestedInternshipsByStudentIdObservable, studentApprovedHireRequestObservable]).subscribe(results => {
      let userInternshipsList = results[0];
      this.studentIsAlreadyAssignedToInternship = results[1];

      let userHasAlreadyConfirmedAnInternship: boolean = false;
      for (let userInternship of userInternshipsList) {
        userInternship.internship.userInternships.push(userInternship);
        this.myInternshipList.push(userInternship.internship);

        if (userInternship.hireConfirmed) {
          userHasAlreadyConfirmedAnInternship = true;
        }
      }

      // If one of the internships is already confirmed, refill table
      if (userHasAlreadyConfirmedAnInternship) {
        this.refillInternshipTable(userInternshipsList);
      }

      if (this.studentIsAlreadyAssignedToInternship) {
        this.internshipService.getInternshipIdByInternshipAssignedUser(studentId).subscribe(data => {
          this.myInternshipId = String(data);
          this.isAllDataFetched = true;
          this.showSpinner = false;
        });
      } else {
        this.isAllDataFetched = true;
        this.showSpinner = false;
      }
    });
  }

  proceedToInternship() {
    this.router.navigate(['student/internship-details', this.myInternshipId]);
  }

  refillInternshipTable(userInternshipsList: UserInternships[]) {
    this.myInternshipList = [];

    for(let userInternship of userInternshipsList) {
      if (userInternship.hireConfirmed) {
        userInternship.internship.userInternships.push(userInternship);
        this.myInternshipList.push(userInternship.internship);
      }
    }
  }
}
