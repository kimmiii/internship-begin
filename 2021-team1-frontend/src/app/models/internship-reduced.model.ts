import {ProjectStatus} from "./project-status.model";
import {InternshipAssignedUser} from "./internship-assigned-user.model";

export class InternshipReduced {
  internshipId: number;
  projectStatus: ProjectStatus;
  contactPersonId: number;
  externalFeedback: string;
  internalFeedback: string;
  InternshipAssignedUser: InternshipAssignedUser[] = [];

  constructor(internshipId: number, projectStatus: ProjectStatus, contactPersonId: number) {
    this.internshipId = internshipId;
    this.projectStatus = projectStatus;
    this.contactPersonId = contactPersonId;
  }
}
