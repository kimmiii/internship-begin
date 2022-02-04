import {Internship} from "./internship.model";
import {Environment} from "./environment.model";

export class InternshipEnvironment {
  internshipId: number;
  environmentId: number;
  internship: Internship;
  environment: Environment;

  constructor(environmentId: number) {
    this.environmentId = environmentId;
  }
}
