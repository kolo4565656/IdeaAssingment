import { Dashboard, PeopleAlt, Newspaper, Countertops, Group, Numbers } from '@mui/icons-material';
import { createTheme } from '@mui/material';
import List from '@mui/material/List';
import ListItem from '@mui/material/ListItem';
import ListItemIcon from '@mui/material/ListItemIcon';
import ListItemText from '@mui/material/ListItemText';
import { makeStyles } from '@mui/styles';
import React from 'react';
import { NavLink } from 'react-router-dom';

const useStyles = makeStyles(() => {
  const theme = createTheme();
  return {
    root: {
      width: '100%',
      maxWidth: 360,
      backgroundColor: theme.palette.background.paper,
    },

    link: {
      color: 'black',
      textDecoration: 'none',

      '&.active > div': {
        backgroundColor: theme.palette.action.selected,
      },
    },
  };
});

export interface SideBarPros {
  defaultt?: boolean;
}

export const Sidebar = ({ defaultt }: SideBarPros) => {
  const classes = useStyles();
  const info = localStorage.getItem('info');
  let currentUser = {
    lastName: '',
    firstName: '',
    userName: '',
    email: '',
    id: '',
    role: '',
  };
  if (info) {
    currentUser = JSON.parse(info);
  }

  return (
    <div className={classes.root}>
      <List component="nav" aria-label="main mailbox folders">
        <NavLink to="all" className={`${classes.link} ${defaultt ? 'active' : ''}`}>
          <ListItem button>
            <ListItemIcon>
              <Newspaper />
            </ListItemIcon>
            <ListItemText primary="Exhibition" />
          </ListItem>
        </NavLink>

        <NavLink to="posts" className={classes.link}>
          <ListItem button>
            <ListItemIcon>
              <Newspaper />
            </ListItemIcon>
            <ListItemText primary="My Ideas" />
          </ListItem>
        </NavLink>

        {/* <NavLink to="counter" className={classes.link}>
          <ListItem button>
            <ListItemIcon>
              <Countertops />
            </ListItemIcon>
            <ListItemText primary="Counter" />
          </ListItem>
        </NavLink> */}

        {currentUser.role && currentUser.role == 'Admin' ? (
          <>
            <NavLink to="statistic" className={classes.link}>
              <ListItem button>
                <ListItemIcon>
                  <Numbers />
                </ListItemIcon>
                <ListItemText primary="Statistic" />
              </ListItem>
            </NavLink>
            <NavLink to="user" className={classes.link}>
              <ListItem button>
                <ListItemIcon>
                  <Group />
                </ListItemIcon>
                <ListItemText primary="User Management" />
              </ListItem>
            </NavLink>
          </>
        ) : (
          ''
        )}
      </List>
    </div>
  );
};
