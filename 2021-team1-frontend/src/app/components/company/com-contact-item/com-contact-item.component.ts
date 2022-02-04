import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {Contact} from "../../../models/contact.model";

@Component({
  selector: 'app-com-contact-item',
  templateUrl: './com-contact-item.component.html',
  styleUrls: ['./com-contact-item.component.css']
})
export class ComContactItemComponent implements OnInit {
  @Input() contact: Contact;
  @Output() viewDetailsClicked: EventEmitter<Contact> = new EventEmitter();
  @Output() deleteContactClicked: EventEmitter<Contact> = new EventEmitter();

  constructor() { }

  ngOnInit() {
  }

  viewDetails() {
    this.viewDetailsClicked.emit(this.contact);
  }

  delete() {
    this.deleteContactClicked.emit(this.contact);
  }
}
