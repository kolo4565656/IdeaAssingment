import React from 'react';
import { Navigate, Outlet } from 'react-router-dom';

interface PublicWrapperProps {}

export const PublicWrapper = (props: PublicWrapperProps) => {
  const isLogged = Boolean(localStorage.getItem('access_token'));
  if (isLogged) return <Navigate to="/idea" />;

  return <Outlet />;
};
