import {Component, OnInit} from '@angular/core';
import {Title} from "@angular/platform-browser";
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {AccountService} from "../../../services/account.service";
import {User} from "../../../models/user.model";
import {StateManagerService} from "../../../services/state-manager.service";
import { saveAs } from 'file-saver';

@Component({
  selector: 'app-coor-students',
  templateUrl: './coor-students.component.html',
  styleUrls: ['./coor-students.component.css']
})
export class CoorStudentsComponent implements OnInit {
  isAllDataFetched: boolean = false;
  studentList: User[] = [];
  userForm: FormGroup;
  wrongFile: boolean = false;
  wrongFileStructure: boolean = false;
  sendCSVClicked: boolean = false;
  file: File;
  showSpinnerBool: boolean = true;
  CSVFileSent: boolean = false;
  responseString: string = '';
  errorList: string[];
  selectedOrderOption: number;
  studentsLoaded: number;

  constructor(private titleService: Title, private formBuilder: FormBuilder,
              private accountService: AccountService, private stateManagerService: StateManagerService) { }

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
    } else {
      this.filePicker.setValue(null);
      this.sendCSVClicked = false;
      this.wrongFile = true;
    }
    this.showSpinnerBool = false;
  }

  ngOnInit() {
    this.titleService.setTitle('Stagebeheer | Studenten');
    this.showSpinnerBool = true;
    this.selectedOrderOption = this.stateManagerService.valueSelectedStudentOrderByCoordinator;
    this.fetchData();
  }

  fetchData() {
    this.showSpinnerBool = true;
    this.studentsLoaded = 0;
    this.accountService.getStudents().subscribe(data => {
      console.log('Studenten zijn succesvol opgehaald.');
      this.studentList = data;
      this.createForm();
      this.orderOptionChanged();
      this.isAllDataFetched = true;
      this.showSpinnerBool = false;
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
      this.accountService.addStudentsByCSV(this.file).subscribe(res => {
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

  redefineStudent($event: User) {
    this.showSpinnerBool = true;
    for(let i=0; i < this.studentList.length; i++) {
      if (this.studentList[i].userId == $event.userId) {
        this.studentList[i] = $event;
      }
    }

    this.studentsLoaded++;

    if (this.studentsLoaded == this.studentList.length) {
      this.showSpinnerBool = false;
    }
  }

  orderOptionChanged() {
    this.stateManagerService.valueSelectedStudentOrderByCoordinator = this.selectedOrderOption;

    switch(this.selectedOrderOption) {
      case 1: // Firstname (ascending)
        const compareFnAsc = (a, b) => {
          if (a.userFirstName < b.userFirstName)
            return -1;
          if (a.userFirstName > b.userFirstName)
            return 1;
          return 0;
        };
        this.studentList.sort(compareFnAsc);
        break;
      case 2: // Firstname (descending)
        const compareFnDesc = (a, b) => {
          if (a.userFirstName > b.userFirstName)
            return -1;
          if (a.userFirstName < b.userFirstName)
            return 1;
          return 0;
        };
        this.studentList.sort(compareFnDesc);
        break;
      case 3: // Surname (ascending)
        const compareSnAsc = (a, b) => {
          if (a.userSurname < b.userSurname)
            return -1;
          if (a.userSurname > b.userSurname)
            return 1;
          return 0;
        };
        this.studentList.sort(compareSnAsc);
        break;
      case 4: // Surname (descending)
        const compareSnDesc = (a, b) => {
          if (a.userSurname > b.userSurname)
            return -1;
          if (a.userSurname < b.userSurname)
            return 1;
          return 0;
        };
        this.studentList.sort(compareSnDesc);
        break;
      case 5: // Count applications (ascending)
        const compareAppAsc = (a, b) => {
          if (a.countApplications > b.countApplications)
            return -1;
          if (a.countApplications < b.countApplications)
            return 1;
          return 0;
        };
        this.studentList.sort(compareAppAsc);
        break;
      case 6: // Count applications (descending)
        const compareAppDesc = (a, b) => {
          if (a.countApplications < b.countApplications)
            return -1;
          if (a.countApplications > b.countApplications)
            return 1;
          return 0;
        };
        this.studentList.sort(compareAppDesc);
        break;
    }
  }

  downloadCsvTemplate() {
    this.showSpinnerBool = true;
    this.accountService.getCsvTemplate('STY').subscribe(res => {
      const blob = new Blob([res], {type: 'application/octet-stream'});
      console.log('CSV template is succesvol opgehaald.');

      const fileName = 'Students_template.csv';
      saveAs(blob, fileName);

      console.log('CSV template is succesvol geopend.');
      this.showSpinnerBool = false;
    });
  }

  // GETTER userForm
  get filePicker() {
    return this.userForm.controls.filePicker;
  }
}
