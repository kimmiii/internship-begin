import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {Company} from "../../../models/company.model";

@Component({
  selector: 'app-coor-company-item',
  templateUrl: './coor-company-item-inactive.component.html',
  styleUrls: ['./coor-company-item-inactive.component.css']
})
export class CoorCompanyItemInactiveComponent implements OnInit {
  @Input() company: Company;
  @Output() showNewCompanyDetails: EventEmitter<Company> = new EventEmitter();
  @Output() approveCompany: EventEmitter<Company> = new EventEmitter();
  @Output() rejectCompany: EventEmitter<Company> = new EventEmitter();

  constructor() { }

  ngOnInit() {
  }

  showDetails() {
    this.showNewCompanyDetails.emit(this.company);
  }

  approve() {
    this.approveCompany.emit(this.company);
  }

  reject() {
    this.rejectCompany.emit(this.company);
  }

}
