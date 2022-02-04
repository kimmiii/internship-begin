import { Company } from './company.model';
import { Contact } from './contact.model';
import { InternshipAssignedUser } from './internship-assigned-user.model';
import { InternshipEnvironment } from './internship-environment.model';
import { InternshipExpectation } from './internship-expectation.model';
import { InternshipPeriod } from './internship-period.model';
import { InternshipSpecialisation } from './internship-specialisation.model';
import { ProjectStatus } from './project-status.model';
import { UserFavourites } from './user-favourites.model';
import { UserInternships } from './user-internships.model';

export class Internship {
  internshipId: string;
  company: Company;
  companyId: number;
  companyName: string;
  contact: Contact;
  contactPersonId: number;
  contactPersonName: string;
  promotorFirstname: string;
  promotorSurname: string;
  promotorFunction: string;
  promotorEmail: string;
  projectStatus: ProjectStatus;
  projectStatusId: number;
  wpStreet: string;
  wpHouseNr: string;
  wpBusNr: string;
  wpZipCode: string;
  wpCity: string;
  wpCountry: string;
  internshipEnvironmentOthers: string;
  assignmentDescription: string;
  technicalDetails: string;
  conditions: string;
  totalInternsRequired: number;
  contactStudentName: string;
  remark: string;
  researchTopicTitle: string;
  researchTopicDescription: string;
  externalFeedback: string;
  internalFeedback: string;
  createdAt: Date;
  countTotalAssignedReviewers: number;
  sentToReviewersAt: Date;
  completed: boolean;
  academicYear: string;
  internshipPeriod: InternshipPeriod[];
  internshipSpecialisation: InternshipSpecialisation[];
  internshipEnvironment: InternshipEnvironment[];
  internshipExpectation: InternshipExpectation[];
  internshipAssignedUser: InternshipAssignedUser[];
  userFavourites: UserFavourites[];
  userInternships: UserInternships[];
  showInEvent: boolean;

  constructor(companyId: number, contactPersonId: number, promotorFirstname: string, promotorSurname: string, promotorFunction: string,
              promotorEmail: string, projectStatusId: number, wpStreet: string,
              wpHouseNr: string, wpBusNr: string, wpZipCode: string, wpCity: string, wpCountry: string, internshipEnvironmentOthers: string,
              assignmentDescription: string, technicalDetails: string, conditions: string, totalInternsRequired: number,
              contactStudentName: string, remark: string, researchTopicTitle: string, researchTopicDescription: string,
              internshipPeriod: InternshipPeriod[], internshipSpecialisation: InternshipSpecialisation[],
              internshipEnvironment: InternshipEnvironment[], internshipExpectation: InternshipExpectation[], internshipId?: string) {
    this.companyId = companyId;
    this.contactPersonId = contactPersonId;
    this.promotorFirstname = promotorFirstname;
    this.promotorSurname = promotorSurname;
    this.promotorFunction = promotorFunction;
    this.promotorEmail = promotorEmail;
    this.projectStatusId = projectStatusId;
    this.wpStreet = wpStreet;
    this.wpHouseNr = wpHouseNr;
    this.wpBusNr = wpBusNr;
    this.wpZipCode = wpZipCode;
    this.wpCity = wpCity;
    this.wpCountry = wpCountry;
    this.internshipEnvironmentOthers = internshipEnvironmentOthers;
    this.assignmentDescription = assignmentDescription;
    this.technicalDetails = technicalDetails;
    this.conditions = conditions;
    this.totalInternsRequired = totalInternsRequired;
    this.contactStudentName = contactStudentName;
    this.remark = remark;
    this.researchTopicTitle = researchTopicTitle;
    this.researchTopicDescription = researchTopicDescription;
    this.internshipPeriod = internshipPeriod;
    this.internshipSpecialisation = internshipSpecialisation;
    this.internshipEnvironment = internshipEnvironment;
    this.internshipExpectation = internshipExpectation;
    this.internshipId = internshipId;
    this.showInEvent = false;
  }
}
