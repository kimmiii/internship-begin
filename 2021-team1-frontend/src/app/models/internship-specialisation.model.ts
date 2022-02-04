import {Internship} from "./internship.model";
import {Specialisation} from "./specialisation.model";

export class InternshipSpecialisation {
  internshipId: number;
  specialisationId: number;
  internship: Internship;
  specialisation: Specialisation;

  constructor(specialisationId: number) {
    this.specialisationId = specialisationId;
  }
}
