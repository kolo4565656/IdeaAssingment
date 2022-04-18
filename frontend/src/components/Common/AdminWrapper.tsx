import { Response, User } from 'models';
import React from 'react';
import { Navigate, Outlet } from 'react-router-dom';
import jwt_decode from 'jwt-decode';

interface AdminWrapperProps {}

export const AdminWrapper = (props: AdminWrapperProps) => {
  const token = localStorage.getItem('access_token');
  const isLogged = Boolean(token);
  if (!isLogged) return <Navigate to="/login" />;
  const decoded: Response = jwt_decode(token as string);
  const role: string | null | undefined =
    decoded['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
  if (!role || role != 'Admin') return <Navigate to="/403" />;

  return <Outlet />;
};
