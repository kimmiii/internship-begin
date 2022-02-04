import {Injectable} from "@angular/core";
import {CanActivate, Router} from "@angular/router";
import {StateManagerService} from "../services/state-manager.service";

@Injectable()
export class CoordinatorGuard implements CanActivate {

  constructor(private router: Router, private stateManagerService: StateManagerService) { }

  canActivate(): boolean {
    let roleCode: string = this.stateManagerService.roleCode;

    // If roleCode = COO, then user will be navigated to dashboard from Coordinator,
    // Else user will be navigated to his own dashboard
    if (!this.stateManagerService.isLoggedIn || roleCode != 'COO') {
      switch(roleCode) {
        case 'STU':
          this.router.navigateByUrl('/student/internships');
          break;
        case 'REV':
          this.router.navigateByUrl('/reviewer/internships');
          break;
        case 'COM':
          this.router.navigateByUrl('/company/internships');
          break;
      }
      return false;
    }

    return true;
  }
}
