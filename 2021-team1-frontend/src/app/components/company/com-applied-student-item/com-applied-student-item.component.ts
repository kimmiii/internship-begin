import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {User} from "../../../models/user.model";
import {AccountService} from "../../../services/account.service";
import {UserInternships} from "../../../models/user-internships.model";
import {Internship} from "../../../models/internship.model";
import {InternshipService} from "../../../services/internship.service";

@Component({
  selector: 'app-com-applied-student-item',
  templateUrl: './com-applied-student-item.component.html',
  styleUrls: ['./com-applied-student-item.component.css']
})
export class ComAppliedStudentItemComponent implements OnInit {
  @Input() student: User;
  @Input() userInternships: UserInternships[];
  @Input() internship: Internship;
  @Output() hireStudentClicked: EventEmitter<User> = new EventEmitter();
  hireButtonDisabled: boolean = false;
  showRejectionNotification: boolean = false;
  studentIsInteresting: boolean = false;
  showSpinner: boolean = false;

  constructor(private accountService: AccountService, private internshipService: InternshipService) { }

  ngOnInit() {
    this.fetchData();
  }

  fetchData() {
    this.internshipService.getUserInternshipByInternshipIdAndStudentId(Number(this.internship.internshipId), this.student.userId).subscribe(data => {
      let userInternship = data;

      if (userInternship.interesting) {
        this.studentIsInteresting = true;
      }

      if ((userInternship.userId == this.student.userId && userInternship.hireRequested) || this.internship.completed) {
        this.hireButtonDisabled = true;

        // student denied
        if (!userInternship.hireApproved && userInternship.rejectionFeedback != null) {
          this.showRejectionNotification = true;
        }
      }
    });
  }

  showCV() {
    this.showSpinner = true;
    let studentId = this.student.userId;

    this.accountService.downloadCV(studentId).subscribe(res => {
      const data = new Blob([res], { type: 'application/pdf' });
      console.log(`CV van student met id ${studentId} succesvol opgehaald.`);

      const url= window.URL.createObjectURL(data);
      window.open(url);

      console.log(`CV van student met id ${studentId} geopend.`);
      this.showSpinner = false;
    });
  }

  hireStudent() {
    this.hireStudentClicked.emit(this.student);
  }

  toggleInteresting() {
    this.showSpinner = true;
    let internshipId: number = Number(this.internship.internshipId);
    let userId: number = this.student.userId;

    this.internshipService.toggleInterestingUserInternship(internshipId, userId).subscribe(() => {
      this.studentIsInteresting = !this.studentIsInteresting;
      this.showSpinner = false;
    });
  }

  oneOfTwoStudentsAssigned(): boolean {
    if (this.internship.totalInternsRequired == 2 && this.internship.internshipAssignedUser[0].user.role.code == 'STU') {
      return true;
    }

    return false;
  }
}
