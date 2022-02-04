import { Component, OnInit } from '@angular/core';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { ActivatedRoute, Router } from '@angular/router';
import { takeUntil } from 'rxjs/operators';
import { CompanyService } from 'src/app/services/company.service';
import { BaseComponent } from 'src/app/shared/components/base/base.component';

import { Internship } from '../../../../models/internship.model';
import { InternshipService } from '../../../../services/internship.service';

@Component({
  selector: 'app-internship-detail',
  templateUrl: './internship-detail.component.html',
  styleUrls: [ './internship-detail.component.scss' ],
})

export class InternshipDetailComponent extends BaseComponent implements OnInit {
  internshipId = this.route.snapshot.params['id'];
  internship: Internship;
  otherCompanyInternships: Internship[] = new Array<Internship>();
  logoSource: SafeUrl;

  constructor(
    private route: ActivatedRoute,
    private internshipService: InternshipService,
    private companyService: CompanyService,
    private domSanitizer: DomSanitizer,
    private router: Router,
  ) {
    super();
  }

  ngOnInit(): void {
    this.router.routeReuseStrategy.shouldReuseRoute = () => false;
    this.fetchInternship();
  }

  private fetchInternship(): void {
    this.internshipService
      .getEventInternshipById(this.internshipId)
      .pipe(takeUntil(this.destroy$))
      .subscribe((internship: Internship) => {
        this.internship = internship;
        this.fetchCompanyLogo(internship.companyId);
        this.fetchOtherInternshipsFromCompany(internship.companyId);
      });
  }

  private fetchOtherInternshipsFromCompany(companyId: number): void {
    this.internshipService
      .getEventInternshipsByCompanyId(companyId)
      .pipe(takeUntil(this.destroy$))
      .subscribe((internships: Internship[]) => {
        this.otherCompanyInternships = internships
          .filter((internship: Internship) => Number(internship.internshipId) !== Number(this.internshipId));
      });
  }

  private fetchCompanyLogo(companyId: number): void {
    this.companyService
      .downloadLogoFromCompany(companyId)
      .pipe(
        takeUntil(this.destroy$),
      )
      .subscribe((logo: Blob) => {
        if (logo) {
          const objectURL = URL.createObjectURL(logo);
          this.logoSource = this.domSanitizer.bypassSecurityTrustUrl(objectURL);
        }
      });
  }

  public OnClickWebsiteButton(website: string): void {
    window.location.href = 'http://' + website;
  }

  public OnClickEmailButton(contactPersonName: string, emailAddress: string): void {
    window.open('mailto:' + contactPersonName + '<' + emailAddress + '>');
  }

  public OnClickAfspraakButton(): void {
    this.router.navigate([ '/event/student/appointments' ], {
      queryParams:
        {
          companyId: this.internship.companyId,
          internshipId: this.internship.internshipId,
        },
    });
  }

  public routeToInternshipDetailPage(internshipId: number): void {
    this.router.navigate([ '/event/student/internship/', internshipId ]);
  }
}
