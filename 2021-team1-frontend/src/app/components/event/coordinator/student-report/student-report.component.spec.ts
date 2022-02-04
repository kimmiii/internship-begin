import { HttpClientTestingModule } from '@angular/common/http/testing';
import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatPaginatorModule } from '@angular/material/paginator';
import { RouterTestingModule } from '@angular/router/testing';
import { TranslatePipe } from '@ngx-translate/core';

import { AccountService } from '../../../../services/account.service';
import { CompanyService } from '../../../../services/company.service';
import { TimeSpanPipe } from '../../shared/time-span.pipe';
import { AppointmentsToStatusColorPipe } from '../shared/appointments-to-status-color.pipe';
import { FilterPipe } from '../shared/filter.pipe';
import { SortPipe } from '../shared/sort.pipe';
import { StudentReportComponent } from './student-report.component';

describe('StudentReportComponent', () => {
  let component: StudentReportComponent;
  let fixture: ComponentFixture<StudentReportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [ MatPaginatorModule, FormsModule, ReactiveFormsModule, HttpClientTestingModule, RouterTestingModule ],
      declarations: [ StudentReportComponent, TimeSpanPipe, SortPipe, TranslatePipe, AppointmentsToStatusColorPipe, FilterPipe],
      schemas: [CUSTOM_ELEMENTS_SCHEMA],
      providers: [AccountService, CompanyService]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(StudentReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
