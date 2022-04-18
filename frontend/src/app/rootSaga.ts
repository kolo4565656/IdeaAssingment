import { all, takeEvery } from 'redux-saga/effects';
import { PayloadAction } from '@reduxjs/toolkit';
import counterSaga from 'features/counter/counterSaga';
import authSaga from 'features/auth/authSaga';
function* takeSaga() {
  yield takeEvery('counter/decrement', (action: PayloadAction) => console.log(action));
}
const helloSaga = () => {
  console.log('hello saga');
};
export default function* rootSaga() {
  console.log('root saga');
  yield all([helloSaga(), takeSaga(), counterSaga(), authSaga()]);
}
