import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {Internship} from "../../../models/internship.model";
import {Message} from "../../../models/message.model";
import {Contact} from "../../../models/contact.model";
import {StateManagerService} from "../../../services/state-manager.service";
import {UserFavourites} from "../../../models/user-favourites.model";
import {InternshipService} from "../../../services/internship.service";
import {User} from "../../../models/user.model";
import {UserInternships} from "../../../models/user-internships.model";
import {forkJoin} from "rxjs";
import {ContactService} from "../../../services/contact.service";

@Component({
  selector: 'app-internship-generic-details',
  templateUrl: './internship-generic-details.component.html',
  styleUrls: ['./internship-generic-details.component.css']
})
export class InternshipGenericDetailsComponent implements OnInit {
  @Input() internship: Internship;
  @Input() student: User;
  @Output() closeModalPopup: EventEmitter<boolean> = new EventEmitter<boolean>();
  roleCodeLoggedInUser: string;
  externalFeedbackList: Message[] = [];
  internalFeedbackList: Message[] = [];
  noInput: string = "Niet ingevuld";
  contactList: Contact[] = [];
  contact: Contact;
  favoriteToggle: boolean = false;
  favouriteCount: number;
  appliedStudentsCount: number;
  appliedStudentsList: User[] = [];
  studentListVisible: boolean = false;
  reviewersList: User[] = [];
  specialisationEditFormIsVisible: boolean = false;
  companyPromotorFormIsVisible: boolean = false;
  studentToHire: User;
  hireStudentModalPopUpIsVisible: boolean = false;
  hireStudentSubmitted: boolean;
  filterIndexAppliedStudents: number;
  showSpinner: boolean = false;
  showFavouriteToggleButton: boolean = true;
  emailSent: boolean = false;
  emailSentToIndex: number[] = [];
  researchTopicDescriptions: string[] = [];

  constructor(private stateManagerService: StateManagerService, private internshipService: InternshipService,
              private contactService: ContactService) { }

  ngOnInit() {
    this.showSpinner = true;
    this.hireStudentSubmitted = false;
    this.roleCodeLoggedInUser = this.stateManagerService.roleCode;
    this.filterIndexAppliedStudents = this.stateManagerService.valueSelectedStudentOrderByCompany;

    if (this.internship.externalFeedback != null) {
      this.fillListWithExternalFeedback();
    }
    if (this.internship.internalFeedback != null) {
      this.fillListWithInternalFeedback();
    }
    // GET data from contact person
    if (this.internship.company.contacts.length > 0) {
      this.fillListWithContacts();
      this.contact = this.contactList.filter(x => x.contactId == this.internship.contactPersonId)[0];
    }
    this.markThisInternshipAsFavourite();
    this.fillReviewersList();

    this.fetchData();
  }

  fetchData() {
    let internshipId: string = this.internship.internshipId;

    let getCountFavouriteInternshipsByIdObservable = this.internshipService.getCountFavouriteInternshipsById(internshipId);
    let getCountAppliedStudentsObservable = this.internshipService.getCountAppliedStudents(internshipId);

    forkJoin([getCountFavouriteInternshipsByIdObservable, getCountAppliedStudentsObservable]).subscribe(results => {
      this.favouriteCount = results[0];
      console.log(`Aantal keer dat deze stageopdracht is gemarkeerd als FAVORIET is succesvol opgehaald: ${this.favouriteCount}.`);
      this.appliedStudentsCount = results[1];
      console.log(`Aantal keer dat er voor deze stageopdracht is gesolliciteerd is succesvol opgehaald: ${this.appliedStudentsCount}.`);

      this.fetchAppliedStudents(this.filterIndexAppliedStudents);
      this.splitResearchTopicDescriptions();
    });
  }

  fillListWithExternalFeedback() {
    this.externalFeedbackList = JSON.parse(this.internship.externalFeedback);
    this.convertMessageDT(this.externalFeedbackList);

    // Fix line breaks
    for(let message of this.externalFeedbackList) {
      message.MessageBody = message.MessageBody.replace(/\n/g, "<br/>");
    }
  }

