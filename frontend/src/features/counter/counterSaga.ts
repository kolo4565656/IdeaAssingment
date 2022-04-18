import { delay, put, takeEvery } from 'redux-saga/effects';
import { incrementSaga, incrementSagaSuccess } from './counterSlice';

function* handleIncrement(action: any) {
  console.log(action);
  yield delay(1000);
  yield put(incrementSagaSuccess(action.payload));
}

export default function* counterSaga() {
  console.log('counter saga');

  yield takeEvery(incrementSaga.toString(), handleIncrement);
}
