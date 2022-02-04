import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { StateManagerService } from '../../../services/state-manager.service';

@Component({
  selector: 'app-company',
  templateUrl: './company.component.html',
  styleUrls: ['./company.component.css']
})
export class CompanyComponent implements OnInit {
  companyName: string;
  roleDescription: string;

  constructor(private router: Router, private stateManagerService: StateManagerService) { }

  ngOnInit(): void {
    this.companyName = this.stateManagerService.companyName;
    this.roleDescription = this.stateManagerService.roleDescription;
  }

  logOut(): void {
    this.stateManagerService.reset();
    this.router.navigateByUrl('/login');
  }
}
