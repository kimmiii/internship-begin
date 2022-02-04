import { Component, OnInit } from '@angular/core';
import {ActivatedRoute, Router} from "@angular/router";
import {Title} from "@angular/platform-browser";
import {InternshipService} from "../../../services/internship.service";
import {Internship} from "../../../models/internship.model";
import {AccountService} from "../../../services/account.service";
import {User} from "../../../models/user.model";
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {UserInternships} from "../../../models/user-internships.model";
import {forkJoin} from "rxjs";

@Component({
  selector: 'app-coor-user-internship-details',
  templateUrl: './coor-user-internship-details.component.html',
  styleUrls: ['./coor-user-internship-details.component.css']
})
export class CoorUserInternshipDetailsComponent implements OnInit {
  internship: Internship;
  internshipId: string;
  student: User;
  studentId: string;
  allDataIsFetched: boolean = false;
  showSpinner: boolean = false;
  showModalPopup: boolean = false;
  showRejectForm: boolean = false;
  rejectForm: FormGroup;
  btnSubmitClicked: boolean = false;
  userIntership: UserInternships;

  constructor(private route: ActivatedRoute, private router: Router, private titleService: Title,
              private internshipService: InternshipService, private accountService: AccountService,
              private formBuilder: FormBuilder) { }

  ngOnInit() {
    this.showSpinner = true;

    this.internshipId = this.route.snapshot.paramMap.get('internshipId');
    this.studentId = this.route.snapshot.paramMap.get('studentId');
    this.titleService.setTitle(`Stagebeheer | Stage nr.${this.internshipId}`);

    this.fetchData();
  }

  fetchData() {
    this.internshipService.getInternshipById(this.internshipId).subscribe(res => {
      if (res.error != null) {
        window.alert(`Internship met id ${this.internshipId} kan niet gevonden worden. U wordt naar de start-pagina geleid.`);
        this.router.navigateByUrl('/coordinator/internships');
      } else {
        console.log(`Internship met id ${this.internshipId} succesvol opgehaald.`);
        this.internship = res;
        this.fetchStudentAndInternshipUser();
      }
    });
  }

  fetchStudentAndInternshipUser() {
    let studentId: number = Number(this.studentId);
    let internshipId: number = Number(this.internshipId);

    let getUserByIdObservable = this.accountService.getUserById(studentId);
    let getUserInternshipByInternshipIdAndStudentIdObservable =
      this.internshipService.getUserInternshipByInternshipIdAndStudentId(internshipId, studentId);

    forkJoin([getUserByIdObservable, getUserInternshipByInternshipIdAndStudentIdObservable]).subscribe(results => {
      this.student = results[0];
      this.userIntership = results[1];
      console.log('Data succesvol opgehaald.');

      this.allDataIsFetched = true;
      this.showSpinner = false;
      this.createForm();

      if (this.userIntership.rejectionFeedback != null) {
        this.fixLineBreakRejectionFeedback();
      }
    });
  }

  createForm() {
    this.rejectForm = this.formBuilder.group({
      feedback: [null, [Validators.required, Validators.maxLength(1000)]]
    });
  }

  fixLineBreakRejectionFeedback() {
    this.userIntership.rejectionFeedback = this.userIntership.rejectionFeedback.replace(/\n/g, "<br/>");
  }

  approveHireRequest() {
    this.showSpinner = true;
    this.internshipService.setHireApprovedToTrue(Number(this.internshipId), Number(this.studentId)).subscribe(() => {
      console.log('Hire Approved is succesvol op TRUE gezet.');

      this.closeModalPopup();
      this.router.navigate(['/coordinator/assignments', {"confirmed":true}]);
      this.showSpinner = false;
    });
  }

  openModalPopup() {
    this.showModalPopup = true;
    this.showRejectForm = false;
  }

  closeModalPopup() {
    this.showModalPopup = false;
  }

  toggleRejectForm() {
    this.showModalPopup = false;
    this.showRejectForm = !this.showRejectForm;
  }

  submitRejectForm() {
    this.showSpinner = true;
    this.btnSubmitClicked = true;
    let userInternships = new UserInternships(Number(this.studentId));
    userInternships.rejectionFeedback = this.feedback.value;

    this.internshipService.setHireApprovedToFalse(Number(this.internshipId), Number(this.studentId), userInternships).subscribe(() => {
      console.log('Hire Approved is succesvol op FALSE gezet.');

      this.router.navigateByUrl('/coordinator/assignments');
      this.showSpinner = false;
    });
  }

  // GETTERS for rejectForm
  get feedback() {
    return this.rejectForm.controls.feedback;
  }
}
