import {Injectable} from "@angular/core";
import {CanActivate, Router} from "@angular/router";
import {StateManagerService} from "../services/state-manager.service";

@Injectable()
export class StudentGuard implements CanActivate {

  constructor(private router: Router, private stateManagerService: StateManagerService) { }

  canActivate(): boolean{
    let roleCode: string = this.stateManagerService.roleCode;

    // If roleCode = STU, then user will be navigated to dashboard from Student,
    // Else user will be navigated to his own dashboard
    if (!this.stateManagerService.isLoggedIn || roleCode != 'STU') {
      switch (roleCode) {
        case 'REV':
          this.router.navigateByUrl('/reviewer/internships');
          break;
        case 'COO':
          this.router.navigateByUrl('/coordinator/internships');
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
