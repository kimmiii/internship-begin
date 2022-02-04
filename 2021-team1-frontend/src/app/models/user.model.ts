import {Internship} from "./internship.model";
import {Role} from "./role.model";
import {UserInternships} from "./user-internships.model";
import {Company} from "./company.model";

export class User {
  userId: number;
  userFirstName: string;
  userSurname: string;
  userPass: string;
  salt: string;
  userEmailAddress: string;
  registrationDate: Date;
  activated: boolean;
  internships: Internship[];
  role: Role;
  roleId: number;
  cvPresent: boolean;
  countApplications: number;
  UserInternships: UserInternships[];
  company: Company;

  constructor(userId: number, userPass: string, salt: string, userEmailAddress: string,
              registrationDate: Date, activated: boolean, internships: Internship[],
              role: Role, roleId: number) {
    this.userId = userId;
    this.userPass = userPass;
    this.salt = salt;
    this.userEmailAddress = userEmailAddress;
    this.registrationDate = registrationDate;
    this.activated = activated;
    this.internships = internships;
    this.role = role;
    this.roleId = roleId;
  }
}
