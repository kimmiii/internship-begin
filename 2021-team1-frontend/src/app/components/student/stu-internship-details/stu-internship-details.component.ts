import {Component, OnInit} from '@angular/core';
import {Internship} from "../../../models/internship.model";
import {Title} from "@angular/platform-browser";
import {ActivatedRoute, Router} from "@angular/router";
import {InternshipService} from "../../../services/internship.service";
import {UserInternships} from "../../../models/user-internships.model";
import {StateManagerService} from "../../../services/state-manager.service";
import {forkJoin} from "rxjs";

@Component({
  selector: 'app-stu-internship-details',
  templateUrl: './stu-internship-details.component.html',
  styleUrls: ['./stu-internship-details.component.css']
})
export class StuInternshipDetailsComponent implements OnInit {
  isAllDataFetched: boolean = false;
  internship: Internship;
  textApplyButton: string = 'SOLLICITEER';
  applyButtonDisabled: boolean;
  appliedSuccessfully: boolean = false;
  rejectedApplicationSuccessfully: boolean = false;
  showInternshipAccept: boolean = false;
  showApplyButton: boolean = true;
  userInternship: UserInternships;
  showHireConfirmedConfirmationMessage: boolean = false;
  studentConfirmedHireRequest: boolean = false;
  showAlreadyAssignedToInternshipMessage: boolean = false;
  myInternshipId: string = '0';
  studentHasAlreadyConfirmedAHireRequest: boolean = false;
  studentIsAlreadyAssignedToInternship: boolean = false;
  showRejectionMessage: boolean = false;
  rejectionMessage: string;
  modalPopupIsVisible: boolean = false;

  constructor(private internshipService: InternshipService, private titleService: Title, private route: ActivatedRoute,
              private router: Router, private stateManagerService: StateManagerService) {
  }

  ngOnInit() {
    this.titleService.setTitle(`Stagebeheer | Stageopdracht`);
    let internshipId: string = this.route.snapshot.paramMap.get('internshipId');

    this.applyButtonDisabled = false;
    this.fetchData(internshipId);
  }

  fetchData(internshipId: string) {
    let studentId: number = Number(this.stateManagerService.userId);

    let internshipByIdObservable = this.internshipService
      .getInternshipById(internshipId); // GET this internship
    let getUserInternshipByInternshipIdAndStudentIdObservable = this.internshipService
      .getUserInternshipByInternshipIdAndStudentId(Number(internshipId), studentId);  // GET this UserInternship
    let studentConfirmedHireRequestObservable = this.internshipService
      .studentConfirmedHireRequest(studentId); // GET TRUE or FALSE if student has already confirmed a hireRequest
    let studentApprovedHireRequestObservable = this.internshipService
      .studentApprovedHireRequest(studentId); // GET TRUE or FALSE if student is already assigned to internship (HireApproved = true)

    forkJoin([internshipByIdObservable, getUserInternshipByInternshipIdAndStudentIdObservable,
    studentConfirmedHireRequestObservable, studentApprovedHireRequestObservable]).subscribe(results => {
      this.internship = results[0];
      this.userInternship = results[1];
      this.studentHasAlreadyConfirmedAHireRequest = results[2];
      this.studentIsAlreadyAssignedToInternship = results[3];
      console.log('Data is succesvol opgehaald.');

      if (this.studentIsAlreadyAssignedToInternship) {
        this.internshipService.getInternshipIdByInternshipAssignedUser(studentId).subscribe(data => {
          this.myInternshipId = String(data);
          this.markApplyButton();
          this.defineMessage();
        });
      } else {
        this.markApplyButton();
        this.defineMessage();
      }
    });
  }

