export interface JWTClaims {
  jti: string;
  exp: number;
  iss: string;
  aud: string;
  nameid: number;
  companyName: string;
  email: string;
  companyId: number;
  RoleId: number;
  firstName: string;
  surname: string;
  roleCode: string;
  roleDescription: string;
  isUserActivated: boolean;
  cvPresent: boolean;
  isCompanyActivated: boolean;
}

export interface JWTToken {
  token: string;
}
