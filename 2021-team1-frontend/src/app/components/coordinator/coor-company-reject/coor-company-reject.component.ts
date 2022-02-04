import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {Company} from "../../../models/company.model";
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {CompanyService} from "../../../services/company.service";

@Component({
  selector: 'app-coor-company-reject',
  templateUrl: './coor-company-reject.component.html',
  styleUrls: ['./coor-company-reject.component.css']
})
export class CoorCompanyRejectComponent implements OnInit {
  @Input() company: Company;
  rejectForm: FormGroup;
  btnSubmitClicked: boolean = false;
  @Output() cancelClicked: EventEmitter<any> = new EventEmitter();
  @Output() companyRejected: EventEmitter<any> = new EventEmitter();
  @Output() showSpinner: EventEmitter<any> = new EventEmitter();

  constructor(private formBuilder: FormBuilder, private companyService: CompanyService) { }

  ngOnInit() {
    this.createForm();
  }

  createForm() {
    this.rejectForm = this.formBuilder.group({
      feedback: [null, [Validators.maxLength(1000)]]
    });
  }

  submitRejectForm() {
   this.btnSubmitClicked = true;

   if (this.rejectForm.valid) {
     this.showSpinner.emit();

     let evaluationFeedback: string = "";
     if (this.feedback.value != null) {
       evaluationFeedback = this.feedback.value;
     }

     this.companyService.rejectCompany(this.company.companyId, evaluationFeedback).subscribe(res => {
       if (res.error != null) {
         window.alert('Dit bedrijf kan niet afgekeurd worden.');
       } else {
         console.log(`Bedrijf met id ${this.company.companyId} is succesvol geweigerd.`);
         this.companyRejected.emit();
       }
     });
   }
  }

  cancel() {
    this.cancelClicked.emit();
  }

  // GETTERS for rejectForm
  get feedback() {
    return this.rejectForm.controls.feedback;
  }
}
