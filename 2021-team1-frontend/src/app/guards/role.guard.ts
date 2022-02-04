import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate } from '@angular/router';

import { StateManagerService } from '../services/state-manager.service';

@Injectable({
  providedIn: 'root',
})
export class RoleGuard implements CanActivate {
  constructor(
    private stateManagerService: StateManagerService,
  ) {
  }

  canActivate(next: ActivatedRouteSnapshot): boolean {
    return next.data.role === this.stateManagerService.roleCode;
  }
}
