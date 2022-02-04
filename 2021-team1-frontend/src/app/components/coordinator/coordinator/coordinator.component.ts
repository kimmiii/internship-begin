import { Component, OnInit } from '@angular/core';
import {Router} from "@angular/router";
import {StateManagerService} from "../../../services/state-manager.service";

@Component({
  selector: 'app-coordinator',
  templateUrl: './coordinator.component.html',
  styleUrls: ['./coordinator.component.css']
})
export class CoordinatorComponent implements OnInit {
  coordinatorName: string;
  roleDescription: string;

  constructor(private router: Router, private stateManagerService: StateManagerService) { }

  ngOnInit() {
    this.coordinatorName = `${this.stateManagerService.userFirstName} ${this.stateManagerService.userSurname}`;
    this.roleDescription = this.stateManagerService.roleDescription;
  }

  logOut() {
    this.stateManagerService.reset();
    this.router.navigateByUrl('/login');
  }
}
