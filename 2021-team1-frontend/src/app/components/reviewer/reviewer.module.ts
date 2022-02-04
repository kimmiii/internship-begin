import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { SharedModule } from '../../shared/shared.module';
import { RevInternshipDetailsComponent } from './rev-internship-details/rev-internship-details.component';
import { RevInternshipItemComponent } from './rev-internship-item/rev-internship-item.component';
import { RevInternshipReviewComponent } from './rev-internship-review/rev-internship-review.component';
import { RevInternshipsComponent } from './rev-internships/rev-internships.component';
import { ReviewerRoutingModule } from './reviewer-routing.module';
import { ReviewerComponent } from './reviewer/reviewer.component';

@NgModule({
  declarations: [
    ReviewerComponent,
    RevInternshipsComponent,
    RevInternshipItemComponent,
    RevInternshipDetailsComponent,
    RevInternshipReviewComponent,
  ],
  imports: [SharedModule, CommonModule, ReviewerRoutingModule],
})
export class ReviewerModule {}
