import { Component, OnInit } from '@angular/core';
import {Router} from "@angular/router";
import {StateManagerService} from "../../../services/state-manager.service";

@Component({
  selector: 'app-server-error',
  templateUrl: './server-error.component.html',
  styleUrls: ['./server-error.component.css']
})
export class ServerErrorComponent implements OnInit {

  constructor(private router: Router, private stateManagerService: StateManagerService) { }

  ngOnInit() {
  }

  proceedToLogin() {
    if (this.stateManagerService.isLoggedIn == true) {
      this.router.navigateByUrl('/company/internships'); // Gebruiker wordt automatisch doorgestuurd naar juiste dashboard
    } else {
      this.router.navigateByUrl('/login');
    }
  }
}
