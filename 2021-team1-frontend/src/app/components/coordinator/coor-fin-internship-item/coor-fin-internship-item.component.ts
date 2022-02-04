import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {forkJoin} from 'rxjs';
import {Internship} from "../../../models/internship.model";
import {Contact} from "../../../models/contact.model";
import {ContactService} from "../../../services/contact.service";
import {InternshipService} from "../../../services/internship.service";
import {InternshipSpecialisation} from "../../../models/internship-specialisation.model";

@Component({
  selector: 'app-coor-fin-internship-item',
  templateUrl: './coor-fin-internship-item.component.html',
  styleUrls: ['./coor-fin-internship-item.component.css']
})
export class CoorFinInternshipItemComponent implements OnInit {
  isAllDataFetched: boolean = false;
  @Output() internshipItemLoaded: EventEmitter<any> = new EventEmitter();
  @Input() internship: Internship;
  contact: Contact;
  specialisationList: InternshipSpecialisation[];
  countStudentSymbol: number[];
  countAssignedStudents: number = 0;
  countAssignedStudentsArray: number[];

  constructor(private contactService: ContactService, private internshipService: InternshipService) { }

  ngOnInit() {
    this.fetchData();
  }

  fetchData() {
    let contactObservable = this.contactService.getContactById(this.internship.contactPersonId);
    let specialisationObservable = this.internshipService.getInternshipById(this.internship.internshipId);

    forkJoin([contactObservable, specialisationObservable]).subscribe(results => {
      this.contact = results[0];
      this.specialisationList = results[1].internshipSpecialisation;

      this.countStudentSymbols();
      this.isAllDataFetched = true;
      this.internshipItemLoaded.emit();
    });
  }

  countStudentSymbols() {
    this.countAssignedStudents = this.internship.internshipAssignedUser.length;
    this.countAssignedStudentsArray = Array(this.countAssignedStudents).fill(1); // array will be filled with number 1 (will not be used)

    // This is the count that defines how much student symbols will be shown
    let count: number = this.internship.totalInternsRequired;
    if (!this.internship.completed && this.internship.projectStatus.code == 'APP') {
      count -= this.countAssignedStudents;
    }
    this.countStudentSymbol = Array(count).fill(1); // array will be filled with number 1 (will not be used)
  }
}
