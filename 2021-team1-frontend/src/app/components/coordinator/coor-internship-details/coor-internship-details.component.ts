import {Component, OnInit} from '@angular/core';
import {Internship} from "../../../models/internship.model";
import {ActivatedRoute, Router} from "@angular/router";
import {InternshipService} from "../../../services/internship.service";
import {saveAs} from 'file-saver';
import {Title} from "@angular/platform-browser";
import {ProjectStatus} from "../../../models/project-status.model";
import {Contact} from "../../../models/contact.model";
import {ContactService} from "../../../services/contact.service";
import {InternshipReduced} from "../../../models/internship-reduced.model";
import {StateManagerService} from "../../../services/state-manager.service";
import {InternshipAssignedUser} from "../../../models/internship-assigned-user.model";

@Component({
  selector: 'app-coor-internship-details',
  templateUrl: './coor-internship-details.component.html',
  styleUrls: ['./coor-internship-details.component.css']
})
export class CoorInternshipDetailsComponent implements OnInit {
  isAllDataFetched: boolean = false;
  internshipId: string;
  internship: Internship;
  moreInfoVisible: boolean = false;
  rejectionVisible: boolean = false;
  buttonsAreEnabled: boolean = true;
  modalPopupIsVisible: boolean = false;
  sendToReviewerIsVisible: boolean = false;
  projectStatusList: ProjectStatus[] = [];
  confirmErrorIsVisible: boolean = false;
  submitted: boolean = false;
  contact: Contact;
  assignToMePopupIsVisible: boolean = false;
  assignToMeSubmitted: boolean = false;
  showSpinner: boolean = true;

  constructor(private route: ActivatedRoute, private internshipService: InternshipService, private titleService: Title,
              private router: Router, private contactService: ContactService, private stateManagerService: StateManagerService) { }

  ngOnInit() {
    this.internshipId = this.route.snapshot.paramMap.get('internshipId');
    this.fetchData();
    this.titleService.setTitle(`Stagebeheer | Stage nr.${this.internshipId}`);
  }

  fetchData() {
    this.internshipService.getInternshipById(this.internshipId).subscribe(res =>  {
      if (res.error != null) {
        window.alert(`Internship met id ${this.internshipId} kan niet gevonden worden. U wordt naar de start-pagina geleid.`);
        this.router.navigateByUrl('/coordinator/internships');
      } else {
        console.log(`Internship met id ${this.internshipId} succesvol opgehaald.`);
        this.internship = res;

        this.fetchStatus();
      }
    });
  }

  fetchStatus() {
    this.internshipService.getStatus().subscribe(data => {
      this.projectStatusList = data;
      this.isAllDataFetched = true;
      this.showSpinner = false;
    });
  }

  createPDF() {
    this.showSpinner = true;
    this.internshipService.createPDFByInternshipId(this.internshipId).subscribe(res => {
      if (res.error != null) {
        window.alert(`Het is niet mogelijk een PDF-bestand van deze stageaanvraag te genereren.`);
        this.showSpinner = false;
      } else {
        let blob = new Blob([res], {type: 'application/pdf'});
        saveAs(blob, 'Stageaanvraag.pdf');
        this.showSpinner = false;
      }
    });
    this.showSpinner = false;
  }

  // When will buttons be disabled?
  disableButton(): boolean {
    let roleCode: string;
    if (this.internship.internshipAssignedUser.length > 0) {
      roleCode = this.internship.internshipAssignedUser[0].user.role.code;
    }

    return !this.buttonsAreEnabled || this.internship.projectStatus.code == 'APP' ||
      this.internship.projectStatus.code == 'REJ' || (this.internship.projectStatus.code == 'REV' && roleCode == 'REV') ||
    (this.internship.projectStatus.code == 'FEE');
  }

  approve() {
    this.modalPopupIsVisible = true;

    this.moreInfoVisible = false;
    this.rejectionVisible = false;
    this.sendToReviewerIsVisible = false;
    this.buttonsAreEnabled = false;
  }

