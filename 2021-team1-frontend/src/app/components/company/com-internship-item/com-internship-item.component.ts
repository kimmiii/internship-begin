import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {forkJoin} from 'rxjs';
import {Internship} from "../../../models/internship.model";
import {InternshipService} from "../../../services/internship.service";
import {ContactService} from "../../../services/contact.service";
import {Contact} from "../../../models/contact.model";
import {ProjectStatus} from "../../../models/project-status.model";

@Component({
  selector: 'app-internship-item',
  templateUrl: './com-internship-item.component.html',
  styleUrls: ['./com-internship-item.component.css']
})
export class ComInternshipItemComponent implements OnInit {
  @Input() internship: Internship;
  @Output() internshipItemLoaded: EventEmitter<any> = new EventEmitter();
  isAllDataFetched: boolean = false;
  statusCode: ProjectStatus;
  contact: Contact;
  favouriteCount: number;
  countAppliedStudents: number;
  countStudentSymbol: number[];
  countAssignedStudents: number = 0;
  countAssignedStudentsArray: number[];

  constructor(private internshipService: InternshipService, private contactService: ContactService) { }

  ngOnInit() {
    this.fetchData();
  }

  fetchData() {
    let statusObservable = this.internshipService.getStatusById(this.internship.projectStatusId);
    let contactObservable = this.contactService.getContactById(this.internship.contactPersonId);
    let favouriteObservable = this.internshipService.getCountFavouriteInternshipsById(this.internship.internshipId);
    let countAppliedStudentsObservable = this.internshipService.getCountAppliedStudents(this.internship.internshipId);

    forkJoin([statusObservable, contactObservable, favouriteObservable, countAppliedStudentsObservable]).subscribe(results => {
      this.statusCode = results[0];
      this.contact = results[1];
      this.favouriteCount = results[2];
      this.countAppliedStudents = results[3];

      this.isAllDataFetched = true;
      console.log(`Alle data van het internship met id ${this.internship.internshipId} is succesvol geladen voor het internship-item.`);

      this.countStudentSymbols();
      this.internshipItemLoaded.emit();
    });
  }

  // When will labels be shown? ...
  showLabel(): boolean {
    return (this.statusCode != null && this.statusCode.code == 'REV') || (this.statusCode != null && this.statusCode.code == 'NEW' && this.internship.externalFeedback != null);
  }

  countStudentSymbols() {
    this.countAssignedStudents = this.internship.internshipAssignedUser.length;
    this.countAssignedStudentsArray = Array(this.countAssignedStudents).fill(1); // array will be filled with number 1 (will not be used)

    // This is the count that defines how much student symbols will be shown
    let count: number = this.internship.totalInternsRequired;
    if (this.internship.projectStatus.code == 'APP') {
      count -= this.countAssignedStudents;
    }
    this.countStudentSymbol = Array(count).fill(1); // array will be filled with number 1 (will not be used)
  }
}
