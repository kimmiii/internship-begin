import { Component, OnInit } from '@angular/core';
import {ActivatedRoute, Router} from "@angular/router";
import {Internship} from "../../../models/internship.model";
import {InternshipService} from "../../../services/internship.service";
import { saveAs } from 'file-saver';
import {Title} from "@angular/platform-browser";

@Component({
  selector: 'app-internship-details',
  templateUrl: './com-internship-details.component.html',
  styleUrls: ['./com-internship-details.component.css']
})
export class ComInternshipDetailsComponent implements OnInit {
  isAllDataFetched: boolean = false;
  internship: Internship;

  constructor(private route: ActivatedRoute, private router: Router, private internshipService: InternshipService,
              private titleService: Title) { }

  ngOnInit() {
    let internshipId: string = this.route.snapshot.paramMap.get('internshipId');
    this.titleService.setTitle(`Stagebeheer | Stage nr.${internshipId}`);

    this.fetchData(internshipId);
  }

  fetchData(internshipId: string) {
    this.internshipService.getInternshipById(internshipId).subscribe( res => {
      if (res.error != null) {
        window.alert(`Er is iets fout gegaan bij het laden van internship met id ${internshipId}. U wordt naar de start-pagina omgeleid.`);
        this.router.navigateByUrl('/company/internships');
      } else {
        this.internship = res;
        console.log(`Internship is succesvol opgehaald: `, this.internship);

        this.isAllDataFetched = true;
      }
    });
  }

  createPDF() {
    this.internshipService.createPDFByInternshipId(this.internship.internshipId).subscribe(res => {
      if (res.error != null) {
        window.alert(`Het is niet mogelijk een PDF-bestand van deze stageaanvraag te genereren.`);
      } else {
        let blob = new Blob([res], {type: 'application/pdf'});
        saveAs(blob, 'Stageaanvraag.pdf');
      }
    });
  }

  proceedToEditScreen() {
    this.router.navigate(['/company/internship-edit', this.internship.internshipId]);
  }

  // When are buttons disabled? ...
  disableButton(): boolean {
    return this.internship.projectStatus.code == 'APP' || this.internship.projectStatus.code == 'REJ' || this.internship.projectStatus.code == 'REV' ||
      (this.internship.projectStatus.code == 'NEW' && this.internship.externalFeedback != null);
  }
}
