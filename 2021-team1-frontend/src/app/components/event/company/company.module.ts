import { CommonModule, DatePipe } from '@angular/common';
import { NgModule } from '@angular/core';

import { InternshipCardComponent } from '../../../shared/components/internship-card/internship-card.component';
import { SharedModule } from '../../../shared/shared.module';
import { EventModule } from '../event.module';
import { CancelAppointmentModalComponent } from './cancel-appointment-modal/cancel-appointment-modal.component';
import { CompanyAppointmentsComponent } from './company-appointments/company-appointments.component';
import { CompanyInternshipsComponent } from './company-internships/company-internships.component';
import { CompanyMenuTabsComponent } from './company-menu-tabs/company-menu-tabs.component';
import { CompanyRoutingModule } from './company-routing.module';
import { CompanySettingsComponent } from './company-settings/company-settings.component';
import { CompanyComponent } from './company.component';

@NgModule({
  declarations: [
    CompanyMenuTabsComponent,
    CompanySettingsComponent,
    CompanyComponent,
    CompanyInternshipsComponent,
    InternshipCardComponent,
    CompanyAppointmentsComponent,
    CancelAppointmentModalComponent,
  ],
  imports: [
    SharedModule,
    CommonModule,
    CompanyRoutingModule,
    EventModule,
  ],
  providers: [
    DatePipe,
  ],
  entryComponents: [
    CancelAppointmentModalComponent
  ]
})
export class CompanyModule {
}
