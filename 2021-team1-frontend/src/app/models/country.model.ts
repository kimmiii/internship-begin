export class Country {
  countryId: number;
  code: string;
  name: string;

  constructor(countryId: number, code: string, name: string) {
    this.countryId = countryId;
    this.code = code;
    this.name = name;
  }
}
