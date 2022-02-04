export class Technology {
    environmentId: number;
    code: string;
    description: string;
  
    constructor(environmentId: number, code: string, description: string) {
      this.environmentId = environmentId;
      this.code = code;
      this.description = description;
    }
  }
  