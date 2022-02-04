import { Component, OnInit } from '@angular/core';
import {Router} from "@angular/router";
import {StateManagerService} from "../../../services/state-manager.service";

@Component({
  selector: 'app-student',
  templateUrl: './student.component.html',
  styleUrls: ['./student.component.css']
})
export class StudentComponent implements OnInit {
  studentName: string;
  roleDescription: string;

  constructor(private router: Router, private stateManagerService: StateManagerService) { }

  ngOnInit() {
    this.studentName = `${this.stateManagerService.userFirstName} ${this.stateManagerService.userSurname}`;
    this.roleDescription = this.stateManagerService.roleDescription;
  }

  logOut() {
    this.stateManagerService.reset();
    this.stateManagerService.resetFilter();
    this.router.navigateByUrl('/login');
  }
}
