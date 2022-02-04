import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {Internship} from "../../../models/internship.model";
import {InternshipSpecialisation} from "../../../models/internship-specialisation.model";
import {InternshipEnvironment} from "../../../models/internship-environment.model";
import {UserFavourites} from "../../../models/user-favourites.model";
import {StateManagerService} from "../../../services/state-manager.service";
import {InternshipService} from "../../../services/internship.service";
import {UserInternships} from "../../../models/user-internships.model";

@Component({
  selector: 'app-stu-internship-item',
  templateUrl: './stu-internship-item.component.html',
  styleUrls: ['./stu-internship-item.component.css']
})
export class StuInternshipItemComponent implements OnInit {
  @Input() internship: Internship;
  @Output() internshipItemLoaded: EventEmitter<boolean> = new EventEmitter<boolean>();
  specialisationList: InternshipSpecialisation[];
  specialisationString: string = '';
  environmentList: InternshipEnvironment[];
  environmentString: string = '';
  isFavourite: boolean = false;
  applied: boolean = false;
  myInternshipId: string;
  allDataFetched: boolean = false;

  constructor(private stateManagerService: StateManagerService, private internshipService: InternshipService) { }

  ngOnInit() {
    this.specialisationList = this.internship.internshipSpecialisation;
    this.environmentList = this.internship.internshipEnvironment;

    this.fetchData();

    this.parseSpecialisationString();
    this.parseEnvironmentString();
    this.markFavourite();
  }

  fetchData() {
    let studentId: number = Number(this.stateManagerService.userId);
    let studentIsAlreadyAssignedToInternship: boolean = false;

    this.internshipService.studentApprovedHireRequest(studentId).subscribe(data => {
      studentIsAlreadyAssignedToInternship = data;
      console.log(`Approved hire request is succesvol opgehaald voor student met id ${studentId}.`);

      if (studentIsAlreadyAssignedToInternship) {
        this.internshipService.getInternshipIdByInternshipAssignedUser(studentId).subscribe(data => {
          console.log(`InternshipId is succesvol opgehaald.`);
          this.myInternshipId = String(data);
          //this.allDataFetched = true;
        });
      } else {
        //this.allDataFetched = true;
      }

      this.markApplied();
    });
  }

  parseSpecialisationString() {
    for (let i = 0; i < this.specialisationList.length; i++) {
      let specialisation: InternshipSpecialisation = this.specialisationList[i];

      this.specialisationString += specialisation.specialisation.description;
      if (i != this.specialisationList.length - 1) {
        this.specialisationString += ', ';
      }
    }
  }

  parseEnvironmentString() {
    for (let i = 0; i < this.environmentList.length; i++) {
      let environment: InternshipEnvironment = this.environmentList[i];

      if (environment.environment.code != 'AND') {
        this.environmentString += environment.environment.description;
        if (i != this.environmentList.length - 1 && this.environmentList[i+1].environment.code != 'AND') {
            this.environmentString += ', ';
        }
      }
    }
  }

  markFavourite() {
    let userFavourites: UserFavourites[] = this.internship.userFavourites;

    for(let item of userFavourites) {
      if (item.userId == this.stateManagerService.userId) {
        this.isFavourite = true;
      }
    }
  }

  markApplied() {
    let userIntership: UserInternships;
    this.internshipService.getUserInternshipByInternshipIdAndStudentId(Number(this.internship.internshipId), this.stateManagerService.userId).subscribe(data => {
      userIntership = data;

      // If the combination for this internship and logged in student exists, ...
      if (userIntership.internshipId != 0 && userIntership.userId != 0) {
        this.applied = true;
      }

      this.allDataFetched = true;
      this.internshipItemLoaded.emit();
    });
  }
}
