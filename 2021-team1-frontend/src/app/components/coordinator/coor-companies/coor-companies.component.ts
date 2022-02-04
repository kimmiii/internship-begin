import { Component, OnInit } from '@angular/core';
import {Title} from "@angular/platform-browser";
import {Company} from "../../../models/company.model";
import {FormBuilder} from "@angular/forms";
import {CompanyService} from "../../../services/company.service";
import {forkJoin} from "rxjs";

@Component({
  selector: 'app-coor-companies',
  templateUrl: './coor-companies.component.html',
  styleUrls: ['./coor-companies.component.css']
})
export class CoorCompaniesComponent implements OnInit {
  isAllDataFetched: boolean = false;
  newCompanyList: Company[] = [];
  activeCompayList: Company[] = [];
  inactiveCompanyList: Company[] = [];
  newCompanyDetailsIsVisible: boolean = false;
  activeCompanyDetailsIsVisible: boolean = false;
  inactiveCompanyDetailsIsVisible: boolean = false;
  companyRejectIsVisible: boolean = false;
  activeCompanyRejectIsVisible: boolean = false;
  selectedCompany: Company;
  selectedNewCompany: Company;
  selectedActiveCompany: Company;
  selectedInactiveCompany: Company;
  modalPopupNewCompanyIsVisible: boolean = false;
  modalPopupInactiveCompanyIsVisible: boolean = false;
  activatedSuccessfully: boolean = false;
  rejectedSuccessfully: boolean = false;
  showSpinnerBool: boolean = true;
  counters: number[] = [];

  constructor(private companyService: CompanyService, private titleService: Title, private formBuilder: FormBuilder) { }

  ngOnInit() {
    this.fetchData();
    this.titleService.setTitle('Stagebeheer | Bedrijven');
  }

  fetchData() {
    this.showSpinnerBool = true;
    let getNewCompaniesObservable = this.companyService.getNewCompanies();
    let getActiveCompaniesObservable = this.companyService.getActiveCompanies();
    let getRejectedCompaniesObservable = this.companyService.getRejectedCompanies();

    forkJoin([getNewCompaniesObservable, getActiveCompaniesObservable, getRejectedCompaniesObservable]).subscribe(results => {
      this.newCompanyList = results[0];
      this.activeCompayList = results[1];
      this.inactiveCompanyList = results[2];
      console.log('Bedrijfslijsten succesvol opgehaald.');

      this.isAllDataFetched = true;
      this.showSpinnerBool = false;
    });
  }

  showNewCompanyDetails($event: Company) {
    this.showSpinnerBool = true;
    this.selectedNewCompany = $event;
    this.newCompanyDetailsIsVisible = true;
    this.modalPopupNewCompanyIsVisible = false;
    this.modalPopupInactiveCompanyIsVisible = false;
    this.companyRejectIsVisible = false;
    this.showSpinnerBool = false;
  }

  showActiveCompanyDetails($event: Company) {
    this.showSpinnerBool = true;
    this.fetchCounters($event.companyId);
    this.selectedActiveCompany = $event;
    this.activeCompanyDetailsIsVisible = true;
    this.modalPopupNewCompanyIsVisible = false;
    this.modalPopupInactiveCompanyIsVisible = false;
    this.companyRejectIsVisible = false;
    this.activeCompanyRejectIsVisible = false;
    this.showSpinnerBool = false;
  }

  showInactiveCompanyDetails($event: Company) {
    this.showSpinnerBool = true;
    this.selectedInactiveCompany = $event;
    this.inactiveCompanyDetailsIsVisible = true;
    this.modalPopupNewCompanyIsVisible = false;
    this.modalPopupInactiveCompanyIsVisible = false;
    this.showSpinnerBool = false;
  }

  approveNewCompany($event: Company) {
    this.showSpinnerBool = true;
    this.selectedNewCompany = $event;
    this.modalPopupNewCompanyIsVisible = true;
    this.modalPopupInactiveCompanyIsVisible = false;
    this.companyRejectIsVisible = false;
    this.activeCompanyRejectIsVisible = false;
    this.showSpinnerBool = false;
  }

