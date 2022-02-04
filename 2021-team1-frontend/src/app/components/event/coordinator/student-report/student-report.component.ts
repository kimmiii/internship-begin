import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { PageEvent, Sort } from '@angular/material';
import * as _ from 'lodash';
import { forkJoin, Observable } from 'rxjs';
import { takeUntil } from 'rxjs/operators';

import { Appointment, AppointmentStatus, StudentAppointmentInfo, StudentInfo } from '../../../../models';
import { Company } from '../../../../models/company.model';
import { User } from '../../../../models/user.model';
import { AccountService } from '../../../../services/account.service';
import { AppointmentService } from '../../../../services/appointment.service';
import { CompanyService } from '../../../../services/company.service';
import { BaseComponent } from '../../../../shared/components/base/base.component';

@Component({
  selector: 'app-student-report',
  templateUrl: './student-report.component.html',
  styleUrls: [ './student-report.component.scss' ],
})
export class StudentReportComponent extends BaseComponent implements OnInit {

  dataSource: StudentAppointmentInfo[];

  pageIndex = 0;
  pageSize = 10;
  pageSizeOptions: number[] = [ 5, 10, 25, 100 ];

  sort: Sort;
  sortByAppointments: (studentAppointmentInfo: StudentAppointmentInfo[]) => StudentAppointmentInfo[] =
    (studentAppointmentInfo: StudentAppointmentInfo[]): StudentAppointmentInfo[] => studentAppointmentInfo // NOSONAR
      .sort((a1, a2) => a1.appointments.length - a2.appointments.length);

  studentOptions: StudentInfo[];
  companyOptions: Observable<Company[]>;
  statusOptions: AppointmentStatus[] = [ AppointmentStatus.RESERVED, AppointmentStatus.CONFIRMED, AppointmentStatus.CANCELLED ];

  filterByStudent: (studentAppointmentInfo: StudentAppointmentInfo, values: number[]) => StudentAppointmentInfo =
    (studentAppointmentInfo: StudentAppointmentInfo, values: number[]): StudentAppointmentInfo =>
      values.includes(studentAppointmentInfo.studentId) && studentAppointmentInfo;
  filterByCompany: (studentAppointmentInfo: StudentAppointmentInfo, values: number[]) => StudentAppointmentInfo =
    (studentAppointmentInfo: StudentAppointmentInfo, values: number[]): StudentAppointmentInfo => {
      const appointments = studentAppointmentInfo.appointments
        .filter((appointment: Appointment) => values.includes(appointment.companyId));
      if (appointments && appointments.length) {
        return {
          ...studentAppointmentInfo,
          appointments,
        };
      }
    };
  filterByStatus: (studentAppointmentInfo: StudentAppointmentInfo, values: AppointmentStatus[]) => StudentAppointmentInfo =
    (studentAppointmentInfo: StudentAppointmentInfo, values: AppointmentStatus[]): StudentAppointmentInfo => {
      const appointments = studentAppointmentInfo.appointments
        .filter((appointment: Appointment) => values.includes(appointment.appointmentStatus));
      if (appointments && appointments.length) {
        return {
          ...studentAppointmentInfo,
          appointments,
        };
      }
    };
  studentValues: number[];
  companyValues: number[];
  statusValues: AppointmentStatus[];

  filterForm: FormGroup;

  readonly studentFilterFormControlName = 'studentId';
  readonly companyFilterFormControlName = 'companyId';
  readonly statusFilterFormControlName = 'status';

  get totalRegisteredStudents(): number {
    return (this.dataSource && this.dataSource.filter((studentInfo: StudentAppointmentInfo) =>
      studentInfo.appointments && studentInfo.appointments.length > 0).length) || 0;
  }

  get startIndex(): number {
    return this.pageIndex * this.pageSize;
  }

  get endIndex(): number {
    return this.startIndex + this.pageSize;
  }

  constructor(
    private appointmentService: AppointmentService,
    private accountService: AccountService,
    private companyService: CompanyService,
    private formBuilder: FormBuilder,
  ) {
    super();
    this.createForm();
  }

  ngOnInit(): void {
    this.fetchAppointmentAndStudentData();
    this.fetchCompanies();
    this.registerFormListener();
  }

  private createForm(): void {
    this.filterForm = this.formBuilder.group({
      [this.studentFilterFormControlName]: [ null ],
      [this.companyFilterFormControlName]: [ null ],
      [this.statusFilterFormControlName]: [ null ],
    });
  }

  private fetchAppointmentAndStudentData(): void {
    forkJoin([
      this.accountService.getStudents(),
      this.appointmentService.getAllAppointments([ AppointmentStatus.RESERVED, AppointmentStatus.CONFIRMED, AppointmentStatus.CANCELLED ]),
    ]).subscribe(([ students, appointments ]) => {
      if (students && students.length) {
        this.studentOptions = students.map((student: User) => {
          return {
            studentId: student.userId,
            studentName: `${student.userFirstName} ${student.userSurname}`,
            appointments: [],
          } as StudentInfo;
        });
      }

      if (appointments && appointments.length) {
        const groupedAppointments = _.groupBy(appointments, 'studentId');
        const mappedGroupedAppointments = _.mapValues(groupedAppointments, (appointments: Appointment[]) => { // NOSONAR
          return { appointments };
        });
        const simpleStudentInfo = _.keyBy(this.studentOptions, 'studentId');
        this.dataSource = Object.values(_.merge(mappedGroupedAppointments, simpleStudentInfo));
      }
    });
  }

  private fetchCompanies(): void {
    this.companyOptions = this.companyService.getAllEventCompanies();
  }

  filterAppointments(): void {
    this.studentValues = this.filterForm.value[this.studentFilterFormControlName];
    this.companyValues = this.filterForm.value[this.companyFilterFormControlName];
    this.pageIndex = 0;
  }

  changePage(pageEvent: PageEvent): void {
    this.pageSize = pageEvent.pageSize;
    this.pageIndex = pageEvent.pageIndex;
  }

  changeSort(sort: Sort): void {
    this.sort = sort;
  }

  private registerFormListener(): void {
    this.filterForm.valueChanges
      .pipe(
        takeUntil(this.destroy$),
      ).subscribe(filters => {
      this.studentValues = filters[this.studentFilterFormControlName];
      this.companyValues = filters[this.companyFilterFormControlName];
      this.statusValues = filters[this.statusFilterFormControlName];
      this.pageIndex = 0;
    });
  }
}
