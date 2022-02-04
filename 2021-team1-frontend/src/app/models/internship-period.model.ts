import {Internship} from "./internship.model";
import {Period} from "./period.model";

export class InternshipPeriod {
  internshipId: number;
  periodId: number;
  internship: Internship;
  period: Period;

  constructor(periodId: number) {
    this.periodId = periodId;
  }
}
