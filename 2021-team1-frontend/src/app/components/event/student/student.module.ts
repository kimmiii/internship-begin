import { CommonModule, DatePipe } from '@angular/common';
import { NgModule } from '@angular/core';
import { NgSelectConfig } from '@ng-select/ng-select';

import { SharedModule } from '../../../shared/shared.module';
import { EventModule } from '../event.module';
import { AppointmentsComponent } from './appointments/appointments.component';
import { CreateAppointmentModalComponent } from './create-appointment-modal/create-appointment-modal.component';
import { InternshipDetailComponent } from './internship-detail/internship-detail.component';
import { InternshipsComponent } from './internships/internships.component';
import { OverviewComponent } from './overview/overview.component';
import { StudentAppointmentComponent } from './student-appointment/student-appointment.component';
import { StudentRoutingModule } from './student-routing.module';

@NgModule({
  declarations: [
    AppointmentsComponent,
    InternshipsComponent,
    OverviewComponent,
    InternshipDetailComponent,
    StudentAppointmentComponent,
    CreateAppointmentModalComponent,
  ],
  imports: [
    SharedModule,
    CommonModule,
    StudentRoutingModule,
    EventModule,
  ],
  entryComponents: [ CreateAppointmentModalComponent ],
  providers: [ DatePipe, NgSelectConfig ],
})
export class StudentModule {
}
