import { Component, OnInit } from '@angular/core';
import {StateManagerService} from "../../../services/state-manager.service";
import {Router} from "@angular/router";

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.css']
})
export class FooterComponent implements OnInit {
  roleDescription: string;
  isLoggedIn: boolean = false;

  constructor(private stateManagerService: StateManagerService, private router: Router) {
  }

  ngOnInit() {
    this.router.events.subscribe(() => this.determineRole());
  }

  determineRole() {
    this.roleDescription= this.stateManagerService.roleDescription;
    this.isLoggedIn = this.stateManagerService.isLoggedIn;
  }
}
