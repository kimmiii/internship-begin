import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { CoorAssignmentsComponent } from './coor-assignments/coor-assignments.component';
import { CoorCompaniesComponent } from './coor-companies/coor-companies.component';
import { CoorFinishedInternshipsComponent } from './coor-finished-internships/coor-finished-internships.component';
import { CoorInternshipDetailsComponent } from './coor-internship-details/coor-internship-details.component';
import { CoorInternshipsComponent } from './coor-internships/coor-internships.component';
import { CoorLectorsComponent } from './coor-lectors/coor-lectors.component';
import { CoorStudentsComponent } from './coor-students/coor-students.component';
import { CoorUserInternshipDetailsComponent } from './coor-user-internship-details/coor-user-internship-details.component';

const routes: Routes = [
  { path: '', redirectTo: 'internships' },
  { path: 'internships', component: CoorInternshipsComponent },
  {
    path: 'finished-internships',
    component: CoorFinishedInternshipsComponent,
  },
  {
    path: 'internship-details/:internshipId',
    component: CoorInternshipDetailsComponent,
  },
  {
    path: 'user-internship-details/:internshipId/:studentId',
    component: CoorUserInternshipDetailsComponent,
  },
  { path: 'assignments', component: CoorAssignmentsComponent },
  { path: 'companies', component: CoorCompaniesComponent },
  { path: 'lectors', component: CoorLectorsComponent },
  { path: 'students', component: CoorStudentsComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class CoordinatorRoutingModule {}
