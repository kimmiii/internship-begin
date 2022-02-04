import { AppointmentType } from '../components/event/shared/timetable/time-table.component';

export interface Appointment {
  id?: string;
  beginHour: string;
  endHour: string;
  attendeeId?: string;
  attendeeName?: string;
  companyId?: number;
  companyName?: string;
  studentId?: number;
  studentName?: string;
  internshipId?: number;
  researchTopicTitle?: string;
  comment?: string;
  disabled?: boolean;
  eventId?: string;
  appointmentStatus: AppointmentStatus;
  cancelMotivation?: string;
  onlineMeetingLink?: string;
  type?: AppointmentType; // FE only
}

export enum AppointmentStatus {
  RESERVED = 'RESERVED',
  CONFIRMED = 'CONFIRMED',
  CANCELLED = 'CANCELLED'
}
