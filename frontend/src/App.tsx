import { useAppDispatch } from 'app/hooks';
import { NotFound, PrivateWrapper, PublicWrapper } from 'components/Common';
import { AdminWrapper } from 'components/Common/AdminWrapper';
import { Admin } from 'components/Layout';
import { Unautorized } from 'components/Layout/Unauthorized';
import LoginPage from 'features/auth/pages/LoginPage';
import React from 'react';
import { Route, Routes } from 'react-router-dom';
import './App.css';

function App() {
  return (
    <div>
      <Routes>
        <Route element={<PublicWrapper />}>
          <Route element={<LoginPage />} path="login" />
        </Route>
        <Route element={<PrivateWrapper />}>
          <Route element={<Admin />} path="idea/*"></Route>
        </Route>
        <Route element={<Unautorized />} path="403" />
        {/* <Route path="*" element={<NotFound />} /> */}
      </Routes>

      <div
        style={{
          height: '100px',
          backgroundColor: 'gray',
          marginTop: '48px',
          fontSize: '38px',
          display: 'flex',
          justifyContent: 'center',
          alignItems: 'center',
        }}
      >
        FOOTER
      </div>
    </div>
  );
}

export default App;
