import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { map, tap } from 'rxjs/operators';

import { EventCompany } from '../../../../models';
import { EventService } from '../../../../services/event.service';

@Injectable({
  providedIn: 'root',
})
export class RegisteredCompanyGuard implements CanActivate {
  constructor(
    private eventService: EventService,
    private router: Router
  ) {
  }

  canActivate(): Observable<boolean> {
    return this.eventService.getCompanyRegistration()
      .pipe(
        map((eventCompany: EventCompany) => !!eventCompany),
        tap((isCompanyRegistered: boolean) => {
         if (!isCompanyRegistered) {
           this.router.navigateByUrl('/event/company/settings');
         }
        })
      );
  }
}
