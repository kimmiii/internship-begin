import { User } from './user.model';

export class Role {
  roleId: number;
  code: string;
  description: string;
  users: User[];

  constructor(
    roleId: number,
    code: string,
    description: string,
    users: User[],
  ) {
    this.roleId = roleId;
    this.code = code;
    this.description = description;
    this.users = users;
  }
}

export enum RoleCode {
  COO = 'COO',
  STU = 'STU',
  REV = 'REV',
  COM = 'COM',
}
