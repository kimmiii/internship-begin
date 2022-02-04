import {Component, EventEmitter, OnInit, Output} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {Contact} from "../../../models/contact.model";
import {ContactService} from "../../../services/contact.service";
import {StateManagerService} from "../../../services/state-manager.service";

@Component({
  selector: 'app-com-contact-add',
  templateUrl: './com-contact-add.component.html',
  styleUrls: ['./com-contact-add.component.css']
})
export class ComContactAddComponent implements OnInit {
  @Output() closeAddContactComponent: EventEmitter<any> = new EventEmitter();
  @Output() contactAdded: EventEmitter<Contact> = new EventEmitter();
  addContactForm: FormGroup;
  submitButtonClicked: boolean = false;
  formSubmitted: boolean = false;
  contactErrorOccurred: boolean = false;
  contactErrorMessage: string;
  contact: Contact;
  showSpinner: boolean = false;

  constructor(private formBuilder: FormBuilder, private contactService: ContactService,
              private stateManagerService: StateManagerService) { }

  ngOnInit() {
    this.createForm();
  }

  createForm() {
    this.addContactForm = this.formBuilder.group({
      firstname: [null, [Validators.required, Validators.maxLength(50)]],
      surname: [null, [Validators.required, Validators.maxLength(50)]],
      function: [null, [Validators.required, Validators.minLength(2), Validators.maxLength(100)]],
      email: [null, [Validators.required, Validators.email, Validators.minLength(3), Validators.maxLength(64)]],
      phoneNumber: [null, [Validators.required, Validators.pattern('\\+[0-9]{10,11}')]]
    });

    console.log('AddContactForm succesvol aangemaakt.');
  }

  addContact() {
    this.showSpinner = true;
    this.submitButtonClicked = true;

    if (this.addContactForm.valid) {
      this.formSubmitted = true;
      this.contact = this.createContactObject();
      console.log(`Contactpersoon-object succesvol aangemaakt:`, this.contact);

      this.contactService.addContact(this.contact).subscribe(res => {
        if (res.error != null) { // error occurred
            this.formSubmitted = false;
            this.contactErrorMessage = res.error.message;
            this.contactErrorOccurred = true;
            this.showSpinner = false;
            console.log(`Fout bij toevoegen van contact: ${this.contactErrorMessage}`);
        } else {
          console.log('Contact succesvol toegevoegd:', this.contact);
          this.resetRegisterForm();
          this.showSpinner = false;
        }
      });
    } else {
      this.showSpinner = false;
    }
  }

  createContactObject(): Contact {
    let companyId: number = this.stateManagerService.companyId;
    let firstname: string = this.firstname.value;
    let surname: string = this.surname.value;
    let email: string = this.email.value;
    let phoneNumber: string = this.phoneNumber.value;
    let contactFunction: string = this.function.value;

    return new Contact(companyId, firstname, surname, email, phoneNumber, contactFunction);
  }

  resetRegisterForm() {
    this.submitButtonClicked = false;
    this.addContactForm.reset();
    this.closeAddContactComponent.emit();
    this.contactAdded.emit(this.contact); // Send contact to application form
  }

  // Getters ContactAddForm
  get firstname() {
    return this.addContactForm.controls.firstname;
  }
  get surname() {
    return this.addContactForm.controls.surname;
  }
  get function() {
    return this.addContactForm.controls.function;
  }
  get email() {
    return this.addContactForm.controls.email;
  }
  get phoneNumber() {
    return this.addContactForm.controls.phoneNumber;
  }
}
