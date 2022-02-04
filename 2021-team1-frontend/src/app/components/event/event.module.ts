import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { SharedModule } from '../../shared/shared.module';
import { EventRoutingModule } from './event-routing.module';
import { AppointmentDetailPanelComponent } from './shared/appointment-detail-panel/appointment-detail-panel.component';
import { AppointmentStatusComponent } from './shared/appointment-status/appointment-status.component';
import { AppointmentUtilService } from './shared/appointment-util.service';
import { ConfirmationDialogComponent } from './shared/confirmation-dialog/confirmation-dialog.component';
import { EventCardComponent } from './shared/event-card/event-card.component';
import { TimeSpanPipe } from './shared/time-span.pipe';
import { TimeTableBlockComponent } from './shared/timetable/time-table-block/time-table-block.component';
import { TimeTableComponent } from './shared/timetable/time-table.component';

@NgModule({
  declarations: [
    EventCardComponent,
    TimeTableComponent,
    ConfirmationDialogComponent,
    TimeTableBlockComponent,
    AppointmentStatusComponent,
    AppointmentDetailPanelComponent,
    TimeSpanPipe,
  ],
  imports: [
    SharedModule,
    CommonModule,
    EventRoutingModule,
  ],
  exports: [
    EventCardComponent,
    TimeTableComponent,
    AppointmentStatusComponent,
    AppointmentDetailPanelComponent,
    TimeSpanPipe,
  ],
  entryComponents: [ ConfirmationDialogComponent ],
  providers: [ AppointmentUtilService ],
})
export class EventModule {
}
