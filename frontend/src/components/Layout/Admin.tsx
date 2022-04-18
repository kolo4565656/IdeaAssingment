import { Box, Theme, createTheme } from '@mui/material';
import { Header, Sidebar } from 'components/Common';
import { makeStyles } from '@mui/styles';
import * as React from 'react';
import { ThemeProvider } from '@emotion/react';
import { Routes, Route } from 'react-router-dom';
import { Navigate, useParams } from 'react-router';
import PostFeature from 'features/post';
import PostDisplayFeature from 'features/postDisplay';
import { Counter } from 'features/counter/Counter';
import UserFeature from 'features/mangements/user';
import ChartFeature from 'features/statistic';
import { AdminWrapper } from 'components/Common/AdminWrapper';

const useStyles = makeStyles(() => {
  const theme = createTheme();
  return {
    root: {
      display: 'grid',
      gridTemplateRows: 'auto 1fr',
      gridTemplateColumns: '240px 1fr',
      gridTemplateAreas: `"header header" "sidebar main"`,

      minHeight: '100vh',
    },

    header: {
      gridArea: 'header',
      borderBottom: '1px solid black',
    },
    sidebar: {
      gridArea: 'sidebar',
      borderRight: `1px solid ${theme.palette.divider}`,
      backgroundColor: theme.palette.background.paper,
    },
    main: {
      gridArea: 'main',
      backgroundColor: theme.palette.background.paper,
      padding: theme.spacing(2, 3),
    },
  };
});

export function Admin() {
  const classes = useStyles();
  const param = useParams();

  return (
    <Box className={classes.root}>
      <Box className={classes.header}>
        <Header />
      </Box>
      <Box className={classes.sidebar}>
        <Sidebar defaultt={param['*'] == ''} />
      </Box>

      <Box className={classes.main}>
        <Routes>
          <Route path="*" element={<Navigate to="posts" replace />} />
          <Route element={<ChartFeature />} path="statistic/*" />
          <Route element={<PostFeature />} path="posts/*" />
          <Route element={<PostDisplayFeature />} path="all/*" />
          <Route element={<Counter />} path="counter" />
          <Route element={<AdminWrapper />}>
            <Route element={<UserFeature />} path="user/*" />
          </Route>
        </Routes>
      </Box>
    </Box>
  );
}
