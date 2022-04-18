import React from 'react';
import { Navigate, Outlet } from 'react-router-dom';

interface PrivateWrapperProps {}

export const PrivateWrapper = (props: PrivateWrapperProps) => {
  const isLogged = Boolean(localStorage.getItem('access_token'));
  if (!isLogged) return <Navigate to="/login" />;

  return <Outlet />;
};
