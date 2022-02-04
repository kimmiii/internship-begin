import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {Company} from "../../../models/company.model";

@Component({
  selector: 'app-coor-company-item-inactive-evaluated',
  templateUrl: './coor-company-item-inactive-evaluated.component.html',
  styleUrls: ['./coor-company-item-inactive-evaluated.component.css']
})
export class CoorCompanyItemInactiveEvaluatedComponent implements OnInit {
  @Input() company: Company;
  @Output() showInactiveCompanyDetails: EventEmitter<Company> = new EventEmitter();
  @Output() approveCompany: EventEmitter<Company> = new EventEmitter();

  constructor() { }

  ngOnInit() {
  }

  showDetails() {
    this.showInactiveCompanyDetails.emit(this.company);
  }

  approve() {
    this.approveCompany.emit(this.company);
  }
}
