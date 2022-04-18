import { Home } from '@mui/icons-material';
import ListItem from '@mui/material/ListItem';
import ListItemIcon from '@mui/material/ListItemIcon';
import ListItemText from '@mui/material/ListItemText';
import React from 'react';
import { NavLink } from 'react-router-dom';
import './../../assets/styles/403.css';

export function Unautorized() {
  return (
    <div style={{ height: '100vh' }} className="text-wrapper">
      <div className="title" data-content="404">
        403 - ACCESS DENIED
      </div>

      <div className="subtitle">Oops, You don't have permission to access this page.</div>

      <NavLink to="../idea" className={'buttons'}>
        Go to homepage
      </NavLink>
    </div>
  );
}
