import {Internship} from "./internship.model";
import {User} from "./user.model";

export class UserInternships {
  internshipId: number;
  userId: number;
  hireRequested: boolean;
  hireConfirmed: boolean;
  hireApproved: boolean;
  internship: Internship;
  user: User;
  evaluatedAt: Date;
  rejectionFeedback: string;
  interesting: boolean;

  constructor(userId: number) {
    this.userId = userId;
  }
}
