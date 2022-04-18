export interface Login {
  userName: string;
  password: string;
}

export interface JWT {
  expiration: string;
  token: string;
}

export interface Response {
  email: string;
  firstName: string;
  lastName: string;
  'http://schemas.microsoft.com/ws/2008/06/identity/claims/role': string;
  'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name': string;
  'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier': string;
  iss: string;
  jti: string;
  aud: string;
}
