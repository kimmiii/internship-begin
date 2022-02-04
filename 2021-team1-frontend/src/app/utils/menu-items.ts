import { MenuItem } from '../container.component';

export const coordinatorMenuItems: MenuItem[] = [
  {
    routerLink: '/coordinator/internships',
    displayText: 'Stageaanvragen',
  },
  {
    routerLink: '/coordinator/finished-internships',
    displayText: 'Stageopdrachten',
  },
  {
    routerLink: '/coordinator/assignments',
    displayText: 'Toewijzingen',
  },
  {
    routerLink: '/coordinator/companies',
    displayText: 'Bedrijven',
  },
  {
    routerLink: '/coordinator/lectors',
    displayText: 'Lectoren',
  },
  {
    routerLink: '/coordinator/students',
    displayText: 'Studenten',
  },
  {
    routerLink: '/event/coordinator',
    displayText: 'Handshake Event',
  },
];

export const companyMenuItems: MenuItem[] = [
  {
    routerLink: '/company/internships',
    displayText: 'Mijn stageaanvragen',
  },
  {
    routerLink: '/company/application-form',
    displayText: 'Nieuwe stageaanvraag',
  },
  {
    routerLink: '/company/profile',
    displayText: 'Profiel',
  },
  {
    routerLink: '/event/company',
    displayText: 'Handshake Event',
  },
];

export const studentMenuItems: MenuItem[] = [
  {
    routerLink: '/student/internships',
    displayText: 'Alle stageopdrachten',
  },
  {
    routerLink: '/student/myinternships',
    displayText: 'Mijn stageopdrachten',
  },
  {
    routerLink: '/student/profile',
    displayText: 'Profiel',
  },
  {
    routerLink: '/event/student',
    displayText: 'Handshake Event',
  },
];

export const reviewerMenuItems: MenuItem[] = [
  {
    routerLink: '/reviewer/internships',
    displayText: 'Stageaanvragen',
  },
];
