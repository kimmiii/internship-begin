export class Period {
  periodId: number;
  code: string;
  description: string;
  internshipPeriod: string[];

  constructor(periodId: number, code: string, description: string, internshipPeriod: string[]) {
    this.periodId = periodId;
    this.code = code;
    this.description = description;
    this.internshipPeriod = internshipPeriod;
  }
}
