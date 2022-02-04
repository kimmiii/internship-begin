import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {InternshipService} from "../../../services/internship.service";
import {Specialisation} from "../../../models/specialisation.model";
import {Internship} from "../../../models/internship.model";
import {CustomValidators} from "../../../validators/custom-validators";
import {InternshipSpecialisation} from "../../../models/internship-specialisation.model";
import {Router} from "@angular/router";

@Component({
  selector: 'app-specialisation-edit-form',
  templateUrl: './specialisation-edit-form.component.html',
  styleUrls: ['./specialisation-edit-form.component.css']
})
export class SpecialisationEditFormComponent implements OnInit {
  @Input() internship: Internship;
  @Output() specialisationEdited: EventEmitter<any> = new EventEmitter();
  allDataIsFetched: boolean = false;
  specialisationEditForm: FormGroup;
  specialisationList: Specialisation[];
  checkedSpecialisationsList: boolean[];
  btnSendClicked: boolean = false;
  formSubmitted: boolean = false;

  constructor(private internshipService: InternshipService, private formBuilder: FormBuilder,
              private router: Router) { }

  ngOnInit() {
    this.fetchData();
  }

  fetchData() {
    this.internshipService.getSpecialisations().subscribe(data => {
      console.log('Afstudeerrichtingen zijn succesvol opgehaald.');
      this.specialisationList = data;
      this.checkedSpecialisationsList = this.initializeChecklist();
      this.createForm();
    });
  }

  initializeChecklist(): Array<boolean> {
    let checkedCheckboxesList: Array<boolean> = [];
    for (let index = 0; index < this.specialisationList.length; index++) {
      checkedCheckboxesList[index] = false;

      this.internship.internshipSpecialisation.forEach(spec => {
        if (spec.specialisationId == this.specialisationList[index].specialisationId) checkedCheckboxesList[index] = true;
      });

    }
    return checkedCheckboxesList;
  }

  createForm() {
    this.specialisationEditForm = this.formBuilder.group({
        specialisations: this.formBuilder.array(
          this.specialisationList.map(() => this.formBuilder.control('')),
          [CustomValidators.multipleCheckboxRequireOne]
        )
      });
    this.allDataIsFetched = true;
  }

  changeSpecialisation() {
    this.allDataIsFetched = false;
    this.btnSendClicked = true;

    if (this.specialisationEditForm.valid) {
      console.log("Formulier om de afstudeerrichting te wijzigen is geldig");
      this.formSubmitted = true;
      let internshipRequest: Internship = this.createInternshipObject();

      this.internshipService.editInternship(this.internship.internshipId, internshipRequest).subscribe(res => {
        if (res.error != null) {
          window.alert('Het is niet mogelijk om deze stageaanvraag te wijzigen. U wordt naar de startpagina geleid.');
          this.router.navigateByUrl('/coordinator/internships');
        } else {
          this.btnSendClicked = false;
          this.specialisationEditForm.reset();
          this.specialisationEdited.emit();
          this.allDataIsFetched = true;
        }
      });
    }
  }

  createInternshipObject(): Internship {
    let internshipRequest: Internship = this.internship;
    let internshipSpecialisations: InternshipSpecialisation[] = [];

    for(let i = 0; i < this.checkedSpecialisationsList.length; i++) {
      if (this.checkedSpecialisationsList[i] == true) {
        let internshipSpecialisation: InternshipSpecialisation = new InternshipSpecialisation(this.specialisationList[i].specialisationId);
        internshipSpecialisation.specialisation = this.specialisationList[i];

        internshipSpecialisations.push(internshipSpecialisation);
      }
    }

    internshipRequest.internshipSpecialisation = internshipSpecialisations;

    return internshipRequest;
  }

  // GETTERS for specialisationsForm
  get specialisations() {
    return this.specialisationEditForm.controls.specialisations;
  }
}
