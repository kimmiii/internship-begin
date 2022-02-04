import {PageCriteriaModel} from "../page-criteria.model";
import {FilteredInternshipModel} from "../filtered-internship.model";

export class GetApprovedInternshipCriteriaModel {
  pageCriteria: PageCriteriaModel;
  filterCriteria: FilteredInternshipModel;

  constructor(pageCriteria: PageCriteriaModel, filterCriteria: FilteredInternshipModel) {
    this.pageCriteria = pageCriteria;
    this.filterCriteria = filterCriteria;
  }
}
