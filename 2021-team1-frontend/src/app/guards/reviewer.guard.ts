import {Injectable} from "@angular/core";
import {CanActivate, Router} from "@angular/router";
import {StateManagerService} from "../services/state-manager.service";

@Injectable()
export class ReviewerGuard implements CanActivate {

  constructor(private router: Router, private stateManagerService: StateManagerService) { }

  canActivate(): boolean {
    let roleCode: string = this.stateManagerService.roleCode;

    // If roleCode = REV, then user will be navigated to dashboard from Reviewer,
    // Else user will be navigated to his own dashboard
    if (!this.stateManagerService.isLoggedIn || roleCode != 'REV') {
      switch(roleCode) {
        case 'STU':
          this.router.navigateByUrl('/student/internships');
          break;
        case 'COM':
          this.router.navigateByUrl('/company/internships');
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
