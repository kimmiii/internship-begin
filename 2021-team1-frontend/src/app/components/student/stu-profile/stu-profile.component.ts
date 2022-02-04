import { Component, OnInit } from '@angular/core';
import {Title} from "@angular/platform-browser";
import {StateManagerService} from "../../../services/state-manager.service";
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {AccountService} from "../../../services/account.service";

@Component({
  selector: 'app-stu-profile',
  templateUrl: './stu-profile.component.html',
  styleUrls: ['./stu-profile.component.css']
})
export class StuProfileComponent implements OnInit {
  showSpinnerBool: boolean = true;
  firstname: string;
  surname: string;
  emailAddress: string;
  cvForm: FormGroup;
  file: File;
  sendPDFClicked: boolean = false;
  wrongFile: boolean = false;
  pDFFileSent: boolean = false;
  responseString: string = '';
  errorList: string[];
  userHasCv: boolean = false;
  deleteSuccessful: boolean = false;

  constructor(private titleService: Title, private stateManagerService: StateManagerService,
              private formBuilder: FormBuilder, private accountService: AccountService) { }

  ngOnInit() {
    this.showSpinnerBool = true;

    this.titleService.setTitle('Stagebeheer | Profiel');
    this.firstname = this.stateManagerService.userFirstName;
    this.surname = this.stateManagerService.userSurname;
    this.emailAddress = this.stateManagerService.userEmailAddress;
    this.userHasCv = this.stateManagerService.cvPresent;


    this.createForm();
  }

  onSelectFile(event) {
    this.resetFlags();

    let fileList: FileList = event.target.files;
    if (fileList.length > 0) {
      this.file = fileList[0];

      let filename: string = this.file.name;
      let filenameSubstring: string = filename.substr(filename.length-3, 3);

      if (filenameSubstring != 'pdf') {
        this.filePicker.setValue(null);
        this.wrongFile = true;
      }
    } else {
      this.filePicker.setValue(null);
    }
  }

  createForm() {
    this.cvForm = this.formBuilder.group({
      filePicker: [null, [Validators.required]]
    });

    this.showSpinnerBool = false;
  }

  submitPDFFile() {
    this.resetFlags();
    this.sendPDFClicked = true;

    this.responseString = '';
    this.errorList = [];

    if (this.cvForm.valid) {
      this.showSpinnerBool = true;

      this.accountService.uploadPDF(this.file, this.stateManagerService.userId).subscribe(res => {
        this.filePicker.setValue(null);
        this.pDFFileSent = true;
        this.sendPDFClicked = false;
        this.stateManagerService.cvPresent = true;
        this.showSpinnerBool = false;
        this.ngOnInit();
      });
    }
  }

  downloadPdf() {
    this.showSpinnerBool = true;
    this.resetFlags();

    this.accountService.downloadCV(this.stateManagerService.userId).subscribe(res => {
      const data = new Blob([res], { type: 'application/pdf' });

      const url= window.URL.createObjectURL(data);
      window.open(url);
      this.showSpinnerBool = false;
    });
  }

  removeCv() {
    this.showSpinnerBool = true;
    this.resetFlags();

    this.accountService.removecV(this.stateManagerService.userId).subscribe(res => {
      this.showSpinnerBool = false;
      this.stateManagerService.cvPresent = false;
      this.userHasCv = false;
      this.deleteSuccessful = true;
      setTimeout(() => this.ngOnInit(), 500);
    });
  }


  resetFlags() {
    this.sendPDFClicked = false;
    this.wrongFile = false;
    this.pDFFileSent = false;
    this.deleteSuccessful = false;
  }

  // GETTER cvForm
  get filePicker() {
    return this.cvForm.controls.filePicker;
  }

}
