import { Component } from '@angular/core';
import { takeUntil } from 'rxjs/operators';

import { EventCompany } from '../../../../models';
import { EventService } from '../../../../services/event.service';
import { BaseComponent } from '../../../../shared/components/base/base.component';
import { RegisteredCompanyService } from '../shared/registered-company.service';

@Component({
  selector: 'app-company-menu-tabs',
  templateUrl: './company-menu-tabs.component.html',
  styleUrls: [ './company-menu-tabs.component.css' ],
})
export class CompanyMenuTabsComponent extends BaseComponent {

  tabRoutes: TabRoute[] = [
    {
      route: '/event/company',
      text: 'STAGEOPDRACHTEN',
      registrationRequired: true,
    },
    {
      route: '/event/company/appointments',
      text: 'AGENDA',
      registrationRequired: true,
    },
    {
      route: '/event/company/settings',
      text: 'INSTELLINGEN',
      registrationRequired: false,
    },
  ];

  constructor(
    public registeredCompanyService: RegisteredCompanyService,
    private eventService: EventService,
  ) {
    super();
    this.checkCompanyRegistration();
  }

  private checkCompanyRegistration() {
    this.eventService.getCompanyRegistration()
      .pipe(
        takeUntil(this.destroy$)
      ).subscribe((eventCompany: EventCompany) => this.registeredCompanyService.isCompanyRegistered.next(!!eventCompany));
  }
}

export interface TabRoute {
  route: string;
  text: string;
  registrationRequired: boolean;
}
