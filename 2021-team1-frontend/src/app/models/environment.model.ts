export class Environment {
  environmentId: number;
  code: string;
  description: string;
  internshipEnvironment: string[];

  constructor(environmentId: number, code: string, description: string, internshipEnvironment: string[]) {
    this.environmentId = environmentId;
    this.code = code;
    this.description = description;
    this.internshipEnvironment = internshipEnvironment;
  }
}
