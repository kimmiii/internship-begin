import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {Internship} from "../../../models/internship.model";
import {Contact} from "../../../models/contact.model";
import {ContactService} from "../../../services/contact.service";
import {InternshipService} from "../../../services/internship.service";

@Component({
  selector: 'app-coor-internship-item',
  templateUrl: './coor-internship-item.component.html',
  styleUrls: ['./coor-internship-item.component.css']
})
export class CoorInternshipItemComponent implements OnInit {
  @Output() internshipItemLoaded: EventEmitter<any> = new EventEmitter();
  @Input() internship: Internship;
  isAllDataFetched: boolean = false;
  contact: Contact;
  countReviewersAnswered: number = 0;
  countStudentSymbol: number[];

  constructor(private internshipService: InternshipService, private contactService: ContactService) { }

  ngOnInit() {
    this.fetchData();
  }

  fetchData() {
    this.contactService.getContactById(this.internship.contactPersonId).subscribe(data => {
      this.contact = data;
      console.log('Contactpersoon succesvol opgehaald.');

      this.countReviewersThatHaveAnswered();
      this.internshipItemLoaded.emit();
      this.countStudentSymbol = Array(this.internship.totalInternsRequired).fill(1); // array will be filled with number 1 (will not be used)
      this.isAllDataFetched = true;
    });
  }

  countReviewersThatHaveAnswered() {
    this.countReviewersAnswered = this.internship.countTotalAssignedReviewers - this.internship.internshipAssignedUser.length;
  }
}
