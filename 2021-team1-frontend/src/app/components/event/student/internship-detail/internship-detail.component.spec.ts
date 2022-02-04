import { HttpClientTestingModule } from '@angular/common/http/testing';
import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { MatCardModule } from '@angular/material/card';
import { RouterTestingModule } from '@angular/router/testing';

import { CompanyService } from '../../../../services/company.service';
import { InternshipService } from '../../../../services/internship.service';
import { InternshipDetailComponent } from './internship-detail.component';

describe('IntershipDetailComponent', () => {
  let component: InternshipDetailComponent;
  let fixture: ComponentFixture<InternshipDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [MatCardModule, RouterTestingModule, HttpClientTestingModule],
      declarations: [ InternshipDetailComponent ],
      schemas: [CUSTOM_ELEMENTS_SCHEMA],
      providers: [InternshipService, CompanyService]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(InternshipDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
