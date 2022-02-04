import {Internship} from "./internship.model";

export class Contact {
  contactId: number;
  companyId: number;
  surname: string;
  firstname: string;
  email: string;
  phoneNumber: string;
  function: string;
  isPromotor: boolean;
  internships: Internship[];
  activated: boolean;

  constructor(companyId: number, firstName: string, surname: string, email: string,
              phoneNumber: string, contactFunction: string) {
    this.companyId = companyId;
    this.firstname = firstName;
    this.surname = surname;
    this.email = email;
    this.phoneNumber = phoneNumber;
    this.function = contactFunction;
  }
}