  fillListWithInternalFeedback() {
    this.internalFeedbackList = JSON.parse(this.internship.internalFeedback);
    this.convertMessageDT(this.internalFeedbackList);

    // Fix line breaks
    for(let message of this.internalFeedbackList) {
      message.MessageBody = message.MessageBody.replace(/\n/g, "<br/>");
    }
  }

  convertMessageDT(feedbackList: Message[]) {
    for (let message of feedbackList) {
      let day: string = message.MessageDT.substr(6, 2);
      let month: string = message.MessageDT.substr(4, 2);
      let year: string = message.MessageDT.substr(0,4);
      let hour: string = message.MessageDT.substr(8, 2);
      let minutes: string = message.MessageDT.substr(10, 2);

      message.MessageDT = `${day}-${month}-${year} (${hour}u${minutes})`;
    }
  }

  fillListWithContacts() {
    for (let contact of this.internship.company.contacts) {
      this.contactList.push(contact);
    }
  }

  // Mark as favourite for a specific student
  markThisInternshipAsFavourite() {
    let userFavourites: UserFavourites[] = this.internship.userFavourites;

    for(let item of userFavourites) {
      if (item.userId == this.stateManagerService.userId) {
        this.favoriteToggle = true;
      }
    }
  }

  fillReviewersList() {
    for (let reviewer of this.internship.internshipAssignedUser) {
      if (reviewer.user.role.code == 'REV') {
        this.reviewersList.push(reviewer.user);
      }
    }
  }

  splitResearchTopicDescriptions() {
    if (this.internship.totalInternsRequired == 2) {
      this.researchTopicDescriptions = this.internship.researchTopicDescription.split('~');
    }
  }

  fetchAppliedStudents(filterIndex: number) {
    this.showSpinner = true;
    this.appliedStudentsList = [];
    this.internshipService.getAppliedStudents(this.internship.internshipId).subscribe(data => {
      let allStudents: User[] = data;
      console.log('Lijst met de studenten die gesolliciteerd hebben voor deze stageopdracht is succesvol opgehaald.');

      switch(filterIndex) {
        case 1: // All students
          this.appliedStudentsList = allStudents;
          break;
        case 2: // Students with cv
          for (let student of allStudents) {
            if (student.cvPresent) {
              this.appliedStudentsList.push(student);
            }
          }
          break;
        case 3: // Students that can do the internship
          for (let student of allStudents) {
            let userInternship: UserInternships;
            this.internshipService.getUserInternshipByInternshipIdAndStudentId(Number(this.internship.internshipId), student.userId).subscribe(data => {
              userInternship = data;
              console.log(`UserInternship voor stageopdracht met id ${this.internship.internshipId} en student met id ${student.userId} is succesvol opgehaald.`);

              if (userInternship.hireRequested) {
                this.appliedStudentsList.push(student);
              }
            });
          }
          break;
        }

        this.getUserInternshipByInternshipIdAndStudentId();
    });
  }

  getUserInternshipByInternshipIdAndStudentId() {
    let internshipId: number = Number(this.internship.internshipId);
    let studentId: number = this.stateManagerService.userId;

    this.internshipService.getUserInternshipByInternshipIdAndStudentId(internshipId, studentId).subscribe(data => {
      if (data.hireRequested) {
        this.showFavouriteToggleButton = false;
      }

      this.showSpinner = false;
    })
  }

  changeFavoriteInternship() {
    this.showSpinner = true;

    let userFavourites = new UserFavourites(this.stateManagerService.userId);
    userFavourites.internshipId = Number(this.internship.internshipId);

    if (this.favoriteToggle) {
      this.internshipService.deleteFavourite(userFavourites).subscribe(() => {
        this.showSpinner = false;
        console.log('Markerking als favoriete stageopdracht is ongedaan gemaakt.');
      });
    } else {
      this.internshipService.setFavourite(userFavourites).subscribe(() => {
        this.showSpinner = false;
        console.log('Markerking als favoriete stageopdracht is succesvol gemaakt.')
      });
    }
  }

