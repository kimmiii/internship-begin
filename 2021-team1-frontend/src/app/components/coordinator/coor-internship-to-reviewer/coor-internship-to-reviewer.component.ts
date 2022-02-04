import {Component, Input, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {User} from "../../../models/user.model";
import {Internship} from "../../../models/internship.model";
import {AccountService} from "../../../services/account.service";
import {InternshipReduced} from "../../../models/internship-reduced.model";
import {ProjectStatus} from "../../../models/project-status.model";
import {InternshipService} from "../../../services/internship.service";
import {StateManagerService} from "../../../services/state-manager.service";
import {InternshipAssignedUser} from "../../../models/internship-assigned-user.model";
import {Router} from "@angular/router";
import {forkJoin} from "rxjs";

@Component({
  selector: 'app-coor-internship-to-reviewer',
  templateUrl: './coor-internship-to-reviewer.component.html',
  styleUrls: ['./coor-internship-to-reviewer.component.css']
})
export class CoorInternshipToReviewerComponent implements OnInit {
  @Input() internship: Internship;
  sendToReviewerForm: FormGroup;
  allDataIsFetched: boolean = false;
  btnSendClicked: boolean = false;
  submitted: boolean = false;
  errorIsVisible: boolean = false;
  mentorList: User[];
  projectStatusList: ProjectStatus[] = [];
  selectedReviewers: User[];

  constructor(private formBuilder: FormBuilder, private accountService: AccountService, private internshipService: InternshipService,
              private stateManagerService: StateManagerService, private router: Router) { }

  ngOnInit() {
    this.fetchData();
  }

  fetchData() {
    let getReviewersObservable = this.accountService.getReviewers();
    let getStatusObservable = this.internshipService.getStatus();

    forkJoin([getReviewersObservable, getStatusObservable]).subscribe(results => {
      this.mentorList = results[0];
      this.projectStatusList = results[1];
      console.log('Data is succesvol opgehaald.');

      this.createForm();
      this.allDataIsFetched = true;
    });
  }

  createForm() {
    this.sendToReviewerForm = this.formBuilder.group( {
      reviewers: [this.selectedReviewers, [Validators.required]],
      internalFeedback: [null, [Validators.required, Validators.maxLength(500)]]
    });
  }

  submitForm() {
    this.btnSendClicked = true;

    if (this.sendToReviewerForm.valid) {
      this.submitted = true;
      let internshipReduced: InternshipReduced = this.createInternshipReducedObject();

      this.internshipService.changeStatus(internshipReduced).subscribe(res => {
        if (res.body.status == 1) {
          console.log(`Status van internship met id ${internshipReduced.internshipId} is succesvol gewijzigd.`);
          this.proceedToStart();
        } else {
          console.log(`Er is iets fout gegaan bij het wijzigen van de status van intersnhip met id ${internshipReduced.internshipId}.`);
          this.errorIsVisible = true;
          this.submitted = false;
        }
      });
    }
  }

  createInternshipReducedObject(): InternshipReduced {
    let internshipId: number = Number(this.internship.internshipId);
    let projectStatus: ProjectStatus = this.projectStatusList.filter(x => x.code == 'REV')[0];
    let contactPersonId: number = this.stateManagerService.userId;
    let internship: InternshipReduced = new InternshipReduced(internshipId, projectStatus, contactPersonId);

    internship.internalFeedback = this.internalFeedback.value;

    this.selectedReviewers.forEach(reviewer => {
      internship.InternshipAssignedUser.push(new InternshipAssignedUser(reviewer.userId))
    });

    return internship;
  }

  proceedToStart() {
    this.router.navigateByUrl('coordinator/internships');
  }

  // GETTER for sendToReviewerForm
  get reviewers() {
    return this.sendToReviewerForm.controls.reviewers;
  }
  get internalFeedback() {
    return this.sendToReviewerForm.controls.internalFeedback;
  }
}
