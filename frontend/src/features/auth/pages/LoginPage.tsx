import { Box, Paper, Theme, Typography } from '@mui/material';
import { makeStyles } from '@mui/styles';
import { useAppDispatch } from 'app/hooks';
import { useAppSelector } from 'app/hooks';
import { selectIsFalied } from '../authSlice';
import { authActions } from '../authSlice';
import { Login } from 'models';
import LoginForm from '../components/LoginForm';

const useStyles = makeStyles((theme: Theme) => ({
  root: {
    display: 'flex',
    flexFlow: 'row nowrap',
    justifyContent: 'center',
    alignItems: 'center',
    minHeight: '100vh',
    backgroundImage: 'url("back-ground1.jpg")',
  },

  box: {
    // padding: theme.spacing(3),
    padding: '24px',
  },

  loginbtn: {
    marginTop: '36px !important',
  },
}));

export default function LoginPage() {
  const classes = useStyles();
  const dispatch = useAppDispatch();
  const Failed = useAppSelector(selectIsFalied);

  const handleStudentFormSubmit = (formValues: Login) => {
    dispatch(authActions.login(formValues));
  };

  const initialValues: Login = {
    userName: '',
    password: '',
  } as Login;

  return (
    <div className={classes.root}>
      <Paper elevation={3} className={classes.box}>
        <Typography textAlign={'center'} variant="h5" component="h1">
          Greenwich Talents
        </Typography>
        <Box mt={3} width={'300px'}>
          <LoginForm
            Failed={Failed}
            initialValues={initialValues}
            onSubmit={handleStudentFormSubmit}
          />
        </Box>
      </Paper>
    </div>
  );
}
