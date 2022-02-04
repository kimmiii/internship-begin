import { Component, EventEmitter, Input, Output } from '@angular/core';

import { Event, EventLocation } from '../../../../models';

@Component({
  selector: 'app-event-card',
  templateUrl: './event-card.component.html',
  styleUrls: ['./event-card.component.scss']
})
export class EventCardComponent {

  @Input() event: Event;
  @Input() hasActiveEvents?: boolean;
  @Input() showStatusChangeActions?: boolean = false;

  @Output() changeStatus: EventEmitter<Event> = new EventEmitter<Event>();

  EventLocation = EventLocation

  changeStatusEvent(event: Event): void {
    this.changeStatus.emit(event);
  }
}