  defineMessage() {
    if (this.myInternshipId != this.internship.internshipId) {
      // Message (hireApproved): you are already assigned to an internship
      if (this.studentIsAlreadyAssignedToInternship) {
        this.showAlreadyAssignedToInternshipMessage = true;
        this.showApplyButton = false;
      }
      // Message (hireConfirmed): wait for confirmation by coordinator (for this internship)
      else if (this.userInternship.hireConfirmed && this.userInternship.rejectionFeedback == null) {
        this.showHireConfirmedConfirmationMessage = true;
        this.showApplyButton = false;
      }
      // Message (hireRequested): accept hire request
      else if ((this.userInternship.hireRequested && !this.studentHasAlreadyConfirmedAHireRequest) && this.userInternship.rejectionFeedback == null) {
        this.showInternshipAccept = true;
        this.showApplyButton = false;
      }
      // Message: can't apply for insternship anymore because you have confirmed another hireRequest
      else if (this.studentHasAlreadyConfirmedAHireRequest && !this.userInternship.hireApproved && this.userInternship.rejectionFeedback == null) {
        this.studentConfirmedHireRequest = true;
        this.showApplyButton = false;
        // Message: rejection message
      }
      else if (!this.userInternship.hireApproved && this.userInternship.rejectionFeedback != null){
        this.showRejectionMessage = true;
        this.showApplyButton = false;
        this.rejectionMessage = this.userInternship.rejectionFeedback.replace(/\n/g, "<br/>");
      }
    }

    if (this.myInternshipId == this.internship.internshipId) {
      this.showApplyButton = false;
    }

    this.isAllDataFetched = true;
  }


  apply() {
    this.isAllDataFetched = false;
    let userInternships = new UserInternships(this.stateManagerService.userId);
    userInternships.internshipId = Number(this.internship.internshipId);
    this.closeModalPopup();

    this.internshipService.applyInternship(userInternships).subscribe( () => {
      console.log(`Succesvol gesolliciteerd voor internship.`);
      this.appliedSuccessfully = true;
      this.ngOnInit();
      this.hideConfirmationPopup();
      }
    );
  }

  removeApplication() {
    this.isAllDataFetched = false;
    let userInternships = new UserInternships(this.stateManagerService.userId);
    userInternships.internshipId = Number(this.internship.internshipId);

    this.internshipService.removeApplication(userInternships).subscribe( res => {
      console.log('Sollicitatie voor internship succesvol ingetrokken.');
      this.rejectedApplicationSuccessfully = true;
      this.ngOnInit();
       this.hideConfirmationPopup();
      }
    );
  }

  markApplyButton() {
    let userInternships: UserInternships[] = this.internship.userInternships;

    for(let item of userInternships) {
      if (item.userId == this.stateManagerService.userId) {
        this.applyButtonDisabled = true;
      }
    }
  }

  acceptHireRequest() {
    let internshipId: number = Number(this.internship.internshipId);
    let userId: number = this.stateManagerService.userId;

    this.internshipService.setHireConfirmedToTrue(internshipId, userId).subscribe(() => {
      this.showInternshipAccept = false;
      this.showHireConfirmedConfirmationMessage = true;
    })
  }

  proceedToInternship() {
    this.isAllDataFetched = false;

    this.internshipService.getInternshipById(this.myInternshipId).subscribe(data => {
      this.internship = data;
      this.resetFlags();
      this.router.navigateByUrl(`student/internship-details/${this.internship.internshipId}`);
      this.isAllDataFetched = true;
    });
  }

  resetFlags() {
    this.showAlreadyAssignedToInternshipMessage = false;
    this.showHireConfirmedConfirmationMessage = false;
    this.showInternshipAccept = false;
    this.studentConfirmedHireRequest = false;
    this.showRejectionMessage = false;
    this.showApplyButton = false;
    this.modalPopupIsVisible = false;
  }

  openModalPopup() {
    this.modalPopupIsVisible = true;
  }

  closeModalPopup() {
    this.modalPopupIsVisible = false;
  }

  hideConfirmationPopup() {
    setTimeout(() => { this.appliedSuccessfully = false }, 5000);
    setTimeout(() => { this.rejectedApplicationSuccessfully = false}, 5000);
  }
}
