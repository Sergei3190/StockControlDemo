import { TestBed } from '@angular/core/testing';

import { AppConfigurationService as AppConfigurationService } from './app-configuration.service';

describe('AppCconfigurationService', () => {
  let service: AppConfigurationService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AppConfigurationService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
