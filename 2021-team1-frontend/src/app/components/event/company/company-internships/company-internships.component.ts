import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { takeUntil } from 'rxjs/operators';

import { Internship } from '../../../../models/internship.model';
import { EventService } from '../../../../services/event.service';
import { BaseComponent } from '../../../../shared/components/base/base.component';
import { NotificationService } from '../../../../shared/services/notification.service';

@Component({
  selector: 'app-company-internships',
  templateUrl: './company-internships.component.html',
  styleUrls: [ './company-internships.component.scss' ],
})
export class CompanyInternshipsComponent extends BaseComponent implements OnInit {

  showAllInEvent = true;
  internships: Internship[] = [];

  constructor(
    private eventService: EventService,
    private notificationService: NotificationService,
    private title: Title
  ) {
    super();
    this.title.setTitle('Handshake Event | Stageopdrachten');
  }

  ngOnInit(): void {
    this.fetchInternshipsFromCompany();
  }

  private fetchInternshipsFromCompany(): void {
    this.eventService.getAllApprovedInternshipsOfCompany()
      .pipe(
        takeUntil(this.destroy$),
      ).subscribe((internships: Internship[]) => this.internships = internships);
  }

  toggleShowAllInEvent(): void {
    this.showAllInEvent = !this.showAllInEvent;
  }

  save(): void {
    this.eventService.saveInternships(this.internships)
      .pipe(
        takeUntil(this.destroy$),
      ).subscribe(() => {
      this.notificationService.showSuccessSnackBar('Geselecteerde stageopdrachten succesvol opgeslagen');
      this.fetchInternshipsFromCompany();
    });
  }
}
