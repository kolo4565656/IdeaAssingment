import React from 'react';
import { Route, Routes } from 'react-router-dom';
import { DashBoard } from './pages/DashBoard';
export default function ChartFeature() {
  return (
    <>
      <Routes>
        <Route element={<DashBoard />} path=""></Route>
      </Routes>
    </>
  );
}
