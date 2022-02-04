import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import {
  DEFAULT_LANGUAGE,
  MissingTranslationHandler,
  TranslateCompiler,
  TranslateLoader, TranslateParser,
  TranslatePipe,
  TranslateService,
  TranslateStore, USE_DEFAULT_LANG, USE_EXTEND, USE_STORE
} from '@ngx-translate/core';

import { AppointmentStatusComponent } from './appointment-status.component';

describe('AppointmentStatusComponent', () => {
  let component: AppointmentStatusComponent;
  let fixture: ComponentFixture<AppointmentStatusComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AppointmentStatusComponent, TranslatePipe ],
      providers: [
        TranslateService, TranslateStore, TranslateLoader, TranslateCompiler, TranslateParser, MissingTranslationHandler,
        { provide: USE_DEFAULT_LANG }, { provide: USE_STORE },  { provide: USE_EXTEND }, { provide: DEFAULT_LANGUAGE }]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AppointmentStatusComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
