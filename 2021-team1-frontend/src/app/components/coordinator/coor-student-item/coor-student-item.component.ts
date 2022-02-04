import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {User} from "../../../models/user.model";
import {InternshipService} from "../../../services/internship.service";

@Component({
  selector: 'app-coor-student-item',
  templateUrl: './coor-student-item.component.html',
  styleUrls: ['./coor-student-item.component.css']
})
export class CoorStudentItemComponent implements OnInit {
  @Input() student: User;
  @Output() studentToListComponent: EventEmitter<User> = new EventEmitter();
  countFavouriteInternships: number;
  countApplicationsInternships: number;

  constructor(private internshipService: InternshipService) { }

  ngOnInit() {
    this.getCountFavouritesByStudentId();
  }

  getCountFavouritesByStudentId() {
    this.internshipService.getCountFavouritesByStudentId(this.student.userId).subscribe(data =>{
      console.log(`Aantal favoriete stageopdrachten voor student met id ${this.student.userId} succesvol opgehaald.`);
      this.countFavouriteInternships = data;
      this.getCountApplicationsByStudentId();
    });
  }

  getCountApplicationsByStudentId() {
    this.internshipService.getApplicationsByStudentId(this.student.userId).subscribe(data => {
      console.log(`Aantal aanmeldingen voor stages voor student met id ${this.student.userId} succesvol opgehaald.`);
      this.countApplicationsInternships = data;
      this.student.countApplications = this.countApplicationsInternships;
      this.studentToListComponent.emit(this.student);
    });
  }
}