  showOriginalStatusDescription(): boolean {
    // No company logged in OR company logged in with internship in status APP, REJ or NEW (with no external feedback)
    return this.roleCodeLoggedInUser != 'COM' ||
      (this.roleCodeLoggedInUser == 'COM' && (this.internship.projectStatus.code == 'APP' || this.internship.projectStatus.code == 'REJ' ||
      (this.internship.projectStatus.code == 'NEW' && this.internship.externalFeedback == null)));
  }

  showExternalFeedback(): boolean {
    // Show only to coordinator and company (and when there is some external feedback)
    return this.internship.externalFeedback != null && (this.roleCodeLoggedInUser == 'COM' || this.roleCodeLoggedInUser == 'COO') ;
  }

  showInternalFeedback(): boolean {
    // Show only to coordinator and reviewer (and when there is some internal feedback)
    return this.internship.internalFeedback != null && (this.roleCodeLoggedInUser == 'REV' || this.roleCodeLoggedInUser == 'COO');
  }

  showReviewers(): boolean {
    // Show only to coordinator and reviewer (and when there are some reviewers assigned to the internship)
    return this.internship.internshipAssignedUser.length > 0 && this.internship.internshipAssignedUser[0].user.role.code == 'REV' &&
      (this.roleCodeLoggedInUser == 'COO' || this.roleCodeLoggedInUser == 'REV');
  }

  showCompanyDetails(): boolean {
    // Show only to coordinator, reviewer and students
    return this.roleCodeLoggedInUser != 'COM';
  }

  toggleAppliedStudentList() {
    this.studentListVisible = !this.studentListVisible;
    this.hireStudentModalPopUpIsVisible = false;
  }

  hireStudent($event: User) {
    this.studentToHire = $event;
    this.hireStudentModalPopUpIsVisible = true;
  }

  confirmHireStudent() { // In modal popup
    this.showSpinner = true;
    this.internshipService.setHireRequestedToTrue(Number(this.internship.internshipId), this.studentToHire.userId).subscribe(() => {
      this.hireStudentModalPopUpIsVisible = false;
      this.hireStudentSubmitted = true;

      let userInternshipsTemporaryList: UserInternships[] = [];
      for(let userInternship of this.internship.userInternships) {
        if (Number(this.internship.internshipId) == userInternship.internshipId && this.studentToHire.userId == userInternship.userId) {
          userInternship.hireRequested = true;
        }

        userInternshipsTemporaryList.push(userInternship);
      }

      this.internship.userInternships = userInternshipsTemporaryList;
      this.showSpinner = false;
      this.ngOnInit();
    });
  }

  sendReminderEmail(reviewerId: number, emailSentToIndex: number) {
    this.showSpinner = true;
    this.contactService.sendReminderEmail(reviewerId, Number(this.internship.internshipId)).subscribe(() => {
      console.log('Herinneringsmail is succesvol verstuurd');
      this.emailSentToIndex.push(emailSentToIndex);
      this.emailSent = true;
      this.showSpinner = false;
    });
  }

  cancelHireStudent() { // In modal popup
    this.hireStudentModalPopUpIsVisible = false;
  }

  toggleSpecialisationEditForm() {
    this.specialisationEditFormIsVisible = !this.specialisationEditFormIsVisible;
    this.hireStudentModalPopUpIsVisible = false;
    this.closeModalPopup.emit(true);
  }

  toggleComponyPromotorForm() {
    this.companyPromotorFormIsVisible = !this.companyPromotorFormIsVisible;
    this.hireStudentModalPopUpIsVisible = false;
  }

  closeCompanyPromotorComponent() {
    this.companyPromotorFormIsVisible = false;
    this.hireStudentModalPopUpIsVisible = false;
  }

  refreshPage() {
    this.specialisationEditFormIsVisible = false;
    this.hireStudentModalPopUpIsVisible = false;
    this.closeModalPopup.emit(true);
    this.ngOnInit();
  }

  filter(event: any) {
    this.stateManagerService.valueSelectedStudentOrderByCompany = Number(event.target.value);
    this.fetchAppliedStudents(Number(event.target.value));
  }

  showEnvelopeOrPaperPlane(index: number): boolean {
    for(let i = 0; i < this.emailSentToIndex.length; i++) {
      if (this.emailSentToIndex[i] == index) {
        return true;
      }
    }

    return false;
  }
}
