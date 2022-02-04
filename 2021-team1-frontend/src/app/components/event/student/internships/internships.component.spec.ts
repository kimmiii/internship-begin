import { HttpClientTestingModule } from '@angular/common/http/testing';
import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';

import { CompanyService } from '../../../../services/company.service';
import { InternshipService } from '../../../../services/internship.service';
import { InternshipsComponent } from './internships.component';

describe('InternshipsComponent', () => {
  let component: InternshipsComponent;
  let fixture: ComponentFixture<InternshipsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule, RouterTestingModule],
      declarations: [ InternshipsComponent ],
      schemas: [CUSTOM_ELEMENTS_SCHEMA],
      providers: [InternshipService, CompanyService]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(InternshipsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
