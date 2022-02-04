import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { SharedModule } from '../../shared/shared.module';
import { StuFilterComponent } from './stu-filter/stu-filter.component';
import { StuInternshipDetailsComponent } from './stu-internship-details/stu-internship-details.component';
import { StuInternshipItemComponent } from './stu-internship-item/stu-internship-item.component';
import { StuInternshipsComponent } from './stu-internships/stu-internships.component';
import { StuMyInternshipItemComponent } from './stu-my-internship-item/stu-my-internship-item.component';
import { StuMyInternshipsComponent } from './stu-my-internships/stu-my-internships.component';
import { StuProfileComponent } from './stu-profile/stu-profile.component';
import { StudentRoutingModule } from './student-routing.module';
import { StudentComponent } from './student/student.component';

@NgModule({
  declarations: [
    StuFilterComponent,
    StuInternshipsComponent,
    StudentComponent,
    StuInternshipItemComponent,
    StuInternshipDetailsComponent,
    StuProfileComponent,
    StuMyInternshipsComponent,
    StuMyInternshipItemComponent,
  ],
  imports: [SharedModule, CommonModule, StudentRoutingModule],
})
export class StudentModule {}
