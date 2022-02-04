import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { SharedModule } from '../../../shared/shared.module';
import { EventModule } from '../event.module';
import { ChangeEventStatusModalComponent } from './change-event-status-modal/change-event-status-modal.component';
import { CoordinatorRoutingModule } from './coordinator-routing.module';
import { CreateEventModalComponent } from './create-event-modal/create-event-modal.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { ManageEventComponent } from './manage-event/manage-event.component';
import { AppointmentsToStatusColorPipe } from './shared/appointments-to-status-color.pipe';
import { FilterPipe } from './shared/filter.pipe';
import { SortPipe } from './shared/sort.pipe';
import { StudentReportComponent } from './student-report/student-report.component';

@NgModule({
  declarations: [
    DashboardComponent,
    ManageEventComponent,
    ChangeEventStatusModalComponent,
    CreateEventModalComponent,
    StudentReportComponent,
    AppointmentsToStatusColorPipe,
    FilterPipe,
    SortPipe,
  ],
  imports: [
    SharedModule,
    CommonModule,
    CoordinatorRoutingModule,
    EventModule,
  ],
  entryComponents: [ ChangeEventStatusModalComponent, CreateEventModalComponent ],
  exports: [
    AppointmentsToStatusColorPipe,
  ]
})
export class CoordinatorModule {
}
