import { Component, OnInit } from '@angular/core';
import {Title} from "@angular/platform-browser";
import {FormBuilder, FormGroup, Validators} from "@angular/forms"
import {AccountService} from "../../../services/account.service";
import {User} from "../../../models/user.model";
import { saveAs } from 'file-saver';

@Component({
  selector: 'app-coor-lectors',
  templateUrl: './coor-lectors.component.html',
  styleUrls: ['./coor-lectors.component.css']
})
export class CoorLectorsComponent implements OnInit {
  isAllDataFetched: boolean = false;
  lectorList: User[] = [];
  userForm: FormGroup;
  wrongFile: boolean =false;
  sendCSVClicked: boolean = false;
  file: File;
  showSpinnerBool: boolean = true;
  CSVFileSent: boolean = false;
  responseString: string = '';
  errorList: string[];
  wrongFileStructure: boolean = false;

  constructor(private titleService: Title, private formBuilder: FormBuilder,
              private accountService: AccountService) { }


  ngOnInit() {
    this.showSpinnerBool = true;
    this.fetchData();
    this.titleService.setTitle('Stagebeheer | Lectoren');
  }

  onSelectFile(event) {
    this.showSpinnerBool = true;
    let fileList: FileList = event.target.files;
    if (fileList.length > 0) {
      this.file = fileList[0];

      let filename: string = this.file.name;
      let filenameSubstring: string = filename.substr(filename.length-3, 3);

      if (filenameSubstring != 'csv') {
        this.filePicker.setValue(null);
        this.sendCSVClicked = false;
        this.wrongFile = true;
      }
    }

    this.showSpinnerBool = false;
  }

  fetchData() {
    this.showSpinnerBool = true;
    this.accountService.getReviewers().subscribe(data => {
      console.log('Reviewers zijn succesvol opgehaald.');
      this.lectorList = data;
      this.createForm();
      this.showSpinnerBool = false;
      this.isAllDataFetched = true;
    });
  }

  createForm() {
    this.userForm = this.formBuilder.group({
      filePicker: [null, [Validators.required]]
    });
  }

  submitCSVFile() {
    this.showSpinnerBool = true;
    this.wrongFile = false;
    this.wrongFileStructure = false;
    this.sendCSVClicked = true;
    this.CSVFileSent = false;
    this.responseString = '';
    this.errorList = [];

    if (this.userForm.valid) {
      this.showSpinnerBool = true;
      this.accountService.addLectorsByCSV(this.file).subscribe(res => {
        if (res.error == null) {
          let responseList: string[] = res.body;

          if (responseList.length > 2) {
            let errorString: string = responseList[3];
            this.fillErrorList(errorString);
          }

          for (let i = 0; i < 2; i++) {
            this.responseString += responseList[i] + "<br>";
          }
          this.refreshPage();
        } else {
          this.wrongFileStructure = true;
        }

        this.filePicker.setValue(null);
        this.CSVFileSent = true;
        this.sendCSVClicked = false;
        this.showSpinnerBool = false;
      });
    }
  }

  fillErrorList(errorString: string) {
    this.errorList = errorString.toString().split(',');
  }

  refreshPage() {
    setTimeout(() => this.fetchData(), 500);
  }

  downloadCsvTemplate() {
    this.showSpinnerBool = true;
    this.accountService.getCsvTemplate('REV').subscribe(res => {
      const blob = new Blob([res], {type: 'application/octet-stream'});
      console.log('CSV template is succesvol opgehaald.');

      const fileName = 'Lectors_template.csv';
      saveAs(blob, fileName);

      console.log('CSV template is succesvol geopend.');
      this.showSpinnerBool = false;
    });
  }

  // GETTER for userForm
  get filePicker() {
    return this.userForm.controls.filePicker;
  }
}
