import {User} from "./user.model";
import {Internship} from "./internship.model";

export class InternshipAssignedUser {
  internshipId: number;
  userId: number;
  internship: Internship;
  user: User;

  constructor(userId: number) {
    this.userId = userId;
  }
}
