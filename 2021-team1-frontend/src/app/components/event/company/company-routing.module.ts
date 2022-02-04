import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { CompanyAppointmentsComponent } from './company-appointments/company-appointments.component';
import { CompanyInternshipsComponent } from './company-internships/company-internships.component';
import { CompanySettingsComponent } from './company-settings/company-settings.component';
import { CompanyComponent } from './company.component';
import { RegisteredCompanyGuard } from './shared/registered-company.guard';

const routes: Routes = [
  {
    path: '',
    component: CompanyComponent,
    children:
      [
        {
          path: '',
          component: CompanyInternshipsComponent,
          canActivate: [ RegisteredCompanyGuard ]
        },
        {
          path: 'appointments',
          component: CompanyAppointmentsComponent
        },
        {
          path: 'settings',
          component: CompanySettingsComponent
        },
      ],
  },
];

@NgModule({
  imports: [ RouterModule.forChild(routes) ],
  exports: [ RouterModule ],
})
export class CompanyRoutingModule {
}