  approveInactiveCompany($event: Company) {
    this.showSpinnerBool = true;
    this.selectedInactiveCompany = $event;
    this.modalPopupInactiveCompanyIsVisible = true;
    this.modalPopupNewCompanyIsVisible = false;
    this.showSpinnerBool = false;
  }

  rejectNewCompany($event: Company) {
    this.showSpinnerBool = true;
    this.selectedNewCompany = $event;
    this.companyRejectIsVisible = true;
    this.newCompanyDetailsIsVisible = false;
    this.activeCompanyRejectIsVisible = false;
    this.modalPopupNewCompanyIsVisible = false;
    this.modalPopupInactiveCompanyIsVisible = false;
    this.showSpinnerBool = false;
  }

  rejectActiveCompany($event: Company) {
    this.showSpinnerBool = true;
    this.selectedActiveCompany = $event;
    this.companyRejectIsVisible = false;
    this.activeCompanyDetailsIsVisible = false;
    this.modalPopupNewCompanyIsVisible = false;
    this.modalPopupInactiveCompanyIsVisible = false;
    this.activeCompanyRejectIsVisible = true;
    this.showSpinnerBool = false;
  }

  hideRejectComponent() {
    this.companyRejectIsVisible = false;
  }

  hideRejectActiveComponent() {
    this.activeCompanyRejectIsVisible = false;
  }

  confirmConfirmationNewCompany() {
    this.showSpinnerBool = true;
    this.companyService.approveCompany(this.selectedNewCompany.companyId).subscribe(res => {
      if (res.error != null) {
        window.alert('Het is niet mogelijk om dit bedrijf goed te keuren');
        this.showSpinnerBool = false;
      } else {
        console.log(`Bedrijf met id ${this.selectedNewCompany.companyId} succesvol geactiveerd.`);
        this.refreshAfterConfimration();
      }
    });
  }

  confirmConfirmationInactiveCompany() {
    this.showSpinnerBool = true;
    this.companyService.approveCompany(this.selectedInactiveCompany.companyId).subscribe(res => {
      if (res.error != null) {
        window.alert('Het is niet mogelijk om dit bedrijf goed te keuren');
        this.showSpinnerBool = false;
      } else {
        console.log(`Bedrijf met id ${this.selectedInactiveCompany.companyId} succesvol geactiveerd.`);
        this.refreshAfterConfimration();
      }
    });
  }

  refreshAfterConfimration() {
    this.resetFlags();
    this.fetchData();

    this.selectedCompany = this.selectedInactiveCompany;
  }

  resetFlags() {
    this.newCompanyDetailsIsVisible = false;
    this.modalPopupNewCompanyIsVisible = false;
    this.modalPopupInactiveCompanyIsVisible = false;
    this.companyRejectIsVisible = false;
  }

  cancelConfirmationNewCompany() {
    this.modalPopupNewCompanyIsVisible = false;
    this.modalPopupInactiveCompanyIsVisible = false;
  }

  showSpinner() {
    this.showSpinnerBool = true;
  }

  refreshPage(selectedCompany: Company) {
    this.showSpinnerBool = true;
    this.companyRejectIsVisible = false;
    this.activeCompanyRejectIsVisible = false;
    setTimeout(() => this.fetchData(), 500);

    this.selectedCompany = selectedCompany;
    this.rejectedSuccessfully = true;
    this.hideConfirmationPopup();
    this.showSpinnerBool = false;
  }

  fetchCounters(companyId: number) {
    this.showSpinnerBool = true;
    this.companyService.getCounters(companyId).subscribe(data => {
      this.counters = data;
      this.showSpinnerBool = false;
    });

    /*
 * Positions from each counter in List counters
 * 0 => count total internships
 * 1 => count total approved internships
 * 2 => count approved internships sem 1
 * 3 => count approved internships sem 2
 * 4 => count total approved internships for specialisation EICT
 * 5 => count total trainees
 * 6 => count total EICT trainees
 */
  }

  hideConfirmationPopup() {
    setTimeout(() => { this.activatedSuccessfully = false }, 5000);
    setTimeout(() => { this.rejectedSuccessfully = false }, 5000);
  }
}
