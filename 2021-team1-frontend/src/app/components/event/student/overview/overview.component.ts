import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';

import { Event } from '../../../../models';
import { EventService } from '../../../../services/event.service';

@Component({
  selector: 'app-overview',
  templateUrl: './overview.component.html',
  styleUrls: [ './overview.component.scss' ],
})
export class OverviewComponent implements OnInit {

  activeEvent$: Observable<Event>;

  constructor(
    private eventService: EventService,
  ) {
  }

  ngOnInit(): void {
    this.fetchActiveEvent();
  }

  private fetchActiveEvent(): void {
    this.activeEvent$ = this.eventService.getActiveEvent();
  }
}
