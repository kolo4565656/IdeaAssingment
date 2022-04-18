export interface User {
  id?: string;
  email?: string;
  firstName?: string;
  lastName?: string;
  userName?: string;
  role: Role | string;
}

export enum Role {
  admin = 0,
  staff = 1,
}

export interface ChangePassword {
  userId?: string;
  currentPassword?: string;
  newPassword?: string;
}
