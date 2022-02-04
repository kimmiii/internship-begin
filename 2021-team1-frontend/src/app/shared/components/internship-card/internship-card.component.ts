import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';

import { Internship } from '../../../models/internship.model';

@Component({
  selector: 'app-internship-card',
  templateUrl: './internship-card.component.html',
  styleUrls: [ './internship-card.component.scss' ],
})
export class InternshipCardComponent implements OnChanges {

  @Input() internship: Internship;
  @Input() show: boolean;

  ngOnChanges(simpleChanges: SimpleChanges): void {
    if (!simpleChanges.show.firstChange) {
      this.internship.showInEvent = this.show;
    }
  }
}
