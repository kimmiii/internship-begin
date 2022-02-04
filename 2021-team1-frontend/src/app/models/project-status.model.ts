export class ProjectStatus {
  projectStatusId: number;
  code: string;
  description: string;

  constructor(code: string) {
    this.code = code;
  }
}
