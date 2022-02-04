import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { SharedModule } from '../../shared/shared.module';
import { CoorAssignmentsComponent } from './coor-assignments/coor-assignments.component';
import { CoorCompaniesComponent } from './coor-companies/coor-companies.component';
import { CoorCompanyItemActiveComponent } from './coor-company-item-active/coor-company-item-active.component';
import { CoorCompanyItemInactiveEvaluatedComponent } from './coor-company-item-inactive-evaluated/coor-company-item-inactive-evaluated.component';
import { CoorCompanyItemInactiveComponent } from './coor-company-item-inactive/coor-company-item-inactive.component';
import { CoorCompanyRejectComponent } from './coor-company-reject/coor-company-reject.component';
import { CoorFinInternshipItemComponent } from './coor-fin-internship-item/coor-fin-internship-item.component';
import { CoorFinishedInternshipsComponent } from './coor-finished-internships/coor-finished-internships.component';
import { CoorInternshipDetailsComponent } from './coor-internship-details/coor-internship-details.component';
import { CoorInternshipItemComponent } from './coor-internship-item/coor-internship-item.component';
import { CoorInternshipMoreInfoComponent } from './coor-internship-more-info/coor-internship-more-info.component';
import { CoorInternshipRejectionComponent } from './coor-internship-rejection/coor-internship-rejection.component';
import { CoorInternshipToReviewerComponent } from './coor-internship-to-reviewer/coor-internship-to-reviewer.component';
import { CoorInternshipsComponent } from './coor-internships/coor-internships.component';
import { CoorLectorItemComponent } from './coor-lector-item/coor-lector-item.component';
import { CoorLectorsComponent } from './coor-lectors/coor-lectors.component';
import { CoorStudentItemComponent } from './coor-student-item/coor-student-item.component';
import { CoorStudentsComponent } from './coor-students/coor-students.component';
import { CoorUserInternshipDetailsComponent } from './coor-user-internship-details/coor-user-internship-details.component';
import { CoorUserInternshipItemComponent } from './coor-user-internship-item/coor-user-internship-item.component';
import { CoordinatorRoutingModule } from './coordinator-routing.module';
import { CoordinatorComponent } from './coordinator/coordinator.component';

@NgModule({
  declarations: [
    CoordinatorComponent,
    CoorInternshipsComponent,
    CoorInternshipItemComponent,
    CoorInternshipDetailsComponent,
    CoorInternshipRejectionComponent,
    CoorFinishedInternshipsComponent,
    CoorFinInternshipItemComponent,
    CoorInternshipMoreInfoComponent,
    CoorCompanyItemInactiveComponent,
    CoorCompanyRejectComponent,
    CoorInternshipToReviewerComponent,
    CoorCompaniesComponent,
    CoorLectorsComponent,
    CoorStudentsComponent,
    CoorCompanyItemActiveComponent,
    CoorLectorItemComponent,
    CoorStudentItemComponent,
    CoorAssignmentsComponent,
    CoorUserInternshipItemComponent,
    CoorUserInternshipDetailsComponent,
    CoorCompanyItemInactiveEvaluatedComponent,
  ],
  imports: [SharedModule, CommonModule, CoordinatorRoutingModule],
})
export class CoordinatorModule {}
