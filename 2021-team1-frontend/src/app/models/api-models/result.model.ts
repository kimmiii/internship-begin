export class Result {
  private status: number;
  private message: string;
  private data: object;

  constructor(status: number, message: string, data: object) {
    this.status = status;
    this.message = message;
    this.data = data;
  }
}
