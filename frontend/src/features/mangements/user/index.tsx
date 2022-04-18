import React from 'react';
import { Route, Routes } from 'react-router-dom';
import ListPage from './pages/ListPage';

export default function UserFeature() {
  return (
    <>
      <Routes>
        <Route element={<ListPage />} path=""></Route>
      </Routes>
    </>
  );
}
