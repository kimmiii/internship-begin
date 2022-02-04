export class Expecation {
  expectationId: number;
  code: string;
  description: string;
  internshipExpectation: string[];

  constructor(expectationId: number, code: string, description: string, internshipExpectation: string[]) {
    this.expectationId = expectationId;
    this.code = code;
    this.description = description;
    this.internshipExpectation = internshipExpectation;
  }
}
