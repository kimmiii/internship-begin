import {Component, Input, OnInit} from '@angular/core';
import {Contact} from "../../../models/contact.model";

@Component({
  selector: 'app-com-contact-details',
  templateUrl: './com-contact-details.component.html',
  styleUrls: ['./com-contact-details.component.css']
})
export class ComContactDetailsComponent implements OnInit {
  @Input() contact: Contact;

  constructor() { }

  ngOnInit() {
  }
}
