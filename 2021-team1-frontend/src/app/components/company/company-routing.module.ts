import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { FormGuard } from '../../guards/form.guard';
import { ComApplicationFormComponent } from './com-application-form/com-application-form.component';
import { ComInternshipDetailsComponent } from './com-internship-details/com-internship-details.component';
import { ComInternshipEditComponent } from './com-internship-edit/com-internship-edit.component';
import { ComInternshipsComponent } from './com-internships/com-internships.component';
import { ComProfileComponent } from './com-profile/com-profile.component';

const routes: Routes = [
  { path: '', redirectTo: 'internships' },
  { path: 'internships', component: ComInternshipsComponent },
  {
    path: 'application-form',
    component: ComApplicationFormComponent,
    canDeactivate: [FormGuard],
  },
  {
    path: 'internship-details/:internshipId',
    component: ComInternshipDetailsComponent,
  },
  {
    path: 'internship-edit/:internshipId',
    component: ComInternshipEditComponent,
    canDeactivate: [FormGuard],
  },
  { path: 'profile', component: ComProfileComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class CompanyRoutingModule {}
