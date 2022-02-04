import { Component, OnInit } from '@angular/core';
import {Title} from "@angular/platform-browser";
import {Company} from "../../../models/company.model";
import {CompanyService} from "../../../services/company.service";
import {Contact} from "../../../models/contact.model";
import {ContactService} from "../../../services/contact.service";
import {forkJoin} from "rxjs";
import {StateManagerService} from "../../../services/state-manager.service";

@Component({
  selector: 'app-com-profile',
  templateUrl: './com-profile.component.html',
  styleUrls: ['./com-profile.component.css']
})
export class ComProfileComponent implements OnInit {
  isAllDataFetched: boolean = false;
  company: Company;
  contactist: Contact[];
  showContactDetailsIsVisible: boolean = false;
  selectedContact: Contact;
  modalPopupIsVisible: boolean = false;
  addContactFormIsVisible: boolean = false;
  deletedSuccessfully: boolean = false;

  constructor(private titleService: Title, private companyService: CompanyService, private contactService: ContactService,
              private stateManagerService: StateManagerService) { }

  ngOnInit() {
    this.titleService.setTitle('Stagebeheer | Profiel');
    this.fetchData();
  }

  fetchData() {
    let companyObservable = this.companyService.getCompanyById(this.stateManagerService.companyId);
    let contactObservable = this.contactService.getContactByCompanyId(this.stateManagerService.companyId);

    forkJoin([companyObservable, contactObservable]).subscribe(results => {
      this.company = results[0];
      this.contactist = results[1];
      console.log(`Bedrijven en contacten zijn succesvol opgehaald.`);

      this.isAllDataFetched = true;
      this.showContactDetailsIsVisible = false;
    });
  }

  showContactDetails($event: Contact) {
    this.selectedContact = $event;
    this.showContactDetailsIsVisible = true;
    this.addContactFormIsVisible = false;
    this.modalPopupIsVisible = false;
  }

  delete($event: Contact) {
    this.selectedContact = $event;
    this.modalPopupIsVisible = true;
    this.addContactFormIsVisible = false;
  }

  confirmDelete() {
    this.isAllDataFetched = false;

    this.contactService.deleteContact(this.selectedContact.contactId).subscribe(res => {
      this.modalPopupIsVisible = false;

      if (res.error != null) {
        window.alert('Deze contactpersoon kan niet verwijderd worden.');
      } else {
        console.log(`Contactpersoon met id ${this.selectedContact.contactId} is succesvol verwijderd.`);
        this.fetchData();
        this.deletedSuccessfully = true;
        setTimeout(() => { this.deletedSuccessfully = false }, 5000);
      }
    });
  }

  cancelDelete() {
    this.modalPopupIsVisible = false;
  }

  toggleAddContactForm() {
    this.addContactFormIsVisible = true;
    this.showContactDetailsIsVisible = false;
    this.modalPopupIsVisible = false;
  }

  closeAddContactComponent() {
    this.addContactFormIsVisible = false;
    this.isAllDataFetched = false;
    this.fetchData();
  }
}
