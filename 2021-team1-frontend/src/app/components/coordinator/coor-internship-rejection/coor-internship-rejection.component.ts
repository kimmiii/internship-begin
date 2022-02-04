import {Component, Input, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {Router} from "@angular/router";
import {ProjectStatus} from "../../../models/project-status.model";
import {InternshipService} from "../../../services/internship.service";
import {Internship} from "../../../models/internship.model";
import {InternshipReduced} from "../../../models/internship-reduced.model";
import {StateManagerService} from "../../../services/state-manager.service";

@Component({
  selector: 'app-coor-rejection',
  templateUrl: './coor-internship-rejection.component.html',
  styleUrls: ['./coor-internship-rejection.component.css']
})
export class CoorInternshipRejectionComponent implements OnInit {
  @Input() internship: Internship;
  rejectionForm: FormGroup;
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
    this.rejectionForm = this.formBuilder.group({
      externalFeedback: [null, [Validators.required, Validators.maxLength(500)]]
    });
  }

  fetchData() {
    this.internshipService.getStatus().subscribe(data => {
      this.projectStatusList = data;
      console.log('Projectstatus succesvol opgehaald.');
    });
  }

  submitRejectedForm() {
    this.btnSendClicked = true;

    if (this.rejectionForm.valid) {
      this.submitted = true;
      let internshipReduced: InternshipReduced = this.createInternshipReducedObject();

      this.internshipService.changeStatus(internshipReduced).subscribe(res => {
        if (res.body.status == 1) {
          console.log(`Status van internship met id ${internshipReduced.internshipId} succesvol gewijzigd.`);
          this.proceedToFinishedInternships();
        } else {
          console.log(`Er is iets fout gegaan bij het wijzigen van internship met id ${internshipReduced.internshipId}.`);
          this.rejectionErrorIsVisible = true;
          this.submitted = false;
        }
      });
    }
  }

  createInternshipReducedObject(): InternshipReduced {
    let internshipId: number = Number(this.internship.internshipId);
    let projectStatus: ProjectStatus = this.projectStatusList.filter(x => x.code == 'REJ')[0];
    let contactPersonId: number = this.stateManagerService.userId;
    let internship: InternshipReduced = new InternshipReduced(internshipId, projectStatus, contactPersonId);

    internship.externalFeedback = this.externalFeedback.value;

    return internship;
  }

  proceedToFinishedInternships() {
    this.router.navigateByUrl('coordinator/finished-internships');
  }

  // GETTERS for rejectionForm
  get externalFeedback() {
    return this.rejectionForm.controls.externalFeedback;
  }
}
