export interface Event {
  id?: string;
  name: string;
  dateEvent: Date | string;
  location: EventLocation;
  academicYearId: string;
  isActivated: boolean;
  startHour: string;
  endHour: string;
}

export enum EventLocation {
  ONLINE,
  ON_CAMPUS,
}

export interface AcademicYear {
  id: string;
  startYear: number;
  endYear: number;
  description: string;
}
