import {Component, Input, OnInit} from '@angular/core';
import {Internship} from "../../../models/internship.model";
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {ProjectStatus} from "../../../models/project-status.model";
import {Router} from "@angular/router";
import {InternshipService} from "../../../services/internship.service";
import {StateManagerService} from "../../../services/state-manager.service";
import {InternshipReduced} from "../../../models/internship-reduced.model";

@Component({
  selector: 'app-rev-internship-review',
  templateUrl: './rev-internship-review.component.html',
  styleUrls: ['./rev-internship-review.component.css']
})
export class RevInternshipReviewComponent implements OnInit {
  @Input() internship: Internship;
  reviewForm: FormGroup;
  btnSendClicked: boolean = false;
  projectStatusList: ProjectStatus[] = [];
  rejectionErrorIsVisible: boolean = false;
  submitted: boolean = false;

  constructor(private formBuilder: FormBuilder, private router: Router, private internshipService: InternshipService,
              private stateManagerService: StateManagerService) { }

  ngOnInit() {
    this.fetchData();
    this.createForm();
  }

  createForm() {
    this.reviewForm = this.formBuilder.group({
      internalFeedback: [null, [Validators.required, Validators.maxLength(500)]]
    });
  }

  fetchData() {
    this.internshipService.getStatus().subscribe(data => {
      this.projectStatusList = data;
      console.log('Statussen succesvol opgehaald.');
    });
  }

  submitReviewForm() {
    this.btnSendClicked = true;

    if (this.reviewForm.valid) {
      this.submitted = true;
      let internshipReduced: InternshipReduced = this.createInternshipReducedObject();

      this.internshipService.changeStatus(internshipReduced).subscribe(res => {
        if (res.body.status == 1) {
          console.log(`Status van internship met id ${internshipReduced.internshipId} succesvol gewijzigd.`);
          this.proceedToStart();
        } else {
          this.rejectionErrorIsVisible = true;
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

    return internship;
  }

  proceedToStart() {
    this.router.navigateByUrl('reviewer/internships');
  }

  // GETTER reviewForm
  get internalFeedback() {
    return this.reviewForm.controls.internalFeedback;
  }
}
