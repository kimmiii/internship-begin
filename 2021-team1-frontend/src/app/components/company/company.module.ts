import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { SharedModule } from '../../shared/shared.module';
import { ComApplicationFormComponent } from './com-application-form/com-application-form.component';
import { ComContactAddComponent } from './com-contact-add/com-contact-add.component';
import { ComContactDetailsComponent } from './com-contact-details/com-contact-details.component';
import { ComContactItemComponent } from './com-contact-item/com-contact-item.component';
import { ComInternshipDetailsComponent } from './com-internship-details/com-internship-details.component';
import { ComInternshipEditComponent } from './com-internship-edit/com-internship-edit.component';
import { ComInternshipItemComponent } from './com-internship-item/com-internship-item.component';
import { ComInternshipsComponent } from './com-internships/com-internships.component';
import { ComProfileComponent } from './com-profile/com-profile.component';
import { CompanyRoutingModule } from './company-routing.module';
import { CompanyComponent } from './company/company.component';

@NgModule({
  declarations: [
    CompanyComponent,
    ComApplicationFormComponent,
    ComInternshipsComponent,
    ComInternshipDetailsComponent,
    ComInternshipItemComponent,
    ComInternshipEditComponent,
    ComProfileComponent,
    ComContactItemComponent,
    ComContactDetailsComponent,
    ComContactAddComponent,
  ],
  imports: [SharedModule, CommonModule, CompanyRoutingModule],
  exports: [],
})
export class CompanyModule {}