  toggleMoreInfo() {
    this.moreInfoVisible = true;

    this.rejectionVisible = false;
    this.modalPopupIsVisible = false;
    this.sendToReviewerIsVisible = false;
  }

  toggleSendToReviewer() {
    this.sendToReviewerIsVisible = true;

    this.moreInfoVisible = false;
    this.rejectionVisible = false;
    this.modalPopupIsVisible = false;
  }

  toggleRejection() {
    this.rejectionVisible = true;

    this.moreInfoVisible = false;
    this.modalPopupIsVisible = false;
    this.sendToReviewerIsVisible = false;
  }

  confirmConfirmation() {
    this.showSpinner = true;
    this.submitted = true;
    let internshipReduced: InternshipReduced = this.createInternshipReducedObject();

    this.internshipService.changeStatus(internshipReduced).subscribe(res => {
      if (res.body.status == 1) {
        console.log(`Status van internship met id ${internshipReduced.internshipId} succes gewijzigd.`);

        this.proceedToFinishedInternships();
        this.showSpinner = false;
      } else {
        console.log(`Er is iets fout gegaan bij het wijzigen van internship met id ${internshipReduced.internshipId}`);

        this.confirmErrorIsVisible = true;
        this.submitted = false;
        this.showSpinner = false;
      }
    });
  }

  createInternshipReducedObject(): InternshipReduced {
    let internshipId: number = Number(this.internship.internshipId);
    let projectStatus: ProjectStatus = this.projectStatusList.filter(x => x.code == 'APP')[0];
    let contactPersonId: number = this.stateManagerService.userId;

    return new InternshipReduced(internshipId, projectStatus, contactPersonId);
  }

  cancelConfirmation() {
    this.modalPopupIsVisible = false;
    this.assignToMePopupIsVisible = false;
    this.buttonsAreEnabled = true;
  }

  proceedToFinishedInternships() {
    this.router.navigateByUrl('coordinator/finished-internships');
  }

  showAssignToMePopup() {
    this.assignToMePopupIsVisible = true;
  }

  assignToMe() {
    this.showSpinner = true;
    this.assignToMeSubmitted = true;
    this.assignToMePopupIsVisible = false;

    let reducedInternship: InternshipReduced = this.createAssignToMeReducedInternship();
    console.log(reducedInternship);

    this.internshipService.changeStatus(reducedInternship).subscribe(res => {
      if (res.error != null) {
        console.log(`Er is iets fout gegaan bij het wijzigen van internship met id ${reducedInternship.internshipId}`);

        this.confirmErrorIsVisible = true;
        this.isAllDataFetched = true;
        this.assignToMeSubmitted = false;
        this.showSpinner = false;
      } else {
        if (res.body.status == 1) {
          console.log(`Status van internship met id ${reducedInternship.internshipId} succes gewijzigd.`);

          this.isAllDataFetched = false;
          this.fetchData();
          this.assignToMeSubmitted = false;
        } else {
          console.log(`Er is iets fout gegaan bij het wijzigen van internship met id ${reducedInternship.internshipId}`);

          this.confirmErrorIsVisible = true;
          this.submitted = false;
          this.isAllDataFetched = true;
          this.assignToMeSubmitted = false;
          this.showSpinner = false;
        }
      }
    });
  }

  createAssignToMeReducedInternship(): InternshipReduced {
    let internshipId: number = Number(this.internship.internshipId);
    let projectStatus: ProjectStatus = this.projectStatusList.filter(x => x.code == 'REV')[0];
    let contactPersonId: number = this.stateManagerService.userId;
    let internshipAssignedUser: InternshipAssignedUser = new InternshipAssignedUser(this.stateManagerService.userId);
    let assignedUsers: InternshipAssignedUser[] = [];
    assignedUsers.push(internshipAssignedUser);

    let internshipReducedObject =  new InternshipReduced(internshipId, projectStatus, contactPersonId);
    internshipReducedObject.InternshipAssignedUser = assignedUsers;

    return internshipReducedObject;
  }
}
