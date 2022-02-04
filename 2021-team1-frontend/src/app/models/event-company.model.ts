export interface EventCompany {
  id?: string;
  eventId?: string;
  website: string;
  companyDescription: string;
  companyId?: number;
  companyName?: string;
  arrivalTime: string;
  departureTime: string;
  timeSlot: number;
  createAppointmentUntil: number;
  cancelAppointmentUntil: number;
  attendees: Attendee[];
}

export interface Attendee {
  id?: string;
  firstName: string;
  lastName: string;
}
