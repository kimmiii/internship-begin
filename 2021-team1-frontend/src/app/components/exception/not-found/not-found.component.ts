import { Component, OnInit } from '@angular/core';
import {Router} from "@angular/router";
import {StateManagerService} from "../../../services/state-manager.service";

@Component({
  selector: 'app-not-found',
  templateUrl: './not-found.component.html',
  styleUrls: ['./not-found.component.css']
})
export class NotFoundComponent implements OnInit {

  constructor(private router: Router, private stateManagerService: StateManagerService) { }

  ngOnInit() {
  }

  proceedToStartpage() {
    if (this.stateManagerService.isLoggedIn == true) {
      this.router.navigateByUrl('/company/internships'); // Gebruiker wordt automatisch doorgestuurd naar juiste dashboard
    } else {
      this.router.navigateByUrl('/login');
    }
  }
}
