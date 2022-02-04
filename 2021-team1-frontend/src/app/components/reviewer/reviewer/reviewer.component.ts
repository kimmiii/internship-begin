import { Component, OnInit } from '@angular/core';
import {Router} from "@angular/router";
import {StateManagerService} from "../../../services/state-manager.service";

@Component({
  selector: 'app-reviewer',
  templateUrl: './reviewer.component.html',
  styleUrls: ['./reviewer.component.css']
})
export class ReviewerComponent implements OnInit {
  reviewerName: string;
  roleDescription: string;

  constructor(private router: Router, private stateManagerService: StateManagerService) { }

  ngOnInit() {
    this.reviewerName = `${this.stateManagerService.userFirstName} ${this.stateManagerService.userSurname}`;
    this.roleDescription = this.stateManagerService.roleDescription;
  }

  logOut() {
    this.stateManagerService.reset();
    this.router.navigateByUrl('/login');
  }

}
