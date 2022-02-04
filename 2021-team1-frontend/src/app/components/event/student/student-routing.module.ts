import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { InternshipDetailComponent } from './internship-detail/internship-detail.component';
import { OverviewComponent } from './overview/overview.component';
import { StudentAppointmentComponent } from './student-appointment/student-appointment.component';


const routes: Routes = [
  {
    path: '',
    component: OverviewComponent,
  },
  {
    path: 'internship/:id',
    component: InternshipDetailComponent,
  },
  {
    path: 'appointments',
    component: StudentAppointmentComponent,
  },
];

@NgModule({
  imports: [ RouterModule.forChild(routes) ],
  exports: [ RouterModule ],
})
export class StudentRoutingModule {
}
