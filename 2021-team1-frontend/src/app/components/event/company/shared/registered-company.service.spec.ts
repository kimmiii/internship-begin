import { TestBed } from '@angular/core/testing';

import { RegisteredCompanyService } from './registered-company.service';

describe('RegisteredCompanyService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: RegisteredCompanyService = TestBed.get(RegisteredCompanyService);
    expect(service).toBeTruthy();
  });
});
