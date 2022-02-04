import { HttpClientTestingModule } from '@angular/common/http/testing';
import { inject, TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';

import { RegisteredCompanyGuard } from './registered-company.guard';

describe('RegisteredCompanyGuard', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule, RouterTestingModule],
      providers: [RegisteredCompanyGuard]
    });
  });

  it('should ...', inject([RegisteredCompanyGuard], (guard: RegisteredCompanyGuard) => {
    expect(guard).toBeTruthy();
  }));
});
