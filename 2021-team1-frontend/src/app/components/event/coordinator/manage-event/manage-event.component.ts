import { Component, OnInit } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material';
import { Title } from '@angular/platform-browser';
import { filter, map, switchMap, takeUntil } from 'rxjs/operators';

import { Event } from '../../../../models';
import { EventService } from '../../../../services/event.service';
import { BaseComponent } from '../../../../shared/components/base/base.component';
import { ChangeEventStatusModalComponent } from '../change-event-status-modal/change-event-status-modal.component';
import { CreateEventModalComponent } from '../create-event-modal/create-event-modal.component';

@Component({
  selector: 'app-manage-event',
  templateUrl: './manage-event.component.html',
  styleUrls: [ './manage-event.component.scss' ],
})
export class ManageEventComponent extends BaseComponent implements OnInit {
  events: Event[] = [];

  constructor(
    private eventService: EventService,
    public dialog: MatDialog,
    private title: Title,
  ) {
    super();
    this.title.setTitle('Handshake Event | Beheer');
  }

  ngOnInit(): void {
    this.fetchEvents();
  }

  private fetchEvents(): void {
    this.eventService
      .getEvents()
      .pipe(
        map((events: Event[]) =>
          events.map((event: Event) => {
            return {
              ...event,
              dateEvent: new Date(event.dateEvent),
            };
          }),
        ),
        takeUntil(this.destroy$),
      )
      .subscribe((events: Event[]) => (this.events = events));
  }

  changeStatusEvent(event: Event): void {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.width = '500px';
    dialogConfig.data = event;

    const dialogRef = this.dialog.open(
      ChangeEventStatusModalComponent,
      dialogConfig,
    );

    dialogRef
      .afterClosed()
      .pipe(
        filter((changeStatus: boolean) => changeStatus),
        switchMap(() => this.eventService.updateEventStatus({
          ...event,
          isActivated: !event.isActivated,
        })),
        takeUntil(this.destroy$),
      )
      .subscribe(() => this.fetchEvents());
  }

  createEvent(): void {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.width = '750px';
    dialogConfig.data = this.events.map((event: Event) => event.academicYearId);

    const dialogRef = this.dialog.open(CreateEventModalComponent, dialogConfig);

    dialogRef
      .afterClosed()
      .pipe(
        filter((event: Event) => !!event),
        switchMap((event: Event) => this.eventService.addEvent(event)),
        takeUntil(this.destroy$),
      )
      .subscribe(() => this.fetchEvents());
  }

  hasActiveEvents(): boolean {
    return this.events
      .filter((event: Event) => event.isActivated)
      .length > 0;
  }
}
