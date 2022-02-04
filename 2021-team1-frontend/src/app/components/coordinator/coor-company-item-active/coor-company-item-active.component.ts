import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {Company} from "../../../models/company.model";

@Component({
  selector: 'app-coor-company-item-active',
  templateUrl: './coor-company-item-active.component.html',
  styleUrls: ['./coor-company-item-active.component.css']
})
export class CoorCompanyItemActiveComponent implements OnInit {
  @Input() company: Company;
  @Output() showCompanyDetails: EventEmitter<Company> = new EventEmitter();
  @Output() rejectActiveCompany: EventEmitter<Company> = new EventEmitter();

  constructor() { }

  ngOnInit() {
  }

  showDetails() {
    this.showCompanyDetails.emit(this.company);
  }

  reject() {
    this.rejectActiveCompany.emit(this.company);
  }

}
