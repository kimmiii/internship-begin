import { HTTP_INTERCEPTORS, HttpClient } from '@angular/common/http';
import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { FlexLayoutModule } from '@angular/flex-layout';
import { FormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgSelectConfig, NgSelectModule } from '@ng-select/ng-select';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';

import { AppComponent } from './components/general/app/app.component';
import { CompanyGuard } from './guards/company.guard';
import { CoordinatorGuard } from './guards/coordinator.guard';
import { FormGuard } from './guards/form.guard';
import { LoggedInGuard } from './guards/logged-in.guard';
import { RegisterGuard } from './guards/register.guard';
import { ReviewerGuard } from './guards/reviewer.guard';
import { StudentGuard } from './guards/student.guard';
import { AppRoutingModule } from './routing/app-routing.module';
import { AccountService } from './services/account.service';
import { CompanyService } from './services/company.service';
import { ContactService } from './services/contact.service';
import { EventService } from './services/event.service';
import { InternshipService } from './services/internship.service';
import { StateManagerService } from './services/state-manager.service';
import { TokenInterceptorService } from './services/token-interceptor.service';
import { SharedModule } from './shared/shared.module';

export function HttpLoaderFactory(http: HttpClient): TranslateHttpLoader {
  return new TranslateHttpLoader(http);
}

@NgModule({
  declarations: [ AppComponent ],
  imports: [
    BrowserAnimationsModule,
    SharedModule,
    AppRoutingModule,
    FlexLayoutModule,
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: HttpLoaderFactory,
        deps: [HttpClient]
      },
      defaultLanguage: 'nl'
    })
  ],
  providers: [
    AccountService,
    InternshipService,
    ContactService,
    StateManagerService,
    CompanyService,
    CompanyGuard,
    CoordinatorGuard,
    ReviewerGuard,
    StudentGuard,
    FormGuard,
    RegisterGuard,
    LoggedInGuard,
    EventService,
    NgSelectModule,
    FormsModule,
    NgSelectConfig,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: TokenInterceptorService,
      multi: true,
    },
  ],
  bootstrap: [ AppComponent ],
  schemas: [ CUSTOM_ELEMENTS_SCHEMA ],
})
export class AppModule {
}
