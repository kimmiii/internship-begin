import {Component, Input, OnInit} from '@angular/core';
import {Internship} from "../../../models/internship.model";
import {User} from "../../../models/user.model";
import {Router} from "@angular/router";

@Component({
  selector: 'app-coor-user-internship-item',
  templateUrl: './coor-user-internship-item.component.html',
  styleUrls: ['./coor-user-internship-item.component.css']
})
export class CoorUserInternshipItemComponent implements OnInit {
  @Input() internship: Internship;
  @Input() student: User;

  constructor(private router: Router) { }

  ngOnInit() {
  }

  proceedToNextPage() {
    let internshipId: string = this.internship.internshipId;
    let studentId: string = String(this.student.userId);

    this.router.navigate(['coordinator/user-internship-details', internshipId, studentId]);
  }
}
