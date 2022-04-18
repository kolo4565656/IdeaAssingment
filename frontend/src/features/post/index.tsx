import React from 'react';
import { Route, Routes } from 'react-router-dom';
import AddEditPage from './pages/AddEditPage';
import AddEditPage2 from './pages/AddEditPage2';
import ListPage from './pages/ListPage';

export default function PostFeature() {
  return (
    <>
      <Routes>
        <Route element={<ListPage />} path=""></Route>
        <Route element={<AddEditPage />} path="add"></Route>
        <Route element={<AddEditPage2 />} path=":postId"></Route>
      </Routes>
    </>
  );
}
