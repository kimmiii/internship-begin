import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';

import { StateManagerService } from '../services/state-manager.service';

@Injectable()
export class LoggedInGuard implements CanActivate {
  constructor(
    private router: Router,
    private stateManagerService: StateManagerService,
  ) {}

  canActivate(): boolean {
    if (this.stateManagerService.isLoggedIn) {
      return true;
    } else {
      this.router.navigateByUrl('login');
    }
  }
}
