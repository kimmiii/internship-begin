import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { Router } from '@angular/router';
import { takeUntil } from 'rxjs/operators';

import { Company } from '../../../../models/company.model';
import { Technology } from '../../../../models/internship-technology.model';
import { Internship } from '../../../../models/internship.model';
import { Location } from '../../../../models/location.model';
import { Specialisation } from '../../../../models/specialisation.model';
import { AppointmentService } from '../../../../services/appointment.service';
import { CompanyService } from '../../../../services/company.service';
import { EventService } from '../../../../services/event.service';
import { InternshipService } from '../../../../services/internship.service';
import { BaseComponent } from '../../../../shared/components/base/base.component';

@Component({
  selector: 'app-internships',
  templateUrl: './internships.component.html',
  styleUrls: [ './internships.component.scss' ],
})
export class InternshipsComponent extends BaseComponent implements OnInit {
  internships: Internship[];
  companies: Company[];
  specialisations: Specialisation[];
  locations: Location[];
  technologies: Technology[];
  selectedCompanies: number[] = [];
  selectedSpecialisations: number[] = [];
  selectedLocations: string[] = [];
  selectedTechnologies: number[] = [];

  constructor(
    private appointmentService: AppointmentService,
    private internshipService: InternshipService,
    private companyService: CompanyService,
    private eventService: EventService,
    private router: Router,
    private title: Title,
  ) {
    super();
    this.title.setTitle('Handshake Event | Stageopdrachten');
  }

  ngOnInit(): void {
    this.fetchInternships();
    this.fetchCompanies();
    this.fetchSpecialisations();
    this.fetchLocations();
    this.fetchTechnologies();
  }

  private fetchInternships(): void {
    this.internshipService
      .getEventInternships()
      .pipe(takeUntil(this.destroy$))
      .subscribe((internships: Internship[]) => {
        this.internships = internships;
      });
  }

  private fetchCompanies(): void {
    this.companyService
      .getAllEventCompanies()
      .pipe(takeUntil(this.destroy$))
      .subscribe((companies: Company[]) => {
        this.companies = companies;
      });
  }

  private fetchSpecialisations(): void {
    this.eventService
      .getEventSpecialisations()
      .pipe(takeUntil(this.destroy$))
      .subscribe((specialisations: Specialisation[]) => {
        this.specialisations = specialisations;
      });
  }

  private fetchLocations(): void {
    this.companyService
      .getEventLocations()
      .pipe(takeUntil(this.destroy$))
      .subscribe((locations: Location[]) => {
        this.locations = locations;
      });
  }

  private fetchTechnologies(): void {
    this.internshipService
      .getEventTechnologies()
      .pipe(takeUntil(this.destroy$))
      .subscribe((technologies: Technology[]) => {
        this.technologies = technologies;
      });
  }

  public filterInternships(): void {
    if (this.selectedLocations.length === 0
      && this.selectedCompanies.length === 0
      && this.selectedSpecialisations.length === 0
      && this.selectedTechnologies.length === 0) {
      this.fetchInternships();
    } else {
      this.internshipService
        .filterEventInternships(this.selectedLocations, this.selectedCompanies, this.selectedSpecialisations, this.selectedTechnologies)
        .pipe(takeUntil(this.destroy$))
        .subscribe((internships: Internship[]) => {
          this.internships = internships;
        });
    }
  }

  public routeToInternshipDetailPage(internshipId: number): void {
    this.router.navigate([ '/event/student/internship/', internshipId ]);
  }
}
