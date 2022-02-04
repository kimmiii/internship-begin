import {InternshipPeriod} from "./internship-period.model";
import {InternshipSpecialisation} from "./internship-specialisation.model";
import {InternshipEnvironment} from "./internship-environment.model";
import {InternshipExpectation} from "./internship-expectation.model";
import {UserFavourites} from "./user-favourites.model";
import {UserInternships} from "./user-internships.model";

export class FilteredInternshipModel {
  companyId: number;
  internshipEnvironmentOthers: string;
  assignmentDescription: string;
  internshipPeriod: InternshipPeriod[];
  internshipSpecialisation: InternshipSpecialisation[] = [];
  internshipEnvironment: InternshipEnvironment[] = [];
  internshipExpectation: InternshipExpectation[] = [];
  userFavourites: UserFavourites[] = [];
  userInternships: UserInternships[] = [];
  hideCompletedInternships: boolean;

  constructor(companyId: number, internshipEnvironmentOthers: string, assignmentDescription: string, internshipPeriod: InternshipPeriod[],
              internshipSpecialisation: InternshipSpecialisation[], internshipEnvironment: InternshipEnvironment[],
              internshipExpectation: InternshipExpectation[], userFavourites: UserFavourites[], userInternships: UserInternships[]) {
    this.companyId = companyId;
    this.internshipEnvironmentOthers = internshipEnvironmentOthers;
    this.assignmentDescription = assignmentDescription;
    this.internshipPeriod = internshipPeriod;
    this.internshipSpecialisation = internshipSpecialisation;
    this.internshipEnvironment = internshipEnvironment;
    this.internshipExpectation = internshipExpectation;
    this.userFavourites = userFavourites;
    this.userInternships = userInternships;
  }

}
