import React from 'react';
import { Route, Routes } from 'react-router-dom';
import DetailPage from './pages/DetailPage';
import ListPage from './pages/ListPage';

export default function PostDisplayFeature() {
  return (
    <>
      <Routes>
        <Route element={<ListPage />} path=""></Route>
        <Route element={<DetailPage />} path=":postId"></Route>
      </Routes>
    </>
  );
}
