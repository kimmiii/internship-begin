import {Internship} from "./internship.model";
import {Expecation} from "./expectation.model";

export class InternshipExpectation {
  internshipId: number;
  expectationId: number;
  internship: Internship;
  expectation: Expecation;

  constructor(expectationId: number) {
    this.expectationId = expectationId;
  }
}
