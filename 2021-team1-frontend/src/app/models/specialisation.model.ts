export class Specialisation {
  specialisationId: number;
  code: string;
  description: string;
  hyperlink: string;
  internshipSpecialisation: string[];

  constructor(specialisationId: number, code: string, description: string, internshipSpecialisation: string[]) {
    this.specialisationId = specialisationId;
    this.code = code;
    this.description = description;
    this.internshipSpecialisation = internshipSpecialisation;
  }
}
