import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FlexLayoutModule, FlexModule } from '@angular/flex-layout';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { NgSelectModule } from '@ng-select/ng-select';
import { TranslateModule } from '@ngx-translate/core';
import { PdfViewerModule } from 'ng2-pdf-viewer';

import { ComAppliedStudentItemComponent } from '../components/company/com-applied-student-item/com-applied-student-item.component';
import { ComEditCompanyPromotorComponent } from '../components/company/com-edit-company-promotor/com-edit-company-promotor.component';
import { CoorCompanyDetailsComponent } from '../components/coordinator/coor-company-details/coor-company-details.component';
import { NotFoundComponent } from '../components/exception/not-found/not-found.component';
import { ServerErrorComponent } from '../components/exception/server-error/server-error.component';
import { FooterComponent } from '../components/general/footer/footer.component';
import { InternshipGenericDetailsComponent } from '../components/general/internship-generic-details/internship-generic-details.component';
import { LoginComponent } from '../components/general/login/login.component';
import { RegisterSuccessfulComponent } from '../components/general/register-successful/register-successful.component';
import { RegisterComponent } from '../components/general/register/register.component';
import { SpecialisationEditFormComponent } from '../components/general/specialisation-edit-form/specialisation-edit-form.component';
import { ContainerComponent } from '../container.component';
import { ReplacePipe } from '../pipes/replace.pipe';
import { BaseComponent } from './components/base/base.component';
import { UploadFileComponent } from './components/upload-file/upload-file.component';
import { MaterialModule } from './material/material.module';
import { TruncatePipe } from './pipes/truncate.pipe';

@NgModule({
  declarations: [
    LoginComponent,
    ContainerComponent,
    RegisterComponent,
    RegisterSuccessfulComponent,
    FooterComponent,
    NotFoundComponent,
    ServerErrorComponent,
    InternshipGenericDetailsComponent,
    SpecialisationEditFormComponent,
    ComAppliedStudentItemComponent,
    CoorCompanyDetailsComponent,
    ComEditCompanyPromotorComponent,
    BaseComponent,
    ReplacePipe,
    TruncatePipe,
    UploadFileComponent,
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    HttpClientModule,
    PdfViewerModule,
    RouterModule,
    MaterialModule,
    NgSelectModule,
    FlexLayoutModule,
    FlexModule,
  ],
  exports: [
    LoginComponent,
    ContainerComponent,
    RegisterComponent,
    RegisterSuccessfulComponent,
    FooterComponent,
    NotFoundComponent,
    ServerErrorComponent,
    InternshipGenericDetailsComponent,
    SpecialisationEditFormComponent,
    ReactiveFormsModule,
    FormsModule,
    RouterModule,
    PdfViewerModule,
    ComAppliedStudentItemComponent,
    CoorCompanyDetailsComponent,
    ComEditCompanyPromotorComponent,
    MaterialModule,
    ReplacePipe,
    TruncatePipe,
    UploadFileComponent,
    NgSelectModule,
    FlexLayoutModule,
    FlexModule,
    TranslateModule
  ],
})
export class SharedModule {}
