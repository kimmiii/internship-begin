import { Appointment } from './appointment.model';

export interface StudentInfo {
  studentId: number;
  studentName: string;
}

export interface StudentAppointmentInfo extends StudentInfo {
  appointments?: Appointment[];
}

