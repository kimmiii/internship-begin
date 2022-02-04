import {Component, Input, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {InternshipService} from "../../../services/internship.service";
import {InternshipReduced} from "../../../models/internship-reduced.model";
import {Internship} from "../../../models/internship.model";
import {ProjectStatus} from "../../../models/project-status.model";
import {StateManagerService} from "../../../services/state-manager.service";
import {Router} from "@angular/router";
import {InternshipAssignedUser} from "../../../models/internship-assigned-user.model";

@Component({
  selector: 'app-coor-more-info',
  templateUrl: './coor-internship-more-info.component.html',
  styleUrls: ['./coor-internship-more-info.component.css']
})
export class CoorInternshipMoreInfoComponent implements OnInit {
  moreInfoForm: FormGroup;
  btnSendClicked: boolean = false;
  @Input() internship: Internship;
  projectStatusList: ProjectStatus[] = [];
  confirmErrorIsVisible: boolean = false;
  submitted: boolean = false;

  constructor(private formBuilder: FormBuilder, private internshipService: InternshipService,
              private stateManagerService: StateManagerService, private router: Router) { }

  ngOnInit() {
    this.createForm();
    this.fetchData();
  }

  createForm() {
    this.moreInfoForm = this.formBuilder.group({
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

    if (this.moreInfoForm.valid) {
      this.submitted = true;
      let internshipReduced: InternshipReduced = this.createInternshipReducedObject();

      this.internshipService.changeStatus(internshipReduced).subscribe(res => {
        if (res.body.status == 1) {
          console.log(`Status van internship met id ${internshipReduced.internshipId} succesvol gewijzigd.`);
          this.proceedToStart();
        } else {
          console.log(`Er is iets fout gegaan bij het wijzigen van internship met id ${internshipReduced.internshipId}.`);
          this.confirmErrorIsVisible = true;
          this.btnSendClicked = false;
          this.submitted = false;
        }
      });
    }
  }

  createInternshipReducedObject(): InternshipReduced {
    let internshipId: number = Number(this.internship.internshipId);
    let projectStatus: ProjectStatus = this.projectStatusList.filter(x => x.code == 'FEE')[0];
    let contactPersonId: number = this.stateManagerService.userId;
    let internship: InternshipReduced = new InternshipReduced(internshipId, projectStatus, contactPersonId);

    internship.externalFeedback = this.externalFeedback.value;

    return internship;
  }

  proceedToStart() {
    this.router.navigateByUrl('coordinator/internships');
  }

  // GETTERS for moreInfoForm
  get externalFeedback() {
    return this.moreInfoForm.controls.externalFeedback;
  }
}
