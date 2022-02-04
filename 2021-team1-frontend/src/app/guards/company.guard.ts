import {Injectable} from "@angular/core";
import {CanActivate, Router} from "@angular/router";
import {StateManagerService} from "../services/state-manager.service";

@Injectable()
export class CompanyGuard implements CanActivate {

  constructor(private router: Router, private stateManagerService: StateManagerService) { }

  canActivate(): boolean {
    let roleCode: string = this.stateManagerService.roleCode;

    // If roleCode = COM, then user will be navigated to dashboard from Company,
    // Else user will be navigated to his own dashboard
    if (!this.stateManagerService.isLoggedIn || roleCode != 'COM') {
      switch(roleCode) {
        case 'STU':
          this.router.navigateByUrl('/student/internships');
          break;
        case 'REV':
          this.router.navigateByUrl('/reviewer/internships');
          break;
        case 'COO':
          this.router.navigateByUrl('/coordinator/internships');
          break;
      }
      return false;
    }

    return true;
  }
}
