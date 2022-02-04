import {Component, OnInit} from '@angular/core';
import {Internship} from "../../../models/internship.model";
import {ProjectStatus} from "../../../models/project-status.model";
import {ActivatedRoute, Router} from "@angular/router";
import {InternshipService} from "../../../services/internship.service";
import {Title} from "@angular/platform-browser";
import {ContactService} from "../../../services/contact.service";
import {StateManagerService} from "../../../services/state-manager.service";
import {saveAs} from 'file-saver';

@Component({
  selector: 'app-rev-internship-details',
  templateUrl: './rev-internship-details.component.html',
  styleUrls: ['./rev-internship-details.component.css']
})
export class RevInternshipDetailsComponent implements OnInit {
  isAllDataFetched: boolean = false;
  internshipId: string;
  internship: Internship;
  projectStatusList: ProjectStatus[] = [];
  confirmErrorIsVisible: boolean = false;
  submitted: boolean = false;
  reviewComponentVisible: boolean = false;
  assignedToCurrentUser: boolean = false;

  constructor(private route: ActivatedRoute, private internshipService: InternshipService, private titleService: Title,
              private router: Router, private contactService: ContactService, private stateManagerService: StateManagerService) { }

  ngOnInit() {
    this.internshipId = this.route.snapshot.paramMap.get('internshipId');
    this.internshipService.getInternshipById(this.internshipId).subscribe(res =>  {
      if (res.error != null) {
        window.alert(`Het is niet mogelijk om de stageaanvraag met id ${this.internshipId} te laden. U wordt naar de start-pagina geleid.`);
        this.router.navigateByUrl('/reviewer/internships');
      } else {
        console.log(`Internship met id ${this.internshipId} succesvol opgehaald.`);
        this.internship = res;
        this.determineCurrentUser();
        this.fetchData();
      }
    });

    this.titleService.setTitle(`Stagebeheer | Stage nr.${this.internshipId}`);
  }

  determineCurrentUser() {
    for(let user of this.internship.internshipAssignedUser) {
      if (user.userId == this.stateManagerService.userId) {
        this.assignedToCurrentUser = true;
      }
    }
  }

  fetchData() {
    this.internshipService.getStatus().subscribe(data => {
      console.log(`Statussen succesvol opgehaald.`);
      this.projectStatusList = data;
      this.isAllDataFetched = true;
    });
  }

  createPDF() {
    this.internshipService.createPDFByInternshipId(this.internshipId).subscribe(res => {
      if (res.error != null) {
        window.alert(`Het is niet mogelijk een PDF-bestand van deze stageaanvraag te genereren.`);
      } else {
        let blob = new Blob([res], {type: 'application/pdf'});
        saveAs(blob, 'Stageaanvraag.pdf');
      }
    });
  }

  disableButton(): boolean {
     return this.internship.projectStatus.code == 'APP' || this.internship.projectStatus.code == 'REJ' ||
             this.internship.projectStatus.code == 'FEE' || !this.assignedToCurrentUser;
  }

  toggleSendToCoordinator() {
    this.reviewComponentVisible = true;
  }
}
