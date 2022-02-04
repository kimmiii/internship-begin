import {Component, Input, OnInit} from '@angular/core';
import {Company} from "../../../models/company.model";

@Component({
  selector: 'app-coor-company-details',
  templateUrl: './coor-company-details.component.html',
  styleUrls: ['./coor-company-details.component.css']
})
export class CoorCompanyDetailsComponent implements OnInit {
  @Input() company: Company;
  @Input() counters;

  constructor() { }

  ngOnInit() {
  }
}
