import AppBar from '@mui/material/AppBar';
import Button from '@mui/material/Button';
import { makeStyles } from '@mui/styles';
import Toolbar from '@mui/material/Toolbar';
import Typography from '@mui/material/Typography';
import { useAppDispatch } from 'app/hooks';
import { authActions } from 'features/auth/authSlice';
import { Theme } from '@mui/material';
import { useAppSelector } from 'app/hooks';
import { selectCurrentUser } from '../../features/auth/authSlice';
import { User } from 'models';
import logo from '../../assets/images/logo1.png';

const useStyles = makeStyles((theme: Theme) => ({
  root: {
    flexGrow: 1,
    width: '100%',
    padding: 0,
  },
  title: {
    flexGrow: 1,
    background: 'linear-gradient(90deg, rgba(255,255,255,1) 50%, rgba(0,212,255,1) 100%)',
    width: '100%',
  },
}));

export const Header = () => {
  const classes = useStyles();
  const dispatch = useAppDispatch();
  const info = localStorage.getItem('info') as string;
  let currentUser: User = {
    lastName: '',
    firstName: '',
    userName: '',
    email: '',
    id: '',
    role: 1,
  };
  if (info) {
    currentUser = JSON.parse(info) as User;
  }

  const handleLogoutClick = () => {
    dispatch(authActions.logout());
  };

  return (
    <div className={classes.root}>
      <AppBar position="static">
        <Toolbar style={{ padding: 0 }}>
          <Typography variant="h6" className={classes.title}>
            <img style={{ width: '251px', marginLeft: '8px' }} alt="" src={logo} />- Welcome,{' '}
            {currentUser?.firstName}
            <Button
              style={{
                float: 'right',
                marginBottom: '13.275px',
                marginTop: '13.275px',
                border: '1px solid white',
                display: 'flex',
                paddingTop: '9px',
                marginRight: '8px',
              }}
              color="inherit"
              onClick={handleLogoutClick}
            >
              Logout
            </Button>
          </Typography>
        </Toolbar>
      </AppBar>
    </div>
  );
};
