import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

import { Company } from '../../../models/company.model';
import { Internship } from '../../../models/internship.model';
import { Message } from '../../../models/message.model';
import { CompanyService } from '../../../services/company.service';

@Component({
  selector: 'app-rev-internship-item',
  templateUrl: './rev-internship-item.component.html',
  styleUrls: ['./rev-internship-item.component.css']
})
export class RevInternshipItemComponent implements OnInit {
  @Input() internship: Internship;
  @Output() internshipItemLoaded: EventEmitter<any> = new EventEmitter();
  company: Company;
  dataIsFetched = false;
  lastAssignedDate: string;
  countStudentSymbol: number[];

  constructor(private companyService: CompanyService) { }

  ngOnInit(): void {
    this.fetchCompany();
    this.internshipItemLoaded.emit();
  }

  fetchCompany(): void {
    this.companyService.getCompanyById(this.internship.companyId).subscribe(res => {
      if (res === null) {
        this.company.name = '[Niet gevonden]';
      } else {
        console.log(`Bedrijf met id ${this.internship.companyId} succesvol opgehaald.`);
        this.company = res;
        this.lastAssignedDate = this.getLastAssignedDate();
        this.countStudentSymbol = Array(this.internship.totalInternsRequired)
          .fill(1); // array will be filled with number 1 (will not be used)
        this.dataIsFetched = true;
      }
    });
  }

  getLastAssignedDate(): string {
    if (this.internship.internalFeedback !== null) {
      const internalFeedbackList: Message[] = JSON.parse(this.internship.internalFeedback);
      const lastIndex: number = internalFeedbackList.length - 1;
      const lastMessageDateTime: string = internalFeedbackList[lastIndex].MessageDT;

      return this.convertDate(lastMessageDateTime);
    }
  }

  convertDate(lastMessageDateTime: string): string {
    const day: string = lastMessageDateTime.substr(6, 2);
    const month: string = lastMessageDateTime.substr(4, 2);
    const year: string = lastMessageDateTime.substr(0,4);
    const hour: string = lastMessageDateTime.substr(8, 2);
    const minutes: string = lastMessageDateTime.substr(10, 2);

    return `${day}-${month}-${year} (${hour}u${minutes})`;
  }
}
