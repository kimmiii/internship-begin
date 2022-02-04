import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';

import { RoleCode } from './models/role.model';
import { StateManagerService } from './services/state-manager.service';
import { companyMenuItems, coordinatorMenuItems, reviewerMenuItems, studentMenuItems } from './utils/menu-items';

@Component({
  selector: 'app-app',
  templateUrl: './container.component.html',
  styleUrls: [ './container.component.scss' ],
})
export class ContainerComponent implements OnInit {
  fullName: string;
  companyName: string;
  roleDescription: string;

  menuItems: MenuItem[] = [];
  routePrefix: string;

  constructor(
    private stateManagerService: StateManagerService,
    private router: Router,
    translate: TranslateService
  ) {
    this.fullName = `${this.stateManagerService.userFirstName} ${this.stateManagerService.userSurname}`;
    this.companyName = this.stateManagerService.companyName;
    this.roleDescription = this.stateManagerService.roleDescription;
    translate.setDefaultLang('nl');
  }

  ngOnInit(): void {
    this.assignCorrectMenuItemsForRole();
    this.openFirstPage();
  }

  private assignCorrectMenuItemsForRole(): void {
    switch (this.stateManagerService.roleCode) {
      case RoleCode.STU:
        this.menuItems = studentMenuItems;
        this.routePrefix = 'student';
        break;
      case RoleCode.REV:
        this.menuItems = reviewerMenuItems;
        this.routePrefix = 'reviewer';
        break;
      case RoleCode.COO:
        this.menuItems = coordinatorMenuItems;
        this.routePrefix = 'coordinator';
        break;
      case RoleCode.COM:
        this.menuItems = companyMenuItems;
        this.routePrefix = 'company';
        break;
    }
  }

  private openFirstPage(): void {
    this.router.navigateByUrl(this.router.url === '/'
      ? this.routePrefix
      : this.router.url);
  }

  logOut(): void {
    this.stateManagerService.reset();
    this.router.navigateByUrl('/login');
  }
}

export interface MenuItem {
  routerLink: string;
  displayText: string;
}
