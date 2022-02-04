import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { StuInternshipDetailsComponent } from './stu-internship-details/stu-internship-details.component';
import { StuInternshipsComponent } from './stu-internships/stu-internships.component';
import { StuMyInternshipsComponent } from './stu-my-internships/stu-my-internships.component';
import { StuProfileComponent } from './stu-profile/stu-profile.component';

const routes: Routes = [
  { path: '', redirectTo: 'internships' },
  { path: 'internships', component: StuInternshipsComponent },
  { path: 'myinternships', component: StuMyInternshipsComponent },
  {
    path: 'internship-details/:internshipId',
    component: StuInternshipDetailsComponent,
  },
  { path: 'profile', component: StuProfileComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class StudentRoutingModule {}
