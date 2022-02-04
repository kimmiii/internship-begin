export class Register {
  userEmailAddress: string;
  userPass: string;
  confirmPassword: string;

  constructor(userEmailAddress: string, userPass: string, confirmPassword: string) {
    this.userEmailAddress = userEmailAddress;
    this.userPass = userPass;
    this.confirmPassword = confirmPassword;
  }
}
