import {Contact} from "./contact.model";

export class Company {
  companyId: number;
  name: string;
  street: string;
  houseNr: string;
  busNr: string;
  zipCode: string;
  city: string;
  country: string;
  vatNumber: string;
  email: string;
  phoneNumber: string;
  totalEmployees: number;
  totalITEmployees: number;
  totalITEmployeesActive: number;
  activated: boolean;
  userId: number;
  evaluatedAt: Date;
  evaluationFeedback: string;
  contacts: Contact[];

  constructor(name: string, street: string, houseNr: string, busNr: string, zipCode: string,
              city: string, country: string, vatNumber: string, email: string, phoneNumber: string,
              totalEmployees: number, totalITEmployees: number, totalITEmployeesActive: number, userId: number,
              evaluatedAt?: Date, evaluationFeedback?: string) {
    this.name = name;
    this.street = street;
    this.street = street;
    this.houseNr = houseNr;
    this.busNr = busNr;
    this.zipCode = zipCode;
    this.city = city;
    this.country = country;
    this.vatNumber = vatNumber;
    this.email = email;
    this.phoneNumber = phoneNumber;
    this.totalEmployees = totalEmployees;
    this.totalITEmployees = totalITEmployees;
    this.totalITEmployeesActive = totalITEmployeesActive;
    this.userId = userId;
    this.evaluatedAt = evaluatedAt;
    this.evaluationFeedback = evaluationFeedback;
  }
}
