export class Message {
  MessageDT: string;
  MessageBody: string;
  UserFrom: number;
  UserFromName: string;
  UserTo: number;
  UserToName: string;

  constructor(messageDT: string, messageBody: string, userFrom: number, userFromName: string, userTo: number, userToName: string) {
    this.MessageDT = messageDT;
    this.MessageBody = messageBody;
    this.UserFrom = userFrom;
    this.UserFromName = userFromName;
    this.UserTo = userTo;
    this.UserToName = userToName;
  }
}
