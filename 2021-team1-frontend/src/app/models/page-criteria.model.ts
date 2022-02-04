export class PageCriteriaModel {
  internshipsPerPage: number;
  pageNumber: number;

  constructor(internshipsPerPage: number, pageNumber: number) {
    this.internshipsPerPage = internshipsPerPage;
    this.pageNumber = pageNumber;
  }
}
