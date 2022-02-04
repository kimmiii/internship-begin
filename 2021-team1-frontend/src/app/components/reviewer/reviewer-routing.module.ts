import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { RevInternshipDetailsComponent } from './rev-internship-details/rev-internship-details.component';
import { RevInternshipsComponent } from './rev-internships/rev-internships.component';

const routes: Routes = [
  { path: '', redirectTo: 'internships' },
  { path: 'internships', component: RevInternshipsComponent },
  {
    path: 'internship-details/:internshipId',
    component: RevInternshipDetailsComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ReviewerRoutingModule {}
