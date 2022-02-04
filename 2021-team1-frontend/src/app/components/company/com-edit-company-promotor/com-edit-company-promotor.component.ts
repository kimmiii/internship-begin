import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {Internship} from "../../../models/internship.model";
import {InternshipService} from "../../../services/internship.service";
import {Router} from "@angular/router";

@Component({
  selector: 'app-com-edit-company-promotor',
  templateUrl: './com-edit-company-promotor.component.html',
  styleUrls: ['./com-edit-company-promotor.component.css']
})
export class ComEditCompanyPromotorComponent implements OnInit {
  @Input() internship: Internship;
  @Output() companyPromotorEdited: EventEmitter<any> = new EventEmitter();
  editCompanyPromotorForm: FormGroup;
  btnSendClicked: boolean = false;
  submitted: boolean = false;
  showSpinner: boolean = true;

  constructor(private formBuilder: FormBuilder, private internshipService: InternshipService,
              private router: Router) { }

  ngOnInit() {
    this.showSpinner = true;
    this.createForm();
  }

  createForm() {
    this.editCompanyPromotorForm = this.formBuilder.group({
      promotorfirstname: [this.internship.promotorFirstname, [Validators.required, Validators.maxLength(50)]],
      promotorSurname: [this.internship.promotorSurname, [Validators.required, Validators.maxLength(50)]],
      promotorFunction: [this.internship.promotorFunction, [Validators.required, Validators.minLength(2), Validators.maxLength(100)]],
      promotorEmail: [this.internship.promotorEmail, [Validators.required, Validators.email, Validators.minLength(3), Validators.maxLength(64)]]
    });

    console.log(`EditCompanyPromotorForm succesvol aangemaakt.`);
    this.showSpinner = false;
  }

  editPromotor() {
    this.showSpinner = true;
    this.btnSendClicked = true;
    this.modifyInternship();

    this.internshipService.editInternship(this.internship.internshipId, this.internship).subscribe(res => {
      if (res.error != null) {
        window.alert('Het is niet mogelijk om deze stageaanvraag te wijzigen. U wordt naar de startpagina geleid.');
        this.router.navigateByUrl('/company/internships');
      } else {
        this.btnSendClicked = false;
        this.companyPromotorEdited.emit();
        this.editCompanyPromotorForm.reset();
        this.showSpinner = false;
      }
    });

    this.submitted = true;
  }

  modifyInternship() {
    this.internship.promotorFirstname = this.promotorfirstname.value;
    this.internship.promotorSurname = this.promotorSurname.value;
    this.internship.promotorFunction = this.promotorFunction.value;
    this.internship.promotorEmail = this.promotorEmail.value;

    console.log(`Internship is succesvol gewijzigd naar: `, this.internship);
  }

  // Getters EditCompanyPromotorForm
  get promotorfirstname() {
    return this.editCompanyPromotorForm.controls.promotorfirstname;
  }

  get promotorSurname() {
    return this.editCompanyPromotorForm.controls.promotorSurname;
  }

  get promotorFunction() {
    return this.editCompanyPromotorForm.controls.promotorFunction;
  }

  get promotorEmail() {
    return this.editCompanyPromotorForm.controls.promotorEmail;
  }
}
